using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Video
{
	public sealed class VideoClip : Object
	{
		public string originalPath
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public ulong frameCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public double frameRate
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public double length
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public uint width
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public uint height
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public ushort audioTrackCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public ushort GetAudioChannelCount(ushort audioTrackIdx)
		{
			return INTERNAL_CALL_GetAudioChannelCount(this, audioTrackIdx);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern ushort INTERNAL_CALL_GetAudioChannelCount(VideoClip self, ushort audioTrackIdx);

		public uint GetAudioSampleRate(ushort audioTrackIdx)
		{
			return INTERNAL_CALL_GetAudioSampleRate(this, audioTrackIdx);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern uint INTERNAL_CALL_GetAudioSampleRate(VideoClip self, ushort audioTrackIdx);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern string GetAudioLanguage(ushort audioTrackIdx);
	}
}
