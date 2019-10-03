using System.ComponentModel;

namespace System.Diagnostics
{
	/// <summary>Specifies the levels of trace messages filtered by the source switch and event type filter.</summary>
	/// <filterpriority>2</filterpriority>
	[Flags]
	public enum SourceLevels
	{
		/// <summary>Does not allow any events through.</summary>
		Off = 0x0,
		/// <summary>Allows only <see cref="F:System.Diagnostics.TraceEventType.Critical" /> events through.</summary>
		Critical = 0x1,
		/// <summary>Allows <see cref="F:System.Diagnostics.TraceEventType.Critical" /> and <see cref="F:System.Diagnostics.TraceEventType.Error" /> events through.</summary>
		Error = 0x3,
		/// <summary>Allows <see cref="F:System.Diagnostics.TraceEventType.Critical" />, <see cref="F:System.Diagnostics.TraceEventType.Error" />, and <see cref="F:System.Diagnostics.TraceEventType.Warning" /> events through.</summary>
		Warning = 0x7,
		/// <summary>Allows <see cref="F:System.Diagnostics.TraceEventType.Critical" />, <see cref="F:System.Diagnostics.TraceEventType.Error" />, <see cref="F:System.Diagnostics.TraceEventType.Warning" />, and <see cref="F:System.Diagnostics.TraceEventType.Information" /> events through.</summary>
		Information = 0xF,
		/// <summary>Allows <see cref="F:System.Diagnostics.TraceEventType.Critical" />, <see cref="F:System.Diagnostics.TraceEventType.Error" />, <see cref="F:System.Diagnostics.TraceEventType.Warning" />, <see cref="F:System.Diagnostics.TraceEventType.Information" />, and <see cref="F:System.Diagnostics.TraceEventType.Verbose" /> events through.</summary>
		Verbose = 0x1F,
		/// <summary>Allows the <see cref="F:System.Diagnostics.TraceEventType.Stop" />, <see cref="F:System.Diagnostics.TraceEventType.Start" />, <see cref="F:System.Diagnostics.TraceEventType.Suspend" />, <see cref="F:System.Diagnostics.TraceEventType.Transfer" />, and <see cref="F:System.Diagnostics.TraceEventType.Resume" /> events through.</summary>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		ActivityTracing = 0xFF00,
		/// <summary>Allows all events through.</summary>
		All = -1
	}
}
