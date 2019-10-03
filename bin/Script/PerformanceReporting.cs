using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Analytics
{
	public static class PerformanceReporting
	{
		[ThreadAndSerializationSafe]
		public static bool enabled
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}
	}
}
