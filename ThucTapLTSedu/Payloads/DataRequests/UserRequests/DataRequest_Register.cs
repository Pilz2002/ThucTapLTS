using System.ComponentModel.DataAnnotations;
using ThucTapLTSedu.Entities;

namespace ThucTapLTSedu.Payloads.DataRequests.UserRequests
{
	public class DataRequest_Register
	{
		public string Username { get; set; }
		public string Email { get; set; }
		public string Name { get; set; }
		public string PhoneNumber { get; set; }
		public string Password { get; set; }
		public string ConfirmPassword { get; set; }

	}
}
