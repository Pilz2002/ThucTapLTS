using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using ThucTapLTSedu.DataContext;
using ThucTapLTSedu.Entities;
using ThucTapLTSedu.Payloads.Converter.UserConverter;
using ThucTapLTSedu.Payloads.DataRequests.UserRequests;
using ThucTapLTSedu.Payloads.DataResponses.AuthResponses;
using ThucTapLTSedu.Payloads.DataResponses.UserResponses;
using ThucTapLTSedu.Payloads.Responses;
using ThucTapLTSedu.Services.Interfaces;
using ThucTapLTSedu.Handler.Validation;
using BCryptNet = BCrypt.Net.BCrypt;
using System.Net.Mail;
using System.Net;

namespace ThucTapLTSedu.Services.Implements
{
	public class AuthServices : IAuthServices
	{
		private readonly IConfiguration _configuration;
		private readonly AppDbContext _context;
		private readonly ResponseObject<DataResponse_Token> _responseTokenObject;
		private readonly ResponseObject<DataResponse_User> _responseUserObject;
		private readonly User_Converter _userConverter;
		private readonly ResponseObject<string> _stringRespone;

		public AuthServices(IConfiguration configuration, AppDbContext context,
			ResponseObject<DataResponse_Token> responseTokenObject,
			ResponseObject<DataResponse_User> responseUserObject,
			User_Converter userConverter, ResponseObject<string> stringRespone)
		{
			_configuration = configuration;
			_context = context;
			_responseTokenObject = responseTokenObject;
			_responseUserObject = responseUserObject;
			_userConverter = userConverter;
			_stringRespone = stringRespone;
		}

		private string GenerateRefreshToken()
		{
			var random = new byte[32];
			using (var item = RandomNumberGenerator.Create())
			{
				item.GetBytes(random);
				return Convert.ToBase64String(random);
			}
		}

		private DataResponse_Token GenerateAccessToken(User user)
		{
			var jwtTokenHandler = new JwtSecurityTokenHandler();
			var secretKeyBytes = System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:SecretKey").Value!);
			var role = _context.Roles.FirstOrDefault(x => x.Id == user.RoleId);

			var tokenDescription = new SecurityTokenDescriptor
			{
				Subject = new System.Security.Claims.ClaimsIdentity(new[]
				{
					new Claim("Id",user.Id.ToString()),
					new Claim("Email",user.Email),
					new Claim(ClaimTypes.Role, role?.Code??"")
				}),
				Expires = DateTime.Now.AddHours(4),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = jwtTokenHandler.CreateToken(tokenDescription);
			var accessToken = jwtTokenHandler.WriteToken(token);
			var refreshToken = GenerateRefreshToken();

			RefreshToken rf = new RefreshToken
			{
				Token = refreshToken,
				ExpiredTime = DateTime.Now.AddHours(6),
				UserId = user.Id
			};
			_context.RefreshTokens.Add(rf);
			_context.SaveChanges();

			DataResponse_Token tokenDTO = new DataResponse_Token
			{
				AccessToken = accessToken,
				RefreshToken = refreshToken
			};
			return tokenDTO;
		}

		public ResponseObject<DataResponse_Token> RenewAccessToken(DataRequest_Token request)
		{
			try
			{
				var jwtTokenHandler = new JwtSecurityTokenHandler();
				var secretKey = _configuration.GetSection("AppSettings:SecretKey").Value;

				var tokenValidation = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					ValidateAudience = false,
					ValidateIssuer = false,
					ValidateLifetime = false,
					IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:SecretKey").Value!))
				};
				var tokenAuthentication = jwtTokenHandler.ValidateToken(request.AccessToken, tokenValidation, out var validatedToken);
				if (validatedToken is not JwtSecurityToken jwtSecurityToken || jwtSecurityToken.Header.Alg != SecurityAlgorithms.HmacSha256)
				{
					return _responseTokenObject.ErrorResponse(StatusCodes.Status400BadRequest, "Token không hợp lệ", null);
				}

				var refreshToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == request.RefreshToken);

				if (refreshToken == null)
				{
					return _responseTokenObject.ErrorResponse(StatusCodes.Status404NotFound, "RefreshToken không tồn tại trong database", null);
				}

				if (refreshToken.ExpiredTime < DateTime.Now)
				{
					return _responseTokenObject.ErrorResponse(StatusCodes.Status401Unauthorized, "RefreshToken đã hết hạn", null);
				}

				var user = _context.Users.FirstOrDefault(x => x.Id == refreshToken.UserId);

				if (user is null)
				{
					return _responseTokenObject.ErrorResponse(StatusCodes.Status404NotFound, "Người dùng không tồn tại", null);
				}
				var newToken = GenerateAccessToken(user);
				return _responseTokenObject.SuccessResponse("Token đã được làm mới", newToken);
			}
			catch (SecurityTokenValidationException ex)
			{
				return _responseTokenObject.ErrorResponse(StatusCodes.Status400BadRequest, "Lỗi xác thực token: " + ex.Message, null);
			}
			catch (Exception ex)
			{
				return _responseTokenObject.ErrorResponse(StatusCodes.Status500InternalServerError, "Lỗi không xác định: " + ex.Message, null);
			}
		}

