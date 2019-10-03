using System;
using System.Text;

namespace UnityEngine.Networking
{
	public class NetworkReader
	{
		private NetBuffer m_buf;

		private const int k_MaxStringLength = 32768;

		private const int k_InitialStringBufferSize = 1024;

		private static byte[] s_StringReaderBuffer;

		private static Encoding s_Encoding;

		public uint Position => m_buf.Position;

		public int Length => m_buf.Length;

		public NetworkReader()
		{
			m_buf = new NetBuffer();
			Initialize();
		}

		public NetworkReader(NetworkWriter writer)
		{
			m_buf = new NetBuffer(writer.AsArray());
			Initialize();
		}

		public NetworkReader(byte[] buffer)
		{
			m_buf = new NetBuffer(buffer);
			Initialize();
		}

		private static void Initialize()
		{
			if (s_Encoding == null)
			{
				s_StringReaderBuffer = new byte[1024];
				s_Encoding = new UTF8Encoding();
			}
		}

		public void SeekZero()
		{
			m_buf.SeekZero();
		}

		internal void Replace(byte[] buffer)
		{
			m_buf.Replace(buffer);
		}

		public uint ReadPackedUInt32()
		{
			byte b = ReadByte();
			if (b < 241)
			{
				return b;
			}
			byte b2 = ReadByte();
			if (b >= 241 && b <= 248)
			{
				return (uint)(240 + 256 * (b - 241) + b2);
			}
			byte b3 = ReadByte();
			if (b == 249)
			{
				return (uint)(2288 + 256 * b2 + b3);
			}
			byte b4 = ReadByte();
			if (b == 250)
			{
				return (uint)(b2 + (b3 << 8) + (b4 << 16));
			}
			byte b5 = ReadByte();
			if (b >= 251)
			{
				return (uint)(b2 + (b3 << 8) + (b4 << 16) + (b5 << 24));
			}
			throw new IndexOutOfRangeException("ReadPackedUInt32() failure: " + b);
		}

		public ulong ReadPackedUInt64()
		{
			byte b = ReadByte();
			if (b < 241)
			{
				return b;
			}
			byte b2 = ReadByte();
			if (b >= 241 && b <= 248)
			{
				return (ulong)(240 + 256 * ((long)b - 241L) + b2);
			}
			byte b3 = ReadByte();
			if (b == 249)
			{
				return (ulong)(2288 + 256L * (long)b2 + b3);
			}
			byte b4 = ReadByte();
			if (b == 250)
			{
				return b2 + ((ulong)b3 << 8) + ((ulong)b4 << 16);
			}
			byte b5 = ReadByte();
			if (b == 251)
			{
				return b2 + ((ulong)b3 << 8) + ((ulong)b4 << 16) + ((ulong)b5 << 24);
			}
			byte b6 = ReadByte();
			if (b == 252)
			{
				return b2 + ((ulong)b3 << 8) + ((ulong)b4 << 16) + ((ulong)b5 << 24) + ((ulong)b6 << 32);
			}
			byte b7 = ReadByte();
			if (b == 253)
			{
				return b2 + ((ulong)b3 << 8) + ((ulong)b4 << 16) + ((ulong)b5 << 24) + ((ulong)b6 << 32) + ((ulong)b7 << 40);
			}
			byte b8 = ReadByte();
			if (b == 254)
			{
				return b2 + ((ulong)b3 << 8) + ((ulong)b4 << 16) + ((ulong)b5 << 24) + ((ulong)b6 << 32) + ((ulong)b7 << 40) + ((ulong)b8 << 48);
			}
			byte b9 = ReadByte();
			if (b == byte.MaxValue)
			{
				return b2 + ((ulong)b3 << 8) + ((ulong)b4 << 16) + ((ulong)b5 << 24) + ((ulong)b6 << 32) + ((ulong)b7 << 40) + ((ulong)b8 << 48) + ((ulong)b9 << 56);
			}
			throw new IndexOutOfRangeException("ReadPackedUInt64() failure: " + b);
		}

		public NetworkInstanceId ReadNetworkId()
		{
			return new NetworkInstanceId(ReadPackedUInt32());
		}

		public NetworkSceneId ReadSceneId()
		{
			return new NetworkSceneId(ReadPackedUInt32());
		}

		public byte ReadByte()
		{
			return m_buf.ReadByte();
		}

