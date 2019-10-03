// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System
{
    public class Exception : Object
    {
      // Fields:
  trace_ips : IntPtr[]
  inner_exception : Exception
  message : String
  help_link : String
  class_name : String
  stack_trace : String
  _remoteStackTraceString : String
  remote_stack_index : Int32
  hresult : Int32
  source : String
  _data : IDictionary
      // Properties:
  InnerException : Exception
  HelpLink : String
  HResult : Int32
  ClassName : String
  Message : String
  Source : String
  StackTrace : String
  TargetSite : MethodBase
  Data : IDictionary
      // Events:
      // Methods:
      public VoidException::.ctor()
      public VoidException::.ctorString)
      VoidException::.ctorRuntime.Serialization.SerializationInfoRuntime.Serialization.StreamingContext)
      public VoidException::.ctorStringException)
      public ExceptionException::get_InnerException()
      public StringException::get_HelpLink()
      public VoidException::set_HelpLinkString)
      Int32Exception::get_HResult()
      VoidException::set_HResultInt32)
      VoidException::SetMessageString)
      VoidException::SetStackTraceString)
      StringException::get_ClassName()
      public StringException::get_Message()
      public StringException::get_Source()
      public VoidException::set_SourceString)
      public StringException::get_StackTrace()
      public Reflection.MethodBaseException::get_TargetSite()
      public Collections.IDictionaryException::get_Data()
      public ExceptionException::GetBaseException()
      public VoidException::GetObjectDataRuntime.Serialization.SerializationInfoRuntime.Serialization.StreamingContext)
      public StringException::ToString()
      ExceptionException::FixRemotingException()
      VoidException::GetFullNameForStackTraceText.StringBuilderReflection.MethodBase)
      public TypeException::GetType()
    }
}
