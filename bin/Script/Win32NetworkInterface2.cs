using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	internal class Win32NetworkInterface2 : NetworkInterface
	{
		private Win32_IP_ADAPTER_ADDRESSES addr;

		private Win32_MIB_IFROW mib4;

		private Win32_MIB_IFROW mib6;

		private Win32IPv4InterfaceStatistics ip4stats;

		private IPInterfaceProperties ip_if_props;

		public override string Description => addr.Description;

		public override string Id => addr.AdapterName;

		public override bool IsReceiveOnly => addr.IsReceiveOnly;

		public override string Name => addr.FriendlyName;

		public override NetworkInterfaceType NetworkInterfaceType => addr.IfType;

		public override OperationalStatus OperationalStatus => addr.OperStatus;

		public override long Speed => (mib6.Index < 0) ? mib4.Speed : mib6.Speed;

		public override bool SupportsMulticast => !addr.NoMulticast;

		private Win32NetworkInterface2(Win32_IP_ADAPTER_ADDRESSES addr)
		{
			this.addr = addr;
			mib4 = default(Win32_MIB_IFROW);
			mib4.Index = addr.Alignment.IfIndex;
			if (GetIfEntry(ref mib4) != 0)
			{
				mib4.Index = -1;
			}
			mib6 = default(Win32_MIB_IFROW);
			mib6.Index = addr.Ipv6IfIndex;
			if (GetIfEntry(ref mib6) != 0)
			{
				mib6.Index = -1;
			}
			ip4stats = new Win32IPv4InterfaceStatistics(mib4);
			ip_if_props = new Win32IPInterfaceProperties2(addr, mib4, mib6);
		}

		[DllImport("iphlpapi.dll", SetLastError = true)]
		private static extern int GetAdaptersInfo(byte[] info, ref int size);

		[DllImport("iphlpapi.dll", SetLastError = true)]
		private static extern int GetAdaptersAddresses(uint family, uint flags, IntPtr reserved, byte[] info, ref int size);

		[DllImport("iphlpapi.dll", SetLastError = true)]
		private static extern int GetIfEntry(ref Win32_MIB_IFROW row);

		public static NetworkInterface[] ImplGetAllNetworkInterfaces()
		{
			Win32_IP_ADAPTER_ADDRESSES[] adaptersAddresses = GetAdaptersAddresses();
			NetworkInterface[] array = new NetworkInterface[adaptersAddresses.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new Win32NetworkInterface2(adaptersAddresses[i]);
			}
			return array;
		}

		public static Win32_IP_ADAPTER_INFO GetAdapterInfoByIndex(int index)
		{
			Win32_IP_ADAPTER_INFO[] adaptersInfo = GetAdaptersInfo();
			foreach (Win32_IP_ADAPTER_INFO win32_IP_ADAPTER_INFO in adaptersInfo)
			{
				if (win32_IP_ADAPTER_INFO.Index == index)
				{
					return win32_IP_ADAPTER_INFO;
				}
			}
			return null;
		}

		private unsafe static Win32_IP_ADAPTER_INFO[] GetAdaptersInfo()
		{
			//IL_0047: Incompatible stack types: I vs Ref
			byte[] info = null;
			int size = 0;
			GetAdaptersInfo(info, ref size);
			info = new byte[size];
			int adaptersInfo = GetAdaptersInfo(info, ref size);
			if (adaptersInfo != 0)
			{
				throw new NetworkInformationException(adaptersInfo);
			}
			List<Win32_IP_ADAPTER_INFO> list = new List<Win32_IP_ADAPTER_INFO>();
			fixed (byte* value = &((info != null && info.Length != 0) ? ref info[0] : ref *(byte*)null))
			{
				IntPtr intPtr = (IntPtr)(void*)value;
				while (intPtr != IntPtr.Zero)
				{
					Win32_IP_ADAPTER_INFO win32_IP_ADAPTER_INFO = new Win32_IP_ADAPTER_INFO();
					Marshal.PtrToStructure(intPtr, win32_IP_ADAPTER_INFO);
					list.Add(win32_IP_ADAPTER_INFO);
					intPtr = win32_IP_ADAPTER_INFO.Next;
				}
			}
			return list.ToArray();
		}

		private unsafe static Win32_IP_ADAPTER_ADDRESSES[] GetAdaptersAddresses()
		{
			//IL_0055: Incompatible stack types: I vs Ref
			byte[] info = null;
			int size = 0;
			GetAdaptersAddresses(0u, 0u, IntPtr.Zero, info, ref size);
			info = new byte[size];
			int adaptersAddresses = GetAdaptersAddresses(0u, 0u, IntPtr.Zero, info, ref size);
			if (adaptersAddresses != 0)
			{
				throw new NetworkInformationException(adaptersAddresses);
			}
			List<Win32_IP_ADAPTER_ADDRESSES> list = new List<Win32_IP_ADAPTER_ADDRESSES>();
			fixed (byte* value = &((info != null && info.Length != 0) ? ref info[0] : ref *(byte*)null))
			{
				IntPtr intPtr = (IntPtr)(void*)value;
				while (intPtr != IntPtr.Zero)
				{
					Win32_IP_ADAPTER_ADDRESSES win32_IP_ADAPTER_ADDRESSES = new Win32_IP_ADAPTER_ADDRESSES();
					Marshal.PtrToStructure(intPtr, win32_IP_ADAPTER_ADDRESSES);
					list.Add(win32_IP_ADAPTER_ADDRESSES);
					intPtr = win32_IP_ADAPTER_ADDRESSES.Next;
				}
			}
			return list.ToArray();
		}

		public override IPInterfaceProperties GetIPProperties()
		{
			return ip_if_props;
		}

		public override IPv4InterfaceStatistics GetIPv4Statistics()
		{
			return ip4stats;
		}

		public override PhysicalAddress GetPhysicalAddress()
		{
			byte[] array = new byte[addr.PhysicalAddressLength];
			Array.Copy(addr.PhysicalAddress, 0, array, 0, array.Length);
			return new PhysicalAddress(array);
		}

		public override bool Supports(NetworkInterfaceComponent networkInterfaceComponent)
		{
			switch (networkInterfaceComponent)
			{
			case NetworkInterfaceComponent.IPv4:
				return mib4.Index >= 0;
			case NetworkInterfaceComponent.IPv6:
				return mib6.Index >= 0;
			default:
				return false;
			}
		}
	}
}
