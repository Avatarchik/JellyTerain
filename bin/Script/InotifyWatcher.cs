using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace System.IO
{
	internal class InotifyWatcher : IFileWatcher
	{
		private static bool failed;

		private static InotifyWatcher instance;

		private static Hashtable watches;

		private static Hashtable requests;

		private static IntPtr FD;

		private static Thread thread;

		private static bool stop;

		private static InotifyMask Interesting = InotifyMask.Modify | InotifyMask.Attrib | InotifyMask.MovedFrom | InotifyMask.MovedTo | InotifyMask.Create | InotifyMask.Delete | InotifyMask.DeleteSelf;

		private InotifyWatcher()
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
			FD = GetInotifyInstance();
			if ((long)FD == -1)
			{
				failed = true;
				watcher = null;
				return false;
			}
			watches = Hashtable.Synchronized(new Hashtable());
			requests = Hashtable.Synchronized(new Hashtable());
			instance = new InotifyWatcher();
			watcher = instance;
			return true;
		}

		public void StartDispatching(FileSystemWatcher fsw)
		{
			ParentInotifyData parentInotifyData;
			lock (this)
			{
				if ((long)FD == -1)
				{
					FD = GetInotifyInstance();
				}
				if (thread == null)
				{
					thread = new Thread(Monitor);
					thread.IsBackground = true;
					thread.Start();
				}
				parentInotifyData = (ParentInotifyData)watches[fsw];
			}
			if (parentInotifyData == null)
			{
				InotifyData inotifyData = new InotifyData();
				inotifyData.FSW = fsw;
				inotifyData.Directory = fsw.FullPath;
				parentInotifyData = new ParentInotifyData();
				parentInotifyData.IncludeSubdirs = fsw.IncludeSubdirectories;
				parentInotifyData.Enabled = true;
				parentInotifyData.children = new ArrayList();
				parentInotifyData.data = inotifyData;
				watches[fsw] = parentInotifyData;
				try
				{
					StartMonitoringDirectory(inotifyData, justcreated: false);
					lock (this)
					{
						AppendRequestData(inotifyData);
						stop = false;
					}
				}
				catch
				{
				}
			}
		}

		private static void AppendRequestData(InotifyData data)
		{
			int watch = data.Watch;
			object obj = requests[watch];
			ArrayList arrayList = null;
			if (obj == null)
			{
				requests[data.Watch] = data;
			}
			else if (obj is InotifyData)
			{
				arrayList = new ArrayList();
				arrayList.Add(obj);
				arrayList.Add(data);
				requests[data.Watch] = arrayList;
			}
			else
			{
				arrayList = (ArrayList)obj;
				arrayList.Add(data);
			}
		}

		private static bool RemoveRequestData(InotifyData data)
		{
			int watch = data.Watch;
			object obj = requests[watch];
			if (obj == null)
			{
				return true;
			}
			if (obj is InotifyData)
			{
				if (obj == data)
				{
					requests.Remove(watch);
					return true;
				}
				return false;
			}
			ArrayList arrayList = (ArrayList)obj;
			arrayList.Remove(data);
			if (arrayList.Count == 0)
			{
				requests.Remove(watch);
				return true;
			}
			return false;
		}

		private static InotifyMask GetMaskFromFilters(NotifyFilters filters)
		{
			InotifyMask inotifyMask = InotifyMask.Create | InotifyMask.Delete | InotifyMask.DeleteSelf | InotifyMask.AddMask;
			if ((filters & NotifyFilters.Attributes) != 0)
			{
				inotifyMask |= InotifyMask.Attrib;
			}
			if ((filters & NotifyFilters.Security) != 0)
			{
				inotifyMask |= InotifyMask.Attrib;
			}
			if ((filters & NotifyFilters.Size) != 0)
			{
				inotifyMask |= InotifyMask.Attrib;
				inotifyMask |= InotifyMask.Modify;
			}
			if ((filters & NotifyFilters.LastAccess) != 0)
			{
				inotifyMask |= InotifyMask.Attrib;
				inotifyMask |= InotifyMask.Access;
				inotifyMask |= InotifyMask.Modify;
			}
			if ((filters & NotifyFilters.LastWrite) != 0)
			{
				inotifyMask |= InotifyMask.Attrib;
				inotifyMask |= InotifyMask.Modify;
			}
			if ((filters & NotifyFilters.FileName) != 0)
			{
				inotifyMask |= InotifyMask.MovedFrom;
				inotifyMask |= InotifyMask.MovedTo;
			}
			if ((filters & NotifyFilters.DirectoryName) != 0)
			{
				inotifyMask |= InotifyMask.MovedFrom;
				inotifyMask |= InotifyMask.MovedTo;
			}
			return inotifyMask;
		}

		private static void StartMonitoringDirectory(InotifyData data, bool justcreated)
		{
			InotifyMask maskFromFilters = GetMaskFromFilters(data.FSW.NotifyFilter);
			int num = AddDirectoryWatch(FD, data.Directory, maskFromFilters);
			if (num == -1)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error == 4)
				{
					string arg = "(unknown)";
					try
					{
						using (StreamReader streamReader = new StreamReader("/proc/sys/fs/inotify/max_user_watches"))
						{
							arg = streamReader.ReadLine();
						}
					}
					catch
					{
					}
					string message = $"The per-user inotify watches limit of {arg} has been reached. If you're experiencing problems with your application, increase that limit in /proc/sys/fs/inotify/max_user_watches.";
					throw new Win32Exception(lastWin32Error, message);
				}
				throw new Win32Exception(lastWin32Error);
			}
			FileSystemWatcher fSW = data.FSW;
			data.Watch = num;
			ParentInotifyData parentInotifyData = (ParentInotifyData)watches[fSW];
			if (parentInotifyData.IncludeSubdirs)
			{
				string[] directories = Directory.GetDirectories(data.Directory);
				foreach (string text in directories)
				{
					InotifyData inotifyData = new InotifyData();
					inotifyData.FSW = fSW;
					inotifyData.Directory = text;
					if (justcreated)
					{
						lock (fSW)
						{
							RenamedEventArgs renamed = null;
							if (fSW.Pattern.IsMatch(text))
							{
								fSW.DispatchEvents(FileAction.Added, text, ref renamed);
								if (fSW.Waiting)
								{
									fSW.Waiting = false;
									System.Threading.Monitor.PulseAll(fSW);
								}
							}
						}
					}
					try
					{
						StartMonitoringDirectory(inotifyData, justcreated);
						AppendRequestData(inotifyData);
						parentInotifyData.children.Add(inotifyData);
					}
					catch
					{
					}
				}
			}
			if (justcreated)
			{
				string[] files = Directory.GetFiles(data.Directory);
				foreach (string text2 in files)
				{
					lock (fSW)
					{
						RenamedEventArgs renamed2 = null;
						if (fSW.Pattern.IsMatch(text2))
						{
							fSW.DispatchEvents(FileAction.Added, text2, ref renamed2);
							fSW.DispatchEvents(FileAction.Modified, text2, ref renamed2);
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

		public void StopDispatching(FileSystemWatcher fsw)
		{
			lock (this)
			{
				ParentInotifyData parentInotifyData = (ParentInotifyData)watches[fsw];
				if (parentInotifyData != null)
				{
					if (RemoveRequestData(parentInotifyData.data))
					{
						StopMonitoringDirectory(parentInotifyData.data);
					}
					watches.Remove(fsw);
					if (watches.Count == 0)
					{
						stop = true;
						IntPtr fD = FD;
						FD = (IntPtr)(-1);
						Close(fD);
					}
					if (parentInotifyData.IncludeSubdirs)
					{
						foreach (InotifyData child in parentInotifyData.children)
						{
							if (RemoveRequestData(child))
							{
								StopMonitoringDirectory(child);
							}
						}
					}
				}
			}
		}

		private static void StopMonitoringDirectory(InotifyData data)
		{
			RemoveWatch(FD, data.Watch);
		}

		private void Monitor()
		{
			byte[] array = new byte[4096];
			while (!stop)
			{
				int num = ReadFromFD(FD, array, (IntPtr)array.Length);
				if (num != -1)
				{
					lock (this)
					{
						ProcessEvents(array, num);
					}
				}
			}
			lock (this)
			{
				thread = null;
				stop = false;
			}
		}

		private static int ReadEvent(byte[] source, int off, int size, out InotifyEvent evt)
		{
			evt = default(InotifyEvent);
			if (size <= 0 || off > size - 16)
			{
				return -1;
			}
			int num;
			if (BitConverter.IsLittleEndian)
			{
				evt.WatchDescriptor = source[off] + (source[off + 1] << 8) + (source[off + 2] << 16) + (source[off + 3] << 24);
				evt.Mask = (InotifyMask)(source[off + 4] + (source[off + 5] << 8) + (source[off + 6] << 16) + (source[off + 7] << 24));
				num = source[off + 12] + (source[off + 13] << 8) + (source[off + 14] << 16) + (source[off + 15] << 24);
			}
			else
			{
				evt.WatchDescriptor = source[off + 3] + (source[off + 2] << 8) + (source[off + 1] << 16) + (source[off] << 24);
				evt.Mask = (InotifyMask)(source[off + 7] + (source[off + 6] << 8) + (source[off + 5] << 16) + (source[off + 4] << 24));
				num = source[off + 15] + (source[off + 14] << 8) + (source[off + 13] << 16) + (source[off + 12] << 24);
			}
			if (num > 0)
			{
				if (off > size - 16 - num)
				{
					return -1;
				}
				string @string = Encoding.UTF8.GetString(source, off + 16, num);
				evt.Name = @string.Trim(default(char));
			}
			else
			{
				evt.Name = null;
			}
			return 16 + num;
		}

		private static IEnumerable GetEnumerator(object source)
		{
			if (source == null)
			{
				yield break;
			}
			if (source is InotifyData)
			{
				yield return source;
			}
			if (source is ArrayList)
			{
				ArrayList list = (ArrayList)source;
				for (int i = 0; i < list.Count; i++)
				{
					yield return list[i];
				}
			}
		}

		private void ProcessEvents(byte[] buffer, int length)
		{
			ArrayList arrayList = null;
			int num = 0;
			RenamedEventArgs renamed = null;
			while (length > num)
			{
				InotifyEvent evt;
				int num2 = ReadEvent(buffer, num, length, out evt);
				if (num2 <= 0)
				{
					break;
				}
				num += num2;
				InotifyMask mask = evt.Mask;
				bool flag = (mask & InotifyMask.Directory) != (InotifyMask)0u;
				mask &= Interesting;
				if (mask != 0)
				{
					foreach (InotifyData item in GetEnumerator(requests[evt.WatchDescriptor]))
					{
						ParentInotifyData parentInotifyData = (ParentInotifyData)watches[item.FSW];
						if (item != null && parentInotifyData.Enabled)
						{
							string directory = item.Directory;
							string text = evt.Name;
							if (text == null)
							{
								text = directory;
							}
							FileSystemWatcher fSW = item.FSW;
							FileAction fileAction = (FileAction)0;
							if ((mask & (InotifyMask.Modify | InotifyMask.Attrib)) != 0)
							{
								fileAction = FileAction.Modified;
							}
							else if ((mask & InotifyMask.Create) != 0)
							{
								fileAction = FileAction.Added;
							}
							else if ((mask & InotifyMask.Delete) != 0)
							{
								fileAction = FileAction.Removed;
							}
							else if ((mask & InotifyMask.DeleteSelf) != 0)
							{
								if (item.Watch != parentInotifyData.data.Watch)
								{
									continue;
								}
								fileAction = FileAction.Removed;
							}
							else
							{
								if ((mask & InotifyMask.MoveSelf) != 0)
								{
									continue;
								}
								if ((mask & InotifyMask.MovedFrom) != 0)
								{
									InotifyEvent evt2;
									int num3 = ReadEvent(buffer, num, length, out evt2);
									if (num3 == -1 || (evt2.Mask & InotifyMask.MovedTo) == (InotifyMask)0u || evt.WatchDescriptor != evt2.WatchDescriptor)
									{
										fileAction = FileAction.Removed;
									}
									else
									{
										num += num3;
										fileAction = FileAction.RenamedNewName;
										renamed = new RenamedEventArgs(WatcherChangeTypes.Renamed, item.Directory, evt2.Name, evt.Name);
										if (evt.Name != item.Directory && !fSW.Pattern.IsMatch(evt.Name))
										{
											text = evt2.Name;
										}
									}
								}
								else if ((mask & InotifyMask.MovedTo) != 0)
								{
									fileAction = FileAction.Added;
								}
							}
							if (fSW.IncludeSubdirectories)
							{
								string fullPath = fSW.FullPath;
								string directory2 = item.Directory;
								if (directory2 != fullPath)
								{
									int length2 = fullPath.Length;
									int num4 = 1;
									if (length2 > 1 && fullPath[length2 - 1] == Path.DirectorySeparatorChar)
									{
										num4 = 0;
									}
									string path = directory2.Substring(fullPath.Length + num4);
									directory2 = Path.Combine(directory2, text);
									text = Path.Combine(path, text);
								}
								else
								{
									directory2 = Path.Combine(fullPath, text);
								}
								if (fileAction == FileAction.Added && flag)
								{
									if (arrayList == null)
									{
										arrayList = new ArrayList(2);
									}
									InotifyData inotifyData2 = new InotifyData();
									inotifyData2.FSW = fSW;
									inotifyData2.Directory = directory2;
									arrayList.Add(inotifyData2);
								}
								if (fileAction == FileAction.RenamedNewName && flag)
								{
									string oldFullPath = renamed.OldFullPath;
									string fullPath2 = renamed.FullPath;
									int length3 = oldFullPath.Length;
									foreach (InotifyData child in parentInotifyData.children)
									{
										if (child.Directory.StartsWith(oldFullPath, StringComparison.Ordinal))
										{
											child.Directory = fullPath2 + child.Directory.Substring(length3);
										}
									}
								}
							}
							if (fileAction == FileAction.Removed && text == item.Directory)
							{
								int num5 = parentInotifyData.children.IndexOf(item);
								if (num5 != -1)
								{
									parentInotifyData.children.RemoveAt(num5);
									if (!fSW.Pattern.IsMatch(Path.GetFileName(text)))
									{
										continue;
									}
								}
							}
							if (!(text != item.Directory) || fSW.Pattern.IsMatch(Path.GetFileName(text)))
							{
								lock (fSW)
								{
									fSW.DispatchEvents(fileAction, text, ref renamed);
									if (fileAction == FileAction.RenamedNewName)
									{
										renamed = null;
									}
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
			if (arrayList != null)
			{
				foreach (InotifyData item2 in arrayList)
				{
					try
					{
						StartMonitoringDirectory(item2, justcreated: true);
						AppendRequestData(item2);
						((ParentInotifyData)watches[item2.FSW]).children.Add(item2);
					}
					catch
					{
					}
				}
				arrayList.Clear();
			}
		}

		private static int AddDirectoryWatch(IntPtr fd, string directory, InotifyMask mask)
		{
			mask |= InotifyMask.Directory;
			return AddWatch(fd, directory, mask);
		}

		[DllImport("libc", EntryPoint = "close")]
		internal static extern int Close(IntPtr fd);

		[DllImport("libc", EntryPoint = "read")]
		private static extern int ReadFromFD(IntPtr fd, byte[] buffer, IntPtr length);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr GetInotifyInstance();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int AddWatch(IntPtr fd, string name, InotifyMask mask);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr RemoveWatch(IntPtr fd, int wd);
	}
}
