using System.ComponentModel;

namespace System.Diagnostics
{
	/// <summary>Identifies the type of event that has caused the trace.</summary>
	/// <filterpriority>2</filterpriority>
	public enum TraceEventType
	{
		/// <summary>Fatal error or application crash.</summary>
		Critical = 1,
		/// <summary>Recoverable error.</summary>
		Error = 2,
		/// <summary>Noncritical problem.</summary>
		Warning = 4,
		/// <summary>Informational message.</summary>
		Information = 8,
		/// <summary>Debugging trace.</summary>
		Verbose = 0x10,
		/// <summary>Starting of a logical operation.</summary>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		Start = 0x100,
		/// <summary>Stopping of a logical operation.</summary>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		Stop = 0x200,
		/// <summary>Suspension of a logical operation.</summary>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		Suspend = 0x400,
		/// <summary>Resumption of a logical operation.</summary>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		Resume = 0x800,
		/// <summary>Changing of correlation identity.</summary>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		Transfer = 0x1000
	}
}
