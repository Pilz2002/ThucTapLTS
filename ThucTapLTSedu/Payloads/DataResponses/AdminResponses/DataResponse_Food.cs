using ThucTapLTSedu.Entities;

namespace ThucTapLTSedu.Payloads.DataResponses.AdminResponses
{
	public class DataResponse_Food
	{
		public double Price { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
		public string NameOfFood { get; set; }
		public bool IsActive { get; set; }
	}
}
