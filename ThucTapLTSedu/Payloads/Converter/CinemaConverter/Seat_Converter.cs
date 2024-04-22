using System.Security.Cryptography.Xml;
using ThucTapLTSedu.DataContext;
using ThucTapLTSedu.Entities;
using ThucTapLTSedu.Payloads.DataResponses.AdminResponses;

namespace ThucTapLTSedu.Payloads.Converter.CinemaConverter
{
    public class Seat_Converter
    {
        private readonly AppDbContext _context;

        public Seat_Converter(AppDbContext context)
        {
            _context = context;
        }

        public DataResponse_Seat SeatDTO(Seat seat)
        {
            var room = _context.Rooms.FirstOrDefault(x => x.Id == seat.RoomId);
            var seatType = _context.SeatTypes.FirstOrDefault(x => x.Id == seat.SeatTypeId).NameType;
            var seatStatus = _context.SeatStatus.FirstOrDefault(x => x.Id == seat.SeatStatusId).NameStatus;
            var cinemaName = _context.Cinemas.FirstOrDefault(x => x.Id == room.CinemaId).NameOfCinema;
            return new DataResponse_Seat
            {
                IsActive = seat.IsActive,
                Line = seat.Line,
                Number = seat.Number,
                RoomName = room.Name,
                SeatStatusName = seatStatus,
                SeatTypeName = seatType,
                CinemaName = cinemaName,
                SeatId = seat.Id
            };
        }
    }
}
