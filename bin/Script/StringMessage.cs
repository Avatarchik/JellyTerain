namespace UnityEngine.Networking.NetworkSystem
{
	public class StringMessage : MessageBase
	{
		public string value;

		public StringMessage()
		{
		}

		public StringMessage(string v)
		{
			value = v;
		}

		public override void Deserialize(NetworkReader reader)
		{
			value = reader.ReadString();
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(value);
		}
	}
}
