// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Runtime.Remoting
{
    class Identity : Object
    {
      // Fields:
  _objectUri : String
  _channelSink : IMessageSink
  _envoySink : IMessageSink
  _clientDynamicProperties : DynamicPropertyCollection
  _serverDynamicProperties : DynamicPropertyCollection
  _objRef : ObjRef
  _disposed : Boolean
      // Properties:
  IsFromThisAppDomain : Boolean
  ChannelSink : IMessageSink
  EnvoySink : IMessageSink
  ObjectUri : String
  IsConnected : Boolean
  Disposed : Boolean
  ClientDynamicProperties : DynamicPropertyCollection
  ServerDynamicProperties : DynamicPropertyCollection
  HasClientDynamicSinks : Boolean
  HasServerDynamicSinks : Boolean
      // Events:
      // Methods:
      public VoidRuntime.Remoting.Identity::.ctorString)
      public Runtime.Remoting.ObjRefRuntime.Remoting.Identity::CreateObjRefType)
      public BooleanRuntime.Remoting.Identity::get_IsFromThisAppDomain()
      public Runtime.Remoting.Messaging.IMessageSinkRuntime.Remoting.Identity::get_ChannelSink()
      public VoidRuntime.Remoting.Identity::set_ChannelSinkRuntime.Remoting.Messaging.IMessageSink)
      public Runtime.Remoting.Messaging.IMessageSinkRuntime.Remoting.Identity::get_EnvoySink()
      public StringRuntime.Remoting.Identity::get_ObjectUri()
      public VoidRuntime.Remoting.Identity::set_ObjectUriString)
      public BooleanRuntime.Remoting.Identity::get_IsConnected()
      public BooleanRuntime.Remoting.Identity::get_Disposed()
      public VoidRuntime.Remoting.Identity::set_DisposedBoolean)
      public Runtime.Remoting.Contexts.DynamicPropertyCollectionRuntime.Remoting.Identity::get_ClientDynamicProperties()
      public Runtime.Remoting.Contexts.DynamicPropertyCollectionRuntime.Remoting.Identity::get_ServerDynamicProperties()
      public BooleanRuntime.Remoting.Identity::get_HasClientDynamicSinks()
      public BooleanRuntime.Remoting.Identity::get_HasServerDynamicSinks()
      public VoidRuntime.Remoting.Identity::NotifyClientDynamicSinksBooleanRuntime.Remoting.Messaging.IMessageBooleanBoolean)
      public VoidRuntime.Remoting.Identity::NotifyServerDynamicSinksBooleanRuntime.Remoting.Messaging.IMessageBooleanBoolean)
    }
}
