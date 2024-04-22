using ThucTapLTSedu.DataContext;
using ThucTapLTSedu.Entities;
using ThucTapLTSedu.Payloads.DataResponses.UserResponses;

namespace ThucTapLTSedu.Payloads.Converter.CinemaConverter
{
	public class BIll_Converter
	{
		private readonly AppDbContext _context;

		public BIll_Converter(AppDbContext context)
		{
			_context = context;
		}

		public DataResponse_Bill BillDTO(Bill bill)
		{
			var billStatus = _context.BillStatuses.FirstOrDefault(x => x.Id == bill.BillStatusId);
			var promotion = _context.Promotions.FirstOrDefault(x => x.Id == bill.PromotionId);
			var user = _context.Users.FirstOrDefault(x => x.Id == bill.CustomerId);
			return new DataResponse_Bill
			{
				BillStatusName = billStatus.Name,
				CreateTime = bill.CreateTime,
				IsActive = bill.IsActive,
				Name = bill.Name,
				PromotionName = promotion?.Name,
				TotalMoney = bill.TotalMoney,
				TradingCode = bill.TradingCode,
				UpdateTime = bill.UpdateTime,
				Username = user.Name
			};
		}
	}
}
