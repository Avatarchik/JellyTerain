// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.IO
{
    public class BufferedStream : Stream
    {
      // Fields:
  m_stream : Stream
  m_buffer : Byte[]
  m_buffer_pos : Int32
  m_buffer_read_ahead : Int32
  m_buffer_reading : Boolean
  disposed : Boolean
      // Properties:
  CanRead : Boolean
  CanWrite : Boolean
  CanSeek : Boolean
  Length : Int64
  Position : Int64
      // Events:
      // Methods:
      public VoidIO.BufferedStream::.ctorIO.Stream)
      public VoidIO.BufferedStream::.ctorIO.StreamInt32)
      public BooleanIO.BufferedStream::get_CanRead()
      public BooleanIO.BufferedStream::get_CanWrite()
      public BooleanIO.BufferedStream::get_CanSeek()
      public Int64IO.BufferedStream::get_Length()
      public Int64IO.BufferedStream::get_Position()
      public VoidIO.BufferedStream::set_PositionInt64)
      VoidIO.BufferedStream::DisposeBoolean)
      public VoidIO.BufferedStream::Flush()
      public Int64IO.BufferedStream::SeekInt64IO.SeekOrigin)
      public VoidIO.BufferedStream::SetLengthInt64)
      public Int32IO.BufferedStream::ReadByte()
      public VoidIO.BufferedStream::WriteByteByte)
      public Int32IO.BufferedStream::ReadByte[]Int32Int32)
      public VoidIO.BufferedStream::WriteByte[]Int32Int32)
      VoidIO.BufferedStream::CheckObjectDisposedException()
    }
}
