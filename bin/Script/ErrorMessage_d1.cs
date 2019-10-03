namespace UnityEngine.Networking.NetworkSystem
{
	public class ErrorMessage : MessageBase
	{
		public int errorCode;

		public override void Deserialize(NetworkReader reader)
		{
			errorCode = reader.ReadUInt16();
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write((ushort)errorCode);
		}
	}
}
