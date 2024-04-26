using ThucTapLTSedu.DataContext;
using ThucTapLTSedu.Payloads.DataResponses.AdminResponses;

namespace ThucTapLTSedu.Payloads.Converter.CinemaConverter
{
	public class CinemaSales_Converter
	{
		private readonly AppDbContext _context;

		public CinemaSales_Converter(AppDbContext context)
		{
			_context = context;
		}

		public DataResponse_CinemaSales CinemaSale(int cinemaId, double totalMoney)
		{
			var cinema = _context.Cinemas.FirstOrDefault(x => x.Id == cinemaId);
			return new DataResponse_CinemaSales
			{
				NameOfCinemas = cinema.NameOfCinema,
				CodeOfCinemas = cinema.Code,
				TotalMoney = totalMoney
			};
		}
	}
}
