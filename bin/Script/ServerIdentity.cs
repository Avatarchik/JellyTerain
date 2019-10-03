// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Runtime.Remoting
{
    class ServerIdentity : Identity
    {
      // Fields:
  _objectType : Type
  _serverObject : MarshalByRefObject
  _serverSink : IMessageSink
  _context : Context
  _lease : Lease
      // Properties:
  ObjectType : Type
  Lease : Lease
  Context : Context
      // Events:
      // Methods:
      public VoidRuntime.Remoting.ServerIdentity::.ctorStringRuntime.Remoting.Contexts.ContextType)
      public TypeRuntime.Remoting.ServerIdentity::get_ObjectType()
      public VoidRuntime.Remoting.ServerIdentity::StartTrackingLifetimeRuntime.Remoting.Lifetime.ILease)
      public VoidRuntime.Remoting.ServerIdentity::OnLifetimeExpired()
      public Runtime.Remoting.ObjRefRuntime.Remoting.ServerIdentity::CreateObjRefType)
      public VoidRuntime.Remoting.ServerIdentity::AttachServerObjectMarshalByRefObjectRuntime.Remoting.Contexts.Context)
      public Runtime.Remoting.Lifetime.LeaseRuntime.Remoting.ServerIdentity::get_Lease()
      public Runtime.Remoting.Contexts.ContextRuntime.Remoting.ServerIdentity::get_Context()
      public VoidRuntime.Remoting.ServerIdentity::set_ContextRuntime.Remoting.Contexts.Context)
      public Runtime.Remoting.Messaging.IMessageRuntime.Remoting.ServerIdentity::SyncObjectProcessMessageRuntime.Remoting.Messaging.IMessage)
      public Runtime.Remoting.Messaging.IMessageCtrlRuntime.Remoting.ServerIdentity::AsyncObjectProcessMessageRuntime.Remoting.Messaging.IMessageRuntime.Remoting.Messaging.IMessageSink)
      VoidRuntime.Remoting.ServerIdentity::DisposeServerObject()
    }
}
