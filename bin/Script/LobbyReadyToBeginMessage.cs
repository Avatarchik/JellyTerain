namespace UnityEngine.Networking.NetworkSystem
{
	internal class LobbyReadyToBeginMessage : MessageBase
	{
		public byte slotId;

		public bool readyState;

		public override void Deserialize(NetworkReader reader)
		{
			slotId = reader.ReadByte();
			readyState = reader.ReadBoolean();
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(slotId);
			writer.Write(readyState);
		}
	}
}
