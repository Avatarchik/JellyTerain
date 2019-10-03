namespace UnityEngine.Networking.NetworkSystem
{
	internal class ClientAuthorityMessage : MessageBase
	{
		public NetworkInstanceId netId;

		public bool authority;

		public override void Deserialize(NetworkReader reader)
		{
			netId = reader.ReadNetworkId();
			authority = reader.ReadBoolean();
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(netId);
			writer.Write(authority);
		}
	}
}
