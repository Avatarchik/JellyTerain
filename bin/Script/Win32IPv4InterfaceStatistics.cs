namespace System.Net.NetworkInformation
{
	internal class Win32IPv4InterfaceStatistics : IPv4InterfaceStatistics
	{
		private Win32_MIB_IFROW info;

		public override long BytesReceived => info.InOctets;

		public override long BytesSent => info.OutOctets;

		public override long IncomingPacketsDiscarded => info.InDiscards;

		public override long IncomingPacketsWithErrors => info.InErrors;

		public override long IncomingUnknownProtocolPackets => info.InUnknownProtos;

		public override long NonUnicastPacketsReceived => info.InNUcastPkts;

		public override long NonUnicastPacketsSent => info.OutNUcastPkts;

		public override long OutgoingPacketsDiscarded => info.OutDiscards;

		public override long OutgoingPacketsWithErrors => info.OutErrors;

		public override long OutputQueueLength => info.OutQLen;

		public override long UnicastPacketsReceived => info.InUcastPkts;

		public override long UnicastPacketsSent => info.OutUcastPkts;

		public Win32IPv4InterfaceStatistics(Win32_MIB_IFROW info)
		{
			this.info = info;
		}
	}
}
