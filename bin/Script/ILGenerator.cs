// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Reflection.Emit
{
    public class ILGenerator : Object
    {
      // Fields:
  defaultFixupSize : Int32
  defaultLabelsSize : Int32
  defaultExceptionStackSize : Int32
  void_type : Type
  code : Byte[]
  code_len : Int32
  max_stack : Int32
  cur_stack : Int32
  locals : LocalBuilder[]
  ex_handlers : ILExceptionInfo[]
  num_token_fixups : Int32
  token_fixups : ILTokenInfo[]
  labels : LabelData[]
  num_labels : Int32
  fixups : LabelFixup[]
  num_fixups : Int32
  module : Module
  cur_block : Int32
  open_blocks : Stack
  token_gen : TokenGenerator
  sequencePointLists : ArrayList
  currentSequence : SequencePointList
      // Properties:
  HasDebugInfo : Boolean
      // Events:
      // Methods:
      VoidReflection.Emit.ILGenerator::.ctorReflection.ModuleReflection.Emit.TokenGeneratorInt32)
      VoidReflection.Emit.ILGenerator::.cctor()
      VoidReflection.Emit.ILGenerator::System.Runtime.InteropServices._ILGenerator.GetIDsOfNamesGuid&IntPtrUInt32UInt32IntPtr)
      VoidReflection.Emit.ILGenerator::System.Runtime.InteropServices._ILGenerator.GetTypeInfoUInt32UInt32IntPtr)
      VoidReflection.Emit.ILGenerator::System.Runtime.InteropServices._ILGenerator.GetTypeInfoCountUInt32&)
      VoidReflection.Emit.ILGenerator::System.Runtime.InteropServices._ILGenerator.InvokeUInt32Guid&UInt32Int16IntPtrIntPtrIntPtrIntPtr)
      VoidReflection.Emit.ILGenerator::add_token_fixupReflection.MemberInfo)
      VoidReflection.Emit.ILGenerator::make_roomInt32)
      VoidReflection.Emit.ILGenerator::emit_intInt32)
      VoidReflection.Emit.ILGenerator::ll_emitReflection.Emit.OpCode)
      Int32Reflection.Emit.ILGenerator::target_lenReflection.Emit.OpCode)
      VoidReflection.Emit.ILGenerator::InternalEndClause()
      public VoidReflection.Emit.ILGenerator::BeginCatchBlockType)
      public VoidReflection.Emit.ILGenerator::BeginExceptFilterBlock()
      public Reflection.Emit.LabelReflection.Emit.ILGenerator::BeginExceptionBlock()
      public VoidReflection.Emit.ILGenerator::BeginFaultBlock()
      public VoidReflection.Emit.ILGenerator::BeginFinallyBlock()
      public VoidReflection.Emit.ILGenerator::BeginScope()
      public Reflection.Emit.LocalBuilderReflection.Emit.ILGenerator::DeclareLocalType)
      public Reflection.Emit.LocalBuilderReflection.Emit.ILGenerator::DeclareLocalTypeBoolean)
      public Reflection.Emit.LabelReflection.Emit.ILGenerator::DefineLabel()
      public VoidReflection.Emit.ILGenerator::EmitReflection.Emit.OpCode)
      public VoidReflection.Emit.ILGenerator::EmitReflection.Emit.OpCodeByte)
      public VoidReflection.Emit.ILGenerator::EmitReflection.Emit.OpCodeReflection.ConstructorInfo)
      public VoidReflection.Emit.ILGenerator::EmitReflection.Emit.OpCodeDouble)
      public VoidReflection.Emit.ILGenerator::EmitReflection.Emit.OpCodeReflection.FieldInfo)
      public VoidReflection.Emit.ILGenerator::EmitReflection.Emit.OpCodeInt16)
      public VoidReflection.Emit.ILGenerator::EmitReflection.Emit.OpCodeInt32)
      public VoidReflection.Emit.ILGenerator::EmitReflection.Emit.OpCodeInt64)
      public VoidReflection.Emit.ILGenerator::EmitReflection.Emit.OpCodeReflection.Emit.Label)
      public VoidReflection.Emit.ILGenerator::EmitReflection.Emit.OpCodeReflection.Emit.Label[])
      public VoidReflection.Emit.ILGenerator::EmitReflection.Emit.OpCodeReflection.Emit.LocalBuilder)
      public VoidReflection.Emit.ILGenerator::EmitReflection.Emit.OpCodeReflection.MethodInfo)
      VoidReflection.Emit.ILGenerator::EmitReflection.Emit.OpCodeReflection.MethodInfoInt32)
      public VoidReflection.Emit.ILGenerator::EmitReflection.Emit.OpCodeSByte)
      public VoidReflection.Emit.ILGenerator::EmitReflection.Emit.OpCodeReflection.Emit.SignatureHelper)
      public VoidReflection.Emit.ILGenerator::EmitReflection.Emit.OpCodeSingle)
      public VoidReflection.Emit.ILGenerator::EmitReflection.Emit.OpCodeString)
      public VoidReflection.Emit.ILGenerator::EmitReflection.Emit.OpCodeType)
      public VoidReflection.Emit.ILGenerator::EmitCallReflection.Emit.OpCodeReflection.MethodInfoType[])
      public VoidReflection.Emit.ILGenerator::EmitCalliReflection.Emit.OpCodeRuntime.InteropServices.CallingConventionTypeType[])
      public VoidReflection.Emit.ILGenerator::EmitCalliReflection.Emit.OpCodeReflection.CallingConventionsTypeType[]Type[])
      public VoidReflection.Emit.ILGenerator::EmitWriteLineReflection.FieldInfo)
      public VoidReflection.Emit.ILGenerator::EmitWriteLineReflection.Emit.LocalBuilder)
      public VoidReflection.Emit.ILGenerator::EmitWriteLineString)
      public VoidReflection.Emit.ILGenerator::EndExceptionBlock()
      public VoidReflection.Emit.ILGenerator::EndScope()
      public VoidReflection.Emit.ILGenerator::MarkLabelReflection.Emit.Label)
      public VoidReflection.Emit.ILGenerator::MarkSequencePointDiagnostics.SymbolStore.ISymbolDocumentWriterInt32Int32Int32Int32)
      VoidReflection.Emit.ILGenerator::GenerateDebugInfoDiagnostics.SymbolStore.ISymbolWriter)
      BooleanReflection.Emit.ILGenerator::get_HasDebugInfo()
      public VoidReflection.Emit.ILGenerator::ThrowExceptionType)
      public VoidReflection.Emit.ILGenerator::UsingNamespaceString)
      VoidReflection.Emit.ILGenerator::label_fixup()
      Int32Reflection.Emit.ILGenerator::Mono_GetCurrentOffsetReflection.Emit.ILGenerator)
    }
}
