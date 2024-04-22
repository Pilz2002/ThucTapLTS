using ThucTapLTSedu.Entities;

namespace ThucTapLTSedu.Payloads.DataResponses.AdminResponses
{
	public class DataResponse_Room
	{
		public int Capacity { get; set; }
		public string Type { get; set; }
		public string Description { get; set; }
		public string TenRap { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public bool IsActive { get; set; }
		public List<DataResponse_Seat> Seats { get; set; }
	}
}
