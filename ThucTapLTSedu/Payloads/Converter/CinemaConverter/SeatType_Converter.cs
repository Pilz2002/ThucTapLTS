using ThucTapLTSedu.Entities;
using ThucTapLTSedu.Payloads.DataResponses.AdminResponses;

namespace ThucTapLTSedu.Payloads.Converter.CinemaConverter
{
	public class SeatType_Converter
	{
		public DataResponse_SeatType SeatTypeDTO(SeatType seatType)
		{
			return new DataResponse_SeatType
			{
				NameType = seatType.NameType
			};
		}
	}
}
