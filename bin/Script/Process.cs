using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;

namespace System.Diagnostics
{
	/// <summary>Provides access to local and remote processes and enables you to start and stop local system processes.</summary>
	/// <filterpriority>1</filterpriority>
	[Designer("System.Diagnostics.Design.ProcessDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultEvent("Exited")]
	[DefaultProperty("StartInfo")]
	[MonitoringDescription("Represents a system process")]
	public class Process : Component
	{
		private struct ProcInfo
		{
			public IntPtr process_handle;

			public IntPtr thread_handle;

			public int pid;

			public int tid;

			public string[] envKeys;

			public string[] envValues;

			public string UserName;

			public string Domain;

			public IntPtr Password;

			public bool LoadUserProfile;
		}

		[Flags]
		private enum AsyncModes
		{
			NoneYet = 0x0,
			SyncOutput = 0x1,
			SyncError = 0x2,
			AsyncOutput = 0x4,
			AsyncError = 0x8
		}

		[StructLayout(LayoutKind.Sequential)]
		private sealed class ProcessAsyncReader
		{
			public object Sock;

			public IntPtr handle;

			public object state;

			public AsyncCallback callback;

			public ManualResetEvent wait_handle;

			public Exception delayedException;

			public object EndPoint;

			private byte[] buffer = new byte[4196];

			public int Offset;

			public int Size;

			public int SockFlags;

			public object AcceptSocket;

			public object[] Addresses;

			public int port;

			public object Buffers;

			public bool ReuseSocket;

			public object acc_socket;

			public int total;

			public bool completed_sync;

			private bool completed;

			private bool err_out;

			internal int error;

			public int operation = 8;

			public object ares;

			public int EndCalled;

			private Process process;

			private Stream stream;

			private StringBuilder sb = new StringBuilder();

			public AsyncReadHandler ReadHandler;

			public bool IsCompleted => completed;

			public WaitHandle WaitHandle
			{
				get
				{
					lock (this)
					{
						if (wait_handle == null)
						{
							wait_handle = new ManualResetEvent(completed);
						}
						return wait_handle;
						IL_0030:
						WaitHandle result;
						return result;
					}
				}
			}

			public ProcessAsyncReader(Process process, IntPtr handle, bool err_out)
			{
				this.process = process;
				this.handle = handle;
				stream = new FileStream(handle, FileAccess.Read, ownsHandle: false);
				ReadHandler = AddInput;
				this.err_out = err_out;
			}

			public void AddInput()
			{
				lock (this)
				{
					int num = stream.Read(buffer, 0, buffer.Length);
					if (num == 0)
					{
						completed = true;
						if (wait_handle != null)
						{
							wait_handle.Set();
						}
						FlushLast();
					}
					else
					{
						try
						{
							sb.Append(Encoding.Default.GetString(buffer, 0, num));
						}
						catch
						{
							for (int i = 0; i < num; i++)
							{
								sb.Append((char)buffer[i]);
							}
						}
						Flush(last: false);
						ReadHandler.BeginInvoke(null, this);
					}
				}
			}

			private void FlushLast()
			{
				Flush(last: true);
				if (err_out)
				{
					process.OnOutputDataReceived(null);
				}
				else
				{
					process.OnErrorDataReceived(null);
				}
			}

			private void Flush(bool last)
			{
				if (sb.Length == 0 || (err_out && process.output_canceled) || (!err_out && process.error_canceled))
				{
					return;
				}
				string text = sb.ToString();
				sb.Length = 0;
				string[] array = text.Split('\n');
				int num = array.Length;
				if (num == 0)
				{
					return;
				}
				for (int i = 0; i < num - 1; i++)
				{
					if (err_out)
					{
						process.OnOutputDataReceived(array[i]);
					}
					else
					{
						process.OnErrorDataReceived(array[i]);
					}
				}
				string text2 = array[num - 1];
				if (last || (num == 1 && text2 == string.Empty))
				{
					if (err_out)
					{
						process.OnOutputDataReceived(text2);
					}
					else
					{
						process.OnErrorDataReceived(text2);
					}
				}
				else
				{
					sb.Append(text2);
				}
			}

			public void Close()
			{
				stream.Close();
			}
		}

		private class ProcessWaitHandle : WaitHandle
		{
			public ProcessWaitHandle(IntPtr handle)
			{
				Handle = ProcessHandle_duplicate(handle);
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			private static extern IntPtr ProcessHandle_duplicate(IntPtr handle);
		}

		private delegate void AsyncReadHandler();

		private IntPtr process_handle;

		private int pid;

		private bool enableRaisingEvents;

		private bool already_waiting;

		private ISynchronizeInvoke synchronizingObject;

		private EventHandler exited_event;

		private IntPtr stdout_rd;

		private IntPtr stderr_rd;

		private ProcessModuleCollection module_collection;

		private string process_name;

		private StreamReader error_stream;

		private StreamWriter input_stream;

		private StreamReader output_stream;

		private ProcessStartInfo start_info;

		private AsyncModes async_mode;

		private bool output_canceled;

		private bool error_canceled;

		private ProcessAsyncReader async_output;

		private ProcessAsyncReader async_error;

		private bool disposed;

		/// <summary>Gets the base priority of the associated process.</summary>
		/// <returns>The base priority, which is computed from the <see cref="P:System.Diagnostics.Process.PriorityClass" /> of the associated process.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me); set the <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> property to false to access this property on Windows 98 and Windows Me.</exception>
		/// <exception cref="T:System.InvalidOperationException">The process has exited.-or- The process has not started, so there is no process ID. </exception>
		/// <filterpriority>2</filterpriority>
		[MonitoringDescription("Base process priority.")]
		[MonoTODO]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int BasePriority => 0;

		/// <summary>Gets or sets whether the <see cref="E:System.Diagnostics.Process.Exited" /> event should be raised when the process terminates.</summary>
		/// <returns>true if the <see cref="E:System.Diagnostics.Process.Exited" /> event should be raised when the associated process is terminated (through either an exit or a call to <see cref="M:System.Diagnostics.Process.Kill" />); otherwise, false. The default is false.</returns>
		/// <filterpriority>2</filterpriority>
		[MonitoringDescription("Check for exiting of the process to raise the apropriate event.")]
		[Browsable(false)]
		[DefaultValue(false)]
		public bool EnableRaisingEvents
		{
			get
			{
				return enableRaisingEvents;
			}
			set
			{
				bool flag = enableRaisingEvents;
				enableRaisingEvents = value;
				if (enableRaisingEvents && !flag)
				{
					StartExitCallbackIfNeeded();
				}
			}
		}

		/// <summary>Gets the value that the associated process specified when it terminated.</summary>
		/// <returns>The code that the associated process specified when it terminated.</returns>
		/// <exception cref="T:System.InvalidOperationException">The process has not exited.-or- The process <see cref="P:System.Diagnostics.Process.Handle" /> is not valid. </exception>
		/// <exception cref="T:System.NotSupportedException">You are trying to access the <see cref="P:System.Diagnostics.Process.ExitCode" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer.</exception>
		/// <filterpriority>1</filterpriority>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("The exit code of the process.")]
		public int ExitCode
		{
			get
			{
				if (process_handle == IntPtr.Zero)
				{
					throw new InvalidOperationException("Process has not been started.");
				}
				int num = ExitCode_internal(process_handle);
				if (num == 259)
				{
					throw new InvalidOperationException("The process must exit before getting the requested information.");
				}
				return num;
			}
		}

		/// <summary>Gets the time that the associated process exited.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> that indicates when the associated process was terminated.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me), which does not support this property. </exception>
		/// <exception cref="T:System.NotSupportedException">You are trying to access the <see cref="P:System.Diagnostics.Process.ExitTime" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer.</exception>
		/// <filterpriority>1</filterpriority>
		[MonitoringDescription("The exit time of the process.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public DateTime ExitTime
		{
			get
			{
				if (process_handle == IntPtr.Zero)
				{
					throw new InvalidOperationException("Process has not been started.");
				}
				if (!HasExited)
				{
					throw new InvalidOperationException("The process must exit before getting the requested information.");
				}
				return DateTime.FromFileTime(ExitTime_internal(process_handle));
			}
		}

		/// <summary>Gets the native handle of the associated process.</summary>
		/// <returns>The handle that the operating system assigned to the associated process when the process was started. The system uses this handle to keep track of process attributes.</returns>
		/// <exception cref="T:System.InvalidOperationException">The process has not been started. The <see cref="P:System.Diagnostics.Process.Handle" /> property cannot be read because there is no process associated with this <see cref="T:System.Diagnostics.Process" /> instance.-or- The <see cref="T:System.Diagnostics.Process" /> instance has been attached to a running process but you do not have the necessary permissions to get a handle with full access rights. </exception>
		/// <exception cref="T:System.NotSupportedException">You are trying to access the <see cref="P:System.Diagnostics.Process.Handle" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer.</exception>
		/// <filterpriority>1</filterpriority>
		[MonitoringDescription("Handle for this process.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public IntPtr Handle => process_handle;

		/// <summary>Gets the number of handles opened by the process.</summary>
		/// <returns>The number of operating system handles the process has opened.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me); set the <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> property to false to access this property on Windows 98 and Windows Me.</exception>
		/// <filterpriority>2</filterpriority>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonoTODO]
		[MonitoringDescription("Handles for this process.")]
		public int HandleCount => 0;

		/// <summary>Gets a value indicating whether the associated process has been terminated.</summary>
		/// <returns>true if the operating system process referenced by the <see cref="T:System.Diagnostics.Process" /> component has terminated; otherwise, false.</returns>
		/// <exception cref="T:System.InvalidOperationException">There is no process associated with the object. </exception>
		/// <exception cref="T:System.ComponentModel.Win32Exception">The exit code for the process could not be retrieved. </exception>
		/// <exception cref="T:System.NotSupportedException">You are trying to access the <see cref="P:System.Diagnostics.Process.HasExited" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer.</exception>
		/// <filterpriority>1</filterpriority>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("Determines if the process is still running.")]
		public bool HasExited
		{
			get
			{
				if (process_handle == IntPtr.Zero)
				{
					throw new InvalidOperationException("Process has not been started.");
				}
				int num = ExitCode_internal(process_handle);
				if (num == 259)
				{
					return false;
				}
				return true;
			}
		}

		/// <summary>Gets the unique identifier for the associated process.</summary>
		/// <returns>The system-generated unique identifier of the process that is referenced by this <see cref="T:System.Diagnostics.Process" /> instance.</returns>
		/// <exception cref="T:System.InvalidOperationException">The process's <see cref="P:System.Diagnostics.Process.Id" /> property has not been set.-or- There is no process associated with this <see cref="T:System.Diagnostics.Process" /> object. </exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me); set the <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> property to false to access this property on Windows 98 and Windows Me.</exception>
		/// <filterpriority>1</filterpriority>
		[MonitoringDescription("Process identifier.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int Id
		{
			get
			{
				if (pid == 0)
				{
					throw new InvalidOperationException("Process ID has not been set.");
				}
				return pid;
			}
		}

		/// <summary>Gets the name of the computer the associated process is running on.</summary>
		/// <returns>The name of the computer that the associated process is running on.</returns>
		/// <exception cref="T:System.InvalidOperationException">There is no process associated with this <see cref="T:System.Diagnostics.Process" /> object. </exception>
		/// <filterpriority>1</filterpriority>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonoTODO]
		[MonitoringDescription("The name of the computer running the process.")]
		[Browsable(false)]
		public string MachineName => "localhost";

		/// <summary>Gets the main module for the associated process.</summary>
		/// <returns>The <see cref="T:System.Diagnostics.ProcessModule" /> that was used to start the process.</returns>
		/// <exception cref="T:System.NotSupportedException">You are trying to access the <see cref="P:System.Diagnostics.Process.MainModule" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer.</exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me); set <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> to false to access this property on Windows 98 and Windows Me.</exception>
		/// <exception cref="T:System.InvalidOperationException">The process <see cref="P:System.Diagnostics.Process.Id" /> is not available.-or- The process has exited. </exception>
		/// <filterpriority>1</filterpriority>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[MonitoringDescription("The main module of the process.")]
		public ProcessModule MainModule => Modules[0];

		/// <summary>Gets the window handle of the main window of the associated process.</summary>
		/// <returns>The system-generated window handle of the main window of the associated process.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Diagnostics.Process.MainWindowHandle" /> is not defined because the process has exited. </exception>
		/// <exception cref="T:System.NotSupportedException">You are trying to access the <see cref="P:System.Diagnostics.Process.MainWindowHandle" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer.</exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me); set <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> to false to access this property on Windows 98 and Windows Me.</exception>
		/// <filterpriority>2</filterpriority>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonoTODO]
		[MonitoringDescription("The handle of the main window of the process.")]
		public IntPtr MainWindowHandle => (IntPtr)0;

		/// <summary>Gets the caption of the main window of the process.</summary>
		/// <returns>The process's main window title.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Diagnostics.Process.MainWindowTitle" /> property is not defined because the process has exited. </exception>
		/// <exception cref="T:System.NotSupportedException">You are trying to access the <see cref="P:System.Diagnostics.Process.MainWindowTitle" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer.</exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me); set <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> to false to access this property on Windows 98 and Windows Me.</exception>
		/// <filterpriority>1</filterpriority>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("The title of the main window of the process.")]
		[MonoTODO]
		public string MainWindowTitle => "null";

		/// <summary>Gets or sets the maximum allowable working set size for the associated process.</summary>
		/// <returns>The maximum working set size that is allowed in memory for the process, in bytes.</returns>
		/// <exception cref="T:System.ArgumentException">The maximum working set size is invalid. It must be greater than or equal to the minimum working set size.</exception>
		/// <exception cref="T:System.ComponentModel.Win32Exception">Working set information cannot be retrieved from the associated process resource.-or- The process identifier or process handle is zero because the process has not been started. </exception>
		/// <exception cref="T:System.NotSupportedException">You are trying to access the <see cref="P:System.Diagnostics.Process.MaxWorkingSet" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer.</exception>
		/// <exception cref="T:System.InvalidOperationException">The process <see cref="P:System.Diagnostics.Process.Id" /> is not available.-or- The process has exited. </exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me), which does not support this property. </exception>
		/// <filterpriority>2</filterpriority>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("The maximum working set for this process.")]
		public IntPtr MaxWorkingSet
		{
			get
			{
				if (HasExited)
				{
					throw new InvalidOperationException("The process " + ProcessName + " (ID " + Id + ") has exited");
				}
				if (!GetWorkingSet_internal(process_handle, out int _, out int max))
				{
					throw new Win32Exception();
				}
				return (IntPtr)max;
			}
			set
			{
				if (HasExited)
				{
					throw new InvalidOperationException("The process " + ProcessName + " (ID " + Id + ") has exited");
				}
				if (!SetWorkingSet_internal(process_handle, 0, value.ToInt32(), use_min: false))
				{
					throw new Win32Exception();
				}
			}
		}

		/// <summary>Gets or sets the minimum allowable working set size for the associated process.</summary>
		/// <returns>The minimum working set size that is required in memory for the process, in bytes.</returns>
		/// <exception cref="T:System.ArgumentException">The minimum working set size is invalid. It must be less than or equal to the maximum working set size.</exception>
		/// <exception cref="T:System.ComponentModel.Win32Exception">Working set information cannot be retrieved from the associated process resource.-or- The process identifier or process handle is zero because the process has not been started. </exception>
		/// <exception cref="T:System.NotSupportedException">You are trying to access the <see cref="P:System.Diagnostics.Process.MinWorkingSet" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer. </exception>
		/// <exception cref="T:System.InvalidOperationException">The process <see cref="P:System.Diagnostics.Process.Id" /> is not available.-or- The process has exited.</exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me), which does not support this property. </exception>
		/// <filterpriority>2</filterpriority>
		[MonitoringDescription("The minimum working set for this process.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IntPtr MinWorkingSet
		{
			get
			{
				if (HasExited)
				{
					throw new InvalidOperationException("The process " + ProcessName + " (ID " + Id + ") has exited");
				}
				if (!GetWorkingSet_internal(process_handle, out int min, out int _))
				{
					throw new Win32Exception();
				}
				return (IntPtr)min;
			}
			set
			{
				if (HasExited)
				{
					throw new InvalidOperationException("The process " + ProcessName + " (ID " + Id + ") has exited");
				}
				if (!SetWorkingSet_internal(process_handle, value.ToInt32(), 0, use_min: true))
				{
					throw new Win32Exception();
				}
			}
		}

		/// <summary>Gets the modules that have been loaded by the associated process.</summary>
		/// <returns>An array of type <see cref="T:System.Diagnostics.ProcessModule" /> that represents the modules that have been loaded by the associated process.</returns>
		/// <exception cref="T:System.NotSupportedException">You are attempting to access the <see cref="P:System.Diagnostics.Process.Modules" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer. </exception>
		/// <exception cref="T:System.InvalidOperationException">The process <see cref="P:System.Diagnostics.Process.Id" /> is not available.</exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me); set <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> to false to access this property on Windows 98 and Windows Me.</exception>
		/// <exception cref="T:System.ComponentModel.Win32Exception">You are attempting to access the <see cref="P:System.Diagnostics.Process.Modules" /> property for either the system process or the idle process. These processes do not have modules.</exception>
		/// <filterpriority>2</filterpriority>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[MonitoringDescription("The modules that are loaded as part of this process.")]
		public ProcessModuleCollection Modules
		{
			get
			{
				if (module_collection == null)
				{
					module_collection = new ProcessModuleCollection(GetModules_internal(process_handle));
				}
				return module_collection;
			}
		}

		/// <summary>Gets the nonpaged system memory size allocated to this process.</summary>
		/// <returns>The amount of memory, in bytes, the system has allocated for the associated process that cannot be written to the virtual memory paging file.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me), which does not support this property. </exception>
		/// <filterpriority>2</filterpriority>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("The number of bytes that are not pageable.")]
		[MonoTODO]
		[Obsolete("Use NonpagedSystemMemorySize64")]
		public int NonpagedSystemMemorySize => 0;

		/// <summary>Gets the paged memory size.</summary>
		/// <returns>The amount of memory, in bytes, allocated by the associated process that can be written to the virtual memory paging file.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me), which does not support this property. </exception>
		/// <filterpriority>2</filterpriority>
		[MonitoringDescription("The number of bytes that are paged.")]
		[MonoTODO]
		[Obsolete("Use PagedMemorySize64")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int PagedMemorySize => 0;

		/// <summary>Gets the paged system memory size.</summary>
		/// <returns>The amount of memory, in bytes, the system has allocated for the associated process that can be written to the virtual memory paging file.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me), which does not support this property. </exception>
		/// <filterpriority>2</filterpriority>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonoTODO]
		[Obsolete("Use PagedSystemMemorySize64")]
		[MonitoringDescription("The amount of paged system memory in bytes.")]
		public int PagedSystemMemorySize => 0;

		/// <summary>Gets the peak paged memory size.</summary>
		/// <returns>The maximum amount of memory, in bytes, allocated by the associated process that could be written to the virtual memory paging file.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me), which does not support this property. </exception>
		/// <filterpriority>2</filterpriority>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Obsolete("Use PeakPagedMemorySize64")]
		[MonoTODO]
		[MonitoringDescription("The maximum amount of paged memory used by this process.")]
		public int PeakPagedMemorySize => 0;

		/// <summary>Gets the peak virtual memory size.</summary>
		/// <returns>The maximum amount of virtual memory, in bytes, that the associated process has requested.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me), which does not support this property. </exception>
		/// <filterpriority>2</filterpriority>
		[MonitoringDescription("The maximum amount of virtual memory used by this process.")]
		[Obsolete("Use PeakVirtualMemorySize64")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int PeakVirtualMemorySize
		{
			get
			{
				int error;
				return (int)GetProcessData(pid, 8, out error);
			}
		}

		/// <summary>Gets the peak working set size for the associated process.</summary>
		/// <returns>The maximum amount of physical memory that the associated process has required all at once, in bytes.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me), which does not support this property. </exception>
		/// <filterpriority>2</filterpriority>
		[MonitoringDescription("The maximum amount of system memory used by this process.")]
		[Obsolete("Use PeakWorkingSet64")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int PeakWorkingSet
		{
			get
			{
				int error;
				return (int)GetProcessData(pid, 5, out error);
			}
		}

		/// <summary>Gets the amount of nonpaged system memory allocated for the associated process.</summary>
		/// <returns>The amount of system memory, in bytes, allocated for the associated process that cannot be written to the virtual memory paging file.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me), which does not support this property.</exception>
		/// <filterpriority>2</filterpriority>
		[MonoTODO]
		[ComVisible(false)]
		[MonitoringDescription("The number of bytes that are not pageable.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public long NonpagedSystemMemorySize64 => 0L;

		/// <summary>Gets the amount of paged memory allocated for the associated process.</summary>
		/// <returns>The amount of memory, in bytes, allocated in the virtual memory paging file for the associated process.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me), which does not support this property.</exception>
		/// <filterpriority>2</filterpriority>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("The number of bytes that are paged.")]
		[MonoTODO]
		[ComVisible(false)]
		public long PagedMemorySize64 => 0L;

		/// <summary>Gets the amount of pageable system memory allocated for the associated process.</summary>
		/// <returns>The amount of system memory, in bytes, allocated for the associated process that can be written to the virtual memory paging file.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me), which does not support this property.</exception>
		/// <filterpriority>2</filterpriority>
		[ComVisible(false)]
		[MonoTODO]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("The amount of paged system memory in bytes.")]
		public long PagedSystemMemorySize64 => 0L;

		/// <summary>Gets the maximum amount of memory in the virtual memory paging file used by the associated process.</summary>
		/// <returns>The maximum amount of memory, in bytes, allocated in the virtual memory paging file for the associated process since it was started.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me), which does not support this property.</exception>
		/// <filterpriority>2</filterpriority>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("The maximum amount of paged memory used by this process.")]
		[ComVisible(false)]
		[MonoTODO]
		public long PeakPagedMemorySize64 => 0L;

		/// <summary>Gets the maximum amount of virtual memory used by the associated process.</summary>
		/// <returns>The maximum amount of virtual memory, in bytes, allocated for the associated process since it was started.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me), which does not support this property.</exception>
		/// <filterpriority>2</filterpriority>
		[MonitoringDescription("The maximum amount of virtual memory used by this process.")]
		[ComVisible(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public long PeakVirtualMemorySize64
		{
			get
			{
				int error;
				return GetProcessData(pid, 8, out error);
			}
		}

		/// <summary>Gets the maximum amount of physical memory used by the associated process.</summary>
		/// <returns>The maximum amount of physical memory, in bytes, allocated for the associated process since it was started.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me), which does not support this property.</exception>
		/// <filterpriority>2</filterpriority>
		[MonitoringDescription("The maximum amount of system memory used by this process.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[ComVisible(false)]
		public long PeakWorkingSet64
		{
			get
			{
				int error;
				return GetProcessData(pid, 5, out error);
			}
		}

		/// <summary>Gets or sets a value indicating whether the associated process priority should temporarily be boosted by the operating system when the main window has the focus.</summary>
		/// <returns>true if dynamic boosting of the process priority should take place for a process when it is taken out of the wait state; otherwise, false. The default is false.</returns>
		/// <exception cref="T:System.ComponentModel.Win32Exception">Priority boost information could not be retrieved from the associated process resource. </exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me), which does not support this property.-or- The process identifier or process handle is zero. (The process has not been started.) </exception>
		/// <exception cref="T:System.NotSupportedException">You are attempting to access the <see cref="P:System.Diagnostics.Process.PriorityBoostEnabled" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer. </exception>
		/// <exception cref="T:System.InvalidOperationException">The process <see cref="P:System.Diagnostics.Process.Id" /> is not available.</exception>
		/// <filterpriority>1</filterpriority>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonoTODO]
		[MonitoringDescription("Process will be of higher priority while it is actively used.")]
		public bool PriorityBoostEnabled
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		/// <summary>Gets or sets the overall priority category for the associated process.</summary>
		/// <returns>The priority category for the associated process, from which the <see cref="P:System.Diagnostics.Process.BasePriority" /> of the process is calculated.</returns>
		/// <exception cref="T:System.ComponentModel.Win32Exception">Process priority information could not be set or retrieved from the associated process resource.-or- The process identifier or process handle is zero. (The process has not been started.) </exception>
		/// <exception cref="T:System.NotSupportedException">You are attempting to access the <see cref="P:System.Diagnostics.Process.PriorityClass" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer. </exception>
		/// <exception cref="T:System.InvalidOperationException">The process <see cref="P:System.Diagnostics.Process.Id" /> is not available.</exception>
		/// <exception cref="T:System.PlatformNotSupportedException">You have set the <see cref="P:System.Diagnostics.Process.PriorityClass" /> to AboveNormal or BelowNormal when using Windows 98 or Windows Millennium Edition (Windows Me). These platforms do not support those values for the priority class. </exception>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">Priority class cannot be set because it does not use a valid value, as defined in the <see cref="T:System.Diagnostics.ProcessPriorityClass" /> enumeration.</exception>
		/// <filterpriority>1</filterpriority>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonoLimitation("Under Unix, only root is allowed to raise the priority.")]
		[MonitoringDescription("The relative process priority.")]
		public ProcessPriorityClass PriorityClass
		{
			get
			{
				if (process_handle == IntPtr.Zero)
				{
					throw new InvalidOperationException("Process has not been started.");
				}
				int error;
				int priorityClass = GetPriorityClass(process_handle, out error);
				if (priorityClass == 0)
				{
					throw new Win32Exception(error);
				}
				return (ProcessPriorityClass)priorityClass;
			}
			set
			{
				if (!Enum.IsDefined(typeof(ProcessPriorityClass), value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ProcessPriorityClass));
				}
				if (process_handle == IntPtr.Zero)
				{
					throw new InvalidOperationException("Process has not been started.");
				}
				if (!SetPriorityClass(process_handle, (int)value, out int error))
				{
					throw new Win32Exception(error);
				}
			}
		}

		/// <summary>Gets the private memory size.</summary>
		/// <returns>The number of bytes allocated by the associated process that cannot be shared with other processes.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me), which does not support this property. </exception>
		/// <filterpriority>2</filterpriority>
		[MonitoringDescription("The amount of memory exclusively used by this process.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Obsolete("Use PrivateMemorySize64")]
		public int PrivateMemorySize
		{
			get
			{
				int error;
				return (int)GetProcessData(pid, 6, out error);
			}
		}

		/// <summary>Gets the Terminal Services session identifier for the associated process.</summary>
		/// <returns>The Terminal Services session identifier for the associated process.</returns>
		/// <exception cref="T:System.NullReferenceException">There is no session associated with this process.</exception>
		/// <exception cref="T:System.InvalidOperationException">There is no process associated with this session identifier.-or-The associated process is not on this machine. </exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The <see cref="P:System.Diagnostics.Process.SessionId" /> property is not supported on Windows 98.</exception>
		/// <filterpriority>1</filterpriority>
		[MonitoringDescription("The session ID for this process.")]
		[MonoNotSupported("")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int SessionId
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>Gets the privileged processor time for this process.</summary>
		/// <returns>A <see cref="T:System.TimeSpan" /> that indicates the amount of time that the process has spent running code inside the operating system core.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me), which does not support this property. </exception>
		/// <exception cref="T:System.NotSupportedException">You are attempting to access the <see cref="P:System.Diagnostics.Process.PrivilegedProcessorTime" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer. </exception>
		/// <filterpriority>2</filterpriority>
		[MonitoringDescription("The amount of processing time spent in the OS core for this process.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TimeSpan PrivilegedProcessorTime => new TimeSpan(Times(process_handle, 1));

		/// <summary>Gets the name of the process.</summary>
		/// <returns>The name that the system uses to identify the process to the user.</returns>
		/// <exception cref="T:System.InvalidOperationException">The process does not have an identifier, or no process is associated with the <see cref="T:System.Diagnostics.Process" />.-or- The associated process has exited. </exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me); set <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> to false to access this property on Windows 98 and Windows Me.</exception>
		/// <filterpriority>1</filterpriority>
		[MonitoringDescription("The name of this process.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string ProcessName
		{
			get
			{
				if (process_name == null)
				{
					if (process_handle == IntPtr.Zero)
					{
						throw new InvalidOperationException("No process is associated with this object.");
					}
					process_name = ProcessName_internal(process_handle);
					if (process_name == null)
					{
						throw new InvalidOperationException("Process has exited, so the requested information is not available.");
					}
					if (process_name.EndsWith(".exe") || process_name.EndsWith(".bat") || process_name.EndsWith(".com"))
					{
						process_name = process_name.Substring(0, process_name.Length - 4);
					}
				}
				return process_name;
			}
		}

		/// <summary>Gets or sets the processors on which the threads in this process can be scheduled to run.</summary>
		/// <returns>A bitmask representing the processors that the threads in the associated process can run on. The default depends on the number of processors on the computer. The default value is 2 n -1, where n is the number of processors.</returns>
		/// <exception cref="T:System.ComponentModel.Win32Exception">
		///   <see cref="P:System.Diagnostics.Process.ProcessorAffinity" /> information could not be set or retrieved from the associated process resource.-or- The process identifier or process handle is zero. (The process has not been started.) </exception>
		/// <exception cref="T:System.NotSupportedException">You are attempting to access the <see cref="P:System.Diagnostics.Process.ProcessorAffinity" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer. </exception>
		/// <exception cref="T:System.InvalidOperationException">The process <see cref="P:System.Diagnostics.Process.Id" /> was not available.-or- The process has exited. </exception>
		/// <filterpriority>2</filterpriority>
		[MonoTODO]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("Allowed processor that can be used by this process.")]
		public IntPtr ProcessorAffinity
		{
			get
			{
				return (IntPtr)0;
			}
			set
			{
			}
		}

		/// <summary>Gets a value indicating whether the user interface of the process is responding.</summary>
		/// <returns>true if the user interface of the associated process is responding to the system; otherwise, false.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me); set <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> to false to access this property on Windows 98 and Windows Me.</exception>
		/// <exception cref="T:System.InvalidOperationException">There is no process associated with this <see cref="T:System.Diagnostics.Process" /> object. </exception>
		/// <exception cref="T:System.NotSupportedException">You are attempting to access the <see cref="P:System.Diagnostics.Process.Responding" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer. </exception>
		/// <filterpriority>1</filterpriority>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonoTODO]
		[MonitoringDescription("Is this process responsive.")]
		public bool Responding => false;

		/// <summary>Gets a stream used to read the error output of the application.</summary>
		/// <returns>A <see cref="T:System.IO.StreamReader" /> that can be used to read the standard error stream of the application.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Diagnostics.Process.StandardError" /> stream has not been defined for redirection; ensure <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardError" /> is set to true and <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> is set to false.- or - The <see cref="P:System.Diagnostics.Process.StandardError" /> stream has been opened for asynchronous read operations with <see cref="M:System.Diagnostics.Process.BeginErrorReadLine" />. </exception>
		/// <filterpriority>1</filterpriority>
		[MonitoringDescription("The standard error stream of this process.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public StreamReader StandardError
		{
			get
			{
				if (error_stream == null)
				{
					throw new InvalidOperationException("Standard error has not been redirected");
				}
				if ((async_mode & AsyncModes.AsyncError) != 0)
				{
					throw new InvalidOperationException("Cannot mix asynchronous and synchonous reads.");
				}
				async_mode |= AsyncModes.SyncError;
				return error_stream;
			}
		}

		/// <summary>Gets a stream used to write the input of the application.</summary>
		/// <returns>A <see cref="T:System.IO.StreamWriter" /> that can be used to write the standard input stream of the application.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Diagnostics.Process.StandardInput" /> stream has not been defined because <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardInput" /> is set to false. </exception>
		/// <filterpriority>1</filterpriority>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[MonitoringDescription("The standard input stream of this process.")]
		public StreamWriter StandardInput
		{
			get
			{
				if (input_stream == null)
				{
					throw new InvalidOperationException("Standard input has not been redirected");
				}
				return input_stream;
			}
		}

		/// <summary>Gets a stream used to read the output of the application.</summary>
		/// <returns>A <see cref="T:System.IO.StreamReader" /> that can be used to read the standard output stream of the application.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Diagnostics.Process.StandardOutput" /> stream has not been defined for redirection; ensure <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardOutput" /> is set to true and <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> is set to false.- or - The <see cref="P:System.Diagnostics.Process.StandardOutput" /> stream has been opened for asynchronous read operations with <see cref="M:System.Diagnostics.Process.BeginOutputReadLine" />. </exception>
		/// <filterpriority>1</filterpriority>
		[MonitoringDescription("The standard output stream of this process.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public StreamReader StandardOutput
		{
			get
			{
				if (output_stream == null)
				{
					throw new InvalidOperationException("Standard output has not been redirected");
				}
				if ((async_mode & AsyncModes.AsyncOutput) != 0)
				{
					throw new InvalidOperationException("Cannot mix asynchronous and synchonous reads.");
				}
				async_mode |= AsyncModes.SyncOutput;
				return output_stream;
			}
		}

		/// <summary>Gets or sets the properties to pass to the <see cref="M:System.Diagnostics.Process.Start" /> method of the <see cref="T:System.Diagnostics.Process" />.</summary>
		/// <returns>The <see cref="T:System.Diagnostics.ProcessStartInfo" /> that represents the data with which to start the process. These arguments include the name of the executable file or document used to start the process.</returns>
		/// <exception cref="T:System.ArgumentNullException">The value that specifies the <see cref="P:System.Diagnostics.Process.StartInfo" /> is null. </exception>
		/// <filterpriority>1</filterpriority>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Browsable(false)]
		[MonitoringDescription("Information for the start of this process.")]
		public ProcessStartInfo StartInfo
		{
			get
			{
				if (start_info == null)
				{
					start_info = new ProcessStartInfo();
				}
				return start_info;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				start_info = value;
			}
		}

		/// <summary>Gets the time that the associated process was started.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> that indicates when the process started. This only has meaning for started processes.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me), which does not support this property. </exception>
		/// <exception cref="T:System.NotSupportedException">You are attempting to access the <see cref="P:System.Diagnostics.Process.StartTime" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer. </exception>
		/// <exception cref="T:System.InvalidOperationException">The process has exited.</exception>
		/// <exception cref="T:System.ComponentModel.Win32Exception">An error occurred in the call to the Windows function.</exception>
		/// <filterpriority>1</filterpriority>
		[MonitoringDescription("The time this process started.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DateTime StartTime => DateTime.FromFileTime(StartTime_internal(process_handle));

		/// <summary>Gets or sets the object used to marshal the event handler calls that are issued as a result of a process exit event.</summary>
		/// <returns>The <see cref="T:System.ComponentModel.ISynchronizeInvoke" /> used to marshal event handler calls that are issued as a result of an <see cref="E:System.Diagnostics.Process.Exited" /> event on the process.</returns>
		/// <filterpriority>2</filterpriority>
		[Browsable(false)]
		[DefaultValue(null)]
		[MonitoringDescription("The object that is used to synchronize event handler calls for this process.")]
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

		/// <summary>Gets the set of threads that are running in the associated process.</summary>
		/// <returns>An array of type <see cref="T:System.Diagnostics.ProcessThread" /> representing the operating system threads currently running in the associated process.</returns>
		/// <exception cref="T:System.SystemException">The process does not have an <see cref="P:System.Diagnostics.Process.Id" />, or no process is associated with the <see cref="T:System.Diagnostics.Process" /> instance.-or- The associated process has exited. </exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me); set <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> to false to access this property on Windows 98 and Windows Me.</exception>
		/// <filterpriority>1</filterpriority>
		[Browsable(false)]
		[MonitoringDescription("The number of threads of this process.")]
		[MonoTODO]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ProcessThreadCollection Threads => ProcessThreadCollection.GetEmpty();

		/// <summary>Gets the total processor time for this process.</summary>
		/// <returns>A <see cref="T:System.TimeSpan" /> that indicates the amount of time that the associated process has spent utilizing the CPU. This value is the sum of the <see cref="P:System.Diagnostics.Process.UserProcessorTime" /> and the <see cref="P:System.Diagnostics.Process.PrivilegedProcessorTime" />.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me), which does not support this property. </exception>
		/// <exception cref="T:System.NotSupportedException">You are attempting to access the <see cref="P:System.Diagnostics.Process.TotalProcessorTime" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer. </exception>
		/// <filterpriority>2</filterpriority>
		[MonitoringDescription("The total CPU time spent for this process.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TimeSpan TotalProcessorTime => new TimeSpan(Times(process_handle, 2));

		/// <summary>Gets the user processor time for this process.</summary>
		/// <returns>A <see cref="T:System.TimeSpan" /> that indicates the amount of time that the associated process has spent running code inside the application portion of the process (not inside the operating system core).</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me), which does not support this property. </exception>
		/// <exception cref="T:System.NotSupportedException">You are attempting to access the <see cref="P:System.Diagnostics.Process.UserProcessorTime" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer. </exception>
		/// <filterpriority>2</filterpriority>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("The CPU time spent for this process in user mode.")]
		public TimeSpan UserProcessorTime => new TimeSpan(Times(process_handle, 0));

		/// <summary>Gets the size of the process's virtual memory.</summary>
		/// <returns>The amount of virtual memory, in bytes, that the associated process has requested.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me), which does not support this property. </exception>
		/// <filterpriority>2</filterpriority>
		[MonitoringDescription("The amount of virtual memory currently used for this process.")]
		[Obsolete("Use VirtualMemorySize64")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int VirtualMemorySize
		{
			get
			{
				int error;
				return (int)GetProcessData(pid, 7, out error);
			}
		}

		/// <summary>Gets the associated process's physical memory usage.</summary>
		/// <returns>The total amount of physical memory the associated process is using, in bytes.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me), which does not support this property. </exception>
		/// <filterpriority>2</filterpriority>
		[Obsolete("Use WorkingSet64")]
		[MonitoringDescription("The amount of physical memory currently used for this process.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int WorkingSet
		{
			get
			{
				int error;
				return (int)GetProcessData(pid, 4, out error);
			}
		}

		/// <summary>Gets the amount of private memory allocated for the associated process.</summary>
		/// <returns>The amount of memory, in bytes, allocated for the associated process that cannot be shared with other processes.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me), which does not support this property.</exception>
		/// <filterpriority>2</filterpriority>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("The amount of memory exclusively used by this process.")]
		[ComVisible(false)]
		public long PrivateMemorySize64
		{
			get
			{
				int error;
				return GetProcessData(pid, 6, out error);
			}
		}

		/// <summary>Gets the amount of the virtual memory allocated for the associated process.</summary>
		/// <returns>The amount of virtual memory, in bytes, allocated for the associated process.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me), which does not support this property.</exception>
		/// <filterpriority>2</filterpriority>
		[ComVisible(false)]
		[MonitoringDescription("The amount of virtual memory currently used for this process.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public long VirtualMemorySize64
		{
			get
			{
				int error;
				return GetProcessData(pid, 7, out error);
			}
		}

		/// <summary>Gets the amount of physical memory allocated for the associated process.</summary>
		/// <returns>The amount of physical memory, in bytes, allocated for the associated process.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me), which does not support this property.</exception>
		/// <filterpriority>2</filterpriority>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("The amount of physical memory currently used for this process.")]
		[ComVisible(false)]
		public long WorkingSet64
		{
			get
			{
				int error;
				return GetProcessData(pid, 4, out error);
			}
		}

		private static bool IsWindows
		{
			get
			{
				PlatformID platform = Environment.OSVersion.Platform;
				if (platform == PlatformID.Win32S || platform == PlatformID.Win32Windows || platform == PlatformID.Win32NT || platform == PlatformID.WinCE)
				{
					return true;
				}
				return false;
			}
		}

		/// <summary>Occurs when an application writes to its redirected <see cref="P:System.Diagnostics.Process.StandardOutput" /> stream.</summary>
		/// <filterpriority>2</filterpriority>
		[Browsable(true)]
		[MonitoringDescription("Raised when it receives output data")]
		public event DataReceivedEventHandler OutputDataReceived;

		/// <summary>Occurs when an application writes to its redirected <see cref="P:System.Diagnostics.Process.StandardError" /> stream.</summary>
		/// <filterpriority>2</filterpriority>
		[Browsable(true)]
		[MonitoringDescription("Raised when it receives error data")]
		public event DataReceivedEventHandler ErrorDataReceived;

		/// <summary>Occurs when a process exits.</summary>
		/// <filterpriority>2</filterpriority>
		[Category("Behavior")]
		[MonitoringDescription("Raised when this process exits.")]
		public event EventHandler Exited
		{
			add
			{
				if (process_handle != IntPtr.Zero && HasExited)
				{
					value.BeginInvoke(null, null, null, null);
					return;
				}
				exited_event = (EventHandler)Delegate.Combine(exited_event, value);
				if (exited_event != null)
				{
					StartExitCallbackIfNeeded();
				}
			}
			remove
			{
				exited_event = (EventHandler)Delegate.Remove(exited_event, value);
			}
		}

		private Process(IntPtr handle, int id)
		{
			process_handle = handle;
			pid = id;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Diagnostics.Process" /> class.</summary>
		public Process()
		{
		}

		private void StartExitCallbackIfNeeded()
		{
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int ExitCode_internal(IntPtr handle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern long ExitTime_internal(IntPtr handle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetWorkingSet_internal(IntPtr handle, out int min, out int max);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SetWorkingSet_internal(IntPtr handle, int min, int max, bool use_min);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern ProcessModule[] GetModules_internal(IntPtr handle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern long GetProcessData(int pid, int data_type, out int error);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetPriorityClass(IntPtr handle, out int error);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SetPriorityClass(IntPtr handle, int priority, out int error);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern long Times(IntPtr handle, int type);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string ProcessName_internal(IntPtr handle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern long StartTime_internal(IntPtr handle);

		/// <summary>Frees all the resources that are associated with this component.</summary>
		/// <filterpriority>2</filterpriority>
		public void Close()
		{
			Dispose(disposing: true);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Kill_internal(IntPtr handle, int signo);

		private bool Close(int signo)
		{
			if (process_handle == IntPtr.Zero)
			{
				throw new SystemException("No process to kill.");
			}
			int num = ExitCode_internal(process_handle);
			if (num != 259)
			{
				throw new InvalidOperationException("The process already finished.");
			}
			return Kill_internal(process_handle, signo);
		}

		/// <summary>Closes a process that has a user interface by sending a close message to its main window.</summary>
		/// <returns>true if the close message was successfully sent; false if the associated process does not have a main window or if the main window is disabled (for example if a modal dialog is being shown).</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">The platform is Windows 98 or Windows Millennium Edition (Windows Me); set the <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> property to false to access this property on Windows 98 and Windows Me.</exception>
		/// <exception cref="T:System.InvalidOperationException">The process has already exited. -or-No process is associated with this <see cref="T:System.Diagnostics.Process" /> object.</exception>
		/// <filterpriority>1</filterpriority>
		public bool CloseMainWindow()
		{
			return Close(2);
		}

		/// <summary>Puts a <see cref="T:System.Diagnostics.Process" /> component in state to interact with operating system processes that run in a special mode by enabling the native property SeDebugPrivilege on the current thread.</summary>
		/// <filterpriority>2</filterpriority>
		[MonoTODO]
		public static void EnterDebugMode()
		{
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr GetProcess_internal(int pid);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetPid_internal();

		/// <summary>Gets a new <see cref="T:System.Diagnostics.Process" /> component and associates it with the currently active process.</summary>
		/// <returns>A new <see cref="T:System.Diagnostics.Process" /> component associated with the process resource that is running the calling application.</returns>
		/// <filterpriority>1</filterpriority>
		public static Process GetCurrentProcess()
		{
			int pid_internal = GetPid_internal();
			IntPtr process_internal = GetProcess_internal(pid_internal);
			if (process_internal == IntPtr.Zero)
			{
				throw new SystemException("Can't find current process");
			}
			return new Process(process_internal, pid_internal);
		}

		/// <summary>Returns a new <see cref="T:System.Diagnostics.Process" /> component, given the identifier of a process on the local computer.</summary>
		/// <returns>A <see cref="T:System.Diagnostics.Process" /> component that is associated with the local process resource identified by the <paramref name="processId" /> parameter.</returns>
		/// <param name="processId">The system-unique identifier of a process resource. </param>
		/// <exception cref="T:System.ArgumentException">The process specified by the <paramref name="processId" /> parameter is not running. The identifier might be expired. </exception>
		/// <exception cref="T:System.InvalidOperationException">The process was not started by this object.</exception>
		/// <filterpriority>1</filterpriority>
		public static Process GetProcessById(int processId)
		{
			IntPtr process_internal = GetProcess_internal(processId);
			if (process_internal == IntPtr.Zero)
			{
				throw new ArgumentException("Can't find process with ID " + processId.ToString());
			}
			return new Process(process_internal, processId);
		}

		/// <summary>Returns a new <see cref="T:System.Diagnostics.Process" /> component, given a process identifier and the name of a computer on the network.</summary>
		/// <returns>A <see cref="T:System.Diagnostics.Process" /> component that is associated with a remote process resource identified by the <paramref name="processId" /> parameter.</returns>
		/// <param name="processId">The system-unique identifier of a process resource. </param>
		/// <param name="machineName">The name of a computer on the network. </param>
		/// <exception cref="T:System.ArgumentException">The process specified by the <paramref name="processId" /> parameter is not running. The identifier might be expired.-or- The <paramref name="machineName" /> parameter syntax is invalid. The name might have length zero (0). </exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="machineName" /> parameter is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The process was not started by this object.</exception>
		/// <filterpriority>1</filterpriority>
		[MonoTODO("There is no support for retrieving process information from a remote machine")]
		public static Process GetProcessById(int processId, string machineName)
		{
			if (machineName == null)
			{
				throw new ArgumentNullException("machineName");
			}
			if (!IsLocalMachine(machineName))
			{
				throw new NotImplementedException();
			}
			return GetProcessById(processId);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int[] GetProcesses_internal();

		/// <summary>Creates a new <see cref="T:System.Diagnostics.Process" /> component for each process resource on the local computer.</summary>
		/// <returns>An array of type <see cref="T:System.Diagnostics.Process" /> that represents all the process resources running on the local computer.</returns>
		/// <filterpriority>1</filterpriority>
		public static Process[] GetProcesses()
		{
			int[] processes_internal = GetProcesses_internal();
			ArrayList arrayList = new ArrayList();
			if (processes_internal == null)
			{
				return new Process[0];
			}
			for (int i = 0; i < processes_internal.Length; i++)
			{
				try
				{
					arrayList.Add(GetProcessById(processes_internal[i]));
				}
				catch (SystemException)
				{
				}
			}
			return (Process[])arrayList.ToArray(typeof(Process));
		}

		/// <summary>Creates a new <see cref="T:System.Diagnostics.Process" /> component for each process resource on the specified computer.</summary>
		/// <returns>An array of type <see cref="T:System.Diagnostics.Process" /> that represents all the process resources running on the specified computer.</returns>
		/// <param name="machineName">The computer from which to read the list of processes. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="machineName" /> parameter syntax is invalid. It might have length zero (0). </exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="machineName" /> parameter is null. </exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The operating system platform does not support this operation on remote computers. </exception>
		/// <exception cref="T:System.InvalidOperationException">There are problems accessing the performance counter API's used to get process information. This exception is specific to Windows NT, Windows 2000, and Windows XP. </exception>
		/// <exception cref="T:System.ComponentModel.Win32Exception">A problem occurred accessing an underlying system API. </exception>
		/// <filterpriority>1</filterpriority>
		[MonoTODO("There is no support for retrieving process information from a remote machine")]
		public static Process[] GetProcesses(string machineName)
		{
			if (machineName == null)
			{
				throw new ArgumentNullException("machineName");
			}
			if (!IsLocalMachine(machineName))
			{
				throw new NotImplementedException();
			}
			return GetProcesses();
		}

		/// <summary>Creates an array of new <see cref="T:System.Diagnostics.Process" /> components and associates them with all the process resources on the local computer that share the specified process name.</summary>
		/// <returns>An array of type <see cref="T:System.Diagnostics.Process" /> that represents the process resources running the specified application or file.</returns>
		/// <param name="processName">The friendly name of the process. </param>
		/// <exception cref="T:System.InvalidOperationException">There are problems accessing the performance counter API's used to get process information. This exception is specific to Windows NT, Windows 2000, and Windows XP. </exception>
		/// <filterpriority>1</filterpriority>
		public static Process[] GetProcessesByName(string processName)
		{
			Process[] processes = GetProcesses();
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < processes.Length; i++)
			{
				try
				{
					if (string.Compare(processName, processes[i].ProcessName, ignoreCase: true) == 0)
					{
						arrayList.Add(processes[i]);
					}
				}
				catch (Exception)
				{
				}
			}
			return (Process[])arrayList.ToArray(typeof(Process));
		}

		/// <summary>Creates an array of new <see cref="T:System.Diagnostics.Process" /> components and associates them with all the process resources on a remote computer that share the specified process name.</summary>
		/// <returns>An array of type <see cref="T:System.Diagnostics.Process" /> that represents the process resources running the specified application or file.</returns>
		/// <param name="processName">The friendly name of the process. </param>
		/// <param name="machineName">The name of a computer on the network. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="machineName" /> parameter syntax is invalid. It might have length zero (0). </exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="machineName" /> parameter is null. </exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The operating system platform does not support this operation on remote computers. </exception>
		/// <exception cref="T:System.InvalidOperationException">There are problems accessing the performance counter API's used to get process information. This exception is specific to Windows NT, Windows 2000, and Windows XP. </exception>
		/// <exception cref="T:System.ComponentModel.Win32Exception">A problem occurred accessing an underlying system API. </exception>
		/// <filterpriority>1</filterpriority>
		[MonoTODO]
		public static Process[] GetProcessesByName(string processName, string machineName)
		{
			throw new NotImplementedException();
		}

		/// <summary>Immediately stops the associated process.</summary>
		/// <exception cref="T:System.ComponentModel.Win32Exception">The associated process could not be terminated. -or-The process is terminating.-or- The associated process is a Win16 executable.</exception>
		/// <exception cref="T:System.NotSupportedException">You are attempting to call <see cref="M:System.Diagnostics.Process.Kill" /> for a process that is running on a remote computer. The method is available only for processes running on the local computer.</exception>
		/// <exception cref="T:System.InvalidOperationException">The process has already exited. -or-There is no process associated with this <see cref="T:System.Diagnostics.Process" /> object.</exception>
		/// <filterpriority>1</filterpriority>
		public void Kill()
		{
			Close(1);
		}

		/// <summary>Takes a <see cref="T:System.Diagnostics.Process" /> component out of the state that lets it interact with operating system processes that run in a special mode.</summary>
		/// <filterpriority>2</filterpriority>
		[MonoTODO]
		public static void LeaveDebugMode()
		{
		}

		/// <summary>Discards any information about the associated process that has been cached inside the process component.</summary>
		/// <filterpriority>1</filterpriority>
		public void Refresh()
		{
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool ShellExecuteEx_internal(ProcessStartInfo startInfo, ref ProcInfo proc_info);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CreateProcess_internal(ProcessStartInfo startInfo, IntPtr stdin, IntPtr stdout, IntPtr stderr, ref ProcInfo proc_info);

		private static bool Start_shell(ProcessStartInfo startInfo, Process process)
		{
			ProcInfo proc_info = default(ProcInfo);
			if (startInfo.RedirectStandardInput || startInfo.RedirectStandardOutput || startInfo.RedirectStandardError)
			{
				throw new InvalidOperationException("UseShellExecute must be false when redirecting I/O.");
			}
			if (startInfo.HaveEnvVars)
			{
				throw new InvalidOperationException("UseShellExecute must be false in order to use environment variables.");
			}
			FillUserInfo(startInfo, ref proc_info);
			bool flag;
			try
			{
				flag = ShellExecuteEx_internal(startInfo, ref proc_info);
			}
			finally
			{
				if (proc_info.Password != IntPtr.Zero)
				{
					Marshal.FreeBSTR(proc_info.Password);
				}
				proc_info.Password = IntPtr.Zero;
			}
			if (!flag)
			{
				throw new Win32Exception(-proc_info.pid);
			}
			process.process_handle = proc_info.process_handle;
			process.pid = proc_info.pid;
			process.StartExitCallbackIfNeeded();
			return flag;
		}

		private static bool Start_noshell(ProcessStartInfo startInfo, Process process)
		{
			ProcInfo proc_info = default(ProcInfo);
			IntPtr read_handle = IntPtr.Zero;
			IntPtr target_handle = IntPtr.Zero;
			if (startInfo.HaveEnvVars)
			{
				string[] array = new string[startInfo.EnvironmentVariables.Count];
				startInfo.EnvironmentVariables.Keys.CopyTo(array, 0);
				proc_info.envKeys = array;
				array = new string[startInfo.EnvironmentVariables.Count];
				startInfo.EnvironmentVariables.Values.CopyTo(array, 0);
				proc_info.envValues = array;
			}
			bool flag;
			System.IO.MonoIOError error;
			if (startInfo.RedirectStandardInput)
			{
				if (IsWindows)
				{
					int options = 2;
					flag = System.IO.MonoIO.CreatePipe(out read_handle, out IntPtr write_handle);
					if (flag)
					{
						flag = System.IO.MonoIO.DuplicateHandle(GetCurrentProcess().Handle, write_handle, GetCurrentProcess().Handle, out target_handle, 0, 0, options);
						System.IO.MonoIO.Close(write_handle, out error);
					}
				}
				else
				{
					flag = System.IO.MonoIO.CreatePipe(out read_handle, out target_handle);
				}
				if (!flag)
				{
					throw new IOException("Error creating standard input pipe");
				}
			}
			else
			{
				read_handle = System.IO.MonoIO.ConsoleInput;
				target_handle = (IntPtr)0;
			}
			IntPtr write_handle2;
			if (startInfo.RedirectStandardOutput)
			{
				IntPtr target_handle2 = IntPtr.Zero;
				if (IsWindows)
				{
					int options2 = 2;
					flag = System.IO.MonoIO.CreatePipe(out IntPtr read_handle2, out write_handle2);
					if (flag)
					{
						System.IO.MonoIO.DuplicateHandle(GetCurrentProcess().Handle, read_handle2, GetCurrentProcess().Handle, out target_handle2, 0, 0, options2);
						System.IO.MonoIO.Close(read_handle2, out error);
					}
				}
				else
				{
					flag = System.IO.MonoIO.CreatePipe(out target_handle2, out write_handle2);
				}
				process.stdout_rd = target_handle2;
				if (!flag)
				{
					if (startInfo.RedirectStandardInput)
					{
						System.IO.MonoIO.Close(read_handle, out error);
						System.IO.MonoIO.Close(target_handle, out error);
					}
					throw new IOException("Error creating standard output pipe");
				}
			}
			else
			{
				process.stdout_rd = (IntPtr)0;
				write_handle2 = System.IO.MonoIO.ConsoleOutput;
			}
			IntPtr write_handle3;
			if (startInfo.RedirectStandardError)
			{
				IntPtr target_handle3 = IntPtr.Zero;
				if (IsWindows)
				{
					int options3 = 2;
					flag = System.IO.MonoIO.CreatePipe(out IntPtr read_handle3, out write_handle3);
					if (flag)
					{
						System.IO.MonoIO.DuplicateHandle(GetCurrentProcess().Handle, read_handle3, GetCurrentProcess().Handle, out target_handle3, 0, 0, options3);
						System.IO.MonoIO.Close(read_handle3, out error);
					}
				}
				else
				{
					flag = System.IO.MonoIO.CreatePipe(out target_handle3, out write_handle3);
				}
				process.stderr_rd = target_handle3;
				if (!flag)
				{
					if (startInfo.RedirectStandardInput)
					{
						System.IO.MonoIO.Close(read_handle, out error);
						System.IO.MonoIO.Close(target_handle, out error);
					}
					if (startInfo.RedirectStandardOutput)
					{
						System.IO.MonoIO.Close(process.stdout_rd, out error);
						System.IO.MonoIO.Close(write_handle2, out error);
					}
					throw new IOException("Error creating standard error pipe");
				}
			}
			else
			{
				process.stderr_rd = (IntPtr)0;
				write_handle3 = System.IO.MonoIO.ConsoleError;
			}
			FillUserInfo(startInfo, ref proc_info);
			try
			{
				flag = CreateProcess_internal(startInfo, read_handle, write_handle2, write_handle3, ref proc_info);
			}
			finally
			{
				if (proc_info.Password != IntPtr.Zero)
				{
					Marshal.FreeBSTR(proc_info.Password);
				}
				proc_info.Password = IntPtr.Zero;
			}
			if (!flag)
			{
				if (startInfo.RedirectStandardInput)
				{
					System.IO.MonoIO.Close(read_handle, out error);
					System.IO.MonoIO.Close(target_handle, out error);
				}
				if (startInfo.RedirectStandardOutput)
				{
					System.IO.MonoIO.Close(process.stdout_rd, out error);
					System.IO.MonoIO.Close(write_handle2, out error);
				}
				if (startInfo.RedirectStandardError)
				{
					System.IO.MonoIO.Close(process.stderr_rd, out error);
					System.IO.MonoIO.Close(write_handle3, out error);
				}
				throw new Win32Exception(-proc_info.pid, "ApplicationName='" + startInfo.FileName + "', CommandLine='" + startInfo.Arguments + "', CurrentDirectory='" + startInfo.WorkingDirectory + "'");
			}
			process.process_handle = proc_info.process_handle;
			process.pid = proc_info.pid;
			if (startInfo.RedirectStandardInput)
			{
				System.IO.MonoIO.Close(read_handle, out error);
				process.input_stream = new StreamWriter(new System.IO.MonoSyncFileStream(target_handle, FileAccess.Write, ownsHandle: true, 8192), Console.Out.Encoding);
				process.input_stream.AutoFlush = true;
			}
			Encoding encoding = startInfo.StandardOutputEncoding ?? Console.Out.Encoding;
			Encoding encoding2 = startInfo.StandardErrorEncoding ?? Console.Out.Encoding;
			if (startInfo.RedirectStandardOutput)
			{
				System.IO.MonoIO.Close(write_handle2, out error);
				process.output_stream = new StreamReader(new System.IO.MonoSyncFileStream(process.stdout_rd, FileAccess.Read, ownsHandle: true, 8192), encoding, detectEncodingFromByteOrderMarks: true, 8192);
			}
			if (startInfo.RedirectStandardError)
			{
				System.IO.MonoIO.Close(write_handle3, out error);
				process.error_stream = new StreamReader(new System.IO.MonoSyncFileStream(process.stderr_rd, FileAccess.Read, ownsHandle: true, 8192), encoding2, detectEncodingFromByteOrderMarks: true, 8192);
			}
			process.StartExitCallbackIfNeeded();
			return flag;
		}

		private static void FillUserInfo(ProcessStartInfo startInfo, ref ProcInfo proc_info)
		{
			if (startInfo.UserName != null)
			{
				proc_info.UserName = startInfo.UserName;
				proc_info.Domain = startInfo.Domain;
				if (startInfo.Password != null)
				{
					proc_info.Password = Marshal.SecureStringToBSTR(startInfo.Password);
				}
				else
				{
					proc_info.Password = IntPtr.Zero;
				}
				proc_info.LoadUserProfile = startInfo.LoadUserProfile;
			}
		}

		private static bool Start_common(ProcessStartInfo startInfo, Process process)
		{
			if (startInfo.FileName == null || startInfo.FileName.Length == 0)
			{
				throw new InvalidOperationException("File name has not been set");
			}
			if (startInfo.StandardErrorEncoding != null && !startInfo.RedirectStandardError)
			{
				throw new InvalidOperationException("StandardErrorEncoding is only supported when standard error is redirected");
			}
			if (startInfo.StandardOutputEncoding != null && !startInfo.RedirectStandardOutput)
			{
				throw new InvalidOperationException("StandardOutputEncoding is only supported when standard output is redirected");
			}
			if (startInfo.UseShellExecute)
			{
				if (!string.IsNullOrEmpty(startInfo.UserName))
				{
					throw new InvalidOperationException("UserShellExecute must be false if an explicit UserName is specified when starting a process");
				}
				return Start_shell(startInfo, process);
			}
			return Start_noshell(startInfo, process);
		}

		/// <summary>Starts (or reuses) the process resource that is specified by the <see cref="P:System.Diagnostics.Process.StartInfo" /> property of this <see cref="T:System.Diagnostics.Process" /> component and associates it with the component.</summary>
		/// <returns>true if a process resource is started; false if no new process resource is started (for example, if an existing process is reused).</returns>
		/// <exception cref="T:System.InvalidOperationException">No file name was specified in the <see cref="T:System.Diagnostics.Process" /> component's <see cref="P:System.Diagnostics.Process.StartInfo" />.-or- The <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> member of the <see cref="P:System.Diagnostics.Process.StartInfo" /> property is true while <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardInput" />, <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardOutput" />, or <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardError" /> is true. </exception>
		/// <exception cref="T:System.ComponentModel.Win32Exception">There was an error in opening the associated file. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The process object has already been disposed. </exception>
		/// <filterpriority>1</filterpriority>
		public bool Start()
		{
			if (process_handle != IntPtr.Zero)
			{
				Process_free_internal(process_handle);
				process_handle = IntPtr.Zero;
			}
			return Start_common(start_info, this);
		}

		/// <summary>Starts the process resource that is specified by the parameter containing process start information (for example, the file name of the process to start) and associates the resource with a new <see cref="T:System.Diagnostics.Process" /> component.</summary>
		/// <returns>A new <see cref="T:System.Diagnostics.Process" /> component that is associated with the process resource, or null if no process resource is started (for example, if an existing process is reused).</returns>
		/// <param name="startInfo">The <see cref="T:System.Diagnostics.ProcessStartInfo" /> that contains the information that is used to start the process, including the file name and any command-line arguments. </param>
		/// <exception cref="T:System.InvalidOperationException">No file name was specified in the <paramref name="startInfo" /> parameter's <see cref="P:System.Diagnostics.ProcessStartInfo.FileName" /> property.-or- The <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> property of the <paramref name="startInfo" /> parameter is true and the <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardInput" />, <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardOutput" />, or <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardError" /> property is also true.-or-The <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> property of the <paramref name="startInfo" /> parameter is true and the <see cref="P:System.Diagnostics.ProcessStartInfo.UserName" /> property is not null or empty or the <see cref="P:System.Diagnostics.ProcessStartInfo.Password" /> property is not null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="startInfo" /> parameter is null. </exception>
		/// <exception cref="T:System.ComponentModel.Win32Exception">There was an error in opening the associated file. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The process object has already been disposed. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file specified in the <paramref name="startInfo" /> parameter's <see cref="P:System.Diagnostics.ProcessStartInfo.FileName" /> property could not be found.</exception>
		/// <exception cref="T:System.ComponentModel.Win32Exception">An error occurred when opening the associated file. -or-The sum of the length of the arguments and the length of the full path to the process exceeds 2080. The error message associated with this exception can be one of the following: "The data area passed to a system call is too small." or "Access is denied."</exception>
		/// <filterpriority>1</filterpriority>
		public static Process Start(ProcessStartInfo startInfo)
		{
			if (startInfo == null)
			{
				throw new ArgumentNullException("startInfo");
			}
			Process process = new Process();
			process.StartInfo = startInfo;
			if (Start_common(startInfo, process))
			{
				return process;
			}
			return null;
		}

		/// <summary>Starts a process resource by specifying the name of a document or application file and associates the resource with a new <see cref="T:System.Diagnostics.Process" /> component.</summary>
		/// <returns>A new <see cref="T:System.Diagnostics.Process" /> component that is associated with the process resource, or null, if no process resource is started (for example, if an existing process is reused).</returns>
		/// <param name="fileName">The name of a document or application file to run in the process. </param>
		/// <exception cref="T:System.ComponentModel.Win32Exception">An error occurred when opening the associated file. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The process object has already been disposed. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The PATH environment variable has a string containing quotes.</exception>
		/// <filterpriority>1</filterpriority>
		public static Process Start(string fileName)
		{
			return Start(new ProcessStartInfo(fileName));
		}

		/// <summary>Starts a process resource by specifying the name of an application and a set of command-line arguments, and associates the resource with a new <see cref="T:System.Diagnostics.Process" /> component.</summary>
		/// <returns>A new <see cref="T:System.Diagnostics.Process" /> component that is associated with the process, or null, if no process resource is started (for example, if an existing process is reused).</returns>
		/// <param name="fileName">The name of an application file to run in the process. </param>
		/// <param name="arguments">Command-line arguments to pass when starting the process. </param>
		/// <exception cref="T:System.InvalidOperationException">The <paramref name="fileName" /> or <paramref name="arguments" /> parameter is null. </exception>
		/// <exception cref="T:System.ComponentModel.Win32Exception">An error occurred when opening the associated file. -or-The sum of the length of the arguments and the length of the full path to the process exceeds 2080. The error message associated with this exception can be one of the following: "The data area passed to a system call is too small." or "Access is denied."</exception>
		/// <exception cref="T:System.ObjectDisposedException">The process object has already been disposed. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The PATH environment variable has a string containing quotes.</exception>
		/// <filterpriority>1</filterpriority>
		public static Process Start(string fileName, string arguments)
		{
			return Start(new ProcessStartInfo(fileName, arguments));
		}

		/// <summary>Starts a process resource by specifying the name of an application, a user name, a password, and a domain and associates the resource with a new <see cref="T:System.Diagnostics.Process" /> component.</summary>
		/// <returns>A new <see cref="T:System.Diagnostics.Process" /> component that is associated with the process resource, or null if no process resource is started (for example, if an existing process is reused).</returns>
		/// <param name="fileName">The name of an application file to run in the process.</param>
		/// <param name="userName">The user name to use when starting the process.</param>
		/// <param name="password">A <see cref="T:System.Security.SecureString" /> that contains the password to use when starting the process.</param>
		/// <param name="domain">The domain to use when starting the process.</param>
		/// <exception cref="T:System.InvalidOperationException">No file name was specified. </exception>
		/// <exception cref="T:System.ComponentModel.Win32Exception">
		///   <paramref name="fileName" /> is not an executable (.exe) file.</exception>
		/// <exception cref="T:System.ComponentModel.Win32Exception">There was an error in opening the associated file. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The process object has already been disposed. </exception>
		/// <filterpriority>1</filterpriority>
		public static Process Start(string fileName, string username, SecureString password, string domain)
		{
			return Start(fileName, null, username, password, domain);
		}

		/// <summary>Starts a process resource by specifying the name of an application, a set of command-line arguments, a user name, a password, and a domain and associates the resource with a new <see cref="T:System.Diagnostics.Process" /> component.</summary>
		/// <returns>A new <see cref="T:System.Diagnostics.Process" /> component that is associated with the process resource, or null if no process resource is started (for example, if an existing process is reused).</returns>
		/// <param name="fileName">The name of an application file to run in the process. </param>
		/// <param name="arguments">Command-line arguments to pass when starting the process. </param>
		/// <param name="userName">The user name to use when starting the process.</param>
		/// <param name="password">A <see cref="T:System.Security.SecureString" /> that contains the password to use when starting the process.</param>
		/// <param name="domain">The domain to use when starting the process.</param>
		/// <exception cref="T:System.InvalidOperationException">No file name was specified.</exception>
		/// <exception cref="T:System.ComponentModel.Win32Exception">An error occurred when opening the associated file. -or-The sum of the length of the arguments and the length of the full path to the associated file exceeds 2080. The error message associated with this exception can be one of the following: "The data area passed to a system call is too small." or "Access is denied."</exception>
		/// <exception cref="T:System.ObjectDisposedException">The process object has already been disposed. </exception>
		/// <filterpriority>1</filterpriority>
		public static Process Start(string fileName, string arguments, string username, SecureString password, string domain)
		{
			ProcessStartInfo processStartInfo = new ProcessStartInfo(fileName, arguments);
			processStartInfo.UserName = username;
			processStartInfo.Password = password;
			processStartInfo.Domain = domain;
			processStartInfo.UseShellExecute = false;
			return Start(processStartInfo);
		}

		/// <summary>Formats the process's name as a string, combined with the parent component type, if applicable.</summary>
		/// <returns>The <see cref="P:System.Diagnostics.Process.ProcessName" />, combined with the base component's <see cref="M:System.Object.ToString" /> return value.</returns>
		/// <exception cref="T:System.PlatformNotSupportedException">
		///   <see cref="M:System.Diagnostics.Process.ToString" /> is not supported on Windows 98.</exception>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return base.ToString() + " (" + ProcessName + ")";
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool WaitForExit_internal(IntPtr handle, int ms);

		/// <summary>Instructs the <see cref="T:System.Diagnostics.Process" /> component to wait indefinitely for the associated process to exit.</summary>
		/// <exception cref="T:System.ComponentModel.Win32Exception">The wait setting could not be accessed. </exception>
		/// <exception cref="T:System.SystemException">No process <see cref="P:System.Diagnostics.Process.Id" /> has been set, and a <see cref="P:System.Diagnostics.Process.Handle" /> from which the <see cref="P:System.Diagnostics.Process.Id" /> property can be determined does not exist.-or- There is no process associated with this <see cref="T:System.Diagnostics.Process" /> object.-or- You are attempting to call <see cref="M:System.Diagnostics.Process.WaitForExit" /> for a process that is running on a remote computer. This method is available only for processes that are running on the local computer. </exception>
		/// <filterpriority>1</filterpriority>
		public void WaitForExit()
		{
			WaitForExit(-1);
		}

		/// <summary>Instructs the <see cref="T:System.Diagnostics.Process" /> component to wait the specified number of milliseconds for the associated process to exit.</summary>
		/// <returns>true if the associated process has exited; otherwise, false.</returns>
		/// <param name="milliseconds">The amount of time, in milliseconds, to wait for the associated process to exit. The maximum is the largest possible value of a 32-bit integer, which represents infinity to the operating system. </param>
		/// <exception cref="T:System.ComponentModel.Win32Exception">The wait setting could not be accessed. </exception>
		/// <exception cref="T:System.SystemException">No process <see cref="P:System.Diagnostics.Process.Id" /> has been set, and a <see cref="P:System.Diagnostics.Process.Handle" /> from which the <see cref="P:System.Diagnostics.Process.Id" /> property can be determined does not exist.-or- There is no process associated with this <see cref="T:System.Diagnostics.Process" /> object.-or- You are attempting to call <see cref="M:System.Diagnostics.Process.WaitForExit(System.Int32)" /> for a process that is running on a remote computer. This method is available only for processes that are running on the local computer. </exception>
		/// <filterpriority>1</filterpriority>
		public bool WaitForExit(int milliseconds)
		{
			int num = milliseconds;
			if (num == int.MaxValue)
			{
				num = -1;
			}
			DateTime d = DateTime.UtcNow;
			if (async_output != null && !async_output.IsCompleted)
			{
				if (!async_output.WaitHandle.WaitOne(num, exitContext: false))
				{
					return false;
				}
				if (num >= 0)
				{
					DateTime utcNow = DateTime.UtcNow;
					num -= (int)(utcNow - d).TotalMilliseconds;
					if (num <= 0)
					{
						return false;
					}
					d = utcNow;
				}
			}
			if (async_error != null && !async_error.IsCompleted)
			{
				if (!async_error.WaitHandle.WaitOne(num, exitContext: false))
				{
					return false;
				}
				if (num >= 0)
				{
					num -= (int)(DateTime.UtcNow - d).TotalMilliseconds;
					if (num <= 0)
					{
						return false;
					}
				}
			}
			return WaitForExit_internal(process_handle, num);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool WaitForInputIdle_internal(IntPtr handle, int ms);

		/// <summary>Causes the <see cref="T:System.Diagnostics.Process" /> component to wait indefinitely for the associated process to enter an idle state. This overload applies only to processes with a user interface and, therefore, a message loop.</summary>
		/// <returns>true if the associated process has reached an idle state.</returns>
		/// <exception cref="T:System.InvalidOperationException">The process does not have a graphical interface.-or-An unknown error occurred. The process failed to enter an idle state.-or-The process has already exited. -or-No process is associated with this <see cref="T:System.Diagnostics.Process" /> object.</exception>
		/// <filterpriority>1</filterpriority>
		[MonoTODO]
		public bool WaitForInputIdle()
		{
			return WaitForInputIdle(-1);
		}

		/// <summary>Causes the <see cref="T:System.Diagnostics.Process" /> component to wait the specified number of milliseconds for the associated process to enter an idle state. This overload applies only to processes with a user interface and, therefore, a message loop.</summary>
		/// <returns>true if the associated process has reached an idle state; otherwise, false.</returns>
		/// <param name="milliseconds">A value of 1 to <see cref="F:System.Int32.MaxValue" /> that specifies the amount of time, in milliseconds, to wait for the associated process to become idle. A value of 0 specifies an immediate return, and a value of -1 specifies an infinite wait. </param>
		/// <exception cref="T:System.InvalidOperationException">The process does not have a graphical interface.-or-An unknown error occurred. The process failed to enter an idle state.-or-The process has already exited. -or-No process is associated with this <see cref="T:System.Diagnostics.Process" /> object.</exception>
		/// <filterpriority>1</filterpriority>
		[MonoTODO]
		public bool WaitForInputIdle(int milliseconds)
		{
			return WaitForInputIdle_internal(process_handle, milliseconds);
		}

		private static bool IsLocalMachine(string machineName)
		{
			if (machineName == "." || machineName.Length == 0)
			{
				return true;
			}
			return string.Compare(machineName, Environment.MachineName, ignoreCase: true) == 0;
		}

		private void OnOutputDataReceived(string str)
		{
			if (this.OutputDataReceived != null)
			{
				this.OutputDataReceived(this, new DataReceivedEventArgs(str));
			}
		}

		private void OnErrorDataReceived(string str)
		{
			if (this.ErrorDataReceived != null)
			{
				this.ErrorDataReceived(this, new DataReceivedEventArgs(str));
			}
		}

		/// <summary>Begins asynchronous read operations on the redirected <see cref="P:System.Diagnostics.Process.StandardOutput" /> stream of the application.</summary>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardOutput" /> property is false.- or - An asynchronous read operation is already in progress on the <see cref="P:System.Diagnostics.Process.StandardOutput" /> stream.- or - The <see cref="P:System.Diagnostics.Process.StandardOutput" /> stream has been used by a synchronous read operation. </exception>
		/// <filterpriority>2</filterpriority>
		[ComVisible(false)]
		public void BeginOutputReadLine()
		{
			if (process_handle == IntPtr.Zero || output_stream == null || !StartInfo.RedirectStandardOutput)
			{
				throw new InvalidOperationException("Standard output has not been redirected or process has not been started.");
			}
			if ((async_mode & AsyncModes.SyncOutput) != 0)
			{
				throw new InvalidOperationException("Cannot mix asynchronous and synchonous reads.");
			}
			async_mode |= AsyncModes.AsyncOutput;
			output_canceled = false;
			if (async_output == null)
			{
				async_output = new ProcessAsyncReader(this, stdout_rd, err_out: true);
				async_output.ReadHandler.BeginInvoke(null, async_output);
			}
		}

		/// <summary>Cancels the asynchronous read operation on the redirected <see cref="P:System.Diagnostics.Process.StandardOutput" /> stream of an application.</summary>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Diagnostics.Process.StandardOutput" /> stream is not enabled for asynchronous read operations. </exception>
		/// <filterpriority>2</filterpriority>
		[ComVisible(false)]
		public void CancelOutputRead()
		{
			if (process_handle == IntPtr.Zero || output_stream == null || !StartInfo.RedirectStandardOutput)
			{
				throw new InvalidOperationException("Standard output has not been redirected or process has not been started.");
			}
			if ((async_mode & AsyncModes.SyncOutput) != 0)
			{
				throw new InvalidOperationException("OutputStream is not enabled for asynchronous read operations.");
			}
			if (async_output == null)
			{
				throw new InvalidOperationException("No async operation in progress.");
			}
			output_canceled = true;
		}

		/// <summary>Begins asynchronous read operations on the redirected <see cref="P:System.Diagnostics.Process.StandardError" /> stream of the application.</summary>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardError" /> property is false.- or - An asynchronous read operation is already in progress on the <see cref="P:System.Diagnostics.Process.StandardError" /> stream.- or - The <see cref="P:System.Diagnostics.Process.StandardError" /> stream has been used by a synchronous read operation. </exception>
		/// <filterpriority>2</filterpriority>
		[ComVisible(false)]
		public void BeginErrorReadLine()
		{
			if (process_handle == IntPtr.Zero || error_stream == null || !StartInfo.RedirectStandardError)
			{
				throw new InvalidOperationException("Standard error has not been redirected or process has not been started.");
			}
			if ((async_mode & AsyncModes.SyncError) != 0)
			{
				throw new InvalidOperationException("Cannot mix asynchronous and synchonous reads.");
			}
			async_mode |= AsyncModes.AsyncError;
			error_canceled = false;
			if (async_error == null)
			{
				async_error = new ProcessAsyncReader(this, stderr_rd, err_out: false);
				async_error.ReadHandler.BeginInvoke(null, async_error);
			}
		}

		/// <summary>Cancels the asynchronous read operation on the redirected <see cref="P:System.Diagnostics.Process.StandardError" /> stream of an application.</summary>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Diagnostics.Process.StandardError" /> stream is not enabled for asynchronous read operations. </exception>
		/// <filterpriority>2</filterpriority>
		[ComVisible(false)]
		public void CancelErrorRead()
		{
			if (process_handle == IntPtr.Zero || output_stream == null || !StartInfo.RedirectStandardOutput)
			{
				throw new InvalidOperationException("Standard output has not been redirected or process has not been started.");
			}
			if ((async_mode & AsyncModes.SyncOutput) != 0)
			{
				throw new InvalidOperationException("OutputStream is not enabled for asynchronous read operations.");
			}
			if (async_error == null)
			{
				throw new InvalidOperationException("No async operation in progress.");
			}
			error_canceled = true;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Process_free_internal(IntPtr handle);

		/// <summary>Release all resources used by this process.</summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
		protected override void Dispose(bool disposing)
		{
			if (!disposed)
			{
				disposed = true;
				if (disposing)
				{
					lock (this)
					{
						if (async_output != null)
						{
							async_output.Close();
						}
						if (async_error != null)
						{
							async_error.Close();
						}
					}
				}
				lock (this)
				{
					if (process_handle != IntPtr.Zero)
					{
						Process_free_internal(process_handle);
						process_handle = IntPtr.Zero;
					}
					if (input_stream != null)
					{
						input_stream.Close();
						input_stream = null;
					}
					if (output_stream != null)
					{
						output_stream.Close();
						output_stream = null;
					}
					if (error_stream != null)
					{
						error_stream.Close();
						error_stream = null;
					}
				}
			}
			base.Dispose(disposing);
		}

		~Process()
		{
			Dispose(disposing: false);
		}

		private static void CBOnExit(object state, bool unused)
		{
			Process process = (Process)state;
			process.OnExited();
		}

		/// <summary>Raises the <see cref="E:System.Diagnostics.Process.Exited" /> event.</summary>
		protected void OnExited()
		{
			if (exited_event == null)
			{
				return;
			}
			if (synchronizingObject == null)
			{
				Delegate[] invocationList = exited_event.GetInvocationList();
				for (int i = 0; i < invocationList.Length; i++)
				{
					EventHandler eventHandler = (EventHandler)invocationList[i];
					try
					{
						eventHandler(this, EventArgs.Empty);
					}
					catch
					{
					}
				}
			}
			else
			{
				object[] args = new object[2]
				{
					this,
					EventArgs.Empty
				};
				synchronizingObject.BeginInvoke(exited_event, args);
			}
		}
	}
}
