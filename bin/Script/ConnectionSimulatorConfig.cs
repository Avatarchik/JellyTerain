using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Networking
{
	public sealed class ConnectionSimulatorConfig : IDisposable
	{
		internal IntPtr m_Ptr;

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern ConnectionSimulatorConfig(int outMinDelay, int outAvgDelay, int inMinDelay, int inAvgDelay, float packetLossPercentage);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		public extern void Dispose();

		~ConnectionSimulatorConfig()
		{
			Dispose();
		}
	}
}
