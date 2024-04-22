namespace ThucTapLTSedu.Payloads.DataRequests.AdminRequests
{
	public class DataRequest_UpdateCinema
	{
		public string Address { get; set; }
		public string Description { get; set; }
		public string NameOfCinema { get; set; }
		public bool IsActive { get; set; }
	}
}
