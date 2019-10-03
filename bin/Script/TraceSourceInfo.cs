namespace System.Diagnostics
{
	internal class TraceSourceInfo
	{
		private string name;

		private SourceLevels levels;

		private TraceListenerCollection listeners;

		public string Name => name;

		public SourceLevels Levels => levels;

		public TraceListenerCollection Listeners => listeners;

		public TraceSourceInfo(string name, SourceLevels levels)
		{
			this.name = name;
			this.levels = levels;
			listeners = new TraceListenerCollection();
		}

		internal TraceSourceInfo(string name, SourceLevels levels, TraceImplSettings settings)
		{
			this.name = name;
			this.levels = levels;
			listeners = new TraceListenerCollection(addDefault: false);
			listeners.Add(new DefaultTraceListener(), settings);
		}
	}
}
