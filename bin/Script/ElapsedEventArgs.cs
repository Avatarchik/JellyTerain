namespace System.Timers
{
	/// <summary>Provides data for the <see cref="E:System.Timers.Timer.Elapsed" /> event.</summary>
	public class ElapsedEventArgs : EventArgs
	{
		private DateTime time;

		/// <summary>Gets the time the <see cref="E:System.Timers.Timer.Elapsed" /> event was raised.</summary>
		/// <returns>The time the <see cref="E:System.Timers.Timer.Elapsed" /> event was raised.</returns>
		public DateTime SignalTime => time;

		internal ElapsedEventArgs(DateTime time)
		{
			this.time = time;
		}
	}
}
