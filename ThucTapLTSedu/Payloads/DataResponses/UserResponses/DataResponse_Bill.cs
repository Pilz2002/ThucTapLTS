using ThucTapLTSedu.Entities;

namespace ThucTapLTSedu.Payloads.DataResponses.UserResponses
{
	public class DataResponse_Bill
	{
		public double TotalMoney { get; set; }
		public string TradingCode { get; set; }
		public DateTime CreateTime { get; set; }
		public string Username { get; set; }
		public string Name { get; set; }
		public DateTime UpdateTime { get; set; }
		public string PromotionName { get; set; }
		public string BillStatusName { get; set; }
		public bool IsActive { get; set; }
	}
}
