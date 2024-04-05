namespace ThucTapLTSedu.Payloads.Responses
{
	public class ResponseObject<T>
	{
		public int StatusCode { get; set; }
		public string Message { get; set; }
		public T Data { get; set; }

		public ResponseObject()
		{
		}

		public ResponseObject(int statusCode, string message, T data)
		{
			StatusCode = statusCode;
			Message = message;
			Data = data;
		}

		public ResponseObject<T> SuccessResponse(string message, T data)
		{
			return new ResponseObject<T>(StatusCodes.Status200OK,message,data);
		}

		public ResponseObject<T> ErrorResponse(int statusCode,string message, T data)
		{
			return new ResponseObject<T>(statusCode, message, data);
		}
	}
}
