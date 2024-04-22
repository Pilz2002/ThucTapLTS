using ThucTapLTSedu.DataContext;
using ThucTapLTSedu.Entities;
using ThucTapLTSedu.Payloads.DataResponses.AdminResponses;

namespace ThucTapLTSedu.Payloads.Converter.CinemaConverter
{
    public class Room_Converter
    {
        private readonly AppDbContext _context;
        private readonly Seat_Converter _seatConverter;

        public Room_Converter(AppDbContext context, Seat_Converter seatConverter)
        {
            _context = context;
            _seatConverter = seatConverter;
        }

        public DataResponse_Room RoomDTO(Room room)
        {
            return new DataResponse_Room
            {
                Capacity = room.Capacity,
                Code = room.Code,
                Description = room.Code,
                IsActive = room.IsActive,
                Name = room.Name,
                Type = room.Type,
                TenRap = _context.Cinemas.FirstOrDefault(x => x.Id == room.CinemaId).NameOfCinema,
                Seats = room.Seats?.Select(x=>_seatConverter.SeatDTO(x)).ToList()
            };
        }
    }
}
