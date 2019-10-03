using System;
using System.Text;

namespace UnityEngine.Networking
{
	public class NetworkWriter
	{
		private const int k_MaxStringLength = 32768;

		private NetBuffer m_Buffer;

		private static Encoding s_Encoding;

		private static byte[] s_StringWriteBuffer;

		private static UIntFloat s_FloatConverter;

		public short Position => (short)m_Buffer.Position;

		public NetworkWriter()
		{
			m_Buffer = new NetBuffer();
			if (s_Encoding == null)
			{
				s_Encoding = new UTF8Encoding();
				s_StringWriteBuffer = new byte[32768];
			}
		}

		public NetworkWriter(byte[] buffer)
		{
			m_Buffer = new NetBuffer(buffer);
			if (s_Encoding == null)
			{
				s_Encoding = new UTF8Encoding();
				s_StringWriteBuffer = new byte[32768];
			}
		}

		public byte[] ToArray()
		{
			byte[] array = new byte[m_Buffer.AsArraySegment().Count];
			Array.Copy(m_Buffer.AsArraySegment().Array, array, m_Buffer.AsArraySegment().Count);
			return array;
		}

		public byte[] AsArray()
		{
			return AsArraySegment().Array;
		}

		internal ArraySegment<byte> AsArraySegment()
		{
			return m_Buffer.AsArraySegment();
		}

		public void WritePackedUInt32(uint value)
		{
			if (value <= 240)
			{
				Write((byte)value);
			}
			else if (value <= 2287)
			{
				Write((byte)((value - 240) / 256u + 241));
				Write((byte)((value - 240) % 256u));
			}
			else if (value <= 67823)
			{
				Write((byte)249);
				Write((byte)((value - 2288) / 256u));
				Write((byte)((value - 2288) % 256u));
			}
			else if (value <= 16777215)
			{
				Write((byte)250);
				Write((byte)(value & 0xFF));
				Write((byte)((value >> 8) & 0xFF));
				Write((byte)((value >> 16) & 0xFF));
			}
			else
			{
				Write((byte)251);
				Write((byte)(value & 0xFF));
				Write((byte)((value >> 8) & 0xFF));
				Write((byte)((value >> 16) & 0xFF));
				Write((byte)((value >> 24) & 0xFF));
			}
		}

		public void WritePackedUInt64(ulong value)
		{
			if (value <= 240)
			{
				Write((byte)value);
			}
			else if (value <= 2287)
			{
				Write((byte)((value - 240) / 256uL + 241));
				Write((byte)((value - 240) % 256uL));
			}
			else if (value <= 67823)
			{
				Write((byte)249);
				Write((byte)((value - 2288) / 256uL));
				Write((byte)((value - 2288) % 256uL));
			}
			else if (value <= 16777215)
			{
				Write((byte)250);
				Write((byte)(value & 0xFF));
				Write((byte)((value >> 8) & 0xFF));
				Write((byte)((value >> 16) & 0xFF));
			}
			else if (value <= uint.MaxValue)
			{
				Write((byte)251);
				Write((byte)(value & 0xFF));
				Write((byte)((value >> 8) & 0xFF));
				Write((byte)((value >> 16) & 0xFF));
				Write((byte)((value >> 24) & 0xFF));
			}
			else if (value <= 1099511627775L)
			{
				Write((byte)252);
				Write((byte)(value & 0xFF));
				Write((byte)((value >> 8) & 0xFF));
				Write((byte)((value >> 16) & 0xFF));
				Write((byte)((value >> 24) & 0xFF));
				Write((byte)((value >> 32) & 0xFF));
			}
			else if (value <= 281474976710655L)
			{
				Write((byte)253);
				Write((byte)(value & 0xFF));
				Write((byte)((value >> 8) & 0xFF));
				Write((byte)((value >> 16) & 0xFF));
				Write((byte)((value >> 24) & 0xFF));
				Write((byte)((value >> 32) & 0xFF));
				Write((byte)((value >> 40) & 0xFF));
			}
			else if (value <= 72057594037927935L)
			{
				Write((byte)254);
				Write((byte)(value & 0xFF));
				Write((byte)((value >> 8) & 0xFF));
				Write((byte)((value >> 16) & 0xFF));
				Write((byte)((value >> 24) & 0xFF));
				Write((byte)((value >> 32) & 0xFF));
				Write((byte)((value >> 40) & 0xFF));
				Write((byte)((value >> 48) & 0xFF));
			}
			else
			{
				Write(byte.MaxValue);
				Write((byte)(value & 0xFF));
				Write((byte)((value >> 8) & 0xFF));
				Write((byte)((value >> 16) & 0xFF));
				Write((byte)((value >> 24) & 0xFF));
				Write((byte)((value >> 32) & 0xFF));
				Write((byte)((value >> 40) & 0xFF));
				Write((byte)((value >> 48) & 0xFF));
				Write((byte)((value >> 56) & 0xFF));
			}
		}

