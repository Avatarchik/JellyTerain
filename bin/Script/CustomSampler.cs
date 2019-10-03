using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Profiling
{
	[UsedByNativeCode]
	public sealed class CustomSampler : Sampler
	{
		internal static CustomSampler s_InvalidCustomSampler = new CustomSampler();

		internal CustomSampler()
		{
		}

		public static CustomSampler Create(string name)
		{
			CustomSampler customSampler = CreateInternal(name);
			return customSampler ?? s_InvalidCustomSampler;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern CustomSampler CreateInternal(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[Conditional("ENABLE_PROFILER")]
		[GeneratedByOldBindingsGenerator]
		public extern void Begin();

		[Conditional("ENABLE_PROFILER")]
		public void Begin(Object targetObject)
		{
			BeginWithObject(targetObject);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void BeginWithObject(Object targetObject);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[Conditional("ENABLE_PROFILER")]
		[GeneratedByOldBindingsGenerator]
		public extern void End();
	}
}
