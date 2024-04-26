using ThucTapLTSedu.Entities;

namespace ThucTapLTSedu.Payloads.DataRequests.AdminRequests
{
	public class DataRequest_AddRankCustomer
	{
		public int Point { get; set; }
		public string Description { get; set; }
		public string Name { get; set; }
	}
}
