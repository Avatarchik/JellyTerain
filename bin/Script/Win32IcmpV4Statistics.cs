namespace System.Net.NetworkInformation
{
	internal class Win32IcmpV4Statistics : IcmpV4Statistics
	{
		private Win32_MIBICMPSTATS iin;

		private Win32_MIBICMPSTATS iout;

		public override long AddressMaskRepliesReceived => iin.AddrMaskReps;

		public override long AddressMaskRepliesSent => iout.AddrMaskReps;

		public override long AddressMaskRequestsReceived => iin.AddrMasks;

		public override long AddressMaskRequestsSent => iout.AddrMasks;

		public override long DestinationUnreachableMessagesReceived => iin.DestUnreachs;

		public override long DestinationUnreachableMessagesSent => iout.DestUnreachs;

		public override long EchoRepliesReceived => iin.EchoReps;

		public override long EchoRepliesSent => iout.EchoReps;

		public override long EchoRequestsReceived => iin.Echos;

		public override long EchoRequestsSent => iout.Echos;

		public override long ErrorsReceived => iin.Errors;

		public override long ErrorsSent => iout.Errors;

		public override long MessagesReceived => iin.Msgs;

		public override long MessagesSent => iout.Msgs;

		public override long ParameterProblemsReceived => iin.ParmProbs;

		public override long ParameterProblemsSent => iout.ParmProbs;

		public override long RedirectsReceived => iin.Redirects;

		public override long RedirectsSent => iout.Redirects;

		public override long SourceQuenchesReceived => iin.SrcQuenchs;

		public override long SourceQuenchesSent => iout.SrcQuenchs;

		public override long TimeExceededMessagesReceived => iin.TimeExcds;

		public override long TimeExceededMessagesSent => iout.TimeExcds;

		public override long TimestampRepliesReceived => iin.TimestampReps;

		public override long TimestampRepliesSent => iout.TimestampReps;

		public override long TimestampRequestsReceived => iin.Timestamps;

		public override long TimestampRequestsSent => iout.Timestamps;

		public Win32IcmpV4Statistics(Win32_MIBICMPINFO info)
		{
			iin = info.InStats;
			iout = info.OutStats;
		}
	}
}
