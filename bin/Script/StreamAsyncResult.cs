// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.IO
{
    class StreamAsyncResult : Object
    {
      // Fields:
  state : Object
  completed : Boolean
  done : Boolean
  exc : Exception
  nbytes : Int32
  wh : ManualResetEvent
      // Properties:
  AsyncState : Object
  AsyncWaitHandle : WaitHandle
  CompletedSynchronously : Boolean
  IsCompleted : Boolean
  Exception : Exception
  NBytes : Int32
  Done : Boolean
      // Events:
      // Methods:
      public VoidIO.StreamAsyncResult::.ctorObject)
      public VoidIO.StreamAsyncResult::SetCompleteException)
      public VoidIO.StreamAsyncResult::SetCompleteExceptionInt32)
      public ObjectIO.StreamAsyncResult::get_AsyncState()
      public Threading.WaitHandleIO.StreamAsyncResult::get_AsyncWaitHandle()
      public BooleanIO.StreamAsyncResult::get_CompletedSynchronously()
      public BooleanIO.StreamAsyncResult::get_IsCompleted()
      public ExceptionIO.StreamAsyncResult::get_Exception()
      public Int32IO.StreamAsyncResult::get_NBytes()
      public BooleanIO.StreamAsyncResult::get_Done()
      public VoidIO.StreamAsyncResult::set_DoneBoolean)
    }
}
