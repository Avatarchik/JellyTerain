using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.CrashReportHandler
{
	public static class CrashReportHandler
	{
		[ThreadAndSerializationSafe]
		public static bool enableCaptureExceptions
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
