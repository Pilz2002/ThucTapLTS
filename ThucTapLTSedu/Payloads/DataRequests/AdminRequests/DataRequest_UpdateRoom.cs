using ThucTapLTSedu.Entities;

namespace ThucTapLTSedu.Payloads.DataRequests.AdminRequests
{
	public class DataRequest_UpdateRoom
	{
		public int Capacity { get; set; }
		public string Type { get; set; }
		public string Description { get; set; }
		public string Name { get; set; }
		public bool IsActive { get; set; }
	}
}
