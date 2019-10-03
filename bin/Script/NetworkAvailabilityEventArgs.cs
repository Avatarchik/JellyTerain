namespace System.Net.NetworkInformation
{
	/// <summary>Provides data for the <see cref="E:System.Net.NetworkInformation.NetworkChange.NetworkAvailabilityChanged" /> event.</summary>
	public class NetworkAvailabilityEventArgs : EventArgs
	{
		private bool available;

		/// <summary>Gets the current status of the network connection.</summary>
		/// <returns>true if the network is available; otherwise, false.</returns>
		public bool IsAvailable => available;

		internal NetworkAvailabilityEventArgs(bool available)
		{
			this.available = available;
		}
	}
}
