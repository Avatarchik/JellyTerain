// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Runtime.Remoting.Messaging
{
    class MonoMethodMessage : Object
    {
      // Fields:
  method : MonoMethod
  args : Object[]
  names : String[]
  arg_types : Byte[]
  public ctx : LogicalCallContext
  public rval : Object
  public exc : Exception
  asyncResult : AsyncResult
  call_type : CallType
  uri : String
  properties : MethodCallDictionary
  methodSignature : Type[]
  identity : Identity
      // Properties:
  System.Runtime.Remoting.Messaging.IInternalMessage.TargetIdentity : Identity
  Properties : IDictionary
  ArgCount : Int32
  Args : Object[]
  HasVarArgs : Boolean
  LogicalCallContext : LogicalCallContext
  MethodBase : MethodBase
  MethodName : String
  MethodSignature : Object
  TypeName : String
  Uri : String
  InArgCount : Int32
  InArgs : Object[]
  Exception : Exception
  OutArgCount : Int32
  OutArgs : Object[]
  ReturnValue : Object
  IsAsync : Boolean
  AsyncResult : AsyncResult
  CallType : CallType
      // Events:
      // Methods:
      public VoidRuntime.Remoting.Messaging.MonoMethodMessage::.ctorReflection.MethodBaseObject[])
      public VoidRuntime.Remoting.Messaging.MonoMethodMessage::.ctorTypeStringObject[])
      Runtime.Remoting.IdentityRuntime.Remoting.Messaging.MonoMethodMessage::System.Runtime.Remoting.Messaging.IInternalMessage.get_TargetIdentity()
      VoidRuntime.Remoting.Messaging.MonoMethodMessage::System.Runtime.Remoting.Messaging.IInternalMessage.set_TargetIdentityRuntime.Remoting.Identity)
      VoidRuntime.Remoting.Messaging.MonoMethodMessage::InitMessageReflection.MonoMethodObject[])
      public Collections.IDictionaryRuntime.Remoting.Messaging.MonoMethodMessage::get_Properties()
      public Int32Runtime.Remoting.Messaging.MonoMethodMessage::get_ArgCount()
      public Object[]Runtime.Remoting.Messaging.MonoMethodMessage::get_Args()
      public BooleanRuntime.Remoting.Messaging.MonoMethodMessage::get_HasVarArgs()
      public Runtime.Remoting.Messaging.LogicalCallContextRuntime.Remoting.Messaging.MonoMethodMessage::get_LogicalCallContext()
      public VoidRuntime.Remoting.Messaging.MonoMethodMessage::set_LogicalCallContextRuntime.Remoting.Messaging.LogicalCallContext)
      public Reflection.MethodBaseRuntime.Remoting.Messaging.MonoMethodMessage::get_MethodBase()
      public StringRuntime.Remoting.Messaging.MonoMethodMessage::get_MethodName()
      public ObjectRuntime.Remoting.Messaging.MonoMethodMessage::get_MethodSignature()
      public StringRuntime.Remoting.Messaging.MonoMethodMessage::get_TypeName()
      public StringRuntime.Remoting.Messaging.MonoMethodMessage::get_Uri()
      public VoidRuntime.Remoting.Messaging.MonoMethodMessage::set_UriString)
      public ObjectRuntime.Remoting.Messaging.MonoMethodMessage::GetArgInt32)
      public StringRuntime.Remoting.Messaging.MonoMethodMessage::GetArgNameInt32)
      public Int32Runtime.Remoting.Messaging.MonoMethodMessage::get_InArgCount()
      public Object[]Runtime.Remoting.Messaging.MonoMethodMessage::get_InArgs()
      public ObjectRuntime.Remoting.Messaging.MonoMethodMessage::GetInArgInt32)
      public StringRuntime.Remoting.Messaging.MonoMethodMessage::GetInArgNameInt32)
      public ExceptionRuntime.Remoting.Messaging.MonoMethodMessage::get_Exception()
      public Int32Runtime.Remoting.Messaging.MonoMethodMessage::get_OutArgCount()
      public Object[]Runtime.Remoting.Messaging.MonoMethodMessage::get_OutArgs()
      public ObjectRuntime.Remoting.Messaging.MonoMethodMessage::get_ReturnValue()
      public ObjectRuntime.Remoting.Messaging.MonoMethodMessage::GetOutArgInt32)
      public StringRuntime.Remoting.Messaging.MonoMethodMessage::GetOutArgNameInt32)
      public BooleanRuntime.Remoting.Messaging.MonoMethodMessage::get_IsAsync()
      public Runtime.Remoting.Messaging.AsyncResultRuntime.Remoting.Messaging.MonoMethodMessage::get_AsyncResult()
      Runtime.Remoting.Messaging.CallTypeRuntime.Remoting.Messaging.MonoMethodMessage::get_CallType()
      public BooleanRuntime.Remoting.Messaging.MonoMethodMessage::NeedsOutProcessingInt32&)
    }
}
