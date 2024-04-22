using ThucTapLTSedu.DataContext;
using ThucTapLTSedu.Entities;
using ThucTapLTSedu.Payloads.DataResponses.UserResponses;

namespace ThucTapLTSedu.Payloads.Converter.CinemaConverter
{
	public class Ticket_Converter
	{
		private readonly AppDbContext _context;

		public Ticket_Converter(AppDbContext context)
		{
			_context = context;
		}

		public DataResponse_Ticket TicketDTO(Ticket ticket)
		{
			var scheduleName = _context.Schedules.FirstOrDefault(x => x.Id == ticket.ScheduleId).Name;
			var seat = _context.Seats.FirstOrDefault(x => x.Id == ticket.SeatId);
			return new DataResponse_Ticket
			{
				Code = ticket.Code,
				PriceTicket = ticket.PriceTicket,
				ScheduleName = scheduleName,
				SeatLine = seat.Line,
				SeatNumber = seat.Number.ToString()
			};
		}
	}
}
