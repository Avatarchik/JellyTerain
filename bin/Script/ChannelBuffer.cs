using System;
using System.Collections.Generic;

namespace UnityEngine.Networking
{
	internal class ChannelBuffer : IDisposable
	{
		private NetworkConnection m_Connection;

		private ChannelPacket m_CurrentPacket;

		private float m_LastFlushTime;

		private byte m_ChannelId;

		private int m_MaxPacketSize;

		private bool m_IsReliable;

		private bool m_AllowFragmentation;

		private bool m_IsBroken;

		private int m_MaxPendingPacketCount;

		private const int k_MaxFreePacketCount = 512;

		public const int MaxPendingPacketCount = 16;

		public const int MaxBufferedPackets = 512;

		private Queue<ChannelPacket> m_PendingPackets;

		private static List<ChannelPacket> s_FreePackets;

		internal static int pendingPacketCount;

		public float maxDelay = 0.01f;

		private float m_LastBufferedMessageCountTimer = Time.realtimeSinceStartup;

		private static NetworkWriter s_SendWriter = new NetworkWriter();

		private static NetworkWriter s_FragmentWriter = new NetworkWriter();

		private const int k_PacketHeaderReserveSize = 100;

		private bool m_Disposed;

		internal NetBuffer fragmentBuffer = new NetBuffer();

		private bool readingFragment = false;

		public int numMsgsOut
		{
			get;
			private set;
		}

		public int numBufferedMsgsOut
		{
			get;
			private set;
		}

		public int numBytesOut
		{
			get;
			private set;
		}

		public int numMsgsIn
		{
			get;
			private set;
		}

		public int numBytesIn
		{
			get;
			private set;
		}

		public int numBufferedPerSecond
		{
			get;
			private set;
		}

		public int lastBufferedPerSecond
		{
			get;
			private set;
		}

		public ChannelBuffer(NetworkConnection conn, int bufferSize, byte cid, bool isReliable, bool isSequenced)
		{
			m_Connection = conn;
			m_MaxPacketSize = bufferSize - 100;
			m_CurrentPacket = new ChannelPacket(m_MaxPacketSize, isReliable);
			m_ChannelId = cid;
			m_MaxPendingPacketCount = 16;
			m_IsReliable = isReliable;
			m_AllowFragmentation = (isReliable && isSequenced);
			if (isReliable)
			{
				m_PendingPackets = new Queue<ChannelPacket>();
				if (s_FreePackets == null)
				{
					s_FreePackets = new List<ChannelPacket>();
				}
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!m_Disposed && disposing && m_PendingPackets != null)
			{
				while (m_PendingPackets.Count > 0)
				{
					pendingPacketCount--;
					ChannelPacket item = m_PendingPackets.Dequeue();
					if (s_FreePackets.Count < 512)
					{
						s_FreePackets.Add(item);
					}
				}
				m_PendingPackets.Clear();
			}
			m_Disposed = true;
		}

		public bool SetOption(ChannelOption option, int value)
		{
			switch (option)
			{
			case ChannelOption.MaxPendingBuffers:
				if (!m_IsReliable)
				{
					return false;
				}
				if (value < 0 || value >= 512)
				{
					if (LogFilter.logError)
					{
						Debug.LogError("Invalid MaxPendingBuffers for channel " + m_ChannelId + ". Must be greater than zero and less than " + 512);
					}
					return false;
				}
				m_MaxPendingPacketCount = value;
				return true;
			case ChannelOption.AllowFragmentation:
				m_AllowFragmentation = (value != 0);
				return true;
			case ChannelOption.MaxPacketSize:
				if (!m_CurrentPacket.IsEmpty() || m_PendingPackets.Count > 0)
				{
					if (LogFilter.logError)
					{
						Debug.LogError("Cannot set MaxPacketSize after sending data.");
					}
					return false;
				}
				if (value <= 0)
				{
					if (LogFilter.logError)
					{
						Debug.LogError("Cannot set MaxPacketSize less than one.");
					}
					return false;
				}
				if (value > m_MaxPacketSize)
				{
					if (LogFilter.logError)
					{
						Debug.LogError("Cannot set MaxPacketSize to greater than the existing maximum (" + m_MaxPacketSize + ").");
					}
					return false;
				}
				m_CurrentPacket = new ChannelPacket(value, m_IsReliable);
				m_MaxPacketSize = value;
				return true;
			default:
				return false;
			}
		}

		public void CheckInternalBuffer()
		{
			if (Time.realtimeSinceStartup - m_LastFlushTime > maxDelay && !m_CurrentPacket.IsEmpty())
			{
				SendInternalBuffer();
				m_LastFlushTime = Time.realtimeSinceStartup;
			}
			if (Time.realtimeSinceStartup - m_LastBufferedMessageCountTimer > 1f)
			{
				lastBufferedPerSecond = numBufferedPerSecond;
				numBufferedPerSecond = 0;
				m_LastBufferedMessageCountTimer = Time.realtimeSinceStartup;
			}
		}

		public bool SendWriter(NetworkWriter writer)
		{
			return SendBytes(writer.AsArraySegment().Array, writer.AsArraySegment().Count);
		}

