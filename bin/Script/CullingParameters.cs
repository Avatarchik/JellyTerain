// Class info from UnityEngine.dll
// 
using UnityEngine;

namespace UnityEngine.Experimental.Rendering
{
    public class CullingParameters : ValueType
    {
      // Fields:
  public isOrthographic : Int32
  public lodParameters : LODParameters
  _cullingPlanes : <_cullingPlanes>__FixedBuffer0
  public cullingPlaneCount : Int32
  public cullingMask : Int32
  _layerFarCullDistances : <_layerFarCullDistances>__FixedBuffer1
  layerCull : Int32
  public cullingMatrix : Matrix4x4
  public position : Vector3
  public shadowDistance : Single
  _cullingFlags : Int32
  _cameraInstanceID : Int32
  public reflectionProbeSortOptions : ReflectionProbeSortOptions
      // Properties:
      // Events:
      // Methods:
      public Single UnityEngine.Experimental.Rendering.CullingParameters::GetLayerCullDistanceInt32)
      public Void UnityEngine.Experimental.Rendering.CullingParameters::SetLayerCullDistanceInt32Single)
      public UnityEngine.Plane UnityEngine.Experimental.Rendering.CullingParameters::GetCullingPlaneInt32)
      public Void UnityEngine.Experimental.Rendering.CullingParameters::SetCullingPlaneInt32,UnityEngine.Plane)
    }
}
