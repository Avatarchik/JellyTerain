using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Audio
{
	public class AudioMixerSnapshot : Object
	{
		public AudioMixer audioMixer
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		internal AudioMixerSnapshot()
		{
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void TransitionTo(float timeToReach);
	}
}
