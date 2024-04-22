using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ThucTapLTSedu.Handler.Paging;
using ThucTapLTSedu.Payloads.DataRequests.UserRequests;
using ThucTapLTSedu.Payloads.DataResponses.AdminResponses;
using ThucTapLTSedu.Payloads.DataResponses.UserResponses;
using ThucTapLTSedu.Services.Interfaces;

namespace ThucTapLTSedu.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserServices _userServices;

		public UserController(IUserServices userServices)
		{
			_userServices = userServices;
		}

		[HttpGet("/GetMovieByCinema")]
		public IActionResult GetMovie([FromQuery]string cinemaCode, int pageSize, int pageNumber)
		{
			return Ok(_userServices.GetMovieByCinema(cinemaCode, pageSize, pageNumber));
		}

		[HttpGet("/GetSeatByRoom")]
		public IActionResult GetSeatByRoom([FromQuery] string cinemaCode, string roomCode, int pageSize, int pageNumber)
		{
			return Ok(_userServices.GetSeatByRoom(cinemaCode,roomCode,pageSize,pageNumber));
		}

		[HttpPost("/SelectMovie")]
		public IActionResult ChooseMovie(string movieName,int pageSize,int pageNumber)
		{
			return Ok(_userServices.ChooseMovie(movieName, pageSize, pageNumber));
		}

		[HttpPost("/ChooseSchedule")]
		public IActionResult ChooseSchedule(string scheduleCode,int pageSize, int pageNumber)
		{
			return Ok(_userServices.ChooseSchedule(scheduleCode, pageSize, pageNumber));
		}

		[HttpPost("/ChooseCinema")]
		public IActionResult ChooseCinema(string cinemaCode, int pageSize, int pageNumber)
		{
			return Ok(_userServices.ChooseCinema(cinemaCode, pageSize, pageNumber));
		}

		[HttpPost("/ChooseRoom")]
		public IActionResult ChooseRoom(string cinemaCode, string roomCode, string scheduleCode, int pageSize, int pageNumber)
		{
			return Ok(_userServices.ChooseRoom(cinemaCode, roomCode, scheduleCode, pageSize, pageNumber));
		}

		[HttpPost("/ChooseSeats")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public IActionResult ChooseSeats(DataRequest_ChooseSeats request, int pageSize, int pageNumber)
		{
			int id = int.Parse(HttpContext.User.FindFirst("Id").Value);
			return Ok(_userServices.ChooseSeats(id,request, pageSize, pageNumber));
		}

		[HttpPost("/ChooseFood")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public IActionResult ChooseFood(List<DataRequest_ChooseFood> requests, int pageSize, int pageNumber)
		{
			int id = int.Parse(HttpContext.User.FindFirst("Id").Value);
			return Ok(_userServices.ChooseFood(id, requests,pageSize,pageNumber));
		}

		[HttpGet("/ConfirmBill")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public IActionResult ConfirmBill()
		{
			int id = int.Parse(HttpContext.User.FindFirst("Id").Value);
			return Ok(_userServices.ConfirmBill(id));
		}

		[HttpPost("/GetPayment")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public IActionResult PayTheBill()
		{
			int id = int.Parse(HttpContext.User.FindFirst("Id").Value);
			return Ok(_userServices.PayForBill(HttpContext,id));
		}

	}
}
