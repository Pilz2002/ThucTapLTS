using ThucTapLTSedu.Entities;
using ThucTapLTSedu.Payloads.DataResponses.AdminResponses;

namespace ThucTapLTSedu.Payloads.Converter.CinemaConverter
{
	public class SeatStatus_Converter
	{
		public DataResponse_SeatStatus SeatStatusDTO(SeatStatus seatStatus)
		{
			return new DataResponse_SeatStatus
			{
				Code = seatStatus.Code,
				NameStatus = seatStatus.NameStatus
			};
		}
	}
}
