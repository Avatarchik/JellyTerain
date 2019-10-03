namespace UnityEngine.Networking.NetworkSystem
{
	internal class AnimationTriggerMessage : MessageBase
	{
		public NetworkInstanceId netId;

		public int hash;

		public override void Deserialize(NetworkReader reader)
		{
			netId = reader.ReadNetworkId();
			hash = (int)reader.ReadPackedUInt32();
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(netId);
			writer.WritePackedUInt32((uint)hash);
		}
	}
}
