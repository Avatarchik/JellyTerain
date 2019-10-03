namespace UnityEngine.Networking.NetworkSystem
{
	internal class OverrideTransformMessage : MessageBase
	{
		public NetworkInstanceId netId;

		public byte[] payload;

		public bool teleport;

		public int time;

		public override void Deserialize(NetworkReader reader)
		{
			netId = reader.ReadNetworkId();
			payload = reader.ReadBytesAndSize();
			teleport = reader.ReadBoolean();
			time = (int)reader.ReadPackedUInt32();
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(netId);
			writer.WriteBytesFull(payload);
			writer.Write(teleport);
			writer.WritePackedUInt32((uint)time);
		}
	}
}
