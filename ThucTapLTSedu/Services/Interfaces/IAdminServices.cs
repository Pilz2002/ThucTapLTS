using ThucTapLTSedu.Handler.Paging;
using ThucTapLTSedu.Payloads.DataRequests.AdminRequests;
using ThucTapLTSedu.Payloads.DataResponses.AdminResponses;
using ThucTapLTSedu.Payloads.DataResponses.UserResponses;
using ThucTapLTSedu.Payloads.Responses;

namespace ThucTapLTSedu.Services.Interfaces
{
    public interface IAdminServices
	{
		// CRUD cinema
		public ResponseObject<DataResponses_Cinema> AddCinema(DataRequest_AddCinema cinemaRequest);
		public ResponseObject<DataResponses_Cinema> UpdateCinema(int cinemaId, DataRequest_UpdateCinema request);
		public ResponseObject<DataResponses_Cinema> DeleteCinema(int cinemaId);

		//CRUD room
		public ResponseObject<DataResponse_Room> UpdateRoom(int roomId, DataRequest_UpdateRoom request);
		public ResponseObject<DataResponse_Room> DeleteRoom(int roomId);

		//CRUD seat status
		public ResponseObject<DataResponse_SeatStatus> AddSeatStatus(DataRequest_AddSeatStatus request);
		public ResponseObject<DataResponse_SeatStatus> UpdateSeatStatus(DataRequest_UpdateSeatStatus request);
		public ResponseObject<DataResponse_SeatStatus> DeleteSeatStatus(int id);

		//CRUD seat type
		public ResponseObject<DataResponse_SeatType> AddSeatType(DataRequest_AddSeatType request);
		public ResponseObject<DataResponse_SeatType> UpdateSeatType(int id, string nameType);
		public ResponseObject<DataResponse_SeatType> DeleteSeatType(int id);

		// CRUD seat
		public ResponseObject<DataResponse_Seat> UpdateSeat(int seatId, DataRequest_UpdateSeat request);
		public ResponseObject<DataResponse_Seat> DeleteSeat(int seatId);

		//CRUD food
		public ResponseObject<DataResponse_Food> AddFood(DataRequest_AddFood request);
		public ResponseObject<DataResponse_Food> UpdateFood(int foodId,DataRequest_UpdateFood request);
		public ResponseObject<DataResponse_Food> DeleteFood(int foodId);

		//CRUD movie
		public ResponseObject<DataResponse_Movie> AddMovie(DataRequest_AddMovie request);
		public ResponseObject<DataResponse_Movie> UpdateMovie(int movieId,DataRequest_UpdateMovie request);
		public ResponseObject<DataResponse_Movie> DeleteMovie(int movieId);

		//CRUD schedule
		public ResponseObject<DataResponse_Schedule> AddSchedule(DataRequest_AddSchedule request);
		public ResponseObject<DataResponse_Schedule> UpdateSchedule(int scheduleId, DataRequest_UpdateSchedule request);
		public ResponseObject<DataResponse_Schedule> DeleteSchedule(int scheduleId);

		//CRUD rank customer
		public ResponseObject<DataResponse_RankCustomer> AddRankCustomer(DataRequest_AddRankCustomer request);
		public ResponseObject<DataResponse_RankCustomer> UpdateRankCustomer(DataRequest_UpdateRankCustomer request);
		public ResponseObject<DataResponse_RankCustomer> DeleteRankCustomer(int id);

		//CRUD promotion
		public ResponseObject<DataResponse_Promotion> AddPromotion(DataRequest_AddPromotion request);

		//thống kê đồ ăn bán chạy
		public ResponseObject<DataResponse_Food> FoodBestSeller();
		//thống kê doanh số từng rạp
		public PageResult<DataResponse_CinemaSales> GetCinemasSales(int pageSize, int pageNumber);
		//Quản lý thông tin người dùng
		public ResponseObject<DataResponse_User> PromoteUserRole(string userName, string codeRole);
	}
}
