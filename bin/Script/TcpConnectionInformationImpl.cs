namespace System.Net.NetworkInformation
{
	internal class TcpConnectionInformationImpl : TcpConnectionInformation
	{
		private IPEndPoint local;

		private IPEndPoint remote;

		private TcpState state;

		public override IPEndPoint LocalEndPoint => local;

		public override IPEndPoint RemoteEndPoint => remote;

		public override TcpState State => state;

		public TcpConnectionInformationImpl(IPEndPoint local, IPEndPoint remote, TcpState state)
		{
			this.local = local;
			this.remote = remote;
			this.state = state;
		}
	}
}
