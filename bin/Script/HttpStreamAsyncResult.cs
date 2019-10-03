using System.Threading;

namespace System.Net
{
	internal class HttpStreamAsyncResult : IAsyncResult
	{
		private object locker = new object();

		private ManualResetEvent handle;

		private bool completed;

		internal byte[] Buffer;

		internal int Offset;

		internal int Count;

		internal AsyncCallback Callback;

		internal object State;

		internal int SynchRead;

		internal Exception Error;

		public object AsyncState => State;

		public WaitHandle AsyncWaitHandle
		{
			get
			{
				lock (locker)
				{
					if (handle == null)
					{
						handle = new ManualResetEvent(completed);
					}
				}
				return handle;
			}
		}

		public bool CompletedSynchronously => SynchRead == Count;

		public bool IsCompleted
		{
			get
			{
				lock (locker)
				{
					return completed;
					IL_0019:
					bool result;
					return result;
				}
			}
		}

		public void Complete(Exception e)
		{
			Error = e;
			Complete();
		}

		public void Complete()
		{
			lock (locker)
			{
				if (!completed)
				{
					completed = true;
					if (handle != null)
					{
						handle.Set();
					}
					if (Callback != null)
					{
						Callback.BeginInvoke(this, null, null);
					}
				}
			}
		}
	}
}
