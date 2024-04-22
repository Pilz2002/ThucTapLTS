using ThucTapLTSedu.Entities;

namespace ThucTapLTSedu.Payloads.DataRequests.AdminRequests
{
	public class DataRequest_AddSeat
	{
		public int Number { get; set; }
		public int SeatStatusId { get; set; }
		public string Line { get; set; }
		public int SeatTypeId { get; set; }
	}
}
