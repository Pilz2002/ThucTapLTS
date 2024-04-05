using ThucTapLTSedu.DataContext;
using ThucTapLTSedu.Entities;
using ThucTapLTSedu.Payloads.DataResponses.UserResponses;

namespace ThucTapLTSedu.Payloads.Converter.UserConverter
{
	public class User_Converter
	{
		private readonly AppDbContext _context;

		public User_Converter(AppDbContext context)
		{
			_context = context;
		}

		public DataResponse_User UserDTO(User user)
		{
			return new DataResponse_User
			{
				Email = user.Email,
				Name = user.Name,
				PhoneNumber = user.PhoneNumber,
				Username = user.Username,
				Status = _context.UserStatuses.FirstOrDefault(x => x.Id == user.UserStatusId)?.Name ?? "" 
			};
		}
	}
}
