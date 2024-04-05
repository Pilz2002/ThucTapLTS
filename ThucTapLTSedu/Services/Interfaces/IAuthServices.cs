using ThucTapLTSedu.Entities;
using ThucTapLTSedu.Payloads.DataRequests.UserRequests;
using ThucTapLTSedu.Payloads.DataResponses.AuthResponses;
using ThucTapLTSedu.Payloads.DataResponses.UserResponses;
using ThucTapLTSedu.Payloads.Responses;

namespace ThucTapLTSedu.Services.Interfaces
{
	public interface IAuthServices
	{
		public ResponseObject<DataResponse_Token> RenewAccessToken(DataRequest_Token request);
		public ResponseObject<DataResponse_Token> Login(DataRequest_Login request);
		public ResponseObject<DataResponse_User> Register(DataRequest_Register request);
		public ResponseObject<DataResponse_User> VerifyEmail(DataRequest_VerifyEmail request,int userId);
		public ResponseObject<string> ForgotPassword(DataRequest_ForgotPassword request);
		public ResponseObject<string> ChangePassword(int userId, DataRequest_ChangePassword request);
		public ResponseObject<string> GetCode(int userId);
	}
}

