using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	internal class MulticastIPAddressInformationImplCollection : MulticastIPAddressInformationCollection
	{
		public static readonly MulticastIPAddressInformationImplCollection Empty = new MulticastIPAddressInformationImplCollection(isReadOnly: true);

		private bool is_readonly;

		public override bool IsReadOnly => is_readonly;

		private MulticastIPAddressInformationImplCollection(bool isReadOnly)
		{
			is_readonly = isReadOnly;
		}

		public static MulticastIPAddressInformationCollection Win32FromMulticast(IntPtr ptr)
		{
			MulticastIPAddressInformationImplCollection multicastIPAddressInformationImplCollection = new MulticastIPAddressInformationImplCollection(isReadOnly: false);
			IntPtr intPtr = ptr;
			while (intPtr != IntPtr.Zero)
			{
				Win32_IP_ADAPTER_MULTICAST_ADDRESS win32_IP_ADAPTER_MULTICAST_ADDRESS = (Win32_IP_ADAPTER_MULTICAST_ADDRESS)Marshal.PtrToStructure(intPtr, typeof(Win32_IP_ADAPTER_MULTICAST_ADDRESS));
				multicastIPAddressInformationImplCollection.Add(new MulticastIPAddressInformationImpl(win32_IP_ADAPTER_MULTICAST_ADDRESS.Address.GetIPAddress(), win32_IP_ADAPTER_MULTICAST_ADDRESS.LengthFlags.IsDnsEligible, win32_IP_ADAPTER_MULTICAST_ADDRESS.LengthFlags.IsTransient));
				intPtr = win32_IP_ADAPTER_MULTICAST_ADDRESS.Next;
			}
			multicastIPAddressInformationImplCollection.is_readonly = true;
			return multicastIPAddressInformationImplCollection;
		}

		public static MulticastIPAddressInformationImplCollection LinuxFromList(List<IPAddress> addresses)
		{
			MulticastIPAddressInformationImplCollection multicastIPAddressInformationImplCollection = new MulticastIPAddressInformationImplCollection(isReadOnly: false);
			foreach (IPAddress address in addresses)
			{
				multicastIPAddressInformationImplCollection.Add(new MulticastIPAddressInformationImpl(address, isDnsEligible: true, isTransient: false));
			}
			multicastIPAddressInformationImplCollection.is_readonly = true;
			return multicastIPAddressInformationImplCollection;
		}
	}
}