		public sbyte ReadSByte()
		{
			return (sbyte)m_buf.ReadByte();
		}

		public short ReadInt16()
		{
			ushort num = 0;
			num = (ushort)(num | m_buf.ReadByte());
			num = (ushort)(num | (ushort)(m_buf.ReadByte() << 8));
			return (short)num;
		}

		public ushort ReadUInt16()
		{
			ushort num = 0;
			num = (ushort)(num | m_buf.ReadByte());
			return (ushort)(num | (ushort)(m_buf.ReadByte() << 8));
		}

		public int ReadInt32()
		{
			uint num = 0u;
			num |= m_buf.ReadByte();
			num = (uint)((int)num | (m_buf.ReadByte() << 8));
			num = (uint)((int)num | (m_buf.ReadByte() << 16));
			return (int)num | (m_buf.ReadByte() << 24);
		}

		public uint ReadUInt32()
		{
			uint num = 0u;
			num |= m_buf.ReadByte();
			num = (uint)((int)num | (m_buf.ReadByte() << 8));
			num = (uint)((int)num | (m_buf.ReadByte() << 16));
			return (uint)((int)num | (m_buf.ReadByte() << 24));
		}

		public long ReadInt64()
		{
			ulong num = 0uL;
			ulong num2 = m_buf.ReadByte();
			num |= num2;
			num2 = (ulong)m_buf.ReadByte() << 8;
			num |= num2;
			num2 = (ulong)m_buf.ReadByte() << 16;
			num |= num2;
			num2 = (ulong)m_buf.ReadByte() << 24;
			num |= num2;
			num2 = (ulong)m_buf.ReadByte() << 32;
			num |= num2;
			num2 = (ulong)m_buf.ReadByte() << 40;
			num |= num2;
			num2 = (ulong)m_buf.ReadByte() << 48;
			num |= num2;
			num2 = (ulong)m_buf.ReadByte() << 56;
			return (long)(num | num2);
		}

		public ulong ReadUInt64()
		{
			ulong num = 0uL;
			ulong num2 = m_buf.ReadByte();
			num |= num2;
			num2 = (ulong)m_buf.ReadByte() << 8;
			num |= num2;
			num2 = (ulong)m_buf.ReadByte() << 16;
			num |= num2;
			num2 = (ulong)m_buf.ReadByte() << 24;
			num |= num2;
			num2 = (ulong)m_buf.ReadByte() << 32;
			num |= num2;
			num2 = (ulong)m_buf.ReadByte() << 40;
			num |= num2;
			num2 = (ulong)m_buf.ReadByte() << 48;
			num |= num2;
			num2 = (ulong)m_buf.ReadByte() << 56;
			return num | num2;
		}

		public decimal ReadDecimal()
		{
			return new decimal(new int[4]
			{
				ReadInt32(),
				ReadInt32(),
				ReadInt32(),
				ReadInt32()
			});
		}

		public float ReadSingle()
		{
			uint value = ReadUInt32();
			return FloatConversion.ToSingle(value);
		}

		public double ReadDouble()
		{
			ulong value = ReadUInt64();
			return FloatConversion.ToDouble(value);
		}

		public string ReadString()
		{
			ushort num = ReadUInt16();
			if (num == 0)
			{
				return "";
			}
			if (num >= 32768)
			{
				throw new IndexOutOfRangeException("ReadString() too long: " + num);
			}
			while (num > s_StringReaderBuffer.Length)
			{
				s_StringReaderBuffer = new byte[s_StringReaderBuffer.Length * 2];
			}
			m_buf.ReadBytes(s_StringReaderBuffer, num);
			char[] chars = s_Encoding.GetChars(s_StringReaderBuffer, 0, num);
			return new string(chars);
		}

		public char ReadChar()
		{
			return (char)m_buf.ReadByte();
		}

		public bool ReadBoolean()
		{
			int num = m_buf.ReadByte();
			return num == 1;
		}

		public byte[] ReadBytes(int count)
		{
			if (count < 0)
			{
				throw new IndexOutOfRangeException("NetworkReader ReadBytes " + count);
			}
			byte[] array = new byte[count];
			m_buf.ReadBytes(array, (uint)count);
			return array;
		}

		public byte[] ReadBytesAndSize()
		{
			ushort num = ReadUInt16();
			if (num == 0)
			{
				return null;
			}
			return ReadBytes(num);
		}

