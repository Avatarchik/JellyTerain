using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.IO
{
	/// <summary>Listens to the file system change notifications and raises events when a directory, or file in a directory, changes.</summary>
	/// <filterpriority>2</filterpriority>
	[IODescription("")]
	[DefaultEvent("Changed")]
	public class FileSystemWatcher : Component, ISupportInitialize
	{
		private enum EventType
		{
			FileSystemEvent,
			ErrorEvent,
			RenameEvent
		}

		private bool enableRaisingEvents;

		private string filter;

		private bool includeSubdirectories;

		private int internalBufferSize;

		private NotifyFilters notifyFilter;

		private string path;

		private string fullpath;

		private ISynchronizeInvoke synchronizingObject;

		private WaitForChangedResult lastData;

		private bool waiting;

		private SearchPattern2 pattern;

		private bool disposed;

		private string mangledFilter;

		private static IFileWatcher watcher;

		private static object lockobj = new object();

		internal bool Waiting
		{
			get
			{
				return waiting;
			}
			set
			{
				waiting = value;
			}
		}

		internal string MangledFilter
		{
			get
			{
				if (filter != "*.*")
				{
					return filter;
				}
				if (mangledFilter != null)
				{
					return mangledFilter;
				}
				string result = "*.*";
				if (watcher.GetType() != typeof(WindowsWatcher))
				{
					result = "*";
				}
				return result;
			}
		}

		internal SearchPattern2 Pattern
		{
			get
			{
				if (pattern == null)
				{
					pattern = new SearchPattern2(MangledFilter);
				}
				return pattern;
			}
		}

		internal string FullPath
		{
			get
			{
				if (fullpath == null)
				{
					if (path == null || path == string.Empty)
					{
						fullpath = Environment.CurrentDirectory;
					}
					else
					{
						fullpath = System.IO.Path.GetFullPath(path);
					}
				}
				return fullpath;
			}
		}

		/// <summary>Gets or sets a value indicating whether the component is enabled.</summary>
		/// <returns>true if the component is enabled; otherwise, false. The default is false. If you are using the component on a designer in Visual Studio 2005, the default is true.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.FileSystemWatcher" /> object has been disposed.</exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The directory specified in <see cref="P:System.IO.FileSystemWatcher.Path" /> could not be found.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <see cref="P:System.IO.FileSystemWatcher.Path" /> has not been set or is invalid.</exception>
		/// <filterpriority>2</filterpriority>
		[DefaultValue(false)]
		[IODescription("Flag to indicate if this instance is active")]
		public bool EnableRaisingEvents
		{
			get
			{
				return enableRaisingEvents;
			}
			set
			{
				if (value != enableRaisingEvents)
				{
					enableRaisingEvents = value;
					if (value)
					{
						Start();
					}
					else
					{
						Stop();
					}
				}
			}
		}

		/// <summary>Gets or sets the filter string used to determine what files are monitored in a directory.</summary>
		/// <returns>The filter string. The default is "*.*" (Watches all files.) </returns>
		/// <filterpriority>2</filterpriority>
		[IODescription("File name filter pattern")]
		[DefaultValue("*.*")]
		[RecommendedAsConfigurable(true)]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public string Filter
		{
			get
			{
				return filter;
			}
			set
			{
				if (value == null || value == string.Empty)
				{
					value = "*.*";
				}
				if (filter != value)
				{
					filter = value;
					pattern = null;
					mangledFilter = null;
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether subdirectories within the specified path should be monitored.</summary>
		/// <returns>true if you want to monitor subdirectories; otherwise, false. The default is false.</returns>
		/// <filterpriority>2</filterpriority>
		[IODescription("Flag to indicate we want to watch subdirectories")]
		[DefaultValue(false)]
		public bool IncludeSubdirectories
		{
			get
			{
				return includeSubdirectories;
			}
			set
			{
				if (includeSubdirectories != value)
				{
					includeSubdirectories = value;
					if (value && enableRaisingEvents)
					{
						Stop();
						Start();
					}
				}
			}
		}

		/// <summary>Gets or sets the size of the internal buffer.</summary>
		/// <returns>The internal buffer size. The default is 8192 (8 KB).</returns>
		/// <filterpriority>2</filterpriority>
		[DefaultValue(8192)]
		[Browsable(false)]
		public int InternalBufferSize
		{
			get
			{
				return internalBufferSize;
			}
			set
			{
				if (internalBufferSize != value)
				{
					if (value < 4196)
					{
						value = 4196;
					}
					internalBufferSize = value;
					if (enableRaisingEvents)
					{
						Stop();
						Start();
					}
				}
			}
		}

		/// <summary>Gets or sets the type of changes to watch for.</summary>
		/// <returns>One of the <see cref="T:System.IO.NotifyFilters" /> values. The default is the bitwise OR combination of LastWrite, FileName, and DirectoryName.</returns>
		/// <exception cref="T:System.ArgumentException">The value is not a valid bitwise OR combination of the <see cref="T:System.IO.NotifyFilters" /> values. </exception>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value that is being set is not valid.</exception>
		/// <filterpriority>2</filterpriority>
		[DefaultValue(NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastWrite)]
		[IODescription("Flag to indicate which change event we want to monitor")]
		public NotifyFilters NotifyFilter
		{
			get
			{
				return notifyFilter;
			}
			set
			{
				if (notifyFilter != value)
				{
					notifyFilter = value;
					if (enableRaisingEvents)
					{
						Stop();
						Start();
					}
				}
			}
		}

		/// <summary>Gets or sets the path of the directory to watch.</summary>
		/// <returns>The path to monitor. The default is an empty string ("").</returns>
		/// <exception cref="T:System.ArgumentException">The specified path does not exist or could not be found.-or- The specified path contains wildcard characters.-or- The specified path contains invalid path characters.</exception>
		/// <filterpriority>2</filterpriority>
		[RecommendedAsConfigurable(true)]
		[DefaultValue("")]
		[IODescription("The directory to monitor")]
		[Editor("System.Diagnostics.Design.FSWPathEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public string Path
		{
			get
			{
				return path;
			}
			set
			{
				if (!(path == value))
				{
					bool flag = false;
					Exception ex = null;
					try
					{
						flag = Directory.Exists(value);
					}
					catch (Exception ex2)
					{
						ex = ex2;
					}
					if (ex != null)
					{
						throw new ArgumentException("Invalid directory name", "value", ex);
					}
					if (!flag)
					{
						throw new ArgumentException("Directory does not exists", "value");
					}
					path = value;
					fullpath = null;
					if (enableRaisingEvents)
					{
						Stop();
						Start();
					}
				}
			}
		}

		/// <summary>Gets or sets an <see cref="T:System.ComponentModel.ISite" /> for the <see cref="T:System.IO.FileSystemWatcher" />.</summary>
		/// <returns>An <see cref="T:System.ComponentModel.ISite" /> for the <see cref="T:System.IO.FileSystemWatcher" />.</returns>
		/// <filterpriority>2</filterpriority>
		[Browsable(false)]
		public override ISite Site
		{
			get
			{
				return base.Site;
			}
			set
			{
				base.Site = value;
			}
		}

		/// <summary>Gets or sets the object used to marshal the event handler calls issued as a result of a directory change.</summary>
		/// <returns>The <see cref="T:System.ComponentModel.ISynchronizeInvoke" /> that represents the object used to marshal the event handler calls issued as a result of a directory change. The default is null.</returns>
		/// <filterpriority>2</filterpriority>
		[Browsable(false)]
		[IODescription("The object used to marshal the event handler calls resulting from a directory change")]
		[DefaultValue(null)]
		public ISynchronizeInvoke SynchronizingObject
		{
			get
			{
				return synchronizingObject;
			}
			set
			{
				synchronizingObject = value;
			}
		}

		/// <summary>Occurs when a file or directory in the specified <see cref="P:System.IO.FileSystemWatcher.Path" /> is changed.</summary>
		/// <filterpriority>2</filterpriority>
		[IODescription("Occurs when a file/directory change matches the filter")]
		public event FileSystemEventHandler Changed;

		/// <summary>Occurs when a file or directory in the specified <see cref="P:System.IO.FileSystemWatcher.Path" /> is created.</summary>
		/// <filterpriority>2</filterpriority>
		[IODescription("Occurs when a file/directory creation matches the filter")]
		public event FileSystemEventHandler Created;

		/// <summary>Occurs when a file or directory in the specified <see cref="P:System.IO.FileSystemWatcher.Path" /> is deleted.</summary>
		/// <filterpriority>2</filterpriority>
		[IODescription("Occurs when a file/directory deletion matches the filter")]
		public event FileSystemEventHandler Deleted;

		/// <summary>Occurs when the internal buffer overflows.</summary>
		/// <filterpriority>2</filterpriority>
		[Browsable(false)]
		public event ErrorEventHandler Error;

		/// <summary>Occurs when a file or directory in the specified <see cref="P:System.IO.FileSystemWatcher.Path" /> is renamed.</summary>
		/// <filterpriority>2</filterpriority>
		[IODescription("Occurs when a file/directory rename matches the filter")]
		public event RenamedEventHandler Renamed;

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.FileSystemWatcher" /> class.</summary>
		public FileSystemWatcher()
		{
			notifyFilter = (NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastWrite);
			enableRaisingEvents = false;
			filter = "*.*";
			includeSubdirectories = false;
			internalBufferSize = 8192;
			path = string.Empty;
			InitWatcher();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.FileSystemWatcher" /> class, given the specified directory to monitor.</summary>
		/// <param name="path">The directory to monitor, in standard or Universal Naming Convention (UNC) notation. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="path" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="path" /> parameter is an empty string ("").-or- The path specified through the <paramref name="path" /> parameter does not exist. </exception>
		public FileSystemWatcher(string path)
			: this(path, "*.*")
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.FileSystemWatcher" /> class, given the specified directory and type of files to monitor.</summary>
		/// <param name="path">The directory to monitor, in standard or Universal Naming Convention (UNC) notation. </param>
		/// <param name="filter">The type of files to watch. For example, "*.txt" watches for changes to all text files. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="path" /> parameter is null.-or- The <paramref name="filter" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="path" /> parameter is an empty string ("").-or- The path specified through the <paramref name="path" /> parameter does not exist. </exception>
		public FileSystemWatcher(string path, string filter)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (filter == null)
			{
				throw new ArgumentNullException("filter");
			}
			if (path == string.Empty)
			{
				throw new ArgumentException("Empty path", "path");
			}
			if (!Directory.Exists(path))
			{
				throw new ArgumentException("Directory does not exists", "path");
			}
			enableRaisingEvents = false;
			this.filter = filter;
			includeSubdirectories = false;
			internalBufferSize = 8192;
			notifyFilter = (NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastWrite);
			this.path = path;
			synchronizingObject = null;
			InitWatcher();
		}

		private void InitWatcher()
		{
			lock (lockobj)
			{
				if (watcher == null)
				{
					string environmentVariable = Environment.GetEnvironmentVariable("MONO_MANAGED_WATCHER");
					int num = 0;
					if (environmentVariable == null)
					{
						num = InternalSupportsFSW();
					}
					bool flag = false;
					switch (num)
					{
					case 1:
						flag = DefaultWatcher.GetInstance(out watcher);
						break;
					case 2:
						flag = FAMWatcher.GetInstance(out watcher, gamin: false);
						break;
					case 3:
						flag = KeventWatcher.GetInstance(out watcher);
						break;
					case 4:
						flag = FAMWatcher.GetInstance(out watcher, gamin: true);
						break;
					case 5:
						flag = InotifyWatcher.GetInstance(out watcher, gamin: true);
						break;
					}
					if (num == 0 || !flag)
					{
						if (string.Compare(environmentVariable, "disabled", ignoreCase: true) == 0)
						{
							NullFileWatcher.GetInstance(out watcher);
						}
						else
						{
							DefaultWatcher.GetInstance(out watcher);
						}
					}
				}
			}
		}

		[Conditional("TRACE")]
		[Conditional("DEBUG")]
		private void ShowWatcherInfo()
		{
			Console.WriteLine("Watcher implementation: {0}", (watcher == null) ? "<none>" : watcher.GetType().ToString());
		}

		/// <summary>Begins the initialization of a <see cref="T:System.IO.FileSystemWatcher" /> used on a form or used by another component. The initialization occurs at run time.</summary>
		/// <filterpriority>2</filterpriority>
		public void BeginInit()
		{
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.FileSystemWatcher" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (!disposed)
			{
				disposed = true;
				Stop();
			}
			base.Dispose(disposing);
		}

		~FileSystemWatcher()
		{
			disposed = true;
			Stop();
		}

		/// <summary>Ends the initialization of a <see cref="T:System.IO.FileSystemWatcher" /> used on a form or used by another component. The initialization occurs at run time.</summary>
		/// <filterpriority>2</filterpriority>
		public void EndInit()
		{
		}

		private void RaiseEvent(Delegate ev, EventArgs arg, EventType evtype)
		{
			if ((object)ev == null)
			{
				return;
			}
			if (synchronizingObject == null)
			{
				switch (evtype)
				{
				case EventType.RenameEvent:
					((RenamedEventHandler)ev).BeginInvoke(this, (RenamedEventArgs)arg, null, null);
					break;
				case EventType.ErrorEvent:
					((ErrorEventHandler)ev).BeginInvoke(this, (ErrorEventArgs)arg, null, null);
					break;
				case EventType.FileSystemEvent:
					((FileSystemEventHandler)ev).BeginInvoke(this, (FileSystemEventArgs)arg, null, null);
					break;
				}
			}
			else
			{
				synchronizingObject.BeginInvoke(ev, new object[2]
				{
					this,
					arg
				});
			}
		}

		/// <summary>Raises the <see cref="E:System.IO.FileSystemWatcher.Changed" /> event.</summary>
		/// <param name="e">A <see cref="T:System.IO.FileSystemEventArgs" /> that contains the event data. </param>
		protected void OnChanged(FileSystemEventArgs e)
		{
			RaiseEvent(this.Changed, e, EventType.FileSystemEvent);
		}

		/// <summary>Raises the <see cref="E:System.IO.FileSystemWatcher.Created" /> event.</summary>
		/// <param name="e">A <see cref="T:System.IO.FileSystemEventArgs" /> that contains the event data. </param>
		protected void OnCreated(FileSystemEventArgs e)
		{
			RaiseEvent(this.Created, e, EventType.FileSystemEvent);
		}

		/// <summary>Raises the <see cref="E:System.IO.FileSystemWatcher.Deleted" /> event.</summary>
		/// <param name="e">A <see cref="T:System.IO.FileSystemEventArgs" /> that contains the event data. </param>
		protected void OnDeleted(FileSystemEventArgs e)
		{
			RaiseEvent(this.Deleted, e, EventType.FileSystemEvent);
		}

		/// <summary>Raises the <see cref="E:System.IO.FileSystemWatcher.Error" /> event.</summary>
		/// <param name="e">An <see cref="T:System.IO.ErrorEventArgs" /> that contains the event data. </param>
		protected void OnError(ErrorEventArgs e)
		{
			RaiseEvent(this.Error, e, EventType.ErrorEvent);
		}

		/// <summary>Raises the <see cref="E:System.IO.FileSystemWatcher.Renamed" /> event.</summary>
		/// <param name="e">A <see cref="T:System.IO.RenamedEventArgs" /> that contains the event data. </param>
		protected void OnRenamed(RenamedEventArgs e)
		{
			RaiseEvent(this.Renamed, e, EventType.RenameEvent);
		}

		/// <summary>A synchronous method that returns a structure that contains specific information on the change that occurred, given the type of change you want to monitor.</summary>
		/// <returns>A <see cref="T:System.IO.WaitForChangedResult" /> that contains specific information on the change that occurred.</returns>
		/// <param name="changeType">The <see cref="T:System.IO.WatcherChangeTypes" /> to watch for. </param>
		/// <filterpriority>2</filterpriority>
		public WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType)
		{
			return WaitForChanged(changeType, -1);
		}

		/// <summary>A synchronous method that returns a structure that contains specific information on the change that occurred, given the type of change you want to monitor and the time (in milliseconds) to wait before timing out.</summary>
		/// <returns>A <see cref="T:System.IO.WaitForChangedResult" /> that contains specific information on the change that occurred.</returns>
		/// <param name="changeType">The <see cref="T:System.IO.WatcherChangeTypes" /> to watch for. </param>
		/// <param name="timeout">The time (in milliseconds) to wait before timing out. </param>
		/// <filterpriority>2</filterpriority>
		public WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, int timeout)
		{
			WaitForChangedResult result = default(WaitForChangedResult);
			bool flag = EnableRaisingEvents;
			if (!flag)
			{
				EnableRaisingEvents = true;
			}
			bool flag2;
			lock (this)
			{
				waiting = true;
				flag2 = Monitor.Wait(this, timeout);
				if (flag2)
				{
					result = lastData;
				}
			}
			EnableRaisingEvents = flag;
			if (!flag2)
			{
				result.TimedOut = true;
			}
			return result;
		}

		internal void DispatchEvents(FileAction act, string filename, ref RenamedEventArgs renamed)
		{
			if (waiting)
			{
				lastData = default(WaitForChangedResult);
			}
			switch (act)
			{
			case FileAction.Added:
				lastData.Name = filename;
				lastData.ChangeType = WatcherChangeTypes.Created;
				OnCreated(new FileSystemEventArgs(WatcherChangeTypes.Created, path, filename));
				break;
			case FileAction.Removed:
				lastData.Name = filename;
				lastData.ChangeType = WatcherChangeTypes.Deleted;
				OnDeleted(new FileSystemEventArgs(WatcherChangeTypes.Deleted, path, filename));
				break;
			case FileAction.Modified:
				lastData.Name = filename;
				lastData.ChangeType = WatcherChangeTypes.Changed;
				OnChanged(new FileSystemEventArgs(WatcherChangeTypes.Changed, path, filename));
				break;
			case FileAction.RenamedOldName:
				if (renamed != null)
				{
					OnRenamed(renamed);
				}
				lastData.OldName = filename;
				lastData.ChangeType = WatcherChangeTypes.Renamed;
				renamed = new RenamedEventArgs(WatcherChangeTypes.Renamed, path, filename, string.Empty);
				break;
			case FileAction.RenamedNewName:
				lastData.Name = filename;
				lastData.ChangeType = WatcherChangeTypes.Renamed;
				if (renamed == null)
				{
					renamed = new RenamedEventArgs(WatcherChangeTypes.Renamed, path, string.Empty, filename);
				}
				OnRenamed(renamed);
				renamed = null;
				break;
			}
		}

		private void Start()
		{
			watcher.StartDispatching(this);
		}

		private void Stop()
		{
			watcher.StopDispatching(this);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int InternalSupportsFSW();
	}
}
