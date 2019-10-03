// Class info from UnityEngine.dll
// 
using UnityEngine;

namespace UnityEngine
{
    public class Bounds : ValueType
    {
      // Fields:
  m_Center : Vector3
  m_Extents : Vector3
      // Properties:
  center : Vector3
  size : Vector3
  extents : Vector3
  min : Vector3
  max : Vector3
      // Events:
      // Methods:
      public Void UnityEngine.Bounds::.ctor(UnityEngine.Vector3,UnityEngine.Vector3)
      Boolean UnityEngine.Bounds::Internal_Contains(UnityEngine.Bounds,UnityEngine.Vector3)
      Boolean UnityEngine.Bounds::INTERNAL_CALL_Internal_Contains(UnityEngine.Bounds&,UnityEngine.Vector3&)
      public Boolean UnityEngine.Bounds::Contains(UnityEngine.Vector3)
      Single UnityEngine.Bounds::Internal_SqrDistance(UnityEngine.Bounds,UnityEngine.Vector3)
      Single UnityEngine.Bounds::INTERNAL_CALL_Internal_SqrDistance(UnityEngine.Bounds&,UnityEngine.Vector3&)
      public Single UnityEngine.Bounds::SqrDistance(UnityEngine.Vector3)
      Boolean UnityEngine.Bounds::Internal_IntersectRay(UnityEngine.Ray&,UnityEngine.Bounds&Single&)
      Boolean UnityEngine.Bounds::INTERNAL_CALL_Internal_IntersectRay(UnityEngine.Ray&,UnityEngine.Bounds&Single&)
      public Boolean UnityEngine.Bounds::IntersectRay(UnityEngine.Ray)
      public Boolean UnityEngine.Bounds::IntersectRay(UnityEngine.RaySingle&)
      UnityEngine.Vector3 UnityEngine.Bounds::Internal_GetClosestPoint(UnityEngine.Bounds&,UnityEngine.Vector3&)
      Void UnityEngine.Bounds::INTERNAL_CALL_Internal_GetClosestPoint(UnityEngine.Bounds&,UnityEngine.Vector3&,UnityEngine.Vector3&)
      public UnityEngine.Vector3 UnityEngine.Bounds::ClosestPoint(UnityEngine.Vector3)
      public Int32 UnityEngine.Bounds::GetHashCode()
      public Boolean UnityEngine.Bounds::EqualsObject)
      public UnityEngine.Vector3 UnityEngine.Bounds::get_center()
      public Void UnityEngine.Bounds::set_center(UnityEngine.Vector3)
      public UnityEngine.Vector3 UnityEngine.Bounds::get_size()
      public Void UnityEngine.Bounds::set_size(UnityEngine.Vector3)
      public UnityEngine.Vector3 UnityEngine.Bounds::get_extents()
      public Void UnityEngine.Bounds::set_extents(UnityEngine.Vector3)
      public UnityEngine.Vector3 UnityEngine.Bounds::get_min()
      public Void UnityEngine.Bounds::set_min(UnityEngine.Vector3)
      public UnityEngine.Vector3 UnityEngine.Bounds::get_max()
      public Void UnityEngine.Bounds::set_max(UnityEngine.Vector3)
      public Boolean UnityEngine.Bounds::op_Equality(UnityEngine.Bounds,UnityEngine.Bounds)
      public Boolean UnityEngine.Bounds::op_Inequality(UnityEngine.Bounds,UnityEngine.Bounds)
      public Void UnityEngine.Bounds::SetMinMax(UnityEngine.Vector3,UnityEngine.Vector3)
      public Void UnityEngine.Bounds::Encapsulate(UnityEngine.Vector3)
      public Void UnityEngine.Bounds::Encapsulate(UnityEngine.Bounds)
      public Void UnityEngine.Bounds::ExpandSingle)
      public Void UnityEngine.Bounds::Expand(UnityEngine.Vector3)
      public Boolean UnityEngine.Bounds::Intersects(UnityEngine.Bounds)
      public String UnityEngine.Bounds::ToString()
      public String UnityEngine.Bounds::ToStringString)
    }
}
