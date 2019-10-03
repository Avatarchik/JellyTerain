using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Audio
{
	public class AudioMixerGroup : Object
	{
		public AudioMixer audioMixer
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		internal AudioMixerGroup()
		{
		}
	}
}
