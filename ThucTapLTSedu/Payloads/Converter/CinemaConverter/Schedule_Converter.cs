using ThucTapLTSedu.DataContext;
using ThucTapLTSedu.Entities;
using ThucTapLTSedu.Payloads.DataResponses.AdminResponses;

namespace ThucTapLTSedu.Payloads.Converter.CinemaConverter
{
	public class Schedule_Converter
	{
		private readonly AppDbContext _context;

		public Schedule_Converter(AppDbContext context)
		{
			_context = context;
		}

		public DataResponse_Schedule ScheduleDTO(Schedule schedule)
		{
			var movieName = _context.Movies.FirstOrDefault(x => x.Id == schedule.MovieId).Name;
			var roomName = _context.Rooms.FirstOrDefault(x => x.Id == schedule.RoomId).Name;
			return new DataResponse_Schedule
			{
				Name = schedule.Name,
				IsActive = schedule.IsActive,
				Price = schedule.Price,
				MovieName = movieName,
				RoomName = roomName,
				StartAt = schedule.StartAt,
				EndAt = schedule.EndAt,
				Code = schedule.Code
			};
		}
	}
}
