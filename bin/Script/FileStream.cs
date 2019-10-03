// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.IO
{
    public class FileStream : Stream
    {
      // Fields:
  DefaultBufferSize : Int32
  access : FileAccess
  owner : Boolean
  async : Boolean
  canseek : Boolean
  append_startpos : Int64
  anonymous : Boolean
  buf : Byte[]
  buf_size : Int32
  buf_length : Int32
  buf_offset : Int32
  buf_dirty : Boolean
  buf_start : Int64
  name : String
  handle : IntPtr
  safeHandle : SafeFileHandle
      // Properties:
  CanRead : Boolean
  CanWrite : Boolean
  CanSeek : Boolean
  IsAsync : Boolean
  Name : String
  Length : Int64
  Position : Int64
  Handle : IntPtr
  SafeFileHandle : SafeFileHandle
      // Events:
      // Methods:
      public VoidIO.FileStream::.ctorIntPtrIO.FileAccess)
      public VoidIO.FileStream::.ctorIntPtrIO.FileAccessBoolean)
      public VoidIO.FileStream::.ctorIntPtrIO.FileAccessBooleanInt32)
      public VoidIO.FileStream::.ctorIntPtrIO.FileAccessBooleanInt32Boolean)
      VoidIO.FileStream::.ctorIntPtrIO.FileAccessBooleanInt32BooleanBoolean)
      public VoidIO.FileStream::.ctorStringIO.FileMode)
      public VoidIO.FileStream::.ctorStringIO.FileModeIO.FileAccess)
      public VoidIO.FileStream::.ctorStringIO.FileModeIO.FileAccessIO.FileShare)
      public VoidIO.FileStream::.ctorStringIO.FileModeIO.FileAccessIO.FileShareInt32)
      public VoidIO.FileStream::.ctorStringIO.FileModeIO.FileAccessIO.FileShareInt32Boolean)
      VoidIO.FileStream::.ctorStringIO.FileModeIO.FileAccessIO.FileShareInt32BooleanBoolean)
      VoidIO.FileStream::.ctorStringIO.FileModeIO.FileAccessIO.FileShareInt32BooleanIO.FileOptions)
      public BooleanIO.FileStream::get_CanRead()
      public BooleanIO.FileStream::get_CanWrite()
      public BooleanIO.FileStream::get_CanSeek()
      public BooleanIO.FileStream::get_IsAsync()
      public StringIO.FileStream::get_Name()
      public Int64IO.FileStream::get_Length()
      public Int64IO.FileStream::get_Position()
      public VoidIO.FileStream::set_PositionInt64)
      public IntPtrIO.FileStream::get_Handle()
      public Microsoft.Win32.SafeHandles.SafeFileHandleIO.FileStream::get_SafeFileHandle()
      public Int32IO.FileStream::ReadByte()
      public VoidIO.FileStream::WriteByteByte)
      public Int32IO.FileStream::ReadByte[]Int32Int32)
      Int32IO.FileStream::ReadInternalByte[]Int32Int32)
      public IAsyncResultIO.FileStream::BeginReadByte[]Int32Int32AsyncCallbackObject)
      public Int32IO.FileStream::EndReadIAsyncResult)
      public VoidIO.FileStream::WriteByte[]Int32Int32)
      VoidIO.FileStream::WriteInternalByte[]Int32Int32)
      public IAsyncResultIO.FileStream::BeginWriteByte[]Int32Int32AsyncCallbackObject)
      public VoidIO.FileStream::EndWriteIAsyncResult)
      public Int64IO.FileStream::SeekInt64IO.SeekOrigin)
      public VoidIO.FileStream::SetLengthInt64)
      public VoidIO.FileStream::Flush()
      public VoidIO.FileStream::LockInt64Int64)
      public VoidIO.FileStream::UnlockInt64Int64)
      VoidIO.FileStream::Finalize()
      VoidIO.FileStream::DisposeBoolean)
      Int32IO.FileStream::ReadSegmentByte[]Int32Int32)
      Int32IO.FileStream::WriteSegmentByte[]Int32Int32)
      VoidIO.FileStream::FlushBufferIO.Stream)
      VoidIO.FileStream::FlushBuffer()
      VoidIO.FileStream::FlushBufferIfDirty()
      VoidIO.FileStream::RefillBuffer()
      Int32IO.FileStream::ReadDataIntPtrByte[]Int32Int32)
      VoidIO.FileStream::InitBufferInt32Boolean)
      StringIO.FileStream::GetSecureFileNameString)
      StringIO.FileStream::GetSecureFileNameStringBoolean)
    }
}
