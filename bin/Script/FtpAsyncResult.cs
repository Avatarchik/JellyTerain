using System.IO;
using System.Threading;

namespace System.Net
{
	internal class FtpAsyncResult : IAsyncResult
	{
		private FtpWebResponse response;

		private ManualResetEvent waitHandle;

		private Exception exception;

		private AsyncCallback callback;

		private Stream stream;

		private object state;

		private bool completed;

		private bool synch;

		private object locker = new object();

		public object AsyncState => state;

		public WaitHandle AsyncWaitHandle
		{
			get
			{
				lock (locker)
				{
					if (waitHandle == null)
					{
						waitHandle = new ManualResetEvent(initialState: false);
					}
				}
				return waitHandle;
			}
		}

		public bool CompletedSynchronously => synch;

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

		internal bool GotException => exception != null;

		internal Exception Exception => exception;

		internal FtpWebResponse Response
		{
			get
			{
				return response;
			}
			set
			{
				response = value;
			}
		}

		internal Stream Stream
		{
			get
			{
				return stream;
			}
			set
			{
				stream = value;
			}
		}

		public FtpAsyncResult(AsyncCallback callback, object state)
		{
			this.callback = callback;
			this.state = state;
		}

		internal void WaitUntilComplete()
		{
			if (!IsCompleted)
			{
				AsyncWaitHandle.WaitOne();
			}
		}

		internal bool WaitUntilComplete(int timeout, bool exitContext)
		{
			if (IsCompleted)
			{
				return true;
			}
			return AsyncWaitHandle.WaitOne(timeout, exitContext);
		}

		internal void SetCompleted(bool synch, Exception exc, FtpWebResponse response)
		{
			this.synch = synch;
			exception = exc;
			this.response = response;
			lock (locker)
			{
				completed = true;
				if (waitHandle != null)
				{
					waitHandle.Set();
				}
			}
			DoCallback();
		}

		internal void SetCompleted(bool synch, FtpWebResponse response)
		{
			SetCompleted(synch, null, response);
		}

		internal void SetCompleted(bool synch, Exception exc)
		{
			SetCompleted(synch, exc, null);
		}

		internal void DoCallback()
		{
			if (callback != null)
			{
				try
				{
					callback(this);
				}
				catch (Exception)
				{
				}
			}
		}

		internal void Reset()
		{
			exception = null;
			synch = false;
			response = null;
			state = null;
			lock (locker)
			{
				completed = false;
				if (waitHandle != null)
				{
					waitHandle.Reset();
				}
			}
		}
	}
}
