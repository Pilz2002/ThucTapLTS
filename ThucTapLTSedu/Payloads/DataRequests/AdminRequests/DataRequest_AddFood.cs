using ThucTapLTSedu.Entities;

namespace ThucTapLTSedu.Payloads.DataRequests.AdminRequests
{
	public class DataRequest_AddFood
	{
		public double Price { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
		public string NameOfFood { get; set; }
	}
}
