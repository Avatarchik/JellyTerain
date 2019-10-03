using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	internal class IPAddressInformationImplCollection : IPAddressInformationCollection
	{
		public static readonly IPAddressInformationImplCollection Empty = new IPAddressInformationImplCollection(isReadOnly: true);

		private bool is_readonly;

		public override bool IsReadOnly => is_readonly;

		private IPAddressInformationImplCollection(bool isReadOnly)
		{
			is_readonly = isReadOnly;
		}

		public static IPAddressInformationCollection Win32FromAnycast(IntPtr ptr)
		{
			IPAddressInformationImplCollection iPAddressInformationImplCollection = new IPAddressInformationImplCollection(isReadOnly: false);
			IntPtr intPtr = ptr;
			while (intPtr != IntPtr.Zero)
			{
				Win32_IP_ADAPTER_ANYCAST_ADDRESS win32_IP_ADAPTER_ANYCAST_ADDRESS = (Win32_IP_ADAPTER_ANYCAST_ADDRESS)Marshal.PtrToStructure(intPtr, typeof(Win32_IP_ADAPTER_ANYCAST_ADDRESS));
				iPAddressInformationImplCollection.Add(new IPAddressInformationImpl(win32_IP_ADAPTER_ANYCAST_ADDRESS.Address.GetIPAddress(), win32_IP_ADAPTER_ANYCAST_ADDRESS.LengthFlags.IsDnsEligible, win32_IP_ADAPTER_ANYCAST_ADDRESS.LengthFlags.IsTransient));
				intPtr = win32_IP_ADAPTER_ANYCAST_ADDRESS.Next;
			}
			iPAddressInformationImplCollection.is_readonly = true;
			return iPAddressInformationImplCollection;
		}

		public static IPAddressInformationImplCollection LinuxFromAnycast(IList<IPAddress> addresses)
		{
			IPAddressInformationImplCollection iPAddressInformationImplCollection = new IPAddressInformationImplCollection(isReadOnly: false);
			foreach (IPAddress address in addresses)
			{
				iPAddressInformationImplCollection.Add(new IPAddressInformationImpl(address, isDnsEligible: false, isTransient: false));
			}
			iPAddressInformationImplCollection.is_readonly = true;
			return iPAddressInformationImplCollection;
		}
	}
}
