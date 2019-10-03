namespace System.Net.NetworkInformation
{
	internal class Win32IcmpV6Statistics : IcmpV6Statistics
	{
		private Win32_MIBICMPSTATS_EX iin;

		private Win32_MIBICMPSTATS_EX iout;

		public override long DestinationUnreachableMessagesReceived => iin.Counts[1];

		public override long DestinationUnreachableMessagesSent => iout.Counts[1];

		public override long EchoRepliesReceived => iin.Counts[129];

		public override long EchoRepliesSent => iout.Counts[129];

		public override long EchoRequestsReceived => iin.Counts[128];

		public override long EchoRequestsSent => iout.Counts[128];

		public override long ErrorsReceived => iin.Errors;

		public override long ErrorsSent => iout.Errors;

		public override long MembershipQueriesReceived => iin.Counts[130];

		public override long MembershipQueriesSent => iout.Counts[130];

		public override long MembershipReductionsReceived => iin.Counts[132];

		public override long MembershipReductionsSent => iout.Counts[132];

		public override long MembershipReportsReceived => iin.Counts[131];

		public override long MembershipReportsSent => iout.Counts[131];

		public override long MessagesReceived => iin.Msgs;

		public override long MessagesSent => iout.Msgs;

		public override long NeighborAdvertisementsReceived => iin.Counts[136];

		public override long NeighborAdvertisementsSent => iout.Counts[136];

		public override long NeighborSolicitsReceived => iin.Counts[135];

		public override long NeighborSolicitsSent => iout.Counts[135];

		public override long PacketTooBigMessagesReceived => iin.Counts[2];

		public override long PacketTooBigMessagesSent => iout.Counts[2];

		public override long ParameterProblemsReceived => iin.Counts[4];

		public override long ParameterProblemsSent => iout.Counts[4];

		public override long RedirectsReceived => iin.Counts[137];

		public override long RedirectsSent => iout.Counts[137];

		public override long RouterAdvertisementsReceived => iin.Counts[134];

		public override long RouterAdvertisementsSent => iout.Counts[134];

		public override long RouterSolicitsReceived => iin.Counts[133];

		public override long RouterSolicitsSent => iout.Counts[133];

		public override long TimeExceededMessagesReceived => iin.Counts[3];

		public override long TimeExceededMessagesSent => iout.Counts[3];

		public Win32IcmpV6Statistics(Win32_MIB_ICMP_EX info)
		{
			iin = info.InStats;
			iout = info.OutStats;
		}
	}
}
