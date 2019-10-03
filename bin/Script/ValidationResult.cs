namespace Mono.Security.Protocol.Tls
{
	public class ValidationResult
	{
		private bool trusted;

		private bool user_denied;

		private int error_code;

		public bool Trusted => trusted;

		public bool UserDenied => user_denied;

		public int ErrorCode => error_code;

		public ValidationResult(bool trusted, bool user_denied, int error_code)
		{
			this.trusted = trusted;
			this.user_denied = user_denied;
			this.error_code = error_code;
		}
	}
}
