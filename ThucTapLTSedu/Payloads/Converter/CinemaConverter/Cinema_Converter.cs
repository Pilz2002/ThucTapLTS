using ThucTapLTSedu.Entities;
using ThucTapLTSedu.Payloads.DataResponses.AdminResponses;

namespace ThucTapLTSedu.Payloads.Converter.CinemaConverter
{
    public class Cinema_Converter
    {
        public readonly Room_Converter _roomConverter;

		public Cinema_Converter(Room_Converter roomConverter)
		{
			_roomConverter = roomConverter;
		}

		public DataResponses_Cinema CinemaDTO(Cinema cinema)
        {
            return new DataResponses_Cinema
            {
                Address = cinema.Address,
                Code = cinema.Code,
                Description = cinema.Description,
                NameOfCinema = cinema.NameOfCinema,
                IsActive = cinema.IsActive,
                Rooms = cinema.Rooms.Select(x=>_roomConverter.RoomDTO(x)).ToList()
            };
        }
    }
}
