using ThucTapLTSedu.DataContext;
using ThucTapLTSedu.Entities;
using ThucTapLTSedu.Payloads.DataResponses.UserResponses;

namespace ThucTapLTSedu.Payloads.Converter.CinemaConverter
{
	public class BillFood_Converter
	{
		private readonly AppDbContext _context;

		public BillFood_Converter(AppDbContext context)
		{
			_context = context;
		}

		public DataResponse_BillFood BillFoodDTO(BillFood billFood)
		{
			var bill = _context.Bills.FirstOrDefault(x => x.Id == billFood.BillId);
			var food = _context.Foods.FirstOrDefault(x => x.Id == billFood.FoodId);
			return new DataResponse_BillFood
			{
				Quantity = billFood.Quantity,
				BillName = bill.Name,
				FoodName = food.NameOfFood
			};
		}
	}
}
