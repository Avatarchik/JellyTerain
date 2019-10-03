// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Runtime.Remoting.Messaging
{
    public class AsyncResult : Object
    {
      // Fields:
  async_state : Object
  handle : WaitHandle
  async_delegate : Object
  data : IntPtr
  object_data : Object
  sync_completed : Boolean
  completed : Boolean
  endinvoke_called : Boolean
  async_callback : Object
  current : ExecutionContext
  original : ExecutionContext
  gchandle : Int32
  call_message : MonoMethodMessage
  message_ctrl : IMessageCtrl
  reply_message : IMessage
      // Properties:
  AsyncState : Object
  AsyncWaitHandle : WaitHandle
  CompletedSynchronously : Boolean
  IsCompleted : Boolean
  EndInvokeCalled : Boolean
  AsyncDelegate : Object
  NextSink : IMessageSink
  CallMessage : MonoMethodMessage
      // Events:
      // Methods:
      VoidRuntime.Remoting.Messaging.AsyncResult::.ctor()
      public ObjectRuntime.Remoting.Messaging.AsyncResult::get_AsyncState()
      public Threading.WaitHandleRuntime.Remoting.Messaging.AsyncResult::get_AsyncWaitHandle()
      public BooleanRuntime.Remoting.Messaging.AsyncResult::get_CompletedSynchronously()
      public BooleanRuntime.Remoting.Messaging.AsyncResult::get_IsCompleted()
      public BooleanRuntime.Remoting.Messaging.AsyncResult::get_EndInvokeCalled()
      public VoidRuntime.Remoting.Messaging.AsyncResult::set_EndInvokeCalledBoolean)
      public ObjectRuntime.Remoting.Messaging.AsyncResult::get_AsyncDelegate()
      public Runtime.Remoting.Messaging.IMessageSinkRuntime.Remoting.Messaging.AsyncResult::get_NextSink()
      public Runtime.Remoting.Messaging.IMessageCtrlRuntime.Remoting.Messaging.AsyncResult::AsyncProcessMessageRuntime.Remoting.Messaging.IMessageRuntime.Remoting.Messaging.IMessageSink)
      public Runtime.Remoting.Messaging.IMessageRuntime.Remoting.Messaging.AsyncResult::GetReplyMessage()
      public VoidRuntime.Remoting.Messaging.AsyncResult::SetMessageCtrlRuntime.Remoting.Messaging.IMessageCtrl)
      VoidRuntime.Remoting.Messaging.AsyncResult::SetCompletedSynchronouslyBoolean)
      Runtime.Remoting.Messaging.IMessageRuntime.Remoting.Messaging.AsyncResult::EndInvoke()
      public Runtime.Remoting.Messaging.IMessageRuntime.Remoting.Messaging.AsyncResult::SyncProcessMessageRuntime.Remoting.Messaging.IMessage)
      Runtime.Remoting.Messaging.MonoMethodMessageRuntime.Remoting.Messaging.AsyncResult::get_CallMessage()
      VoidRuntime.Remoting.Messaging.AsyncResult::set_CallMessageRuntime.Remoting.Messaging.MonoMethodMessage)
    }
}
