using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	internal class UnicastIPAddressInformationImplCollection : UnicastIPAddressInformationCollection
	{
		public static readonly UnicastIPAddressInformationImplCollection Empty = new UnicastIPAddressInformationImplCollection(isReadOnly: true);

		private bool is_readonly;

		public override bool IsReadOnly => is_readonly;

		private UnicastIPAddressInformationImplCollection(bool isReadOnly)
		{
			is_readonly = isReadOnly;
		}

		public static UnicastIPAddressInformationCollection Win32FromUnicast(int ifIndex, IntPtr ptr)
		{
			UnicastIPAddressInformationImplCollection unicastIPAddressInformationImplCollection = new UnicastIPAddressInformationImplCollection(isReadOnly: false);
			IntPtr intPtr = ptr;
			while (intPtr != IntPtr.Zero)
			{
				Win32_IP_ADAPTER_UNICAST_ADDRESS info = (Win32_IP_ADAPTER_UNICAST_ADDRESS)Marshal.PtrToStructure(intPtr, typeof(Win32_IP_ADAPTER_UNICAST_ADDRESS));
				unicastIPAddressInformationImplCollection.Add(new Win32UnicastIPAddressInformation(ifIndex, info));
				intPtr = info.Next;
			}
			unicastIPAddressInformationImplCollection.is_readonly = true;
			return unicastIPAddressInformationImplCollection;
		}

		public static UnicastIPAddressInformationCollection LinuxFromList(List<IPAddress> addresses)
		{
			UnicastIPAddressInformationImplCollection unicastIPAddressInformationImplCollection = new UnicastIPAddressInformationImplCollection(isReadOnly: false);
			foreach (IPAddress address in addresses)
			{
				unicastIPAddressInformationImplCollection.Add(new LinuxUnicastIPAddressInformation(address));
			}
			unicastIPAddressInformationImplCollection.is_readonly = true;
			return unicastIPAddressInformationImplCollection;
		}
	}
}
