// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Security.Cryptography
{
    public class CryptoStream : Stream
    {
      // Fields:
  _stream : Stream
  _transform : ICryptoTransform
  _mode : CryptoStreamMode
  _currentBlock : Byte[]
  _disposed : Boolean
  _flushedFinalBlock : Boolean
  _partialCount : Int32
  _endOfStream : Boolean
  _waitingBlock : Byte[]
  _waitingCount : Int32
  _transformedBlock : Byte[]
  _transformedPos : Int32
  _transformedCount : Int32
  _workingBlock : Byte[]
  _workingCount : Int32
      // Properties:
  CanRead : Boolean
  CanSeek : Boolean
  CanWrite : Boolean
  Length : Int64
  Position : Int64
      // Events:
      // Methods:
      public VoidSecurity.Cryptography.CryptoStream::.ctorIO.StreamSecurity.Cryptography.ICryptoTransformSecurity.Cryptography.CryptoStreamMode)
      VoidSecurity.Cryptography.CryptoStream::Finalize()
      public BooleanSecurity.Cryptography.CryptoStream::get_CanRead()
      public BooleanSecurity.Cryptography.CryptoStream::get_CanSeek()
      public BooleanSecurity.Cryptography.CryptoStream::get_CanWrite()
      public Int64Security.Cryptography.CryptoStream::get_Length()
      public Int64Security.Cryptography.CryptoStream::get_Position()
      public VoidSecurity.Cryptography.CryptoStream::set_PositionInt64)
      public VoidSecurity.Cryptography.CryptoStream::Clear()
      public VoidSecurity.Cryptography.CryptoStream::Close()
      public Int32Security.Cryptography.CryptoStream::ReadByte[]Int32Int32)
      public VoidSecurity.Cryptography.CryptoStream::WriteByte[]Int32Int32)
      public VoidSecurity.Cryptography.CryptoStream::Flush()
      public VoidSecurity.Cryptography.CryptoStream::FlushFinalBlock()
      public Int64Security.Cryptography.CryptoStream::SeekInt64IO.SeekOrigin)
      public VoidSecurity.Cryptography.CryptoStream::SetLengthInt64)
      VoidSecurity.Cryptography.CryptoStream::DisposeBoolean)
    }
}
