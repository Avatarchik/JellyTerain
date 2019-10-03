using System.Collections.Generic;

namespace UnityEngine.Experimental.Rendering
{
	public abstract class RenderPipelineAsset : ScriptableObject, IRenderPipelineAsset
	{
		private readonly List<IRenderPipeline> m_CreatedPipelines = new List<IRenderPipeline>();

		public void DestroyCreatedInstances()
		{
			foreach (IRenderPipeline createdPipeline in m_CreatedPipelines)
			{
				createdPipeline.Dispose();
			}
			m_CreatedPipelines.Clear();
		}

		public IRenderPipeline CreatePipeline()
		{
			IRenderPipeline renderPipeline = InternalCreatePipeline();
			if (renderPipeline != null)
			{
				m_CreatedPipelines.Add(renderPipeline);
			}
			return renderPipeline;
		}

		protected abstract IRenderPipeline InternalCreatePipeline();

		private void OnValidate()
		{
			DestroyCreatedInstances();
		}

		private void OnDisable()
		{
			DestroyCreatedInstances();
		}
	}
}
