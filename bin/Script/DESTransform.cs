// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Security.Cryptography
{
    class DESTransform : SymmetricTransform
    {
      // Fields:
  KEY_BIT_SIZE : Int32
  KEY_BYTE_SIZE : Int32
  BLOCK_BIT_SIZE : Int32
  BLOCK_BYTE_SIZE : Int32
  keySchedule : Byte[]
  byteBuff : Byte[]
  dwordBuff : UInt32[]
  spBoxes : UInt32[]
  PC1 : Byte[]
  leftRotTotal : Byte[]
  PC2 : Byte[]
  ipTab : UInt32[]
  fpTab : UInt32[]
      // Properties:
      // Events:
      // Methods:
      VoidSecurity.Cryptography.DESTransform::.ctorSecurity.Cryptography.SymmetricAlgorithmBooleanByte[]Byte[])
      VoidSecurity.Cryptography.DESTransform::.cctor()
      UInt32Security.Cryptography.DESTransform::CipherFunctUInt32Int32)
      VoidSecurity.Cryptography.DESTransform::PermutationByte[]Byte[]UInt32[]Boolean)
      VoidSecurity.Cryptography.DESTransform::BSwapByte[])
      VoidSecurity.Cryptography.DESTransform::SetKeyByte[])
      public VoidSecurity.Cryptography.DESTransform::ProcessBlockByte[]Byte[])
      VoidSecurity.Cryptography.DESTransform::ECBByte[]Byte[])
      Byte[]Security.Cryptography.DESTransform::GetStrongKey()
    }
}
