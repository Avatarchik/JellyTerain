// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.IO
{
    public class MemoryStream : Stream
    {
      // Fields:
  canWrite : Boolean
  allowGetBuffer : Boolean
  capacity : Int32
  length : Int32
  internalBuffer : Byte[]
  initialIndex : Int32
  expandable : Boolean
  streamClosed : Boolean
  position : Int32
  dirty_bytes : Int32
      // Properties:
  CanRead : Boolean
  CanSeek : Boolean
  CanWrite : Boolean
  Capacity : Int32
  Length : Int64
  Position : Int64
      // Events:
      // Methods:
      public VoidIO.MemoryStream::.ctor()
      public VoidIO.MemoryStream::.ctorInt32)
      public VoidIO.MemoryStream::.ctorByte[])
      public VoidIO.MemoryStream::.ctorByte[]Boolean)
      public VoidIO.MemoryStream::.ctorByte[]Int32Int32)
      public VoidIO.MemoryStream::.ctorByte[]Int32Int32Boolean)
      public VoidIO.MemoryStream::.ctorByte[]Int32Int32BooleanBoolean)
      VoidIO.MemoryStream::InternalConstructorByte[]Int32Int32BooleanBoolean)
      VoidIO.MemoryStream::CheckIfClosedThrowDisposed()
      public BooleanIO.MemoryStream::get_CanRead()
      public BooleanIO.MemoryStream::get_CanSeek()
      public BooleanIO.MemoryStream::get_CanWrite()
      public Int32IO.MemoryStream::get_Capacity()
      public VoidIO.MemoryStream::set_CapacityInt32)
      public Int64IO.MemoryStream::get_Length()
      public Int64IO.MemoryStream::get_Position()
      public VoidIO.MemoryStream::set_PositionInt64)
      VoidIO.MemoryStream::DisposeBoolean)
      public VoidIO.MemoryStream::Flush()
      public Byte[]IO.MemoryStream::GetBuffer()
      public Int32IO.MemoryStream::ReadByte[]Int32Int32)
      public Int32IO.MemoryStream::ReadByte()
      public Int64IO.MemoryStream::SeekInt64IO.SeekOrigin)
      Int32IO.MemoryStream::CalculateNewCapacityInt32)
      VoidIO.MemoryStream::ExpandInt32)
      public VoidIO.MemoryStream::SetLengthInt64)
      public Byte[]IO.MemoryStream::ToArray()
      public VoidIO.MemoryStream::WriteByte[]Int32Int32)
      public VoidIO.MemoryStream::WriteByteByte)
      public VoidIO.MemoryStream::WriteToIO.Stream)
    }
}
