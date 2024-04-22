using ThucTapLTSedu.Entities;

namespace ThucTapLTSedu.Payloads.DataRequests.AdminRequests
{
	public class DataRequest_AddSchedule
	{
		public string Name { get; set; }
		public string Code { get; set; }
		public double Price { get; set; }
		public DateTime StartAt { get; set; }
		public DateTime EndAt { get; set; }
		public int MovieId { get; set; }
		public int RoomId { get; set; }
	}
}
