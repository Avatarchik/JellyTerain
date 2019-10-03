namespace System.Net.Mail
{
	/// <summary>Describes the delivery notification options for e-mail.</summary>
	[Flags]
	public enum DeliveryNotificationOptions
	{
		/// <summary>No notification.</summary>
		None = 0x0,
		/// <summary>Notify if the delivery is successful.</summary>
		OnSuccess = 0x1,
		/// <summary>Notify if the delivery is unsuccessful.</summary>
		OnFailure = 0x2,
		/// <summary>Notify if the delivery is delayed</summary>
		Delay = 0x4,
		/// <summary>Never notify.</summary>
		Never = 0x8000000
	}
}