		public ResponseObject<DataResponse_Token> Login(DataRequest_Login request)
		{
			if (string.IsNullOrWhiteSpace(request.Username)
				|| string.IsNullOrWhiteSpace(request.Password))
			{
				return _responseTokenObject.ErrorResponse(StatusCodes.Status400BadRequest, "Không bỏ trống thông tin", null);
			}
			var user = _context.Users.FirstOrDefault(x => x.Username == request.Username);
			if (user is null)
			{
				return _responseTokenObject.ErrorResponse(StatusCodes.Status400BadRequest, "Tài khoản không tồn tại", null);
			}
			if (!BCryptNet.Verify(request.Password, user.Password))
			{
				return _responseTokenObject.ErrorResponse(StatusCodes.Status400BadRequest, "Sai mật khẩu", null);
			}
			if (user.UserStatusId == 3)
			{
				return _responseTokenObject.ErrorResponse(StatusCodes.Status400BadRequest, "Tài khoản bị khóa", null);
			}
			return _responseTokenObject.SuccessResponse("Đăng nhập thành công", GenerateAccessToken(user));
		}

		public ResponseObject<DataResponse_User> Register(DataRequest_Register request)
		{
			if (string.IsNullOrWhiteSpace(request.Username)
			 || string.IsNullOrWhiteSpace(request.Password)
			 || string.IsNullOrWhiteSpace(request.ConfirmPassword)
			 || string.IsNullOrWhiteSpace(request.Email)
			 || string.IsNullOrWhiteSpace(request.Name)
			 || string.IsNullOrWhiteSpace(request.PhoneNumber)
				)
			{
				return _responseUserObject.ErrorResponse(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);
			}

			if (request.Password != request.ConfirmPassword)
			{
				return _responseUserObject.ErrorResponse(StatusCodes.Status400BadRequest, "Mật khẩu và xác nhận mật khẩu không khớp nhau", null);
			}

			if (!PasswordValidation.IsStrongPass(request.Password))
			{
				return _responseUserObject.ErrorResponse(StatusCodes.Status400BadRequest, "Mật khẩu phải trên 8 ký tự có chứa chữ cái/chữ số/ ký tự đặc biệt", null);
			}

			if (!EmailValidation.IsValidEmail(request.Email))
			{
				return _responseUserObject.ErrorResponse(StatusCodes.Status400BadRequest, "Định dạng Email không hợp lệ", null);
			}

			if (!PhoneValidation.IsValidPhoneNumber(request.PhoneNumber))
			{
				return _responseUserObject.ErrorResponse(StatusCodes.Status400BadRequest, "Định dạng số điện thoại không hợp lệ", null);
			}

			if (_context.Users.FirstOrDefault(x => x.Username.Equals(request.Username)) != null)
			{
				return _responseUserObject.ErrorResponse(StatusCodes.Status400BadRequest, "Tên tài khoản này đã được đăng ký", null);
			}

			if (_context.Users.FirstOrDefault(x => x.Email.Equals(request.Email)) != null)
			{
				return _responseUserObject.ErrorResponse(StatusCodes.Status400BadRequest, "Email này đã được đăng ký", null);
			}

			if (_context.Users.FirstOrDefault(x => x.PhoneNumber.Equals(request.PhoneNumber)) != null)
			{
				return _responseUserObject.ErrorResponse(StatusCodes.Status400BadRequest, "Số điện thoại này đã được đăng ký", null);
			}
			try
			{
				User user = new User()
				{
					Point = 0,
					Username = request.Username,
					Email = request.Email,
					Name = request.Name,
					PhoneNumber = request.PhoneNumber,
					Password = BCryptNet.HashPassword(request.Password),
					RankCustomerId = 1,// Vip 0
					UserStatusId = 1,// Unverified - chưa xác minh email
					IsActive = false,
					RoleId = 1 // normal user
				};
				_context.Users.Add(user);
				_context.SaveChanges();
				return _responseUserObject.SuccessResponse("Bạn đã đăng ký tài khoản thành công", _userConverter.UserDTO(user));
			}
			catch (Exception ex)
			{
				return _responseUserObject.ErrorResponse(StatusCodes.Status500InternalServerError, ex.Message, null);
			}
		}

		private int RandomCode()
		{
			Random random = new Random();
			return random.Next(10000000, 99999999);
		}

		private string RandomPassword()
		{
			Random random = new Random();
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			char[] randomChars = new char[12];

			for (int i = 0; i < 12; i++)
			{
				randomChars[i] = chars[random.Next(chars.Length)];
			}

			return new string(randomChars);
		}

