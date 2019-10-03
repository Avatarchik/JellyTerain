// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Runtime.Remoting.Contexts
{
    public class SynchronizationAttribute : ContextAttribute
    {
      // Fields:
  public NOT_SUPPORTED : Int32
  public SUPPORTED : Int32
  public REQUIRED : Int32
  public REQUIRES_NEW : Int32
  _bReEntrant : Boolean
  _flavor : Int32
  _locked : Boolean
  _lockCount : Int32
  _mutex : Mutex
  _ownerThread : Thread
      // Properties:
  IsReEntrant : Boolean
  Locked : Boolean
      // Events:
      // Methods:
      public VoidRuntime.Remoting.Contexts.SynchronizationAttribute::.ctor()
      public VoidRuntime.Remoting.Contexts.SynchronizationAttribute::.ctorBoolean)
      public VoidRuntime.Remoting.Contexts.SynchronizationAttribute::.ctorInt32)
      public VoidRuntime.Remoting.Contexts.SynchronizationAttribute::.ctorInt32Boolean)
      public BooleanRuntime.Remoting.Contexts.SynchronizationAttribute::get_IsReEntrant()
      public BooleanRuntime.Remoting.Contexts.SynchronizationAttribute::get_Locked()
      public VoidRuntime.Remoting.Contexts.SynchronizationAttribute::set_LockedBoolean)
      VoidRuntime.Remoting.Contexts.SynchronizationAttribute::AcquireLock()
      VoidRuntime.Remoting.Contexts.SynchronizationAttribute::ReleaseLock()
      public VoidRuntime.Remoting.Contexts.SynchronizationAttribute::GetPropertiesForNewContextRuntime.Remoting.Activation.IConstructionCallMessage)
      public Runtime.Remoting.Messaging.IMessageSinkRuntime.Remoting.Contexts.SynchronizationAttribute::GetClientContextSinkRuntime.Remoting.Messaging.IMessageSink)
      public Runtime.Remoting.Messaging.IMessageSinkRuntime.Remoting.Contexts.SynchronizationAttribute::GetServerContextSinkRuntime.Remoting.Messaging.IMessageSink)
      public BooleanRuntime.Remoting.Contexts.SynchronizationAttribute::IsContextOKRuntime.Remoting.Contexts.ContextRuntime.Remoting.Activation.IConstructionCallMessage)
      VoidRuntime.Remoting.Contexts.SynchronizationAttribute::ExitContext()
      VoidRuntime.Remoting.Contexts.SynchronizationAttribute::EnterContext()
    }
}
