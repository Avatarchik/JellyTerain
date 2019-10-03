// Class info from UnityEngine.dll
// 
using UnityEngine;

namespace UnityEngine
{
    public class ContactFilter2D : ValueType
    {
      // Fields:
  public useTriggers : Boolean
  public useLayerMask : Boolean
  public useDepth : Boolean
  public useOutsideDepth : Boolean
  public useNormalAngle : Boolean
  public useOutsideNormalAngle : Boolean
  public layerMask : LayerMask
  public minDepth : Single
  public maxDepth : Single
  public minNormalAngle : Single
  public maxNormalAngle : Single
  public NormalAngleUpperLimit : Single
      // Properties:
  isFiltering : Boolean
      // Events:
      // Methods:
      public UnityEngine.ContactFilter2D UnityEngine.ContactFilter2D::NoFilter()
      Void UnityEngine.ContactFilter2D::CheckConsistency()
      public Void UnityEngine.ContactFilter2D::ClearLayerMask()
      public Void UnityEngine.ContactFilter2D::SetLayerMask(UnityEngine.LayerMask)
      public Void UnityEngine.ContactFilter2D::ClearDepth()
      public Void UnityEngine.ContactFilter2D::SetDepthSingleSingle)
      public Void UnityEngine.ContactFilter2D::ClearNormalAngle()
      public Void UnityEngine.ContactFilter2D::SetNormalAngleSingleSingle)
      public Boolean UnityEngine.ContactFilter2D::get_isFiltering()
      public Boolean UnityEngine.ContactFilter2D::IsFilteringTrigger(UnityEngine.Collider2D)
      public Boolean UnityEngine.ContactFilter2D::IsFilteringLayerMask(UnityEngine.GameObject)
      public Boolean UnityEngine.ContactFilter2D::IsFilteringDepth(UnityEngine.GameObject)
      public Boolean UnityEngine.ContactFilter2D::IsFilteringNormalAngle(UnityEngine.Vector2)
      public Boolean UnityEngine.ContactFilter2D::IsFilteringNormalAngleSingle)
      UnityEngine.ContactFilter2D UnityEngine.ContactFilter2D::CreateLegacyFilterInt32SingleSingle)
    }
}
