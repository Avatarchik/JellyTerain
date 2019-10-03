// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Reflection
{
    public class MethodBody : Object
    {
      // Fields:
  clauses : ExceptionHandlingClause[]
  locals : LocalVariableInfo[]
  il : Byte[]
  init_locals : Boolean
  sig_token : Int32
  max_stack : Int32
      // Properties:
  ExceptionHandlingClauses : IList`1
  LocalVariables : IList`1
  InitLocals : Boolean
  LocalSignatureMetadataToken : Int32
  MaxStackSize : Int32
      // Events:
      // Methods:
      VoidReflection.MethodBody::.ctor()
      public Collections.Generic.IList`1<System.Reflection.ExceptionHandlingClause>Reflection.MethodBody::get_ExceptionHandlingClauses()
      public Collections.Generic.IList`1<System.Reflection.LocalVariableInfo>Reflection.MethodBody::get_LocalVariables()
      public BooleanReflection.MethodBody::get_InitLocals()
      public Int32Reflection.MethodBody::get_LocalSignatureMetadataToken()
      public Int32Reflection.MethodBody::get_MaxStackSize()
      public Byte[]Reflection.MethodBody::GetILAsByteArray()
    }
}
