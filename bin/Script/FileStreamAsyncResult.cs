// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.IO
{
    class FileStreamAsyncResult : Object
    {
      // Fields:
  state : Object
  completed : Boolean
  done : Boolean
  exc : Exception
  wh : ManualResetEvent
  cb : AsyncCallback
  completedSynch : Boolean
  public Buffer : Byte[]
  public Offset : Int32
  public Count : Int32
  public OriginalCount : Int32
  public BytesRead : Int32
  realcb : AsyncCallback
      // Properties:
  AsyncState : Object
  CompletedSynchronously : Boolean
  AsyncWaitHandle : WaitHandle
  IsCompleted : Boolean
  Exception : Exception
  Done : Boolean
      // Events:
      // Methods:
      public VoidIO.FileStreamAsyncResult::.ctorAsyncCallbackObject)
      VoidIO.FileStreamAsyncResult::CBWrapperIAsyncResult)
      public VoidIO.FileStreamAsyncResult::SetCompleteException)
      public VoidIO.FileStreamAsyncResult::SetCompleteExceptionInt32)
      public VoidIO.FileStreamAsyncResult::SetCompleteExceptionInt32Boolean)
      public ObjectIO.FileStreamAsyncResult::get_AsyncState()
      public BooleanIO.FileStreamAsyncResult::get_CompletedSynchronously()
      public Threading.WaitHandleIO.FileStreamAsyncResult::get_AsyncWaitHandle()
      public BooleanIO.FileStreamAsyncResult::get_IsCompleted()
      public ExceptionIO.FileStreamAsyncResult::get_Exception()
      public BooleanIO.FileStreamAsyncResult::get_Done()
      public VoidIO.FileStreamAsyncResult::set_DoneBoolean)
    }
}
