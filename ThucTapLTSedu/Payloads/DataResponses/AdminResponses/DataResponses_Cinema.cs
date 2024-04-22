using ThucTapLTSedu.Entities;

namespace ThucTapLTSedu.Payloads.DataResponses.AdminResponses
{
	public class DataResponses_Cinema
	{
		public string Address { get; set; }
		public string Description { get; set; }
		public string Code { get; set; }
		public string NameOfCinema { get; set; }
		public bool IsActive { get; set; }
		public List<DataResponse_Room> Rooms { get; set; }
	}
}
