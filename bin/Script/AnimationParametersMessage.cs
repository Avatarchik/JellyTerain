namespace UnityEngine.Networking.NetworkSystem
{
	internal class AnimationParametersMessage : MessageBase
	{
		public NetworkInstanceId netId;

		public byte[] parameters;

		public override void Deserialize(NetworkReader reader)
		{
			netId = reader.ReadNetworkId();
			parameters = reader.ReadBytesAndSize();
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(netId);
			if (parameters == null)
			{
				writer.WriteBytesAndSize(parameters, 0);
			}
			else
			{
				writer.WriteBytesAndSize(parameters, parameters.Length);
			}
		}
	}
}