		private ResponseObject<string> SendCodeToEmail(int userId, EmailTo emailTo)
		{
			var user = _context.Users.FirstOrDefault(x => x.Id == userId);
			var smtpClient = new SmtpClient("smtp.gmail.com")
			{
				Port = 587,
				Credentials = new NetworkCredential($"{_configuration.GetSection("SmtpEmail:EmailHost").Value}", $"{_configuration.GetSection("SmtpEmail:EmailHostPass").Value}"),
				EnableSsl = true,
				UseDefaultCredentials = false
			};
			try
			{
				var email = _configuration.GetSection("SmtpEmail:EmailHost").Value;
				var message = new MailMessage();
				message.From = new MailAddress($"{_configuration.GetSection("SmtpEmail:EmailHost").Value}", "QuyenNguyenCinema");
				message.To.Add(user.Email);
				message.Subject = $"{emailTo.Subject}";
				message.Body = $"{emailTo.Content}: {emailTo.Code}";
				message.IsBodyHtml = true;
				smtpClient.Send(message);
				smtpClient.Dispose();
				return _stringRespone.SuccessResponse("Gửi email thành công", null);
			}
			catch (Exception ex)
			{
				return _stringRespone.ErrorResponse(StatusCodes.Status500InternalServerError, "Lỗi khi gửi email", ex.Message);
			}
		}

		public ResponseObject<string> GetCode(int userId)
		{
			var user = _context.Users.FirstOrDefault(x => x.Id == userId);

			if (user.UserStatusId == 2)
			{
				return _stringRespone.ErrorResponse(StatusCodes.Status400BadRequest, "Tài khoản này đã xác thực Email rồi", null);
			}

			EmailTo emailTo = new EmailTo
			{
				Subject = "Mã xác nhận email",
				Code = RandomCode().ToString(),
				Content = "Mã xác nhận email của bạn sẽ hết hạn sau 30 phút"
			};

			ConfirmEmail confirmEmail = new ConfirmEmail
			{
				UserId = userId,
				ConfirmCode = emailTo.Code,
				ExpiredTime = DateTime.Now.AddMinutes(30),
				IsConfirm = false
			};

			_context.ConfirmEmails.Add(confirmEmail);
			_context.SaveChanges();

			var mess = SendCodeToEmail(userId, emailTo);
			return mess;
		}

		public ResponseObject<DataResponse_User> VerifyEmail(DataRequest_VerifyEmail request, int userId)
		{
			var user = _context.Users.FirstOrDefault(x => x.Id == userId);
			var confirmEmail = _context.ConfirmEmails.Where(x => x.UserId == userId && x.IsConfirm == false)
				.OrderByDescending(x => x.ExpiredTime).First();
			if (confirmEmail.ConfirmCode == request.CodeConfirm)
			{
				confirmEmail.IsConfirm = true;
				user.UserStatusId = 2;
				_context.Users.Update(user);
				_context.ConfirmEmails.Update(confirmEmail);
				_context.SaveChanges();
				return _responseUserObject.SuccessResponse("Xác nhận email thành công", _userConverter.UserDTO(user));
			}
			return _responseUserObject.ErrorResponse(StatusCodes.Status400BadRequest, "Sai mã xác nhận", null);
		}

		public ResponseObject<string> ForgotPassword(DataRequest_ForgotPassword request)
		{
			var user = _context.Users.FirstOrDefault(x => x.Email == request.Email);
			if (user is null)
			{
				return _stringRespone.ErrorResponse(StatusCodes.Status400BadRequest, "Email này không tồn tại", null);
			}
			EmailTo emailTo = new EmailTo
			{
				Code = RandomPassword(),
				Content = "Mật khẩu của bạn được đặt thành",
				Subject = "Mật khẩu của bạn đã được reset lại. Vui lòng giữ bảo mật mật khẩu này"
			};
			var mess = SendCodeToEmail(user.Id, emailTo);
			user.Password = BCryptNet.HashPassword(emailTo.Code);
			_context.Users.Update(user);
			_context.SaveChanges();
			return _stringRespone.SuccessResponse("Mật khẩu của bạn đã được đặt lại", null);
		}

		public ResponseObject<string> ChangePassword(int userId, DataRequest_ChangePassword request)
		{
			var user = _context.Users.FirstOrDefault(x => x.Id == userId);
			bool checkPass = BCryptNet.Verify(request.OldPassword, user.Password);
			if (!checkPass)
			{
				return _stringRespone.ErrorResponse(StatusCodes.Status400BadRequest, "Sai mật khẩu", null);
			}
			if (request.NewPassword != request.ConfirmPassword)
			{
				return _stringRespone.ErrorResponse(StatusCodes.Status400BadRequest, "Xác nhận mật khẩu không trùng khớp mật khẩu", null);
			}
			user.Password = BCryptNet.HashPassword(request.NewPassword);
			_context.Users.Update(user);
			_context.SaveChanges();
			return _stringRespone.SuccessResponse("Đổi mật khẩu thành công", null);
		}
	}
}
