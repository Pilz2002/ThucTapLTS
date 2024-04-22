using ThucTapLTSedu.Handler.Paging;
using ThucTapLTSedu.Payloads.DataRequests.AdminRequests;
using ThucTapLTSedu.Payloads.DataResponses.AdminResponses;
using ThucTapLTSedu.Payloads.Responses;

namespace ThucTapLTSedu.Services.Interfaces
{
	public interface IAdminServices
	{
		public ResponseObject<DataResponses_Cinema> AddCinema(DataRequest_AddCinema cinemaRequest);
		public ResponseObject<DataResponses_Cinema> UpdateCinema(int cinemaId, DataRequest_UpdateCinema request);
		public ResponseObject<DataResponses_Cinema> DeleteCinema(int cinemaId);

		public ResponseObject<DataResponse_Room> UpdateRoom(int roomId, DataRequest_UpdateRoom request);
		public ResponseObject<DataResponse_Room> DeleteRoom(int roomId);

		public ResponseObject<DataResponse_Seat> UpdateSeat(int seatId, DataRequest_UpdateSeat request);
		public ResponseObject<DataResponse_Seat> DeleteSeat(int seatId);

		public ResponseObject<DataResponse_Food> AddFood(DataRequest_AddFood request);
		public ResponseObject<DataResponse_Food> UpdateFood(int foodId,DataRequest_UpdateFood request);
		public ResponseObject<DataResponse_Food> DeleteFood(int foodId);

		public ResponseObject<DataResponse_Movie> AddMovie(DataRequest_AddMovie request);
		public ResponseObject<DataResponse_Movie> UpdateMovie(int movieId,DataRequest_UpdateMovie request);
		public ResponseObject<DataResponse_Movie> DeleteMovie(int movieId);

		public ResponseObject<DataResponse_Schedule> AddSchedule(DataRequest_AddSchedule request);
		public ResponseObject<DataResponse_Schedule> UpdateSchedule(int scheduleId, DataRequest_UpdateSchedule request);
		public ResponseObject<DataResponse_Schedule> DeleteSchedule(int scheduleId);


	}
}
