using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text;
using ThucTapLTSedu.DataContext;
using ThucTapLTSedu.Entities;
using ThucTapLTSedu.Handler.Paging;
using ThucTapLTSedu.Handler.PaymentMethod;
using ThucTapLTSedu.Payloads.Converter.CinemaConverter;
using ThucTapLTSedu.Payloads.DataRequests.UserRequests;
using ThucTapLTSedu.Payloads.DataRequests.VnPayRequest;
using ThucTapLTSedu.Payloads.DataResponses.AdminResponses;
using ThucTapLTSedu.Payloads.DataResponses.UserResponses;
using ThucTapLTSedu.Payloads.DataResponses.VnPayResponse;
using ThucTapLTSedu.Payloads.Responses;
using ThucTapLTSedu.Services.Interfaces;

namespace ThucTapLTSedu.Services.Implements
{
	public class UserServices : IUserServices
	{
		private readonly AppDbContext _context;
		private readonly Movie_Converter _movieConverter;
		private readonly Seat_Converter _seatConverter;
		private readonly Schedule_Converter _scheduleConverter;
		private readonly Cinema_Converter _cinemaConverter;
		private readonly Room_Converter _roomConverter;
		private readonly Ticket_Converter _ticketConverter;
		private readonly BillFood_Converter _billFoodConverter;
		private readonly BIll_Converter _billConverter;

		private readonly ResponseObject<DataResponse_Bill> _billResponse;

		private readonly IConfiguration _configuration;

		public UserServices(AppDbContext context, Movie_Converter movieConverter, Seat_Converter seatConverter,
			Schedule_Converter scheduleConverter, Cinema_Converter cinemaConverter, Room_Converter roomConverter,
			Ticket_Converter ticketConverter, BillFood_Converter billFoodConverter,
			BIll_Converter billConverter, ResponseObject<DataResponse_Bill> billResponse, IConfiguration configuration)
		{
			_context = context;
			_movieConverter = movieConverter;
			_seatConverter = seatConverter;
			_scheduleConverter = scheduleConverter;
			_cinemaConverter = cinemaConverter;
			_roomConverter = roomConverter;
			_ticketConverter = ticketConverter;
			_billFoodConverter = billFoodConverter;
			_billConverter = billConverter;

			_billResponse = billResponse;

			_configuration = configuration;
		}

		public PageResult<DataResponse_Movie> GetMovieByCinema(string cinemaCode, int pageSize, int pageNumber)
		{
			var query = _context.Cinemas
				.Include(x => x.Rooms)
				.ThenInclude(x => x.Schedules)
				.ThenInclude(x => x.Movie)
				.FirstOrDefault(x => x.Code == cinemaCode);

			// Lấy rừng room trong query
			List<Room> listRooms = new List<Room>();
			foreach (var room in query.Rooms)
			{
				listRooms.Add(room);
			}
			//Lấy từng schedules trong room
			List<Schedule> listSchedules = new List<Schedule>();
			foreach (var room in listRooms)
			{
				var schedulesOfRoom = room.Schedules.ToList();
				foreach (var schedule in schedulesOfRoom)
				{
					listSchedules.Add(schedule);
				}
			}
			//Lấy movie dựa theo schedule
			List<Movie> listMovie = new List<Movie>();
			foreach (var schedule in listSchedules)
			{
				var movie = _context.Movies.FirstOrDefault(x => x.Id == schedule.MovieId);
				if (movie is not null)
				{
					listMovie.Add(movie);
				}
			}
			//Loại bỏ các giá trị trùng lặp
			List<Movie> distinctMovies = listMovie.GroupBy(movie => movie.Id).Select(group => group.First()).ToList();
			var movieDTO = distinctMovies.Select(x => _movieConverter.MovieDTO(x));
			var ret = Pagination.GetPagedData(movieDTO.AsQueryable(), pageSize, pageNumber);
			return ret;
		}

