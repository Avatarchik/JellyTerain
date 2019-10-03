namespace UnityEngine.Networking.NetworkSystem
{
	public class ReconnectMessage : MessageBase
	{
		public int oldConnectionId;

		public short playerControllerId;

		public NetworkInstanceId netId;

		public int msgSize;

		public byte[] msgData;

		public override void Deserialize(NetworkReader reader)
		{
			oldConnectionId = (int)reader.ReadPackedUInt32();
			playerControllerId = (short)reader.ReadPackedUInt32();
			netId = reader.ReadNetworkId();
			msgData = reader.ReadBytesAndSize();
			if (msgData == null)
			{
				msgSize = 0;
			}
			else
			{
				msgSize = msgData.Length;
			}
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.WritePackedUInt32((uint)oldConnectionId);
			writer.WritePackedUInt32((uint)playerControllerId);
			writer.Write(netId);
			writer.WriteBytesAndSize(msgData, msgSize);
		}
	}
}
