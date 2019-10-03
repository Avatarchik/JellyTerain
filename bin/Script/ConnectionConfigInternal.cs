using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Networking
{
	internal sealed class ConnectionConfigInternal : IDisposable
	{
		internal IntPtr m_Ptr;

		public int ChannelSize
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		private ConnectionConfigInternal()
		{
		}

		public ConnectionConfigInternal(ConnectionConfig config)
		{
			if (config == null)
			{
				throw new NullReferenceException("config is not defined");
			}
			InitWrapper();
			InitPacketSize(config.PacketSize);
			InitFragmentSize(config.FragmentSize);
			InitResendTimeout(config.ResendTimeout);
			InitDisconnectTimeout(config.DisconnectTimeout);
			InitConnectTimeout(config.ConnectTimeout);
			InitMinUpdateTimeout(config.MinUpdateTimeout);
			InitPingTimeout(config.PingTimeout);
			InitReducedPingTimeout(config.ReducedPingTimeout);
			InitAllCostTimeout(config.AllCostTimeout);
			InitNetworkDropThreshold(config.NetworkDropThreshold);
			InitOverflowDropThreshold(config.OverflowDropThreshold);
			InitMaxConnectionAttempt(config.MaxConnectionAttempt);
			InitAckDelay(config.AckDelay);
			InitSendDelay(config.SendDelay);
			InitMaxCombinedReliableMessageSize(config.MaxCombinedReliableMessageSize);
			InitMaxCombinedReliableMessageCount(config.MaxCombinedReliableMessageCount);
			InitMaxSentMessageQueueSize(config.MaxSentMessageQueueSize);
			InitAcksType((int)config.AcksType);
			InitUsePlatformSpecificProtocols(config.UsePlatformSpecificProtocols);
			InitInitialBandwidth(config.InitialBandwidth);
			InitBandwidthPeakFactor(config.BandwidthPeakFactor);
			InitWebSocketReceiveBufferMaxSize(config.WebSocketReceiveBufferMaxSize);
			InitUdpSocketReceiveBufferMaxSize(config.UdpSocketReceiveBufferMaxSize);
			if (config.SSLCertFilePath != null)
			{
				int num = InitSSLCertFilePath(config.SSLCertFilePath);
				if (num != 0)
				{
					throw new ArgumentOutOfRangeException("SSLCertFilePath cannot be > than " + num.ToString());
				}
			}
			if (config.SSLPrivateKeyFilePath != null)
			{
				int num2 = InitSSLPrivateKeyFilePath(config.SSLPrivateKeyFilePath);
				if (num2 != 0)
				{
					throw new ArgumentOutOfRangeException("SSLPrivateKeyFilePath cannot be > than " + num2.ToString());
				}
			}
			if (config.SSLCAFilePath != null)
			{
				int num3 = InitSSLCAFilePath(config.SSLCAFilePath);
				if (num3 != 0)
				{
					throw new ArgumentOutOfRangeException("SSLCAFilePath cannot be > than " + num3.ToString());
				}
			}
			for (byte b = 0; b < config.ChannelCount; b = (byte)(b + 1))
			{
				AddChannel(config.GetChannel(b));
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitWrapper();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern byte AddChannel(QosType value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern QosType GetChannel(int i);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitPacketSize(ushort value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitFragmentSize(ushort value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitResendTimeout(uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitDisconnectTimeout(uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitConnectTimeout(uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitMinUpdateTimeout(uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitPingTimeout(uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitReducedPingTimeout(uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitAllCostTimeout(uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitNetworkDropThreshold(byte value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitOverflowDropThreshold(byte value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitMaxConnectionAttempt(byte value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitAckDelay(uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitSendDelay(uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitMaxCombinedReliableMessageSize(ushort value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitMaxCombinedReliableMessageCount(ushort value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitMaxSentMessageQueueSize(ushort value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitAcksType(int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitUsePlatformSpecificProtocols(bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitInitialBandwidth(uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitBandwidthPeakFactor(float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitWebSocketReceiveBufferMaxSize(ushort value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitUdpSocketReceiveBufferMaxSize(uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern int InitSSLCertFilePath(string value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern int InitSSLPrivateKeyFilePath(string value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern int InitSSLCAFilePath(string value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public extern void Dispose();

		~ConnectionConfigInternal()
		{
			Dispose();
		}
	}
}
