using System.Collections.Generic;

namespace UnityEngine.Networking.NetworkSystem
{
	public class PeerInfoMessage : MessageBase
	{
		public int connectionId;

		public string address;

		public int port;

		public bool isHost;

		public bool isYou;

		public PeerInfoPlayer[] playerIds;

		public override void Deserialize(NetworkReader reader)
		{
			connectionId = (int)reader.ReadPackedUInt32();
			address = reader.ReadString();
			port = (int)reader.ReadPackedUInt32();
			isHost = reader.ReadBoolean();
			isYou = reader.ReadBoolean();
			uint num = reader.ReadPackedUInt32();
			if (num != 0)
			{
				List<PeerInfoPlayer> list = new List<PeerInfoPlayer>();
				PeerInfoPlayer item = default(PeerInfoPlayer);
				for (uint num2 = 0u; num2 < num; num2++)
				{
					item.netId = reader.ReadNetworkId();
					item.playerControllerId = (short)reader.ReadPackedUInt32();
					list.Add(item);
				}
				playerIds = list.ToArray();
			}
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.WritePackedUInt32((uint)connectionId);
			writer.Write(address);
			writer.WritePackedUInt32((uint)port);
			writer.Write(isHost);
			writer.Write(isYou);
			if (playerIds == null)
			{
				writer.WritePackedUInt32(0u);
				return;
			}
			writer.WritePackedUInt32((uint)playerIds.Length);
			for (int i = 0; i < playerIds.Length; i++)
			{
				writer.Write(playerIds[i].netId);
				writer.WritePackedUInt32((uint)playerIds[i].playerControllerId);
			}
		}

		public override string ToString()
		{
			return "PeerInfo conn:" + connectionId + " addr:" + address + ":" + port + " host:" + isHost + " isYou:" + isYou;
		}
	}
}