		public bool Send(short msgType, MessageBase msg)
		{
			s_SendWriter.StartMessage(msgType);
			msg.Serialize(s_SendWriter);
			s_SendWriter.FinishMessage();
			numMsgsOut++;
			return SendWriter(s_SendWriter);
		}

		internal bool HandleFragment(NetworkReader reader)
		{
			if (reader.ReadByte() == 0)
			{
				if (!readingFragment)
				{
					fragmentBuffer.SeekZero();
					readingFragment = true;
				}
				byte[] array = reader.ReadBytesAndSize();
				fragmentBuffer.WriteBytes(array, (ushort)array.Length);
				return false;
			}
			readingFragment = false;
			return true;
		}

		internal bool SendFragmentBytes(byte[] bytes, int bytesToSend)
		{
			int num = 0;
			while (bytesToSend > 0)
			{
				int num2 = Math.Min(bytesToSend, m_MaxPacketSize - 32);
				byte[] array = new byte[num2];
				Array.Copy(bytes, num, array, 0, num2);
				s_FragmentWriter.StartMessage(17);
				s_FragmentWriter.Write((byte)0);
				s_FragmentWriter.WriteBytesFull(array);
				s_FragmentWriter.FinishMessage();
				SendWriter(s_FragmentWriter);
				num += num2;
				bytesToSend -= num2;
			}
			s_FragmentWriter.StartMessage(17);
			s_FragmentWriter.Write((byte)1);
			s_FragmentWriter.FinishMessage();
			SendWriter(s_FragmentWriter);
			return true;
		}

		internal bool SendBytes(byte[] bytes, int bytesToSend)
		{
			if (bytesToSend >= 65535)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("ChannelBuffer:SendBytes cannot send packet larger than " + ushort.MaxValue + " bytes");
				}
				return false;
			}
			if (bytesToSend <= 0)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("ChannelBuffer:SendBytes cannot send zero bytes");
				}
				return false;
			}
			if (bytesToSend > m_MaxPacketSize)
			{
				if (m_AllowFragmentation)
				{
					return SendFragmentBytes(bytes, bytesToSend);
				}
				if (LogFilter.logError)
				{
					Debug.LogError("Failed to send big message of " + bytesToSend + " bytes. The maximum is " + m_MaxPacketSize + " bytes on channel:" + m_ChannelId);
				}
				return false;
			}
			if (!m_CurrentPacket.HasSpace(bytesToSend))
			{
				if (m_IsReliable)
				{
					if (m_PendingPackets.Count == 0)
					{
						if (!m_CurrentPacket.SendToTransport(m_Connection, m_ChannelId))
						{
							QueuePacket();
						}
						m_CurrentPacket.Write(bytes, bytesToSend);
						return true;
					}
					if (m_PendingPackets.Count >= m_MaxPendingPacketCount)
					{
						if (!m_IsBroken && LogFilter.logError)
						{
							Debug.LogError("ChannelBuffer buffer limit of " + m_PendingPackets.Count + " packets reached.");
						}
						m_IsBroken = true;
						return false;
					}
					QueuePacket();
					m_CurrentPacket.Write(bytes, bytesToSend);
					return true;
				}
				if (!m_CurrentPacket.SendToTransport(m_Connection, m_ChannelId))
				{
					if (LogFilter.logError)
					{
						Debug.Log("ChannelBuffer SendBytes no space on unreliable channel " + m_ChannelId);
					}
					return false;
				}
				m_CurrentPacket.Write(bytes, bytesToSend);
				return true;
			}
			m_CurrentPacket.Write(bytes, bytesToSend);
			if (maxDelay == 0f)
			{
				return SendInternalBuffer();
			}
			return true;
		}

		private void QueuePacket()
		{
			pendingPacketCount++;
			m_PendingPackets.Enqueue(m_CurrentPacket);
			m_CurrentPacket = AllocPacket();
		}

		private ChannelPacket AllocPacket()
		{
			if (s_FreePackets.Count == 0)
			{
				return new ChannelPacket(m_MaxPacketSize, m_IsReliable);
			}
			ChannelPacket result = s_FreePackets[s_FreePackets.Count - 1];
			s_FreePackets.RemoveAt(s_FreePackets.Count - 1);
			result.Reset();
			return result;
		}

		private static void FreePacket(ChannelPacket packet)
		{
			if (s_FreePackets.Count < 512)
			{
				s_FreePackets.Add(packet);
			}
		}

		public bool SendInternalBuffer()
		{
			if (m_IsReliable && m_PendingPackets.Count > 0)
			{
				while (m_PendingPackets.Count > 0)
				{
					ChannelPacket channelPacket = m_PendingPackets.Dequeue();
					if (!channelPacket.SendToTransport(m_Connection, m_ChannelId))
					{
						m_PendingPackets.Enqueue(channelPacket);
						break;
					}
					pendingPacketCount--;
					FreePacket(channelPacket);
					if (m_IsBroken && m_PendingPackets.Count < m_MaxPendingPacketCount / 2)
					{
						if (LogFilter.logWarn)
						{
							Debug.LogWarning("ChannelBuffer recovered from overflow but data was lost.");
						}
						m_IsBroken = false;
					}
				}
				return true;
			}
			return m_CurrentPacket.SendToTransport(m_Connection, m_ChannelId);
		}
	}
}
