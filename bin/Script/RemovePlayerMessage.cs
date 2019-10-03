namespace UnityEngine.Networking.NetworkSystem
{
	public class RemovePlayerMessage : MessageBase
	{
		public short playerControllerId;

		public override void Deserialize(NetworkReader reader)
		{
			playerControllerId = (short)reader.ReadUInt16();
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write((ushort)playerControllerId);
		}
	}
}
