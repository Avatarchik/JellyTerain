// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.IO
{
    public class UnmanagedMemoryStream : Stream
    {
      // Fields:
  length : Int64
  closed : Boolean
  capacity : Int64
  fileaccess : FileAccess
  initial_pointer : IntPtr
  initial_position : Int64
  current_position : Int64
  Closed : EventHandler
      // Properties:
  CanRead : Boolean
  CanSeek : Boolean
  CanWrite : Boolean
  Capacity : Int64
  Length : Int64
  Position : Int64
  PositionPointer : Byte*
      // Events:
      Closed : EventHandler
      // Methods:
      VoidIO.UnmanagedMemoryStream::.ctor()
      public VoidIO.UnmanagedMemoryStream::.ctorByte*Int64)
      public VoidIO.UnmanagedMemoryStream::.ctorByte*Int64Int64IO.FileAccess)
      VoidIO.UnmanagedMemoryStream::add_ClosedEventHandler)
      VoidIO.UnmanagedMemoryStream::remove_ClosedEventHandler)
      public BooleanIO.UnmanagedMemoryStream::get_CanRead()
      public BooleanIO.UnmanagedMemoryStream::get_CanSeek()
      public BooleanIO.UnmanagedMemoryStream::get_CanWrite()
      public Int64IO.UnmanagedMemoryStream::get_Capacity()
      public Int64IO.UnmanagedMemoryStream::get_Length()
      public Int64IO.UnmanagedMemoryStream::get_Position()
      public VoidIO.UnmanagedMemoryStream::set_PositionInt64)
      public Byte*IO.UnmanagedMemoryStream::get_PositionPointer()
      public VoidIO.UnmanagedMemoryStream::set_PositionPointerByte*)
      public Int32IO.UnmanagedMemoryStream::ReadByte[]Int32Int32)
      public Int32IO.UnmanagedMemoryStream::ReadByte()
      public Int64IO.UnmanagedMemoryStream::SeekInt64IO.SeekOrigin)
      public VoidIO.UnmanagedMemoryStream::SetLengthInt64)
      public VoidIO.UnmanagedMemoryStream::Flush()
      VoidIO.UnmanagedMemoryStream::DisposeBoolean)
      public VoidIO.UnmanagedMemoryStream::WriteByte[]Int32Int32)
      public VoidIO.UnmanagedMemoryStream::WriteByteByte)
      VoidIO.UnmanagedMemoryStream::InitializeByte*Int64Int64IO.FileAccess)
    }
}
