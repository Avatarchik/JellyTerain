using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Profiling
{
	[UsedByNativeCode]
	public class Sampler
	{
		internal IntPtr m_Ptr;

		internal static Sampler s_InvalidSampler = new Sampler();

		public bool isValid => m_Ptr != IntPtr.Zero;

		[ThreadAndSerializationSafe]
		public string name
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		internal Sampler()
		{
		}

		public Recorder GetRecorder()
		{
			Recorder recorderInternal = GetRecorderInternal();
			return recorderInternal ?? Recorder.s_InvalidRecorder;
		}

		public static Sampler Get(string name)
		{
			Sampler samplerInternal = GetSamplerInternal(name);
			return samplerInternal ?? s_InvalidSampler;
		}

		public static int GetNames(List<string> names)
		{
			return GetSamplerNamesInternal(names);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern Recorder GetRecorderInternal();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern Sampler GetSamplerInternal(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int GetSamplerNamesInternal(object namesScriptingPtr);
	}
}
