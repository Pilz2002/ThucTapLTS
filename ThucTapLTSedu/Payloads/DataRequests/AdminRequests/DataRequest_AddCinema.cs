using ThucTapLTSedu.Entities;

namespace ThucTapLTSedu.Payloads.DataRequests.AdminRequests
{
	public class DataRequest_AddCinema
	{
		public string Address { get; set; }
		public string Description { get; set; }
		public string Code { get; set; }
		public string NameOfCinema { get; set; }
		public required List<DataRequest_AddRoom> Rooms { get; set; }
	}
}
