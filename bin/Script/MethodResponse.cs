// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Runtime.Remoting.Messaging
{
    public class MethodResponse : Object
    {
      // Fields:
  _methodName : String
  _uri : String
  _typeName : String
  _methodBase : MethodBase
  _returnValue : Object
  _exception : Exception
  _methodSignature : Type[]
  _inArgInfo : ArgInfo
  _args : Object[]
  _outArgs : Object[]
  _callMsg : IMethodCallMessage
  _callContext : LogicalCallContext
  _targetIdentity : Identity
  ExternalProperties : IDictionary
  InternalProperties : IDictionary
  <>f__switch$map25 : Dictionary`2
      // Properties:
  System.Runtime.Remoting.Messaging.IInternalMessage.Uri : String
  System.Runtime.Remoting.Messaging.IInternalMessage.TargetIdentity : Identity
  ArgCount : Int32
  Args : Object[]
  Exception : Exception
  HasVarArgs : Boolean
  LogicalCallContext : LogicalCallContext
  MethodBase : MethodBase
  MethodName : String
  MethodSignature : Object
  OutArgCount : Int32
  OutArgs : Object[]
  Properties : IDictionary
  ReturnValue : Object
  TypeName : String
  Uri : String
      // Events:
      // Methods:
      public VoidRuntime.Remoting.Messaging.MethodResponse::.ctorRuntime.Remoting.Messaging.Header[]Runtime.Remoting.Messaging.IMethodCallMessage)
      VoidRuntime.Remoting.Messaging.MethodResponse::.ctorExceptionRuntime.Remoting.Messaging.IMethodCallMessage)
      VoidRuntime.Remoting.Messaging.MethodResponse::.ctorObjectObject[]Runtime.Remoting.Messaging.LogicalCallContextRuntime.Remoting.Messaging.IMethodCallMessage)
      VoidRuntime.Remoting.Messaging.MethodResponse::.ctorRuntime.Remoting.Messaging.IMethodCallMessageRuntime.Remoting.Messaging.CADMethodReturnMessage)
      VoidRuntime.Remoting.Messaging.MethodResponse::.ctorRuntime.Serialization.SerializationInfoRuntime.Serialization.StreamingContext)
      StringRuntime.Remoting.Messaging.MethodResponse::System.Runtime.Remoting.Messaging.IInternalMessage.get_Uri()
      VoidRuntime.Remoting.Messaging.MethodResponse::System.Runtime.Remoting.Messaging.IInternalMessage.set_UriString)
      Runtime.Remoting.IdentityRuntime.Remoting.Messaging.MethodResponse::System.Runtime.Remoting.Messaging.IInternalMessage.get_TargetIdentity()
      VoidRuntime.Remoting.Messaging.MethodResponse::System.Runtime.Remoting.Messaging.IInternalMessage.set_TargetIdentityRuntime.Remoting.Identity)
      VoidRuntime.Remoting.Messaging.MethodResponse::InitMethodPropertyStringObject)
      public Int32Runtime.Remoting.Messaging.MethodResponse::get_ArgCount()
      public Object[]Runtime.Remoting.Messaging.MethodResponse::get_Args()
      public ExceptionRuntime.Remoting.Messaging.MethodResponse::get_Exception()
      public BooleanRuntime.Remoting.Messaging.MethodResponse::get_HasVarArgs()
      public Runtime.Remoting.Messaging.LogicalCallContextRuntime.Remoting.Messaging.MethodResponse::get_LogicalCallContext()
      public Reflection.MethodBaseRuntime.Remoting.Messaging.MethodResponse::get_MethodBase()
      public StringRuntime.Remoting.Messaging.MethodResponse::get_MethodName()
      public ObjectRuntime.Remoting.Messaging.MethodResponse::get_MethodSignature()
      public Int32Runtime.Remoting.Messaging.MethodResponse::get_OutArgCount()
      public Object[]Runtime.Remoting.Messaging.MethodResponse::get_OutArgs()
      public Collections.IDictionaryRuntime.Remoting.Messaging.MethodResponse::get_Properties()
      public ObjectRuntime.Remoting.Messaging.MethodResponse::get_ReturnValue()
      public StringRuntime.Remoting.Messaging.MethodResponse::get_TypeName()
      public StringRuntime.Remoting.Messaging.MethodResponse::get_Uri()
      public VoidRuntime.Remoting.Messaging.MethodResponse::set_UriString)
      public ObjectRuntime.Remoting.Messaging.MethodResponse::GetArgInt32)
      public StringRuntime.Remoting.Messaging.MethodResponse::GetArgNameInt32)
      public VoidRuntime.Remoting.Messaging.MethodResponse::GetObjectDataRuntime.Serialization.SerializationInfoRuntime.Serialization.StreamingContext)
      public ObjectRuntime.Remoting.Messaging.MethodResponse::GetOutArgInt32)
      public StringRuntime.Remoting.Messaging.MethodResponse::GetOutArgNameInt32)
      public ObjectRuntime.Remoting.Messaging.MethodResponse::HeaderHandlerRuntime.Remoting.Messaging.Header[])
      public VoidRuntime.Remoting.Messaging.MethodResponse::RootSetObjectDataRuntime.Serialization.SerializationInfoRuntime.Serialization.StreamingContext)
    }
}
