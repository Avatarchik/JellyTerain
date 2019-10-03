namespace System.Net.NetworkInformation
{
	internal class Win32UdpStatistics : UdpStatistics
	{
		private Win32_MIB_UDPSTATS info;

		public override long DatagramsReceived => info.InDatagrams;

		public override long DatagramsSent => info.OutDatagrams;

		public override long IncomingDatagramsDiscarded => info.NoPorts;

		public override long IncomingDatagramsWithErrors => info.InErrors;

		public override int UdpListeners => info.NumAddrs;

		public Win32UdpStatistics(Win32_MIB_UDPSTATS info)
		{
			this.info = info;
		}
	}
}
