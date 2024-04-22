using ThucTapLTSedu.Entities;

namespace ThucTapLTSedu.Payloads.DataResponses.UserResponses
{
	public class DataResponse_Ticket
	{
		public string Code { get; set; }
		public string ScheduleName { get; set; }
		public string SeatNumber { get; set; }
		public string SeatLine { get; set; }
		public double PriceTicket { get; set; }
	}
}
