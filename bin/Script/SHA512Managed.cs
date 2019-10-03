// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Security.Cryptography
{
    public class SHA512Managed : SHA512
    {
      // Fields:
  xBuf : Byte[]
  xBufOff : Int32
  byteCount1 : UInt64
  byteCount2 : UInt64
  H1 : UInt64
  H2 : UInt64
  H3 : UInt64
  H4 : UInt64
  H5 : UInt64
  H6 : UInt64
  H7 : UInt64
  H8 : UInt64
  W : UInt64[]
  wOff : Int32
      // Properties:
      // Events:
      // Methods:
      public VoidSecurity.Cryptography.SHA512Managed::.ctor()
      VoidSecurity.Cryptography.SHA512Managed::InitializeBoolean)
      public VoidSecurity.Cryptography.SHA512Managed::Initialize()
      VoidSecurity.Cryptography.SHA512Managed::HashCoreByte[]Int32Int32)
      Byte[]Security.Cryptography.SHA512Managed::HashFinal()
      VoidSecurity.Cryptography.SHA512Managed::updateByte)
      VoidSecurity.Cryptography.SHA512Managed::processWordByte[]Int32)
      VoidSecurity.Cryptography.SHA512Managed::unpackWordUInt64Byte[]Int32)
      VoidSecurity.Cryptography.SHA512Managed::adjustByteCounts()
      VoidSecurity.Cryptography.SHA512Managed::processLengthUInt64UInt64)
      VoidSecurity.Cryptography.SHA512Managed::processBlock()
      UInt64Security.Cryptography.SHA512Managed::rotateRightUInt64Int32)
      UInt64Security.Cryptography.SHA512Managed::ChUInt64UInt64UInt64)
      UInt64Security.Cryptography.SHA512Managed::MajUInt64UInt64UInt64)
      UInt64Security.Cryptography.SHA512Managed::Sum0UInt64)
      UInt64Security.Cryptography.SHA512Managed::Sum1UInt64)
      UInt64Security.Cryptography.SHA512Managed::Sigma0UInt64)
      UInt64Security.Cryptography.SHA512Managed::Sigma1UInt64)
    }
}
