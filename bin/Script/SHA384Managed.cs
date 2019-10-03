// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Security.Cryptography
{
    public class SHA384Managed : SHA384
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
      public VoidSecurity.Cryptography.SHA384Managed::.ctor()
      VoidSecurity.Cryptography.SHA384Managed::InitializeBoolean)
      public VoidSecurity.Cryptography.SHA384Managed::Initialize()
      VoidSecurity.Cryptography.SHA384Managed::HashCoreByte[]Int32Int32)
      Byte[]Security.Cryptography.SHA384Managed::HashFinal()
      VoidSecurity.Cryptography.SHA384Managed::updateByte)
      VoidSecurity.Cryptography.SHA384Managed::processWordByte[]Int32)
      VoidSecurity.Cryptography.SHA384Managed::unpackWordUInt64Byte[]Int32)
      VoidSecurity.Cryptography.SHA384Managed::adjustByteCounts()
      VoidSecurity.Cryptography.SHA384Managed::processLengthUInt64UInt64)
      VoidSecurity.Cryptography.SHA384Managed::processBlock()
    }
}
