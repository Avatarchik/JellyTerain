using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Diagnostics
{
	public static class PlayerConnection
	{
		public static bool connected
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void SendFile(string remoteFilePath, byte[] data);
	}
}
