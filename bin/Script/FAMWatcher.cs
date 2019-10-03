using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.IO
{
	internal class FAMWatcher : IFileWatcher
	{
		private const NotifyFilters changed = NotifyFilters.Attributes | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Size;

		private static bool failed;

		private static FAMWatcher instance;

		private static Hashtable watches;

		private static Hashtable requests;

		private static FAMConnection conn;

		private static Thread thread;

		private static bool stop;

		private static bool use_gamin;

		private FAMWatcher()
		{
		}

		public static bool GetInstance(out IFileWatcher watcher, bool gamin)
		{
			if (failed)
			{
				watcher = null;
				return false;
			}
			if (instance != null)
			{
				watcher = instance;
				return true;
			}
			use_gamin = gamin;
			watches = Hashtable.Synchronized(new Hashtable());
			requests = Hashtable.Synchronized(new Hashtable());
			if (FAMOpen(out conn) == -1)
			{
				failed = true;
				watcher = null;
				return false;
			}
			instance = new FAMWatcher();
			watcher = instance;
			return true;
		}

		public void StartDispatching(FileSystemWatcher fsw)
		{
			FAMData fAMData;
			lock (this)
			{
				if (thread == null)
				{
					thread = new Thread(Monitor);
					thread.IsBackground = true;
					thread.Start();
				}
				fAMData = (FAMData)watches[fsw];
			}
			if (fAMData == null)
			{
				fAMData = new FAMData();
				fAMData.FSW = fsw;
				fAMData.Directory = fsw.FullPath;
				fAMData.FileMask = fsw.MangledFilter;
				fAMData.IncludeSubdirs = fsw.IncludeSubdirectories;
				if (fAMData.IncludeSubdirs)
				{
					fAMData.SubDirs = new Hashtable();
				}
				fAMData.Enabled = true;
				StartMonitoringDirectory(fAMData, justcreated: false);
				lock (this)
				{
					watches[fsw] = fAMData;
					requests[fAMData.Request.ReqNum] = fAMData;
					stop = false;
				}
			}
		}

		private static void StartMonitoringDirectory(FAMData data, bool justcreated)
		{
			if (FAMMonitorDirectory(ref conn, data.Directory, out FAMRequest fr, IntPtr.Zero) == -1)
			{
				throw new Win32Exception();
			}
			FileSystemWatcher fSW = data.FSW;
			data.Request = fr;
			if (data.IncludeSubdirs)
			{
				string[] directories = Directory.GetDirectories(data.Directory);
				foreach (string text in directories)
				{
					FAMData fAMData = new FAMData();
					fAMData.FSW = data.FSW;
					fAMData.Directory = text;
					fAMData.FileMask = data.FSW.MangledFilter;
					fAMData.IncludeSubdirs = true;
					fAMData.SubDirs = new Hashtable();
					fAMData.Enabled = true;
					if (justcreated)
					{
						lock (fSW)
						{
							RenamedEventArgs renamed = null;
							fSW.DispatchEvents(FileAction.Added, text, ref renamed);
							if (fSW.Waiting)
							{
								fSW.Waiting = false;
								System.Threading.Monitor.PulseAll(fSW);
							}
						}
					}
					StartMonitoringDirectory(fAMData, justcreated);
					data.SubDirs[text] = fAMData;
					requests[fAMData.Request.ReqNum] = fAMData;
				}
			}
			if (justcreated)
			{
				string[] files = Directory.GetFiles(data.Directory);
				foreach (string filename in files)
				{
					lock (fSW)
					{
						RenamedEventArgs renamed2 = null;
						fSW.DispatchEvents(FileAction.Added, filename, ref renamed2);
						fSW.DispatchEvents(FileAction.Modified, filename, ref renamed2);
						if (fSW.Waiting)
						{
							fSW.Waiting = false;
							System.Threading.Monitor.PulseAll(fSW);
						}
					}
				}
			}
		}

		public void StopDispatching(FileSystemWatcher fsw)
		{
			lock (this)
			{
				FAMData fAMData = (FAMData)watches[fsw];
				if (fAMData != null)
				{
					StopMonitoringDirectory(fAMData);
					watches.Remove(fsw);
					requests.Remove(fAMData.Request.ReqNum);
					if (watches.Count == 0)
					{
						stop = true;
					}
					if (fAMData.IncludeSubdirs)
					{
						foreach (FAMData value in fAMData.SubDirs.Values)
						{
							StopMonitoringDirectory(value);
							requests.Remove(value.Request.ReqNum);
						}
					}
				}
			}
		}

		private static void StopMonitoringDirectory(FAMData data)
		{
			if (FAMCancelMonitor(ref conn, ref data.Request) == -1)
			{
				throw new Win32Exception();
			}
		}

		private void Monitor()
		{
			while (!stop)
			{
				int num;
				lock (this)
				{
					num = FAMPending(ref conn);
				}
				if (num > 0)
				{
					ProcessEvents();
				}
				else
				{
					Thread.Sleep(500);
				}
			}
			lock (this)
			{
				thread = null;
				stop = false;
			}
		}

		private void ProcessEvents()
		{
			ArrayList arrayList = null;
			lock (this)
			{
				do
				{
					if (InternalFAMNextEvent(ref conn, out string filename, out int code, out int reqnum) != 1)
					{
						return;
					}
					bool flag = false;
					switch (code)
					{
					case 1:
					case 2:
					case 5:
						flag = requests.ContainsKey(reqnum);
						break;
					default:
						flag = false;
						break;
					}
					if (flag)
					{
						FAMData fAMData = (FAMData)requests[reqnum];
						if (fAMData.Enabled)
						{
							FileSystemWatcher fSW = fAMData.FSW;
							NotifyFilters notifyFilter = fSW.NotifyFilter;
							RenamedEventArgs renamed = null;
							FileAction fileAction = (FileAction)0;
							if (code == 1 && (notifyFilter & (NotifyFilters.Attributes | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Size)) != 0)
							{
								fileAction = FileAction.Modified;
							}
							else
							{
								switch (code)
								{
								case 2:
									fileAction = FileAction.Removed;
									break;
								case 5:
									fileAction = FileAction.Added;
									break;
								}
							}
							if (fileAction != 0)
							{
								if (fSW.IncludeSubdirectories)
								{
									string fullPath = fSW.FullPath;
									string directory = fAMData.Directory;
									if (directory != fullPath)
									{
										int length = fullPath.Length;
										int num = 1;
										if (length > 1 && fullPath[length - 1] == Path.DirectorySeparatorChar)
										{
											num = 0;
										}
										string path = directory.Substring(fullPath.Length + num);
										directory = Path.Combine(directory, filename);
										filename = Path.Combine(path, filename);
									}
									else
									{
										directory = Path.Combine(fullPath, filename);
									}
									if (fileAction == FileAction.Added && Directory.Exists(directory))
									{
										if (arrayList == null)
										{
											arrayList = new ArrayList(4);
										}
										FAMData fAMData2 = new FAMData();
										fAMData2.FSW = fSW;
										fAMData2.Directory = directory;
										fAMData2.FileMask = fSW.MangledFilter;
										fAMData2.IncludeSubdirs = true;
										fAMData2.SubDirs = new Hashtable();
										fAMData2.Enabled = true;
										arrayList.Add(fAMData2);
										arrayList.Add(fAMData);
									}
								}
								if (!(filename != fAMData.Directory) || fSW.Pattern.IsMatch(filename))
								{
									lock (fSW)
									{
										fSW.DispatchEvents(fileAction, filename, ref renamed);
										if (fSW.Waiting)
										{
											fSW.Waiting = false;
											System.Threading.Monitor.PulseAll(fSW);
										}
									}
								}
							}
						}
					}
				}
				while (FAMPending(ref conn) > 0);
			}
			if (arrayList != null)
			{
				int count = arrayList.Count;
				for (int i = 0; i < count; i += 2)
				{
					FAMData fAMData3 = (FAMData)arrayList[i];
					FAMData fAMData4 = (FAMData)arrayList[i + 1];
					StartMonitoringDirectory(fAMData3, justcreated: true);
					requests[fAMData3.Request.ReqNum] = fAMData3;
					lock (fAMData4)
					{
						fAMData4.SubDirs[fAMData3.Directory] = fAMData3;
					}
				}
				arrayList.Clear();
			}
		}

		~FAMWatcher()
		{
			FAMClose(ref conn);
		}

		private static int FAMOpen(out FAMConnection fc)
		{
			if (use_gamin)
			{
				return gamin_Open(out fc);
			}
			return fam_Open(out fc);
		}

		private static int FAMClose(ref FAMConnection fc)
		{
			if (use_gamin)
			{
				return gamin_Close(ref fc);
			}
			return fam_Close(ref fc);
		}

		private static int FAMMonitorDirectory(ref FAMConnection fc, string filename, out FAMRequest fr, IntPtr user_data)
		{
			if (use_gamin)
			{
				return gamin_MonitorDirectory(ref fc, filename, out fr, user_data);
			}
			return fam_MonitorDirectory(ref fc, filename, out fr, user_data);
		}

		private static int FAMCancelMonitor(ref FAMConnection fc, ref FAMRequest fr)
		{
			if (use_gamin)
			{
				return gamin_CancelMonitor(ref fc, ref fr);
			}
			return fam_CancelMonitor(ref fc, ref fr);
		}

		private static int FAMPending(ref FAMConnection fc)
		{
			if (use_gamin)
			{
				return gamin_Pending(ref fc);
			}
			return fam_Pending(ref fc);
		}

		[DllImport("libfam.so.0", EntryPoint = "FAMOpen")]
		private static extern int fam_Open(out FAMConnection fc);

		[DllImport("libfam.so.0", EntryPoint = "FAMClose")]
		private static extern int fam_Close(ref FAMConnection fc);

		[DllImport("libfam.so.0", EntryPoint = "FAMMonitorDirectory")]
		private static extern int fam_MonitorDirectory(ref FAMConnection fc, string filename, out FAMRequest fr, IntPtr user_data);

		[DllImport("libfam.so.0", EntryPoint = "FAMCancelMonitor")]
		private static extern int fam_CancelMonitor(ref FAMConnection fc, ref FAMRequest fr);

		[DllImport("libfam.so.0", EntryPoint = "FAMPending")]
		private static extern int fam_Pending(ref FAMConnection fc);

		[DllImport("libgamin-1.so.0", EntryPoint = "FAMOpen")]
		private static extern int gamin_Open(out FAMConnection fc);

		[DllImport("libgamin-1.so.0", EntryPoint = "FAMClose")]
		private static extern int gamin_Close(ref FAMConnection fc);

		[DllImport("libgamin-1.so.0", EntryPoint = "FAMMonitorDirectory")]
		private static extern int gamin_MonitorDirectory(ref FAMConnection fc, string filename, out FAMRequest fr, IntPtr user_data);

		[DllImport("libgamin-1.so.0", EntryPoint = "FAMCancelMonitor")]
		private static extern int gamin_CancelMonitor(ref FAMConnection fc, ref FAMRequest fr);

		[DllImport("libgamin-1.so.0", EntryPoint = "FAMPending")]
		private static extern int gamin_Pending(ref FAMConnection fc);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int InternalFAMNextEvent(ref FAMConnection fc, out string filename, out int code, out int reqnum);
	}
}
