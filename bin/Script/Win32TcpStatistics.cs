namespace System.Net.NetworkInformation
{
	internal class Win32TcpStatistics : TcpStatistics
	{
		private Win32_MIB_TCPSTATS info;

		public override long ConnectionsAccepted => info.PassiveOpens;

		public override long ConnectionsInitiated => info.ActiveOpens;

		public override long CumulativeConnections => info.NumConns;

		public override long CurrentConnections => info.CurrEstab;

		public override long ErrorsReceived => info.InErrs;

		public override long FailedConnectionAttempts => info.AttemptFails;

		public override long MaximumConnections => info.MaxConn;

		public override long MaximumTransmissionTimeout => info.RtoMax;

		public override long MinimumTransmissionTimeout => info.RtoMin;

		public override long ResetConnections => info.EstabResets;

		public override long ResetsSent => info.OutRsts;

		public override long SegmentsReceived => info.InSegs;

		public override long SegmentsResent => info.RetransSegs;

		public override long SegmentsSent => info.OutSegs;

		public Win32TcpStatistics(Win32_MIB_TCPSTATS info)
		{
			this.info = info;
		}
	}
}
