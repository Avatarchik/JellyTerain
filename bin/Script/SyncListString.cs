using System;

namespace UnityEngine.Networking
{
	public sealed class SyncListString : SyncList<string>
	{
		protected override void SerializeItem(NetworkWriter writer, string item)
		{
			writer.Write(item);
		}

		protected override string DeserializeItem(NetworkReader reader)
		{
			return reader.ReadString();
		}

		[Obsolete("ReadReference is now used instead")]
		public static SyncListString ReadInstance(NetworkReader reader)
		{
			ushort num = reader.ReadUInt16();
			SyncListString syncListString = new SyncListString();
			for (ushort num2 = 0; num2 < num; num2 = (ushort)(num2 + 1))
			{
				syncListString.AddInternal(reader.ReadString());
			}
			return syncListString;
		}

		public static void ReadReference(NetworkReader reader, SyncListString syncList)
		{
			ushort num = reader.ReadUInt16();
			syncList.Clear();
			for (ushort num2 = 0; num2 < num; num2 = (ushort)(num2 + 1))
			{
				syncList.AddInternal(reader.ReadString());
			}
		}

		public static void WriteInstance(NetworkWriter writer, SyncListString items)
		{
			writer.Write((ushort)items.Count);
			for (int i = 0; i < items.Count; i++)
			{
				writer.Write(items[i]);
			}
		}
	}
}
