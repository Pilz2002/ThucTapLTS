using ThucTapLTSedu.Entities;

namespace ThucTapLTSedu.Payloads.DataRequests.AdminRequests
{
	public class DataRequest_UpdateFood
	{
		public double Price { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
		public string NameOfFood { get; set; }
		public bool IsActive { get; set; }
	}
}
