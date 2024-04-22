using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThucTapLTSedu.Payloads.DataRequests.UserRequests;
using ThucTapLTSedu.Services.Interfaces;

namespace ThucTapLTSedu.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthServices _authServices;

		public AuthController(IAuthServices authServices)
		{
			_authServices = authServices;
		}

		[HttpPost("/Account/Register")]
		public IActionResult Register([FromBody] DataRequest_Register request)
		{
			return Ok(_authServices.Register(request));
		}

		[HttpPost("/Account/Login")]
		public IActionResult Login([FromBody] DataRequest_Login request)
		{
			return Ok(_authServices.Login(request));
		}

		[HttpPost("/Account/RenewToken")]
		public IActionResult RenewToken(DataRequest_Token request)
		{
			return Ok(_authServices.RenewAccessToken(request));
		}

		[HttpGet("Account/GetEmailCode")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public IActionResult GetEmailCode()
		{
			int Id = int.Parse(HttpContext.User.FindFirst("Id").Value);
			return Ok(_authServices.GetCode(Id));
		}

		[HttpPost("Account/VerifyEmail")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public IActionResult VerifyEmail(DataRequest_VerifyEmail request)
		{
			int Id = int.Parse(HttpContext.User.FindFirst("Id").Value);
			return Ok(_authServices.VerifyEmail(request,Id));
		}

		[HttpPost("Account/ForgotPassword")]
		public IActionResult ForgotPassword(DataRequest_ForgotPassword request)
		{
			return Ok(_authServices.ForgotPassword(request));
		}

		[HttpPost("Account/ChangePassword")]
		[Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
		public IActionResult ChangePassword(DataRequest_ChangePassword request)
		{
			int Id = int.Parse(HttpContext.User.FindFirst("Id").Value);
			return Ok(_authServices.ChangePassword(Id,request));
		}

	}
}
