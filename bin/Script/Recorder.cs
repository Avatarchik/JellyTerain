using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Profiling
{
	[UsedByNativeCode]
	public sealed class Recorder
	{
		internal IntPtr m_Ptr;

		internal static Recorder s_InvalidRecorder = new Recorder();

		public bool isValid => m_Ptr != IntPtr.Zero;

		[ThreadAndSerializationSafe]
		public bool enabled
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		[ThreadAndSerializationSafe]
		public long elapsedNanoseconds
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		[ThreadAndSerializationSafe]
		public int sampleBlockCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		internal Recorder()
		{
		}

		~Recorder()
		{
			if (m_Ptr != IntPtr.Zero)
			{
				DisposeNative();
			}
		}

		public static Recorder Get(string samplerName)
		{
			Sampler sampler = Sampler.Get(samplerName);
			return sampler.GetRecorder();
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		private extern void DisposeNative();
	}
}
