using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThucTapLTSedu.Payloads.DataRequests.AdminRequests;
using ThucTapLTSedu.Services.Interfaces;

namespace ThucTapLTSedu.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AdminController : ControllerBase
	{
		private readonly IAdminServices _adminServices;

		public AdminController(IAdminServices adminServices)
		{
			_adminServices = adminServices;
		}

		[HttpPost("/Admin/AddCinema")]
		[Authorize(Roles = "Admin")]
		public IActionResult AddCinema([FromBody]DataRequest_AddCinema cinemaRequest)
		{
			return Ok(_adminServices.AddCinema(cinemaRequest));
		}

		[HttpPost("/Admin/AddFood")]
		[Authorize(Roles = "Admin")]
		public IActionResult AddFood(DataRequest_AddFood request)
		{
			return Ok(_adminServices.AddFood(request));
		}

		[HttpPut("/Admin/UpdateCinema")]
		[Authorize(Roles = "Admin")]
		public IActionResult UpdateCinema(int cinemaId, DataRequest_UpdateCinema request)
		{
			return Ok(_adminServices.UpdateCinema(cinemaId,request));
		}

		[HttpDelete("/Admin/DeleteCinema")]
		[Authorize(Roles = "Admin")]
		public IActionResult DeleteCinema(int cinemaId)
		{
			return Ok(_adminServices.DeleteCinema(cinemaId));
		}

		[HttpPut("/Admin/UpdateRoom")]
		[Authorize(Roles = "Admin")]
		public IActionResult UpdateRoom(int roomId, DataRequest_UpdateRoom request)
		{
			return Ok(_adminServices.UpdateRoom(roomId,request));
		}

		[HttpDelete("/Admin/DeleteRoom")]
		[Authorize(Roles = "Admin")]
		public IActionResult DeleteRoom(int roomId)
		{
			return Ok(_adminServices.DeleteRoom(roomId));
		}

		[HttpPut("/Admin/UpdateSeat")]
		[Authorize(Roles = "Admin")]
		public IActionResult UpdateSeat(int seatId, DataRequest_UpdateSeat request)
		{
			return Ok(_adminServices.UpdateSeat(seatId,request));
		}

		[HttpDelete("/Admin/DeleteSeat")]
		[Authorize(Roles = "Admin")]
		public IActionResult DeleteSeat(int seatId)
		{
			return Ok(_adminServices.DeleteSeat(seatId));
		}

		[HttpPut("/Admin/UpdateFood")]
		[Authorize(Roles = "Admin")]
		public IActionResult UpdateFood(int foodId, DataRequest_UpdateFood request)
		{
			return Ok(_adminServices.UpdateFood(foodId, request));
		}

		[HttpDelete("/Admin/DeleteFood")]
		[Authorize(Roles = "Admin")]
		public IActionResult DeleteFood(int foodId)
		{
			return Ok(_adminServices.DeleteFood(foodId));
		}

		[HttpPost("/Admin/AddMovie")]
		[Authorize(Roles = "Admin")]
		public IActionResult AddMovie(DataRequest_AddMovie request)
		{
			return Ok(_adminServices.AddMovie(request));
		}

		[HttpPut("/Admin/UpdateMovie")]
		[Authorize(Roles = "Admin")]
		public IActionResult UpdateMovie(int movieId, DataRequest_UpdateMovie request)
		{
			return Ok(_adminServices.UpdateMovie(movieId, request));
		}

		[HttpDelete("/Admin/DeleteMovie")]
		[Authorize(Roles = "Admin")]
		public IActionResult DeleteMovie(int movieId)
		{
			return Ok(_adminServices.DeleteMovie(movieId));
		}

		[HttpPost("/Admin/AddSchedule")]
		[Authorize(Roles = "Admin")]
		public IActionResult AddSchedule(DataRequest_AddSchedule request)
		{
			return Ok(_adminServices.AddSchedule(request));
		}

		[HttpPut("/Admin/UpdateSchedule")]
		[Authorize(Roles = "Admin")]
		public IActionResult UpdateSchedule(int scheduleId, DataRequest_UpdateSchedule request)
		{
			return Ok(_adminServices.UpdateSchedule(scheduleId,request));
		}

		[HttpDelete("/Admin/DeleteSchedule")]
		[Authorize(Roles = "Admin")]
		public IActionResult DeleteSchedule(int scheduleId)
		{
			return Ok(_adminServices.DeleteSchedule(scheduleId));
		}

		[HttpPost("/Admin/AddSeatType")]
		[Authorize(Roles = "Admin")]
		public IActionResult AddSeatType(DataRequest_AddSeatType request)
		{
			return Ok(_adminServices.AddSeatType(request));
		}

		[HttpPut("/Admin/UpdateSeatType")]
		[Authorize(Roles = "Admin")]
		public IActionResult UpdateSeatType(int id, string nameType)
		{
			return Ok(_adminServices.UpdateSeatType(id, nameType));
		}

		[HttpDelete("/Admin/DeleteSeatType")]
		[Authorize(Roles = "Admin")]
		public IActionResult DeleteSeatType(int id)
		{
			return Ok(_adminServices.DeleteSeatType(id));
		}

		[HttpPost("/Admin/AddSeatStatus")]
		[Authorize(Roles = "Admin")]
		public IActionResult AddSeatStatus(DataRequest_AddSeatStatus request)
		{
			return Ok(_adminServices.AddSeatStatus(request));
		}

		[HttpPut("/Admin/UpdateSeatStatus")]
		[Authorize(Roles = "Admin")]
		public IActionResult UpdateSeatStatus(DataRequest_UpdateSeatStatus request)
		{
			return Ok(_adminServices.UpdateSeatStatus(request));
		}

		[HttpDelete("/Admin/DeleteSeatStatus")]
		[Authorize(Roles = "Admin")]
		public IActionResult DeleteSeatStatus(int id)
		{
			return Ok(_adminServices.DeleteSeatStatus(id));
		}

		[HttpGet("/Admin/FoodBestSeller")]
		[Authorize(Roles = "Admin")]
		public IActionResult FoodBestSeller()
		{
			return Ok(_adminServices.FoodBestSeller());
		}

		[HttpGet("/Admin/GetCinemasSales")]
		[Authorize(Roles = "Admin")]
		public IActionResult GetCinemasSales([FromQuery]int pageSize, int pageNumber)
		{
			return Ok(_adminServices.GetCinemasSales(pageSize,pageNumber));
		}

		[HttpPost("/Admin/PromoteUserRole")]
		[Authorize(Roles = "Admin")]
		public IActionResult PromoteUserRole(string userName , string codeRole)
		{
			return Ok(_adminServices.PromoteUserRole(userName, codeRole));
		}

		[HttpPost("/Admin/AddPromotion")]
		[Authorize(Roles = "Admin")]
		public IActionResult AddPromotion(DataRequest_AddPromotion request)
		{
			return Ok(_adminServices.AddPromotion(request));
		}
	}
}
