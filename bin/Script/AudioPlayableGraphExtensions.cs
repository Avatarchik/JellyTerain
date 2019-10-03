using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Director
{
	public static class AudioPlayableGraphExtensions
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int internalAudioOutputCount(ref PlayableGraph graph);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool InternalGetAudioOutput(ref PlayableGraph graph, int index, out PlayableOutput output);
	}
}