		public PageResult<DataResponse_Seat> GetSeatByRoom(string cinemaCode, string roomCode, int pageSize, int pageNumber)
		{
			var query = _context.Cinemas.Include(x => x.Rooms).FirstOrDefault(x => x.Code == cinemaCode);
			List<Room> listRooms = new List<Room>();
			foreach (var room in query.Rooms)
			{
				if (room.Code == roomCode)
				{
					listRooms.Add(room);
				}
			}
			List<Seat> listSeats = new List<Seat>();
			foreach (var room in listRooms)
			{
				var seats = _context.Seats.Where(x => x.RoomId == room.Id).ToList();
				listSeats.AddRange(seats);
			}
			var seatDTO = listSeats.Select(x => _seatConverter.SeatDTO(x));
			var ret = Pagination.GetPagedData(seatDTO.AsQueryable(), pageSize, pageNumber);
			return ret;
		}

		private string GenerateRandomString()
		{
			Random random = new Random();
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			StringBuilder stringBuilder = new StringBuilder(10);

			for (int i = 0; i < 10; i++)
			{
				stringBuilder.Append(chars[random.Next(chars.Length)]);
			}

			return stringBuilder.ToString();
		}

		public PageResult<DataResponse_Schedule> ChooseMovie(string movieName, int pageSize, int pageNumber)
		{
			var movie = _context.Movies.FirstOrDefault(x => x.Name == movieName);
			var listSchedules = _context.Schedules.Where(x => x.MovieId == movie.Id).ToList();
			var scheduleDTO = listSchedules.Select(x => _scheduleConverter.ScheduleDTO(x));
			var ret = Pagination.GetPagedData(scheduleDTO.AsQueryable(), pageSize, pageNumber);
			return ret;
		}

		public PageResult<DataResponses_Cinema> ChooseSchedule(string scheduleCode, int pageSize, int pageNumber)
		{
			var schedule = _context.Schedules.FirstOrDefault(x => x.Code == scheduleCode);
			var listRoom = _context.Rooms.Where(x => x.Id == schedule.RoomId).ToList();
			List<Cinema> listCinemas = new List<Cinema>();
			foreach (var room in listRoom)
			{
				var cinema = _context.Cinemas.FirstOrDefault(x => x.Id == room.CinemaId);
				listCinemas.Add(cinema);
			}
			var cinemaDTO = listCinemas.Select(x => _cinemaConverter.CinemaDTO(x));
			var ret = Pagination.GetPagedData(cinemaDTO.AsQueryable(), pageSize, pageNumber);
			return ret;
		}

		public PageResult<DataResponse_Room> ChooseCinema(string cinemaCode, int pageSize, int pageNumber)
		{
			var cinema = _context.Cinemas.FirstOrDefault(x => x.Code == cinemaCode);
			var listRoom = _context.Rooms.Where(x => x.CinemaId == cinema.Id).ToList();
			var roomDTO = listRoom.Select(x => _roomConverter.RoomDTO(x));
			var ret = Pagination.GetPagedData(roomDTO.AsQueryable(), pageSize, pageNumber);
			return ret;
		}

		public PageResult<DataResponse_Seat> ChooseRoom(string cinemaCode, string roomCode, string scheduleCode, int pageSize, int pageNumber)
		{
			var room = _context.Rooms.FirstOrDefault(x => x.Code == roomCode && x.Cinema.Code == cinemaCode);
			var schedule = _context.Schedules.FirstOrDefault(x => x.Code == scheduleCode);
			var tickets = _context.Tickets.Where(x => x.ScheduleId == schedule.Id && x.IsActive == true).ToList();
			var listSeats = _context.Seats.Where(x => x.RoomId == room.Id && x.IsActive == true).ToList();
			//Hiển thị trạng thái ghế đã được đặt chưa theo lịch chiếu
			foreach (var seat in listSeats)
			{
				foreach (var ticket in tickets)
				{
					if (seat.Id == ticket.SeatId)
					{
						seat.SeatStatusId = 2;
						break;
					}
				}
			}
			var seatDTO = listSeats.Select(x => _seatConverter.SeatDTO(x));
			var ret = Pagination.GetPagedData(seatDTO.AsQueryable(), pageSize, pageNumber);
			return ret;
		}

