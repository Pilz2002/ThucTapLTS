using ThucTapLTSedu.Entities;
using ThucTapLTSedu.Payloads.DataResponses.AdminResponses;

namespace ThucTapLTSedu.Payloads.Converter.CinemaConverter
{
	public class RankCustomer_Converter
	{
		public DataResponse_RankCustomer RankCustomerDTO(RankCustomer rankCustomer)
		{
			return new DataResponse_RankCustomer
			{
				Description = rankCustomer.Description,
				IsActive = rankCustomer.IsActive,
				Name = rankCustomer.Name,
				Point = rankCustomer.Point
			};
		}
	}
}
