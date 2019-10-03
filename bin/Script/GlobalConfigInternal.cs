using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Networking
{
	internal sealed class GlobalConfigInternal : IDisposable
	{
		internal IntPtr m_Ptr;

		public GlobalConfigInternal(GlobalConfig config)
		{
			InitWrapper();
			InitThreadAwakeTimeout(config.ThreadAwakeTimeout);
			InitReactorModel((byte)config.ReactorModel);
			InitReactorMaximumReceivedMessages(config.ReactorMaximumReceivedMessages);
			InitReactorMaximumSentMessages(config.ReactorMaximumSentMessages);
			InitMaxPacketSize(config.MaxPacketSize);
			InitMaxHosts(config.MaxHosts);
			if (config.ThreadPoolSize == 0 || config.ThreadPoolSize > 254)
			{
				throw new ArgumentOutOfRangeException("Worker thread pool size should be >= 1 && < 254 (for server only)");
			}
			InitThreadPoolSize(config.ThreadPoolSize);
			InitMinTimerTimeout(config.MinTimerTimeout);
			InitMaxTimerTimeout(config.MaxTimerTimeout);
			InitMinNetSimulatorTimeout(config.MinNetSimulatorTimeout);
			InitMaxNetSimulatorTimeout(config.MaxNetSimulatorTimeout);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitWrapper();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitThreadAwakeTimeout(uint ms);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitReactorModel(byte model);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitReactorMaximumReceivedMessages(ushort size);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitReactorMaximumSentMessages(ushort size);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitMaxPacketSize(ushort size);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitMaxHosts(ushort size);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitThreadPoolSize(byte size);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitMinTimerTimeout(uint ms);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitMaxTimerTimeout(uint ms);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitMinNetSimulatorTimeout(uint ms);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InitMaxNetSimulatorTimeout(uint ms);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public extern void Dispose();

		~GlobalConfigInternal()
		{
			Dispose();
		}
	}
}
