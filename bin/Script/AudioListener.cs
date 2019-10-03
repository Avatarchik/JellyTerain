using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[RequireComponent(typeof(Transform))]
	public sealed class AudioListener : Behaviour
	{
		public static float volume
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static bool pause
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public AudioVelocityUpdateMode velocityUpdateMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void GetOutputDataHelper(float[] samples, int channel);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void GetSpectrumDataHelper(float[] samples, int channel, FFTWindow window);

		[Obsolete("GetOutputData returning a float[] is deprecated, use GetOutputData and pass a pre allocated array instead.")]
		public static float[] GetOutputData(int numSamples, int channel)
		{
			float[] array = new float[numSamples];
			GetOutputDataHelper(array, channel);
			return array;
		}

		public static void GetOutputData(float[] samples, int channel)
		{
			GetOutputDataHelper(samples, channel);
		}

		[Obsolete("GetSpectrumData returning a float[] is deprecated, use GetOutputData and pass a pre allocated array instead.")]
		public static float[] GetSpectrumData(int numSamples, int channel, FFTWindow window)
		{
			float[] array = new float[numSamples];
			GetSpectrumDataHelper(array, channel, window);
			return array;
		}

		public static void GetSpectrumData(float[] samples, int channel, FFTWindow window)
		{
			GetSpectrumDataHelper(samples, channel, window);
		}
	}
}
