// Class info from UnityEngine.dll
// 
using UnityEngine;

namespace UnityEngine
{
    public class Matrix4x4 : ValueType
    {
      // Fields:
  public m00 : Single
  public m10 : Single
  public m20 : Single
  public m30 : Single
  public m01 : Single
  public m11 : Single
  public m21 : Single
  public m31 : Single
  public m02 : Single
  public m12 : Single
  public m22 : Single
  public m32 : Single
  public m03 : Single
  public m13 : Single
  public m23 : Single
  public m33 : Single
      // Properties:
  inverse : Matrix4x4
  transpose : Matrix4x4
  isIdentity : Boolean
  determinant : Single
  Item : Single
  Item : Single
  zero : Matrix4x4
  identity : Matrix4x4
      // Events:
      // Methods:
      public UnityEngine.Matrix4x4 UnityEngine.Matrix4x4::Inverse(UnityEngine.Matrix4x4)
      Void UnityEngine.Matrix4x4::INTERNAL_CALL_Inverse(UnityEngine.Matrix4x4&,UnityEngine.Matrix4x4&)
      public UnityEngine.Matrix4x4 UnityEngine.Matrix4x4::Transpose(UnityEngine.Matrix4x4)
      Void UnityEngine.Matrix4x4::INTERNAL_CALL_Transpose(UnityEngine.Matrix4x4&,UnityEngine.Matrix4x4&)
      Boolean UnityEngine.Matrix4x4::Invert(UnityEngine.Matrix4x4,UnityEngine.Matrix4x4&)
      Boolean UnityEngine.Matrix4x4::INTERNAL_CALL_Invert(UnityEngine.Matrix4x4&,UnityEngine.Matrix4x4&)
      public UnityEngine.Matrix4x4 UnityEngine.Matrix4x4::get_inverse()
      public UnityEngine.Matrix4x4 UnityEngine.Matrix4x4::get_transpose()
      public Boolean UnityEngine.Matrix4x4::get_isIdentity()
      public Single UnityEngine.Matrix4x4::Determinant(UnityEngine.Matrix4x4)
      Single UnityEngine.Matrix4x4::INTERNAL_CALL_Determinant(UnityEngine.Matrix4x4&)
      public Single UnityEngine.Matrix4x4::get_determinant()
      public Void UnityEngine.Matrix4x4::SetTRS(UnityEngine.Vector3,UnityEngine.Quaternion,UnityEngine.Vector3)
      public UnityEngine.Matrix4x4 UnityEngine.Matrix4x4::TRS(UnityEngine.Vector3,UnityEngine.Quaternion,UnityEngine.Vector3)
      Void UnityEngine.Matrix4x4::INTERNAL_CALL_TRS(UnityEngine.Vector3&,UnityEngine.Quaternion&,UnityEngine.Vector3&,UnityEngine.Matrix4x4&)
      public UnityEngine.Matrix4x4 UnityEngine.Matrix4x4::OrthoSingleSingleSingleSingleSingleSingle)
      Void UnityEngine.Matrix4x4::INTERNAL_CALL_OrthoSingleSingleSingleSingleSingleSingle,UnityEngine.Matrix4x4&)
      public UnityEngine.Matrix4x4 UnityEngine.Matrix4x4::PerspectiveSingleSingleSingleSingle)
      Void UnityEngine.Matrix4x4::INTERNAL_CALL_PerspectiveSingleSingleSingleSingle,UnityEngine.Matrix4x4&)
      public UnityEngine.Matrix4x4 UnityEngine.Matrix4x4::LookAt(UnityEngine.Vector3,UnityEngine.Vector3,UnityEngine.Vector3)
      Void UnityEngine.Matrix4x4::INTERNAL_CALL_LookAt(UnityEngine.Vector3&,UnityEngine.Vector3&,UnityEngine.Vector3&,UnityEngine.Matrix4x4&)
      public Single UnityEngine.Matrix4x4::get_ItemInt32Int32)
      public Void UnityEngine.Matrix4x4::set_ItemInt32Int32Single)
      public Single UnityEngine.Matrix4x4::get_ItemInt32)
      public Void UnityEngine.Matrix4x4::set_ItemInt32Single)
      public Int32 UnityEngine.Matrix4x4::GetHashCode()
      public Boolean UnityEngine.Matrix4x4::EqualsObject)
      public UnityEngine.Matrix4x4 UnityEngine.Matrix4x4::op_Multiply(UnityEngine.Matrix4x4,UnityEngine.Matrix4x4)
      public UnityEngine.Vector4 UnityEngine.Matrix4x4::op_Multiply(UnityEngine.Matrix4x4,UnityEngine.Vector4)
      public Boolean UnityEngine.Matrix4x4::op_Equality(UnityEngine.Matrix4x4,UnityEngine.Matrix4x4)
      public Boolean UnityEngine.Matrix4x4::op_Inequality(UnityEngine.Matrix4x4,UnityEngine.Matrix4x4)
      public UnityEngine.Vector4 UnityEngine.Matrix4x4::GetColumnInt32)
      public UnityEngine.Vector4 UnityEngine.Matrix4x4::GetRowInt32)
      public Void UnityEngine.Matrix4x4::SetColumnInt32,UnityEngine.Vector4)
      public Void UnityEngine.Matrix4x4::SetRowInt32,UnityEngine.Vector4)
      public UnityEngine.Vector3 UnityEngine.Matrix4x4::MultiplyPoint(UnityEngine.Vector3)
      public UnityEngine.Vector3 UnityEngine.Matrix4x4::MultiplyPoint3x4(UnityEngine.Vector3)
      public UnityEngine.Vector3 UnityEngine.Matrix4x4::MultiplyVector(UnityEngine.Vector3)
      public UnityEngine.Matrix4x4 UnityEngine.Matrix4x4::Scale(UnityEngine.Vector3)
      public UnityEngine.Matrix4x4 UnityEngine.Matrix4x4::Translate(UnityEngine.Vector3)
      public UnityEngine.Matrix4x4 UnityEngine.Matrix4x4::get_zero()
      public UnityEngine.Matrix4x4 UnityEngine.Matrix4x4::get_identity()
      public String UnityEngine.Matrix4x4::ToString()
      public String UnityEngine.Matrix4x4::ToStringString)
    }
}
