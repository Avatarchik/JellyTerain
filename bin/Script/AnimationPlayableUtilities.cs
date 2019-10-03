namespace UnityEngine.Experimental.Director
{
	public class AnimationPlayableUtilities
	{
		public static void Play(Animator animator, PlayableHandle playable, PlayableGraph graph)
		{
			graph.CreateAnimationOutput("AnimationClip", animator).sourcePlayable = playable;
			graph.SyncUpdateAndTimeMode(animator);
			graph.Play();
		}

		public static PlayableHandle PlayClip(Animator animator, AnimationClip clip, out PlayableGraph graph)
		{
			graph = PlayableGraph.CreateGraph();
			PlayableHandle result = graph.CreateAnimationOutput("AnimationClip", animator).sourcePlayable = graph.CreateAnimationClipPlayable(clip);
			graph.SyncUpdateAndTimeMode(animator);
			graph.Play();
			return result;
		}

		public static PlayableHandle PlayMixer(Animator animator, int inputCount, out PlayableGraph graph)
		{
			graph = PlayableGraph.CreateGraph();
			PlayableHandle result = graph.CreateAnimationOutput("Mixer", animator).sourcePlayable = graph.CreateAnimationMixerPlayable(inputCount);
			graph.SyncUpdateAndTimeMode(animator);
			graph.Play();
			return result;
		}

		public static PlayableHandle PlayAnimatorController(Animator animator, RuntimeAnimatorController controller, out PlayableGraph graph)
		{
			graph = PlayableGraph.CreateGraph();
			PlayableHandle result = graph.CreateAnimationOutput("AnimatorControllerPlayable", animator).sourcePlayable = graph.CreateAnimatorControllerPlayable(controller);
			graph.SyncUpdateAndTimeMode(animator);
			graph.Play();
			return result;
		}
	}
}
