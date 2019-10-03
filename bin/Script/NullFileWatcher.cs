namespace System.IO
{
	internal class NullFileWatcher : IFileWatcher
	{
		private static IFileWatcher instance;

		public void StartDispatching(FileSystemWatcher fsw)
		{
		}

		public void StopDispatching(FileSystemWatcher fsw)
		{
		}

		public static bool GetInstance(out IFileWatcher watcher)
		{
			if (instance != null)
			{
				watcher = instance;
				return true;
			}
			instance = (watcher = new NullFileWatcher());
			return true;
		}
	}
}
