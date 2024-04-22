using ThucTapLTSedu.Handler.Paging;
using ThucTapLTSedu.Payloads.DataRequests.UserRequests;
using ThucTapLTSedu.Payloads.DataRequests.VnPayRequest;
using ThucTapLTSedu.Payloads.DataResponses.AdminResponses;
using ThucTapLTSedu.Payloads.DataResponses.UserResponses;
using ThucTapLTSedu.Payloads.DataResponses.VnPayResponse;
using ThucTapLTSedu.Payloads.Responses;

namespace ThucTapLTSedu.Services.Interfaces
{
	public interface IUserServices
	{
		public PageResult<DataResponse_Movie> GetMovieByCinema(string cinemaCode, int pageSize, int pageNumber);
		public PageResult<DataResponse_Seat> GetSeatByRoom(string cinemaCode, string roomCode,int pageSize, int pageNumber);
		public PageResult<DataResponse_Schedule> ChooseMovie(string movieName, int pageSize, int pageNumber);
		public PageResult<DataResponses_Cinema> ChooseSchedule(string scheduleCode, int pageSize, int pageNumber);
		public PageResult<DataResponse_Room> ChooseCinema(string cinemaCode, int pageSize, int pageNumber);
		public PageResult<DataResponse_Seat> ChooseRoom(string cinemaCode ,string roomCode, string scheduleCode, int pageSize, int pageNumber);
		public PageResult<DataResponse_Ticket> ChooseSeats(int userId, DataRequest_ChooseSeats request,int pageSize, int pageNumber);
		public PageResult<DataResponse_BillFood> ChooseFood(int userId, List<DataRequest_ChooseFood> requests, int pageSize,int pageNumber);
		public ResponseObject<DataResponse_Bill> ConfirmBill(int userId);
		public string PayForBill(HttpContext httpContext, int userId);
		public DataResponse_Vnpay PaymentExecute(IQueryCollection collections);
	}
}
