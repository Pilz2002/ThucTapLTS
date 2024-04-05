using System.ComponentModel.DataAnnotations;

namespace ThucTapLTSedu.Handler.Validation
{
	public class EmailValidation
	{
		public static bool IsValidEmail(string email)
		{
			var emailAttribute = new EmailAddressAttribute();
			return emailAttribute.IsValid(email);
		}
	}
}
