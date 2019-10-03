using System.Runtime.CompilerServices;

namespace System.IO
{
	internal sealed class MonoIO
	{
		public static IntPtr ConsoleOutput
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static IntPtr ConsoleInput
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static IntPtr ConsoleError
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool Close(IntPtr handle, out MonoIOError error);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool CreatePipe(out IntPtr read_handle, out IntPtr write_handle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool DuplicateHandle(IntPtr source_process_handle, IntPtr source_handle, IntPtr target_process_handle, out IntPtr target_handle, int access, int inherit, int options);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetTempPath(out string path);
	}
}
