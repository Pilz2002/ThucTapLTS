using ThucTapLTSedu.Entities;

namespace ThucTapLTSedu.Payloads.DataResponses.AdminResponses
{
	public class DataResponse_Seat
	{
		public string CinemaName { get; set; }
		public string RoomName { get; set; }
		public string Line { get; set; }
		public int Number { get; set; }
		public string SeatStatusName { get; set; }
		public string SeatTypeName { get; set; }
		public bool IsActive { get; set; }
		public int SeatId { get; set; }
	}
}
