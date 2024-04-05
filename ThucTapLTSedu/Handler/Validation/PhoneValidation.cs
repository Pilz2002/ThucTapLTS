using System.Text.RegularExpressions;

namespace ThucTapLTSedu.Handler.Validation
{
	public class PhoneValidation
	{
		public static bool IsValidPhoneNumber(string phoneNumber)
		{
			string pattern = @"^(84|0[35789])[0-9]{8}$";
			return Regex.IsMatch(phoneNumber, pattern);
		}
	}
}