		public Vector2 ReadVector2()
		{
			return new Vector2(ReadSingle(), ReadSingle());
		}

		public Vector3 ReadVector3()
		{
			return new Vector3(ReadSingle(), ReadSingle(), ReadSingle());
		}

		public Vector4 ReadVector4()
		{
			return new Vector4(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
		}

		public Color ReadColor()
		{
			return new Color(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
		}

		public Color32 ReadColor32()
		{
			return new Color32(ReadByte(), ReadByte(), ReadByte(), ReadByte());
		}

		public Quaternion ReadQuaternion()
		{
			return new Quaternion(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
		}

		public Rect ReadRect()
		{
			return new Rect(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
		}

		public Plane ReadPlane()
		{
			return new Plane(ReadVector3(), ReadSingle());
		}

		public Ray ReadRay()
		{
			return new Ray(ReadVector3(), ReadVector3());
		}

		public Matrix4x4 ReadMatrix4x4()
		{
			Matrix4x4 result = default(Matrix4x4);
			result.m00 = ReadSingle();
			result.m01 = ReadSingle();
			result.m02 = ReadSingle();
			result.m03 = ReadSingle();
			result.m10 = ReadSingle();
			result.m11 = ReadSingle();
			result.m12 = ReadSingle();
			result.m13 = ReadSingle();
			result.m20 = ReadSingle();
			result.m21 = ReadSingle();
			result.m22 = ReadSingle();
			result.m23 = ReadSingle();
			result.m30 = ReadSingle();
			result.m31 = ReadSingle();
			result.m32 = ReadSingle();
			result.m33 = ReadSingle();
			return result;
		}

		public NetworkHash128 ReadNetworkHash128()
		{
			NetworkHash128 result = default(NetworkHash128);
			result.i0 = ReadByte();
			result.i1 = ReadByte();
			result.i2 = ReadByte();
			result.i3 = ReadByte();
			result.i4 = ReadByte();
			result.i5 = ReadByte();
			result.i6 = ReadByte();
			result.i7 = ReadByte();
			result.i8 = ReadByte();
			result.i9 = ReadByte();
			result.i10 = ReadByte();
			result.i11 = ReadByte();
			result.i12 = ReadByte();
			result.i13 = ReadByte();
			result.i14 = ReadByte();
			result.i15 = ReadByte();
			return result;
		}

		public Transform ReadTransform()
		{
			NetworkInstanceId networkInstanceId = ReadNetworkId();
			if (networkInstanceId.IsEmpty())
			{
				return null;
			}
			GameObject gameObject = ClientScene.FindLocalObject(networkInstanceId);
			if (gameObject == null)
			{
				if (LogFilter.logDebug)
				{
					Debug.Log("ReadTransform netId:" + networkInstanceId);
				}
				return null;
			}
			return gameObject.transform;
		}

		public GameObject ReadGameObject()
		{
			NetworkInstanceId networkInstanceId = ReadNetworkId();
			if (networkInstanceId.IsEmpty())
			{
				return null;
			}
			GameObject gameObject = (!NetworkServer.active) ? ClientScene.FindLocalObject(networkInstanceId) : NetworkServer.FindLocalObject(networkInstanceId);
			if (gameObject == null && LogFilter.logDebug)
			{
				Debug.Log("ReadGameObject netId:" + networkInstanceId + "go: null");
			}
			return gameObject;
		}

		public NetworkIdentity ReadNetworkIdentity()
		{
			NetworkInstanceId networkInstanceId = ReadNetworkId();
			if (networkInstanceId.IsEmpty())
			{
				return null;
			}
			GameObject gameObject = (!NetworkServer.active) ? ClientScene.FindLocalObject(networkInstanceId) : NetworkServer.FindLocalObject(networkInstanceId);
			if (gameObject == null)
			{
				if (LogFilter.logDebug)
				{
					Debug.Log("ReadNetworkIdentity netId:" + networkInstanceId + "go: null");
				}
				return null;
			}
			return gameObject.GetComponent<NetworkIdentity>();
		}

		public override string ToString()
		{
			return m_buf.ToString();
		}

		public TMsg ReadMessage<TMsg>() where TMsg : MessageBase, new()
		{
			TMsg result = new TMsg();
			result.Deserialize(this);
			return result;
		}
	}
}
