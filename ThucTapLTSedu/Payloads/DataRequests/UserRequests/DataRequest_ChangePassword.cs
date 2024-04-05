namespace ThucTapLTSedu.Payloads.DataRequests.UserRequests
{
	public class DataRequest_ChangePassword
	{
		public string OldPassword { get; set; }
		public string NewPassword { get; set; }
		public string ConfirmPassword { get; set; }
	}
}
