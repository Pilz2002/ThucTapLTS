using ThucTapLTSedu.DataContext;
using ThucTapLTSedu.Entities;
using ThucTapLTSedu.Payloads.DataResponses.AdminResponses;

namespace ThucTapLTSedu.Payloads.Converter.CinemaConverter
{
	public class Movie_Converter
	{
		private readonly AppDbContext _context;

		public Movie_Converter(AppDbContext context)
		{
			_context = context;
		}

		public DataResponse_Movie MovieDTO(Movie movie)
		{
			var movieType = _context.MovieTypes.FirstOrDefault(x => x.Id == movie.MovieTypeId).MovieTypeName;
			var rate = _context.Rates.FirstOrDefault(x => x.Id == movie.RateId).Description;
			return new DataResponse_Movie
			{
				Description = movie.Description,
				Director = movie.Director,
				EndTime = movie.EndTime,
				HeroImage = movie.HeroImage,
				Image = movie.Image,
				IsActive = movie.IsActive,
				Language = movie.Language,
				MovieDuration = movie.MovieDuration,
				MovieTypeName = movieType,
				RateDescription = rate,
				Name = movie.Name,
				PremiereDate = movie.PremiereDate,
				Trailer = movie.Trailer
			};
		}

		

	}
}
