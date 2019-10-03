namespace Mono.Security.Protocol.Tls
{
	internal class Alert
	{
		private AlertLevel level;

		private AlertDescription description;

		public AlertLevel Level => level;

		public AlertDescription Description => description;

		public string Message => GetAlertMessage(description);

		public bool IsWarning => (level == AlertLevel.Warning) ? true : false;

		public bool IsCloseNotify
		{
			get
			{
				if (IsWarning && description == AlertDescription.CloseNotify)
				{
					return true;
				}
				return false;
			}
		}

		public Alert(AlertDescription description)
		{
			inferAlertLevel();
			this.description = description;
		}

		public Alert(AlertLevel level, AlertDescription description)
		{
			this.level = level;
			this.description = description;
		}

		private void inferAlertLevel()
		{
			switch (description)
			{
			case AlertDescription.CloseNotify:
			case AlertDescription.UserCancelled:
			case AlertDescription.NoRenegotiation:
				level = AlertLevel.Warning;
				break;
			default:
				level = AlertLevel.Fatal;
				break;
			}
		}

		public static string GetAlertMessage(AlertDescription description)
		{
			return "The authentication or decryption has failed.";
		}
	}
}
