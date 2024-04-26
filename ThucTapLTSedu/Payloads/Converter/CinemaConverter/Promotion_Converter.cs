using ThucTapLTSedu.DataContext;
using ThucTapLTSedu.Entities;
using ThucTapLTSedu.Payloads.DataResponses.UserResponses;

namespace ThucTapLTSedu.Payloads.Converter.CinemaConverter
{
	public class Promotion_Converter
	{
		private readonly AppDbContext _context;

		public Promotion_Converter(AppDbContext context)
		{
			_context = context;
		}

		public DataResponse_Promotion PromotionDTO(Promotion promotion)
		{
			var rankCustomer = _context.RankCustomers.FirstOrDefault(x => x.Id == promotion.RankCustomerId);

			return new DataResponse_Promotion
			{
				Description = promotion.Description,
				StartTime = promotion.StartTime,
				EndTime = promotion.EndTime,
				Name = promotion.Name,
				Percent = promotion.Percent,
				IsActive = promotion.IsActive,
				Quantity = promotion.Quantity,
				Type = promotion.Type,
				RankCustomer = rankCustomer.Name
			};
		}
	}
}
