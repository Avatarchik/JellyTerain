// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Runtime.Remoting.Contexts
{
    public class Context : Object
    {
      // Fields:
  domain_id : Int32
  context_id : Int32
  static_data : UIntPtr
  default_server_context_sink : IMessageSink
  server_context_sink_chain : IMessageSink
  client_context_sink_chain : IMessageSink
  datastore : Object[]
  context_properties : ArrayList
  frozen : Boolean
  global_count : Int32
  namedSlots : Hashtable
  global_dynamic_properties : DynamicPropertyCollection
  context_dynamic_properties : DynamicPropertyCollection
  callback_object : ContextCallbackObject
      // Properties:
  DefaultContext : Context
  ContextID : Int32
  ContextProperties : IContextProperty[]
  IsDefaultContext : Boolean
  NeedsContextSink : Boolean
  HasGlobalDynamicSinks : Boolean
  HasDynamicSinks : Boolean
  HasExitSinks : Boolean
      // Events:
      // Methods:
      public VoidRuntime.Remoting.Contexts.Context::.ctor()
      VoidRuntime.Remoting.Contexts.Context::.cctor()
      VoidRuntime.Remoting.Contexts.Context::Finalize()
      public Runtime.Remoting.Contexts.ContextRuntime.Remoting.Contexts.Context::get_DefaultContext()
      public Int32Runtime.Remoting.Contexts.Context::get_ContextID()
      public Runtime.Remoting.Contexts.IContextProperty[]Runtime.Remoting.Contexts.Context::get_ContextProperties()
      BooleanRuntime.Remoting.Contexts.Context::get_IsDefaultContext()
      BooleanRuntime.Remoting.Contexts.Context::get_NeedsContextSink()
      public BooleanRuntime.Remoting.Contexts.Context::RegisterDynamicPropertyRuntime.Remoting.Contexts.IDynamicPropertyContextBoundObjectRuntime.Remoting.Contexts.Context)
      public BooleanRuntime.Remoting.Contexts.Context::UnregisterDynamicPropertyStringContextBoundObjectRuntime.Remoting.Contexts.Context)
      Runtime.Remoting.Contexts.DynamicPropertyCollectionRuntime.Remoting.Contexts.Context::GetDynamicPropertyCollectionContextBoundObjectRuntime.Remoting.Contexts.Context)
      VoidRuntime.Remoting.Contexts.Context::NotifyGlobalDynamicSinksBooleanRuntime.Remoting.Messaging.IMessageBooleanBoolean)
      BooleanRuntime.Remoting.Contexts.Context::get_HasGlobalDynamicSinks()
      VoidRuntime.Remoting.Contexts.Context::NotifyDynamicSinksBooleanRuntime.Remoting.Messaging.IMessageBooleanBoolean)
      BooleanRuntime.Remoting.Contexts.Context::get_HasDynamicSinks()
      BooleanRuntime.Remoting.Contexts.Context::get_HasExitSinks()
      public Runtime.Remoting.Contexts.IContextPropertyRuntime.Remoting.Contexts.Context::GetPropertyString)
      public VoidRuntime.Remoting.Contexts.Context::SetPropertyRuntime.Remoting.Contexts.IContextProperty)
      public VoidRuntime.Remoting.Contexts.Context::Freeze()
      public StringRuntime.Remoting.Contexts.Context::ToString()
      Runtime.Remoting.Messaging.IMessageSinkRuntime.Remoting.Contexts.Context::GetServerContextSinkChain()
      Runtime.Remoting.Messaging.IMessageSinkRuntime.Remoting.Contexts.Context::GetClientContextSinkChain()
      Runtime.Remoting.Messaging.IMessageSinkRuntime.Remoting.Contexts.Context::CreateServerObjectSinkChainMarshalByRefObjectBoolean)
      Runtime.Remoting.Messaging.IMessageSinkRuntime.Remoting.Contexts.Context::CreateEnvoySinkMarshalByRefObject)
      Runtime.Remoting.Contexts.ContextRuntime.Remoting.Contexts.Context::SwitchToContextRuntime.Remoting.Contexts.Context)
      Runtime.Remoting.Contexts.ContextRuntime.Remoting.Contexts.Context::CreateNewContextRuntime.Remoting.Activation.IConstructionCallMessage)
      public VoidRuntime.Remoting.Contexts.Context::DoCallBackRuntime.Remoting.Contexts.CrossContextDelegate)
      public LocalDataStoreSlotRuntime.Remoting.Contexts.Context::AllocateDataSlot()
      public LocalDataStoreSlotRuntime.Remoting.Contexts.Context::AllocateNamedDataSlotString)
      public VoidRuntime.Remoting.Contexts.Context::FreeNamedDataSlotString)
      public ObjectRuntime.Remoting.Contexts.Context::GetDataLocalDataStoreSlot)
      public LocalDataStoreSlotRuntime.Remoting.Contexts.Context::GetNamedDataSlotString)
      public VoidRuntime.Remoting.Contexts.Context::SetDataLocalDataStoreSlotObject)
    }
}
