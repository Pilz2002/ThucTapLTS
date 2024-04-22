using ThucTapLTSedu.Entities;

namespace ThucTapLTSedu.Payloads.DataResponses.AdminResponses
{
	public class DataResponse_Schedule
	{
		public double Price { get; set; }
		public DateTime StartAt { get; set; }
		public DateTime EndAt { get; set; }
		public string MovieName { get; set; }
		public string Name { get; set; }
		public string RoomName { get; set; }
		public bool IsActive { get; set; }
		public string Code { get; set; }
	}
}
