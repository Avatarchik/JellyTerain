// Class info from System.dll
// 
using UnityEngine;

namespace System.Diagnostics
{
    public class TraceListener : MarshalByRefObject
    {
      // Fields:
  indentLevel : Int32
  indentSize : Int32
  attributes : StringDictionary
  filter : TraceFilter
  options : TraceOptions
  name : String
  needIndent : Boolean
      // Properties:
  IndentLevel : Int32
  IndentSize : Int32
  Name : String
  NeedIndent : Boolean
  IsThreadSafe : Boolean
  Attributes : StringDictionary
  Filter : TraceFilter
  TraceOutputOptions : TraceOptions
      // Events:
      // Methods:
      VoidDiagnostics.TraceListener::.ctor()
      VoidDiagnostics.TraceListener::.ctorString)
      public Int32Diagnostics.TraceListener::get_IndentLevel()
      public VoidDiagnostics.TraceListener::set_IndentLevelInt32)
      public Int32Diagnostics.TraceListener::get_IndentSize()
      public VoidDiagnostics.TraceListener::set_IndentSizeInt32)
      public StringDiagnostics.TraceListener::get_Name()
      public VoidDiagnostics.TraceListener::set_NameString)
      BooleanDiagnostics.TraceListener::get_NeedIndent()
      VoidDiagnostics.TraceListener::set_NeedIndentBoolean)
      public BooleanDiagnostics.TraceListener::get_IsThreadSafe()
      public VoidDiagnostics.TraceListener::Close()
      public VoidDiagnostics.TraceListener::Dispose()
      VoidDiagnostics.TraceListener::DisposeBoolean)
      public VoidDiagnostics.TraceListener::FailString)
      public VoidDiagnostics.TraceListener::FailStringString)
      public VoidDiagnostics.TraceListener::Flush()
      public VoidDiagnostics.TraceListener::WriteObject)
      public VoidDiagnostics.TraceListener::WriteString)
      public VoidDiagnostics.TraceListener::WriteObjectString)
      public VoidDiagnostics.TraceListener::WriteStringString)
      VoidDiagnostics.TraceListener::WriteIndent()
      public VoidDiagnostics.TraceListener::WriteLineObject)
      public VoidDiagnostics.TraceListener::WriteLineString)
      public VoidDiagnostics.TraceListener::WriteLineObjectString)
      public VoidDiagnostics.TraceListener::WriteLineStringString)
      StringDiagnostics.TraceListener::FormatArrayCollections.ICollectionString)
      public VoidDiagnostics.TraceListener::TraceDataDiagnostics.TraceEventCacheStringDiagnostics.TraceEventTypeInt32Object)
      public VoidDiagnostics.TraceListener::TraceDataDiagnostics.TraceEventCacheStringDiagnostics.TraceEventTypeInt32Object[])
      public VoidDiagnostics.TraceListener::TraceEventDiagnostics.TraceEventCacheStringDiagnostics.TraceEventTypeInt32)
      public VoidDiagnostics.TraceListener::TraceEventDiagnostics.TraceEventCacheStringDiagnostics.TraceEventTypeInt32String)
      public VoidDiagnostics.TraceListener::TraceEventDiagnostics.TraceEventCacheStringDiagnostics.TraceEventTypeInt32StringObject[])
      public VoidDiagnostics.TraceListener::TraceTransferDiagnostics.TraceEventCacheStringInt32StringGuid)
      String[]Diagnostics.TraceListener::GetSupportedAttributes()
      public Collections.Specialized.StringDictionaryDiagnostics.TraceListener::get_Attributes()
      public Diagnostics.TraceFilterDiagnostics.TraceListener::get_Filter()
      public VoidDiagnostics.TraceListener::set_FilterDiagnostics.TraceFilter)
      public Diagnostics.TraceOptionsDiagnostics.TraceListener::get_TraceOutputOptions()
      public VoidDiagnostics.TraceListener::set_TraceOutputOptionsDiagnostics.TraceOptions)
    }
}
