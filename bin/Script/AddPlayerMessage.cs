namespace UnityEngine.Networking.NetworkSystem
{
	public class AddPlayerMessage : MessageBase
	{
		public short playerControllerId;

		public int msgSize;

		public byte[] msgData;

		public override void Deserialize(NetworkReader reader)
		{
			playerControllerId = (short)reader.ReadUInt16();
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
			writer.Write((ushort)playerControllerId);
			writer.WriteBytesAndSize(msgData, msgSize);
		}
	}
}
