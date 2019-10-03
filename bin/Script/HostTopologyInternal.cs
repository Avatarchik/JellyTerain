using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Networking
{
	internal sealed class HostTopologyInternal : IDisposable
	{
		internal IntPtr m_Ptr;

		public HostTopologyInternal(HostTopology topology)
		{
			ConnectionConfigInternal config = new ConnectionConfigInternal(topology.DefaultConfig);
			InitWrapper(config, topology.MaxDefaultConnections);
			for (int i = 1; i <= topology.SpecialConnectionConfigsCount; i++)
			{
				ConnectionConfig specialConnectionConfig = topology.GetSpecialConnectionConfig(i);
				ConnectionConfigInternal config2 = new ConnectionConfigInternal(specialConnectionConfig);
				AddSpecialConnectionConfig(config2);
			}
			InitOtherParameters(topology);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitWrapper(ConnectionConfigInternal config, int maxDefaultConnections);

		private int AddSpecialConnectionConfig(ConnectionConfigInternal config)
		{
			return AddSpecialConnectionConfigWrapper(config);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern int AddSpecialConnectionConfigWrapper(ConnectionConfigInternal config);

		private void InitOtherParameters(HostTopology topology)
		{
			InitReceivedPoolSize(topology.ReceivedMessagePoolSize);
			InitSentMessagePoolSize(topology.SentMessagePoolSize);
			InitMessagePoolSizeGrowthFactor(topology.MessagePoolSizeGrowthFactor);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitReceivedPoolSize(ushort pool);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitSentMessagePoolSize(ushort pool);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitMessagePoolSizeGrowthFactor(float factor);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public extern void Dispose();

		~HostTopologyInternal()
		{
			Dispose();
		}
	}
}
