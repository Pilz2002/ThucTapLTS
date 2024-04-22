namespace ThucTapLTSedu.Payloads.DataRequests.UserRequests
{
	public class DataRequest_ChooseSeats
	{
		public List<int> seatIds { get; set; }
		public string ScheduleCode { get; set; }
	}
}
