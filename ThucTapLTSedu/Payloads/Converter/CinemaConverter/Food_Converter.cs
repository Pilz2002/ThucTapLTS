using ThucTapLTSedu.Entities;
using ThucTapLTSedu.Payloads.DataResponses.AdminResponses;

namespace ThucTapLTSedu.Payloads.Converter.CinemaConverter
{
	public class Food_Converter
	{
		public DataResponse_Food FoodDTO(Food food)
		{
			return new DataResponse_Food
			{
				Description = food.Description,
				Image = food.Image,
				NameOfFood = food.NameOfFood,
				Price = food.Price,
				IsActive = food.IsActive
			};
		}
	}
}
