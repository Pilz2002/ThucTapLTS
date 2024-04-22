namespace ThucTapLTSedu.Payloads.DataRequests.VnPayRequest
{
	public class OrderInfor
	{
		public long OrderId { get; set; }
		public long Amount { get; set; }
		public DateTime CreatedDate { get; set; }
		public string Status { get; set; }
	}
}
