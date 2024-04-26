namespace ThucTapLTSedu.Payloads.DataRequests.AdminRequests
{
	public class DataRequest_UpdateRankCustomer
	{
		public int RankCustomerId { get; set; }
		public int Point { get; set; }
		public string Description { get; set; }
		public string Name { get; set; }
		public bool IsActive { get; set; }
	}
}
