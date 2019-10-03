namespace System.Diagnostics
{
	/// <summary>Specifies the current execution state of the thread.</summary>
	/// <filterpriority>1</filterpriority>
	public enum ThreadState
	{
		/// <summary>A state that indicates the thread has been initialized, but has not yet started.</summary>
		Initialized = 0,
		/// <summary>A state that indicates the thread is waiting to use a processor because no processor is free. The thread is prepared to run on the next available processor.</summary>
		Ready = 1,
		/// <summary>A state that indicates the thread is currently using a processor.</summary>
		Running = 2,
		/// <summary>A state that indicates the thread is about to use a processor. Only one thread can be in this state at a time.</summary>
		Standby = 3,
		/// <summary>A state that indicates the thread has finished executing and has exited.</summary>
		Terminated = 4,
		/// <summary>A state that indicates the thread is waiting for a resource, other than the processor, before it can execute. For example, it might be waiting for its execution stack to be paged in from disk.</summary>
		Transition = 6,
		/// <summary>The state of the thread is unknown.</summary>
		Unknown = 7,
		/// <summary>A state that indicates the thread is not ready to use the processor because it is waiting for a peripheral operation to complete or a resource to become free. When the thread is ready, it will be rescheduled.</summary>
		Wait = 5
	}
}
