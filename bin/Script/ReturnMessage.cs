// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Runtime.Remoting.Messaging
{
    public class ReturnMessage : Object
    {
      // Fields:
  _outArgs : Object[]
  _args : Object[]
  _outArgsCount : Int32
  _callCtx : LogicalCallContext
  _returnValue : Object
  _uri : String
  _exception : Exception
  _methodBase : MethodBase
  _methodName : String
  _methodSignature : Type[]
  _typeName : String
  _properties : MethodReturnDictionary
  _targetIdentity : Identity
  _inArgInfo : ArgInfo
      // Properties:
  System.Runtime.Remoting.Messaging.IInternalMessage.Uri : String
  System.Runtime.Remoting.Messaging.IInternalMessage.TargetIdentity : Identity
  ArgCount : Int32
  Args : Object[]
  HasVarArgs : Boolean
  LogicalCallContext : LogicalCallContext
  MethodBase : MethodBase
  MethodName : String
  MethodSignature : Object
  Properties : IDictionary
  TypeName : String
  Uri : String
  Exception : Exception
  OutArgCount : Int32
  OutArgs : Object[]
  ReturnValue : Object
      // Events:
      // Methods:
      public VoidRuntime.Remoting.Messaging.ReturnMessage::.ctorObjectObject[]Int32Runtime.Remoting.Messaging.LogicalCallContextRuntime.Remoting.Messaging.IMethodCallMessage)
      public VoidRuntime.Remoting.Messaging.ReturnMessage::.ctorExceptionRuntime.Remoting.Messaging.IMethodCallMessage)
      StringRuntime.Remoting.Messaging.ReturnMessage::System.Runtime.Remoting.Messaging.IInternalMessage.get_Uri()
      VoidRuntime.Remoting.Messaging.ReturnMessage::System.Runtime.Remoting.Messaging.IInternalMessage.set_UriString)
      Runtime.Remoting.IdentityRuntime.Remoting.Messaging.ReturnMessage::System.Runtime.Remoting.Messaging.IInternalMessage.get_TargetIdentity()
      VoidRuntime.Remoting.Messaging.ReturnMessage::System.Runtime.Remoting.Messaging.IInternalMessage.set_TargetIdentityRuntime.Remoting.Identity)
      public Int32Runtime.Remoting.Messaging.ReturnMessage::get_ArgCount()
      public Object[]Runtime.Remoting.Messaging.ReturnMessage::get_Args()
      public BooleanRuntime.Remoting.Messaging.ReturnMessage::get_HasVarArgs()
      public Runtime.Remoting.Messaging.LogicalCallContextRuntime.Remoting.Messaging.ReturnMessage::get_LogicalCallContext()
      public Reflection.MethodBaseRuntime.Remoting.Messaging.ReturnMessage::get_MethodBase()
      public StringRuntime.Remoting.Messaging.ReturnMessage::get_MethodName()
      public ObjectRuntime.Remoting.Messaging.ReturnMessage::get_MethodSignature()
      public Collections.IDictionaryRuntime.Remoting.Messaging.ReturnMessage::get_Properties()
      public StringRuntime.Remoting.Messaging.ReturnMessage::get_TypeName()
      public StringRuntime.Remoting.Messaging.ReturnMessage::get_Uri()
      public VoidRuntime.Remoting.Messaging.ReturnMessage::set_UriString)
      public ObjectRuntime.Remoting.Messaging.ReturnMessage::GetArgInt32)
      public StringRuntime.Remoting.Messaging.ReturnMessage::GetArgNameInt32)
      public ExceptionRuntime.Remoting.Messaging.ReturnMessage::get_Exception()
      public Int32Runtime.Remoting.Messaging.ReturnMessage::get_OutArgCount()
      public Object[]Runtime.Remoting.Messaging.ReturnMessage::get_OutArgs()
      public ObjectRuntime.Remoting.Messaging.ReturnMessage::get_ReturnValue()
      public ObjectRuntime.Remoting.Messaging.ReturnMessage::GetOutArgInt32)
      public StringRuntime.Remoting.Messaging.ReturnMessage::GetOutArgNameInt32)
    }
}
