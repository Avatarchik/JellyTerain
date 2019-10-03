// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Diagnostics
{
    public class StackFrame : Object
    {
      // Fields:
  public OFFSET_UNKNOWN : Int32
  ilOffset : Int32
  nativeOffset : Int32
  methodBase : MethodBase
  fileName : String
  lineNumber : Int32
  columnNumber : Int32
  internalMethodName : String
      // Properties:
      // Events:
      // Methods:
      public VoidDiagnostics.StackFrame::.ctor()
      public VoidDiagnostics.StackFrame::.ctorBoolean)
      public VoidDiagnostics.StackFrame::.ctorInt32)
      public VoidDiagnostics.StackFrame::.ctorInt32Boolean)
      public VoidDiagnostics.StackFrame::.ctorStringInt32)
      public VoidDiagnostics.StackFrame::.ctorStringInt32Int32)
      BooleanDiagnostics.StackFrame::get_frame_infoInt32BooleanReflection.MethodBase&Int32&Int32&String&Int32&Int32&)
      public Int32Diagnostics.StackFrame::GetFileLineNumber()
      public Int32Diagnostics.StackFrame::GetFileColumnNumber()
      public StringDiagnostics.StackFrame::GetFileName()
      StringDiagnostics.StackFrame::GetSecureFileName()
      public Int32Diagnostics.StackFrame::GetILOffset()
      public Reflection.MethodBaseDiagnostics.StackFrame::GetMethod()
      public Int32Diagnostics.StackFrame::GetNativeOffset()
      StringDiagnostics.StackFrame::GetInternalMethodName()
      public StringDiagnostics.StackFrame::ToString()
    }
}
