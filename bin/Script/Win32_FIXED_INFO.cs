using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	[StructLayout(LayoutKind.Sequential)]
	internal class Win32_FIXED_INFO
	{
		private const int MAX_HOSTNAME_LEN = 128;

		private const int MAX_DOMAIN_NAME_LEN = 128;

		private const int MAX_SCOPE_ID_LEN = 256;

		private static Win32_FIXED_INFO fixed_info;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 132)]
		public string HostName;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 132)]
		public string DomainName;

		public IntPtr CurrentDnsServer;

		public Win32_IP_ADDR_STRING DnsServerList;

		public NetBiosNodeType NodeType;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string ScopeId;

		public uint EnableRouting;

		public uint EnableProxy;

		public uint EnableDns;

		public static Win32_FIXED_INFO Instance
		{
			get
			{
				if (fixed_info == null)
				{
					fixed_info = GetInstance();
				}
				return fixed_info;
			}
		}

		[DllImport("iphlpapi.dll", SetLastError = true)]
		private static extern int GetNetworkParams(byte[] bytes, ref int size);

		private unsafe static Win32_FIXED_INFO GetInstance()
		{
			//IL_003a: Incompatible stack types: I vs Ref
			int size = 0;
			byte[] array = null;
			GetNetworkParams(null, ref size);
			array = new byte[size];
			GetNetworkParams(array, ref size);
			Win32_FIXED_INFO win32_FIXED_INFO = new Win32_FIXED_INFO();
			fixed (byte* value = &((array != null && array.Length != 0) ? ref array[0] : ref *(byte*)null))
			{
				Marshal.PtrToStructure((IntPtr)(void*)value, win32_FIXED_INFO);
			}
			return win32_FIXED_INFO;
		}
	}
}
