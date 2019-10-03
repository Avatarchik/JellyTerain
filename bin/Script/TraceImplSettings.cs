namespace System.Diagnostics
{
	internal class TraceImplSettings
	{
		public const string Key = ".__TraceInfoSettingsKey__.";

		public bool AutoFlush;

		public int IndentLevel;

		public int IndentSize = 4;

		public TraceListenerCollection Listeners = new TraceListenerCollection(addDefault: false);

		public TraceImplSettings()
		{
			Listeners.Add(new DefaultTraceListener(), this);
		}
	}
}
