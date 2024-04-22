using ThucTapLTSedu.Entities;

namespace ThucTapLTSedu.Payloads.DataRequests.AdminRequests
{
	public class DataRequest_AddRoom
	{
		public int Capacity { get; set; }
		public string Type { get; set; }
		public string Description { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public required List<DataRequest_AddSeat> Seats { get; set; }
	}
}
