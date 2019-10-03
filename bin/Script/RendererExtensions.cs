using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public static class RendererExtensions
	{
		public static void UpdateGIMaterials(this Renderer renderer)
		{
			UpdateGIMaterialsForRenderer(renderer);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal static extern void UpdateGIMaterialsForRenderer(Renderer renderer);
	}
}