		public PageResult<DataResponse_Ticket> ChooseSeats(int userId, DataRequest_ChooseSeats request, int pageSize,int pageNumber)
		{
			//GenerateBill
			var user = _context.Users.FirstOrDefault(x => x.Id == userId);
			var rankCustomer = _context.RankCustomers.FirstOrDefault(x => x.Id == user.RankCustomerId);
			var promotion = _context.Promotions.FirstOrDefault(x => x.RankCustomerId == rankCustomer.Id
			&& x.StartTime <= DateTime.Now && x.EndTime >= DateTime.Now);
			Bill newBill = new Bill
			{
				TotalMoney = 0,
				TradingCode = GenerateRandomString(),
				CreateTime = DateTime.Now,
				CustomerId = userId,
				Name = user.Email + "-" + request.ScheduleCode,
				UpdateTime = DateTime.Now,
				PromotionId = promotion?.Id ?? null,
				BillStatusId = 1,
				IsActive = true,
			};
			_context.Bills.Add(newBill);
			_context.SaveChanges();

			//Generate Tikcet
			var schedule = _context.Schedules.FirstOrDefault(x => x.Code == request.ScheduleCode);
			List<Ticket> listTickets = new List<Ticket>();
			foreach (var seatId in request.seatIds)
			{
				Ticket newticket = new Ticket
				{
					Code = GenerateRandomString(),
					ScheduleId = schedule.Id,
					SeatId = seatId,
					PriceTicket = schedule.Price,
					IsActive = true
				};
				listTickets.Add(newticket);
			}
			_context.Tickets.AddRange(listTickets);
			_context.SaveChanges();

			//Generate Bill Ticket
			var bill = _context.Bills.OrderByDescending(x => x.CreateTime).FirstOrDefault(x => x.CustomerId == userId && x.BillStatusId == 1 && x.IsActive == true);
			List<BillTicket> lstBillTicket = new List<BillTicket>();
			foreach (var ticket in listTickets)
			{
				BillTicket billTicket = new BillTicket
				{
					Quantity = 1,
					BillId = bill.Id,
					TicketId = ticket.Id
				};
				lstBillTicket.Add(billTicket);
			}
			_context.BillTickets.AddRange(lstBillTicket);
			_context.SaveChanges();
			var ticketDTO = listTickets.Select(x => _ticketConverter.TicketDTO(x));
			var ret = Pagination.GetPagedData(ticketDTO.AsQueryable(), pageSize, pageNumber);
			return ret;
		}

		public PageResult<DataResponse_BillFood> ChooseFood(int userId, List<DataRequest_ChooseFood> requests, int pageSize,int pageNumber)
		{
			var bill = _context.Bills.OrderByDescending(x => x.CreateTime).FirstOrDefault(x => x.CustomerId == userId && x.BillStatusId == 1 && x.IsActive == true);
			List<BillFood> lstBillFood = new List<BillFood>();
			foreach (var request in requests)
			{
				BillFood billFood = new BillFood
				{
					BillId = bill.Id,
					FoodId = _context.Foods.FirstOrDefault(x => x.NameOfFood == request.FoodName).Id,
					Quantity = request.Quantity
				};
				lstBillFood.Add(billFood);
			}
			_context.BillFoods.AddRange(lstBillFood);
			_context.SaveChanges();
			var billFoodDTO = lstBillFood.Select(x => _billFoodConverter.BillFoodDTO(x));
			var ret = Pagination.GetPagedData(billFoodDTO.AsQueryable(), pageSize, pageNumber);
			return ret;
		}

