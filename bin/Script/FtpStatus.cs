namespace System.Net
{
	internal class FtpStatus
	{
		private readonly FtpStatusCode statusCode;

		private readonly string statusDescription;

		public FtpStatusCode StatusCode => statusCode;

		public string StatusDescription => statusDescription;

		public FtpStatus(FtpStatusCode statusCode, string statusDescription)
		{
			this.statusCode = statusCode;
			this.statusDescription = statusDescription;
		}
	}
}
