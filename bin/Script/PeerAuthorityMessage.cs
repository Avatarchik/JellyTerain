namespace UnityEngine.Networking.NetworkSystem
{
	public class PeerAuthorityMessage : MessageBase
	{
		public int connectionId;

		public NetworkInstanceId netId;

		public bool authorityState;

		public override void Deserialize(NetworkReader reader)
		{
			connectionId = (int)reader.ReadPackedUInt32();
			netId = reader.ReadNetworkId();
			authorityState = reader.ReadBoolean();
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.WritePackedUInt32((uint)connectionId);
			writer.Write(netId);
			writer.Write(authorityState);
		}
	}
}
