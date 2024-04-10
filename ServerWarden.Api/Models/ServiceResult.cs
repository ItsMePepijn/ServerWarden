namespace ServerWarden.Api.Models
{
	public class ServiceResult<T>(ResultCode code, T? data = default)
	{
		public bool Success => Code == ResultCode.Success;
		public ResultCode Code { get; set; } = code;
		public string? Message => Code.ToString();
		public T? Data { get; set; } = data;
	}

	public class ServiceResult(ResultCode code)
	{
		public bool Success => Code == ResultCode.Success;
		public ResultCode Code { get; set; } = code;
		public string? Message => Code.ToString();
	}

	public enum ResultCode
	{
		Success,
		InvalidParameters,
		Failure,

		InvalidNewPassword,
		InvalidNewUsername,
		InvalidPassword,
		UserExists,
		UserNotFound,
		UserNotAuthorized,

		ServerNotFound,
		InvalidServerType,
	}
}
