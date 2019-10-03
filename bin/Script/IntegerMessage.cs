namespace UnityEngine.Networking.NetworkSystem
{
	public class IntegerMessage : MessageBase
	{
		public int value;

		public IntegerMessage()
		{
		}

		public IntegerMessage(int v)
		{
			value = v;
		}

		public override void Deserialize(NetworkReader reader)
		{
			value = (int)reader.ReadPackedUInt32();
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.WritePackedUInt32((uint)value);
		}
	}
}
