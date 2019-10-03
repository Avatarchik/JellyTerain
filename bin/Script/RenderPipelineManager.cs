using System;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Rendering
{
	internal static class RenderPipelineManager
	{
		private static IRenderPipelineAsset s_CurrentPipelineAsset;

		public static IRenderPipeline currentPipeline
		{
			get;
			private set;
		}

		[RequiredByNativeCode]
		internal static void CleanupRenderPipeline()
		{
			if (s_CurrentPipelineAsset != null)
			{
				s_CurrentPipelineAsset.DestroyCreatedInstances();
			}
			s_CurrentPipelineAsset = null;
			currentPipeline = null;
		}

		[RequiredByNativeCode]
		private static bool DoRenderLoop_Internal(IRenderPipelineAsset pipe, Camera[] cameras, IntPtr loopPtr)
		{
			if (!PrepareRenderPipeline(pipe))
			{
				return false;
			}
			ScriptableRenderContext renderContext = default(ScriptableRenderContext);
			renderContext.Initialize(loopPtr);
			currentPipeline.Render(renderContext, cameras);
			return true;
		}

		private static bool PrepareRenderPipeline(IRenderPipelineAsset pipe)
		{
			if (s_CurrentPipelineAsset != pipe)
			{
				if (s_CurrentPipelineAsset != null)
				{
					CleanupRenderPipeline();
				}
				s_CurrentPipelineAsset = pipe;
			}
			if (s_CurrentPipelineAsset != null && (currentPipeline == null || currentPipeline.disposed))
			{
				currentPipeline = s_CurrentPipelineAsset.CreatePipeline();
			}
			return s_CurrentPipelineAsset != null;
		}
	}
}
