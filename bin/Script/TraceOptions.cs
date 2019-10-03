namespace System.Diagnostics
{
	/// <summary>Specifies trace data options to be written to the trace output.</summary>
	/// <filterpriority>2</filterpriority>
	[Flags]
	public enum TraceOptions
	{
		/// <summary>Do not write any elements.</summary>
		None = 0x0,
		/// <summary>Write the logical operation stack, which is represented by the return value of the <see cref="P:System.Diagnostics.CorrelationManager.LogicalOperationStack" /> property.</summary>
		LogicalOperationStack = 0x1,
		/// <summary>Write the date and time. </summary>
		DateTime = 0x2,
		/// <summary>Write the timestamp, which is represented by the return value of the <see cref="M:System.Diagnostics.Stopwatch.GetTimestamp" /> method.</summary>
		Timestamp = 0x4,
		/// <summary>Write the process identity, which is represented by the return value of the <see cref="P:System.Diagnostics.Process.Id" /> property.</summary>
		ProcessId = 0x8,
		/// <summary>Write the thread identity, which is represented by the return value of the <see cref="P:System.Threading.Thread.ManagedThreadId" /> property for the current thread.</summary>
		ThreadId = 0x10,
		/// <summary>Write the call stack, which is represented by the return value of the <see cref="P:System.Environment.StackTrace" /> property.</summary>
		Callstack = 0x20
	}
}
