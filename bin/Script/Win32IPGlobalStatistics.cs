namespace System.Net.NetworkInformation
{
	internal class Win32IPGlobalStatistics : IPGlobalStatistics
	{
		private Win32_MIB_IPSTATS info;

		public override int DefaultTtl => info.DefaultTTL;

		public override bool ForwardingEnabled => info.Forwarding != 0;

		public override int NumberOfInterfaces => info.NumIf;

		public override int NumberOfIPAddresses => info.NumAddr;

		public override int NumberOfRoutes => info.NumRoutes;

		public override long OutputPacketRequests => info.OutRequests;

		public override long OutputPacketRoutingDiscards => info.RoutingDiscards;

		public override long OutputPacketsDiscarded => info.OutDiscards;

		public override long OutputPacketsWithNoRoute => info.OutNoRoutes;

		public override long PacketFragmentFailures => info.FragFails;

		public override long PacketReassembliesRequired => info.ReasmReqds;

		public override long PacketReassemblyFailures => info.ReasmFails;

		public override long PacketReassemblyTimeout => info.ReasmTimeout;

		public override long PacketsFragmented => info.FragOks;

		public override long PacketsReassembled => info.ReasmOks;

		public override long ReceivedPackets => info.InReceives;

		public override long ReceivedPacketsDelivered => info.InDelivers;

		public override long ReceivedPacketsDiscarded => info.InDiscards;

		public override long ReceivedPacketsForwarded => info.ForwDatagrams;

		public override long ReceivedPacketsWithAddressErrors => info.InAddrErrors;

		public override long ReceivedPacketsWithHeadersErrors => info.InHdrErrors;

		public override long ReceivedPacketsWithUnknownProtocol => info.InUnknownProtos;

		public Win32IPGlobalStatistics(Win32_MIB_IPSTATS info)
		{
			this.info = info;
		}
	}
}