		public void Write(NetworkInstanceId value)
		{
			WritePackedUInt32(value.Value);
		}

		public void Write(NetworkSceneId value)
		{
			WritePackedUInt32(value.Value);
		}

		public void Write(char value)
		{
			m_Buffer.WriteByte((byte)value);
		}

		public void Write(byte value)
		{
			m_Buffer.WriteByte(value);
		}

		public void Write(sbyte value)
		{
			m_Buffer.WriteByte((byte)value);
		}

		public void Write(short value)
		{
			m_Buffer.WriteByte2((byte)(value & 0xFF), (byte)((value >> 8) & 0xFF));
		}

		public void Write(ushort value)
		{
			m_Buffer.WriteByte2((byte)(value & 0xFF), (byte)((value >> 8) & 0xFF));
		}

		public void Write(int value)
		{
			m_Buffer.WriteByte4((byte)(value & 0xFF), (byte)((value >> 8) & 0xFF), (byte)((value >> 16) & 0xFF), (byte)((value >> 24) & 0xFF));
		}

		public void Write(uint value)
		{
			m_Buffer.WriteByte4((byte)(value & 0xFF), (byte)((value >> 8) & 0xFF), (byte)((value >> 16) & 0xFF), (byte)((value >> 24) & 0xFF));
		}

		public void Write(long value)
		{
			m_Buffer.WriteByte8((byte)(value & 0xFF), (byte)((value >> 8) & 0xFF), (byte)((value >> 16) & 0xFF), (byte)((value >> 24) & 0xFF), (byte)((value >> 32) & 0xFF), (byte)((value >> 40) & 0xFF), (byte)((value >> 48) & 0xFF), (byte)((value >> 56) & 0xFF));
		}

		public void Write(ulong value)
		{
			m_Buffer.WriteByte8((byte)(value & 0xFF), (byte)((value >> 8) & 0xFF), (byte)((value >> 16) & 0xFF), (byte)((value >> 24) & 0xFF), (byte)((value >> 32) & 0xFF), (byte)((value >> 40) & 0xFF), (byte)((value >> 48) & 0xFF), (byte)((value >> 56) & 0xFF));
		}

		public void Write(float value)
		{
			s_FloatConverter.floatValue = value;
			Write(s_FloatConverter.intValue);
		}

		public void Write(double value)
		{
			s_FloatConverter.doubleValue = value;
			Write(s_FloatConverter.longValue);
		}

		public void Write(decimal value)
		{
			int[] bits = decimal.GetBits(value);
			Write(bits[0]);
			Write(bits[1]);
			Write(bits[2]);
			Write(bits[3]);
		}

		public void Write(string value)
		{
			if (value == null)
			{
				m_Buffer.WriteByte2(0, 0);
				return;
			}
			int byteCount = s_Encoding.GetByteCount(value);
			if (byteCount >= 32768)
			{
				throw new IndexOutOfRangeException("Serialize(string) too long: " + value.Length);
			}
			Write((ushort)byteCount);
			int bytes = s_Encoding.GetBytes(value, 0, value.Length, s_StringWriteBuffer, 0);
			m_Buffer.WriteBytes(s_StringWriteBuffer, (ushort)bytes);
		}

		public void Write(bool value)
		{
			if (value)
			{
				m_Buffer.WriteByte(1);
			}
			else
			{
				m_Buffer.WriteByte(0);
			}
		}