		public ResponseObject<DataResponse_Bill> ConfirmBill(int userId)
		{
			var bill = _context.Bills.OrderByDescending(x=>x.CreateTime).FirstOrDefault(x => x.CustomerId == userId && x.BillStatusId == 1 && x.IsActive == true);
			var billFoods = _context.BillFoods.Where(x => x.BillId == bill.Id).ToList();
			var billTickets = _context.BillTickets.Where(x => x.BillId == bill.Id).ToList();
			var promotion = _context.Promotions.FirstOrDefault(x => x.Id == bill.PromotionId);
			double totalMoney = 0;
			foreach (var billFood in billFoods)
			{
				var food = _context.Foods.FirstOrDefault(x => x.Id == billFood.FoodId);
				totalMoney = totalMoney + (food.Price * (double)billFood.Quantity);
			}
			foreach (var billTicket in billTickets)
			{
				var ticket = _context.Tickets.FirstOrDefault(x => x.Id == billTicket.TicketId);
				totalMoney = totalMoney + (ticket.PriceTicket * (double)billTicket.Quantity);
			}
			if (promotion is not null)
			{

				bill.TotalMoney = totalMoney - (totalMoney * promotion.Percent / 100);
			}
			else
			{
				bill.TotalMoney = totalMoney;
			}
			bill.IsActive = false;
			_context.Bills.Update(bill);
			_context.SaveChanges();
			return _billResponse.SuccessResponse("Tạo bill thành công", _billConverter.BillDTO(bill));
		}


		public string PayForBill(HttpContext httpContext, int userId)
		{
			var user = _context.Users.FirstOrDefault(x => x.Id == userId);
			var bill = _context.Bills.OrderByDescending(x => x.CreateTime).FirstOrDefault(x => x.CustomerId == userId && x.BillStatusId == 1 && x.IsActive == true);
			OrderInfor order = new OrderInfor();
			order.OrderId = DateTime.Now.Ticks; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
			order.Amount = Convert.ToInt64(bill.TotalMoney); // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 100,000 VND
			order.Status = "0"; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending" khởi tạo giao dịch chưa có IPN
			order.CreatedDate = DateTime.Now;
			var ret = CreatePaymentUrl(httpContext, order);
			return ret;
		}


		private string CreatePaymentUrl(HttpContext context, OrderInfor order)
		{
			//Get Config Info
			string vnp_Returnurl = _configuration.GetSection("VnPay:vnp_Returnurl").Value!; //URL nhan ket qua tra ve 
			string vnp_Url = _configuration.GetSection("VnPay:vnp_Url").Value!; //URL thanh toan cua VNPAY 
			string vnp_TmnCode = _configuration.GetSection("VnPay:vnp_TmnCode").Value!; //Ma định danh merchant kết nối (Terminal Id)
			string vnp_HashSecret = _configuration.GetSection("VnPay:vnp_HashSecret").Value!; //Secret Key
			//Build URL for VNPAY
			VnPayLibrary vnpay = new VnPayLibrary();

			vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
			vnpay.AddRequestData("vnp_Command", "pay");
			vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
			vnpay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000

			vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
			vnpay.AddRequestData("vnp_CurrCode", "VND");
			vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
			vnpay.AddRequestData("vnp_Locale", "vn");

			vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order.OrderId);
			vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

			vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
			vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

			//Add Params of 2.1.0 Version
			//Billing
			string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
			return paymentUrl;
		}

		public DataResponse_Vnpay PaymentExecute(IQueryCollection collections)
		{
			string vnp_HashSecret = _configuration.GetSection("AppSettings:vnp_HashSecret").Value!;//Chuoi bi mat
			VnPayLibrary vnpay = new VnPayLibrary();
			foreach (var (key, values) in collections)
			{
				//get all querystring data
				if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
				{
					vnpay.AddResponseData(key, values.ToString());
				}
			}
			//vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay    
			//vnp_TransactionNo: Ma GD tai he thong VNPAY
			//vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
			//vnp_SecureHash: HmacSHA512 cua du lieu tra ve

			long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
			long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
			String vnp_SecureHash = collections.FirstOrDefault(x => x.Key == "vnp_HashSecret").Value!;
			string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
			var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");

			bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
			if (!checkSignature)
			{
				return new DataResponse_Vnpay
				{
					Success = false
				};
			}

			return new DataResponse_Vnpay
			{
				Success = true,
				PaymentMethod = "VNPay",
				OrderDescription = vnp_OrderInfo,
				TransactionId = vnpayTranId.ToString(),
				Token = vnp_HashSecret,
				VnPayResponseCode = vnp_ResponseCode.ToString()
			};
		}
	}
}
