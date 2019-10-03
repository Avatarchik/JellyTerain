// Class info from UnityEngine.dll
// 
using UnityEngine;

namespace UnityEngine.Experimental.Rendering
{
    public class CullResults : ValueType
    {
      // Fields:
  public visibleLights : VisibleLight[]
  public visibleReflectionProbes : VisibleReflectionProbe[]
  cullResults : IntPtr
      // Properties:
      // Events:
      // Methods:
      public Boolean UnityEngine.Experimental.Rendering.CullResults::GetCullingParameters(UnityEngine.Camera,UnityEngine.Experimental.Rendering.CullingParameters&)
      Boolean UnityEngine.Experimental.Rendering.CullResults::GetCullingParameters_Internal(UnityEngine.Camera,UnityEngine.Experimental.Rendering.CullingParameters&Int32)
      Void UnityEngine.Experimental.Rendering.CullResults::Internal_Cull(UnityEngine.Experimental.Rendering.CullingParameters&,UnityEngine.Experimental.Rendering.ScriptableRenderContext,UnityEngine.Experimental.Rendering.CullResults&)
      Void UnityEngine.Experimental.Rendering.CullResults::INTERNAL_CALL_Internal_Cull(UnityEngine.Experimental.Rendering.CullingParameters&,UnityEngine.Experimental.Rendering.ScriptableRenderContext&,UnityEngine.Experimental.Rendering.CullResults&)
      public UnityEngine.Experimental.Rendering.CullResults UnityEngine.Experimental.Rendering.CullResults::Cull(UnityEngine.Experimental.Rendering.CullingParameters&,UnityEngine.Experimental.Rendering.ScriptableRenderContext)
      public Boolean UnityEngine.Experimental.Rendering.CullResults::Cull(UnityEngine.Camera,UnityEngine.Experimental.Rendering.ScriptableRenderContext,UnityEngine.Experimental.Rendering.CullResults&)
      public Boolean UnityEngine.Experimental.Rendering.CullResults::GetShadowCasterBoundsInt32,UnityEngine.Bounds&)
      Boolean UnityEngine.Experimental.Rendering.CullResults::GetShadowCasterBoundsIntPtrInt32,UnityEngine.Bounds&)
      public Int32 UnityEngine.Experimental.Rendering.CullResults::GetLightIndicesCount()
      Int32 UnityEngine.Experimental.Rendering.CullResults::GetLightIndicesCountIntPtr)
      public Void UnityEngine.Experimental.Rendering.CullResults::FillLightIndices(UnityEngine.ComputeBuffer)
      Void UnityEngine.Experimental.Rendering.CullResults::FillLightIndicesIntPtr,UnityEngine.ComputeBuffer)
      public Boolean UnityEngine.Experimental.Rendering.CullResults::ComputeSpotShadowMatricesAndCullingPrimitivesInt32,UnityEngine.Matrix4x4&,UnityEngine.Matrix4x4&,UnityEngine.Experimental.Rendering.ShadowSplitData&)
      Boolean UnityEngine.Experimental.Rendering.CullResults::ComputeSpotShadowMatricesAndCullingPrimitivesIntPtrInt32,UnityEngine.Matrix4x4&,UnityEngine.Matrix4x4&,UnityEngine.Experimental.Rendering.ShadowSplitData&)
      public Boolean UnityEngine.Experimental.Rendering.CullResults::ComputePointShadowMatricesAndCullingPrimitivesInt32,UnityEngine.CubemapFaceSingle,UnityEngine.Matrix4x4&,UnityEngine.Matrix4x4&,UnityEngine.Experimental.Rendering.ShadowSplitData&)
      Boolean UnityEngine.Experimental.Rendering.CullResults::ComputePointShadowMatricesAndCullingPrimitivesIntPtrInt32,UnityEngine.CubemapFaceSingle,UnityEngine.Matrix4x4&,UnityEngine.Matrix4x4&,UnityEngine.Experimental.Rendering.ShadowSplitData&)
      public Boolean UnityEngine.Experimental.Rendering.CullResults::ComputeDirectionalShadowMatricesAndCullingPrimitivesInt32Int32Int32,UnityEngine.Vector3Int32Single,UnityEngine.Matrix4x4&,UnityEngine.Matrix4x4&,UnityEngine.Experimental.Rendering.ShadowSplitData&)
      Boolean UnityEngine.Experimental.Rendering.CullResults::ComputeDirectionalShadowMatricesAndCullingPrimitivesIntPtrInt32Int32Int32,UnityEngine.Vector3Int32Single,UnityEngine.Matrix4x4&,UnityEngine.Matrix4x4&,UnityEngine.Experimental.Rendering.ShadowSplitData&)
      Boolean UnityEngine.Experimental.Rendering.CullResults::INTERNAL_CALL_ComputeDirectionalShadowMatricesAndCullingPrimitivesIntPtrInt32Int32Int32,UnityEngine.Vector3&Int32Single,UnityEngine.Matrix4x4&,UnityEngine.Matrix4x4&,UnityEngine.Experimental.Rendering.ShadowSplitData&)
    }
}
