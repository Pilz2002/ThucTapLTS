namespace ThucTapLTSedu.Handler.Validation
{
	public class PasswordValidation
	{
		public static bool IsStrongPass(string password)
		{
			if (password.Length < 8)
			{
				return false;
			}
			if (!password.Any(char.IsDigit))
			{
				return false;
			}
			if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
			{
				return false;
			}
			return true;
		}
	}
}
