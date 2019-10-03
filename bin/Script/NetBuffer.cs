using System;

namespace UnityEngine.Networking
{
	internal class NetBuffer
	{
		private byte[] m_Buffer;

		private uint m_Pos;

		private const int k_InitialSize = 64;

		private const float k_GrowthFactor = 1.5f;

		private const int k_BufferSizeWarning = 134217728;

		public uint Position => m_Pos;

		public int Length => m_Buffer.Length;

		public NetBuffer()
		{
			m_Buffer = new byte[64];
		}

		public NetBuffer(byte[] buffer)
		{
			m_Buffer = buffer;
		}

		public byte ReadByte()
		{
			if (m_Pos >= m_Buffer.Length)
			{
				throw new IndexOutOfRangeException("NetworkReader:ReadByte out of range:" + ToString());
			}
			return m_Buffer[m_Pos++];
		}

		public void ReadBytes(byte[] buffer, uint count)
		{
			if (m_Pos + count > m_Buffer.Length)
			{
				throw new IndexOutOfRangeException("NetworkReader:ReadBytes out of range: (" + count + ") " + ToString());
			}
			for (ushort num = 0; num < count; num = (ushort)(num + 1))
			{
				buffer[num] = m_Buffer[m_Pos + num];
			}
			m_Pos += count;
		}

		internal ArraySegment<byte> AsArraySegment()
		{
			return new ArraySegment<byte>(m_Buffer, 0, (int)m_Pos);
		}

		public void WriteByte(byte value)
		{
			WriteCheckForSpace(1);
			m_Buffer[m_Pos] = value;
			m_Pos++;
		}

		public void WriteByte2(byte value0, byte value1)
		{
			WriteCheckForSpace(2);
			m_Buffer[m_Pos] = value0;
			m_Buffer[m_Pos + 1] = value1;
			m_Pos += 2u;
		}

		public void WriteByte4(byte value0, byte value1, byte value2, byte value3)
		{
			WriteCheckForSpace(4);
			m_Buffer[m_Pos] = value0;
			m_Buffer[m_Pos + 1] = value1;
			m_Buffer[m_Pos + 2] = value2;
			m_Buffer[m_Pos + 3] = value3;
			m_Pos += 4u;
		}

		public void WriteByte8(byte value0, byte value1, byte value2, byte value3, byte value4, byte value5, byte value6, byte value7)
		{
			WriteCheckForSpace(8);
			m_Buffer[m_Pos] = value0;
			m_Buffer[m_Pos + 1] = value1;
			m_Buffer[m_Pos + 2] = value2;
			m_Buffer[m_Pos + 3] = value3;
			m_Buffer[m_Pos + 4] = value4;
			m_Buffer[m_Pos + 5] = value5;
			m_Buffer[m_Pos + 6] = value6;
			m_Buffer[m_Pos + 7] = value7;
			m_Pos += 8u;
		}

		public void WriteBytesAtOffset(byte[] buffer, ushort targetOffset, ushort count)
		{
			uint num = (uint)(count + targetOffset);
			WriteCheckForSpace((ushort)num);
			if (targetOffset == 0 && count == buffer.Length)
			{
				buffer.CopyTo(m_Buffer, (int)m_Pos);
			}
			else
			{
				for (int i = 0; i < count; i++)
				{
					m_Buffer[targetOffset + i] = buffer[i];
				}
			}
			if (num > m_Pos)
			{
				m_Pos = num;
			}
		}

		public void WriteBytes(byte[] buffer, ushort count)
		{
			WriteCheckForSpace(count);
			if (count == buffer.Length)
			{
				buffer.CopyTo(m_Buffer, (int)m_Pos);
			}
			else
			{
				for (int i = 0; i < count; i++)
				{
					m_Buffer[m_Pos + i] = buffer[i];
				}
			}
			m_Pos += count;
		}

		private void WriteCheckForSpace(ushort count)
		{
			if (m_Pos + count < m_Buffer.Length)
			{
				return;
			}
			int num = (int)Math.Ceiling((float)m_Buffer.Length * 1.5f);
			while (m_Pos + count >= num)
			{
				num = (int)Math.Ceiling((float)num * 1.5f);
				if (num > 134217728)
				{
					Debug.LogWarning("NetworkBuffer size is " + num + " bytes!");
				}
			}
			byte[] array = new byte[num];
			m_Buffer.CopyTo(array, 0);
			m_Buffer = array;
		}

		public void FinishMessage()
		{
			ushort num = (ushort)(m_Pos - 4);
			m_Buffer[0] = (byte)(num & 0xFF);
			m_Buffer[1] = (byte)((num >> 8) & 0xFF);
		}

		public void SeekZero()
		{
			m_Pos = 0u;
		}

		public void Replace(byte[] buffer)
		{
			m_Buffer = buffer;
			m_Pos = 0u;
		}

		public override string ToString()
		{
			return $"NetBuf sz:{m_Buffer.Length} pos:{m_Pos}";
		}
	}
}
