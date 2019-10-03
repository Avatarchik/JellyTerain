// Class info from UnityEngine.dll
// 
using UnityEngine;

namespace UnityEngine
{
    public class RaycastHit : ValueType
    {
      // Fields:
  m_Point : Vector3
  m_Normal : Vector3
  m_FaceID : Int32
  m_Distance : Single
  m_UV : Vector2
  m_Collider : Collider
      // Properties:
  point : Vector3
  normal : Vector3
  barycentricCoordinate : Vector3
  distance : Single
  triangleIndex : Int32
  textureCoord : Vector2
  textureCoord2 : Vector2
  textureCoord1 : Vector2
  lightmapCoord : Vector2
  collider : Collider
  rigidbody : Rigidbody
  transform : Transform
      // Events:
      // Methods:
      Void UnityEngine.RaycastHit::CalculateRaycastTexCoord(UnityEngine.Vector2&,UnityEngine.Collider,UnityEngine.Vector2,UnityEngine.Vector3Int32Int32)
      Void UnityEngine.RaycastHit::INTERNAL_CALL_CalculateRaycastTexCoord(UnityEngine.Vector2&,UnityEngine.Collider,UnityEngine.Vector2&,UnityEngine.Vector3&Int32Int32)
      public UnityEngine.Vector3 UnityEngine.RaycastHit::get_point()
      public Void UnityEngine.RaycastHit::set_point(UnityEngine.Vector3)
      public UnityEngine.Vector3 UnityEngine.RaycastHit::get_normal()
      public Void UnityEngine.RaycastHit::set_normal(UnityEngine.Vector3)
      public UnityEngine.Vector3 UnityEngine.RaycastHit::get_barycentricCoordinate()
      public Void UnityEngine.RaycastHit::set_barycentricCoordinate(UnityEngine.Vector3)
      public Single UnityEngine.RaycastHit::get_distance()
      public Void UnityEngine.RaycastHit::set_distanceSingle)
      public Int32 UnityEngine.RaycastHit::get_triangleIndex()
      public UnityEngine.Vector2 UnityEngine.RaycastHit::get_textureCoord()
      public UnityEngine.Vector2 UnityEngine.RaycastHit::get_textureCoord2()
      public UnityEngine.Vector2 UnityEngine.RaycastHit::get_textureCoord1()
      public UnityEngine.Vector2 UnityEngine.RaycastHit::get_lightmapCoord()
      public UnityEngine.Collider UnityEngine.RaycastHit::get_collider()
      public UnityEngine.Rigidbody UnityEngine.RaycastHit::get_rigidbody()
      public UnityEngine.Transform UnityEngine.RaycastHit::get_transform()
    }
}