		public void Write(byte[] buffer, int count)
		{
			if (count > 65535)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("NetworkWriter Write: buffer is too large (" + count + ") bytes. The maximum buffer size is 64K bytes.");
				}
			}
			else
			{
				m_Buffer.WriteBytes(buffer, (ushort)count);
			}
		}

		public void Write(byte[] buffer, int offset, int count)
		{
			if (count > 65535)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("NetworkWriter Write: buffer is too large (" + count + ") bytes. The maximum buffer size is 64K bytes.");
				}
			}
			else
			{
				m_Buffer.WriteBytesAtOffset(buffer, (ushort)offset, (ushort)count);
			}
		}

		public void WriteBytesAndSize(byte[] buffer, int count)
		{
			if (buffer == null || count == 0)
			{
				Write((ushort)0);
			}
			else if (count > 65535)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("NetworkWriter WriteBytesAndSize: buffer is too large (" + count + ") bytes. The maximum buffer size is 64K bytes.");
				}
			}
			else
			{
				Write((ushort)count);
				m_Buffer.WriteBytes(buffer, (ushort)count);
			}
		}

		public void WriteBytesFull(byte[] buffer)
		{
			if (buffer == null)
			{
				Write((ushort)0);
			}
			else if (buffer.Length > 65535)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("NetworkWriter WriteBytes: buffer is too large (" + buffer.Length + ") bytes. The maximum buffer size is 64K bytes.");
				}
			}
			else
			{
				Write((ushort)buffer.Length);
				m_Buffer.WriteBytes(buffer, (ushort)buffer.Length);
			}
		}

		public void Write(Vector2 value)
		{
			Write(value.x);
			Write(value.y);
		}

		public void Write(Vector3 value)
		{
			Write(value.x);
			Write(value.y);
			Write(value.z);
		}

		public void Write(Vector4 value)
		{
			Write(value.x);
			Write(value.y);
			Write(value.z);
			Write(value.w);
		}

		public void Write(Color value)
		{
			Write(value.r);
			Write(value.g);
			Write(value.b);
			Write(value.a);
		}

		public void Write(Color32 value)
		{
			Write(value.r);
			Write(value.g);
			Write(value.b);
			Write(value.a);
		}

		public void Write(Quaternion value)
		{
			Write(value.x);
			Write(value.y);
			Write(value.z);
			Write(value.w);
		}

		public void Write(Rect value)
		{
			Write(value.xMin);
			Write(value.yMin);
			Write(value.width);
			Write(value.height);
		}

		public void Write(Plane value)
		{
			Write(value.normal);
			Write(value.distance);
		}

		public void Write(Ray value)
		{
			Write(value.direction);
			Write(value.origin);
		}

		public void Write(Matrix4x4 value)
		{
			Write(value.m00);
			Write(value.m01);
			Write(value.m02);
			Write(value.m03);
			Write(value.m10);
			Write(value.m11);
			Write(value.m12);
			Write(value.m13);
			Write(value.m20);
			Write(value.m21);
			Write(value.m22);
			Write(value.m23);
			Write(value.m30);
			Write(value.m31);
			Write(value.m32);
			Write(value.m33);
		}

		public void Write(NetworkHash128 value)
		{
			Write(value.i0);
			Write(value.i1);
			Write(value.i2);
			Write(value.i3);
			Write(value.i4);
			Write(value.i5);
			Write(value.i6);
			Write(value.i7);
			Write(value.i8);
			Write(value.i9);
			Write(value.i10);
			Write(value.i11);
			Write(value.i12);
			Write(value.i13);
			Write(value.i14);
			Write(value.i15);
		}

		public void Write(NetworkIdentity value)
		{
			if (value == null)
			{
				WritePackedUInt32(0u);
			}
			else
			{
				Write(value.netId);
			}
		}

		public void Write(Transform value)
		{
			if (value == null || value.gameObject == null)
			{
				WritePackedUInt32(0u);
				return;
			}
			NetworkIdentity component = value.gameObject.GetComponent<NetworkIdentity>();
			if (component != null)
			{
				Write(component.netId);
				return;
			}
			if (LogFilter.logWarn)
			{
				Debug.LogWarning("NetworkWriter " + value + " has no NetworkIdentity");
			}
			WritePackedUInt32(0u);
		}

		public void Write(GameObject value)
		{
			if (value == null)
			{
				WritePackedUInt32(0u);
				return;
			}
			NetworkIdentity component = value.GetComponent<NetworkIdentity>();
			if (component != null)
			{
				Write(component.netId);
				return;
			}
			if (LogFilter.logWarn)
			{
				Debug.LogWarning("NetworkWriter " + value + " has no NetworkIdentity");
			}
			WritePackedUInt32(0u);
		}

		public void Write(MessageBase msg)
		{
			msg.Serialize(this);
		}

		public void SeekZero()
		{
			m_Buffer.SeekZero();
		}

		public void StartMessage(short msgType)
		{
			SeekZero();
			m_Buffer.WriteByte2(0, 0);
			Write(msgType);
		}

		public void FinishMessage()
		{
			m_Buffer.FinishMessage();
		}
	}
}
