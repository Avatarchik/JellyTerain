// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Reflection.Emit
{
    public class ConstructorBuilder : ConstructorInfo
    {
      // Fields:
  mhandle : RuntimeMethodHandle
  ilgen : ILGenerator
  parameters : Type[]
  attrs : MethodAttributes
  iattrs : MethodImplAttributes
  table_idx : Int32
  call_conv : CallingConventions
  type : TypeBuilder
  pinfo : ParameterBuilder[]
  cattrs : CustomAttributeBuilder[]
  init_locals : Boolean
  paramModReq : Type[][]
  paramModOpt : Type[][]
  permissions : RefEmitPermissionSet[]
      // Properties:
  CallingConvention : CallingConventions
  InitLocals : Boolean
  TypeBuilder : TypeBuilder
  MethodHandle : RuntimeMethodHandle
  Attributes : MethodAttributes
  ReflectedType : Type
  DeclaringType : Type
  ReturnType : Type
  Name : String
  Signature : String
  Module : Module
  IsCompilerContext : Boolean
      // Events:
      // Methods:
      VoidReflection.Emit.ConstructorBuilder::.ctorReflection.Emit.TypeBuilderReflection.MethodAttributesReflection.CallingConventionsType[]Type[][]Type[][])
      VoidReflection.Emit.ConstructorBuilder::System.Runtime.InteropServices._ConstructorBuilder.GetIDsOfNamesGuid&IntPtrUInt32UInt32IntPtr)
      VoidReflection.Emit.ConstructorBuilder::System.Runtime.InteropServices._ConstructorBuilder.GetTypeInfoUInt32UInt32IntPtr)
      VoidReflection.Emit.ConstructorBuilder::System.Runtime.InteropServices._ConstructorBuilder.GetTypeInfoCountUInt32&)
      VoidReflection.Emit.ConstructorBuilder::System.Runtime.InteropServices._ConstructorBuilder.InvokeUInt32Guid&UInt32Int16IntPtrIntPtrIntPtrIntPtr)
      public Reflection.CallingConventionsReflection.Emit.ConstructorBuilder::get_CallingConvention()
      public BooleanReflection.Emit.ConstructorBuilder::get_InitLocals()
      public VoidReflection.Emit.ConstructorBuilder::set_InitLocalsBoolean)
      Reflection.Emit.TypeBuilderReflection.Emit.ConstructorBuilder::get_TypeBuilder()
      public Reflection.MethodImplAttributesReflection.Emit.ConstructorBuilder::GetMethodImplementationFlags()
      public Reflection.ParameterInfo[]Reflection.Emit.ConstructorBuilder::GetParameters()
      Reflection.ParameterInfo[]Reflection.Emit.ConstructorBuilder::GetParametersInternal()
      Int32Reflection.Emit.ConstructorBuilder::GetParameterCount()
      public ObjectReflection.Emit.ConstructorBuilder::InvokeObjectReflection.BindingFlagsReflection.BinderObject[]Globalization.CultureInfo)
      public ObjectReflection.Emit.ConstructorBuilder::InvokeReflection.BindingFlagsReflection.BinderObject[]Globalization.CultureInfo)
      public RuntimeMethodHandleReflection.Emit.ConstructorBuilder::get_MethodHandle()
      public Reflection.MethodAttributesReflection.Emit.ConstructorBuilder::get_Attributes()
      public TypeReflection.Emit.ConstructorBuilder::get_ReflectedType()
      public TypeReflection.Emit.ConstructorBuilder::get_DeclaringType()
      public TypeReflection.Emit.ConstructorBuilder::get_ReturnType()
      public StringReflection.Emit.ConstructorBuilder::get_Name()
      public StringReflection.Emit.ConstructorBuilder::get_Signature()
      public VoidReflection.Emit.ConstructorBuilder::AddDeclarativeSecuritySecurity.Permissions.SecurityActionSecurity.PermissionSet)
      public Reflection.Emit.ParameterBuilderReflection.Emit.ConstructorBuilder::DefineParameterInt32Reflection.ParameterAttributesString)
      public BooleanReflection.Emit.ConstructorBuilder::IsDefinedTypeBoolean)
      public Object[]Reflection.Emit.ConstructorBuilder::GetCustomAttributesBoolean)
      public Object[]Reflection.Emit.ConstructorBuilder::GetCustomAttributesTypeBoolean)
      public Reflection.Emit.ILGeneratorReflection.Emit.ConstructorBuilder::GetILGenerator()
      public Reflection.Emit.ILGeneratorReflection.Emit.ConstructorBuilder::GetILGeneratorInt32)
      public VoidReflection.Emit.ConstructorBuilder::SetCustomAttributeReflection.Emit.CustomAttributeBuilder)
      public VoidReflection.Emit.ConstructorBuilder::SetCustomAttributeReflection.ConstructorInfoByte[])
      public VoidReflection.Emit.ConstructorBuilder::SetImplementationFlagsReflection.MethodImplAttributes)
      public Reflection.ModuleReflection.Emit.ConstructorBuilder::GetModule()
      public Reflection.Emit.MethodTokenReflection.Emit.ConstructorBuilder::GetToken()
      public VoidReflection.Emit.ConstructorBuilder::SetSymCustomAttributeStringByte[])
      public Reflection.ModuleReflection.Emit.ConstructorBuilder::get_Module()
      public StringReflection.Emit.ConstructorBuilder::ToString()
      VoidReflection.Emit.ConstructorBuilder::fixup()
      VoidReflection.Emit.ConstructorBuilder::GenerateDebugInfoDiagnostics.SymbolStore.ISymbolWriter)
      Int32Reflection.Emit.ConstructorBuilder::get_next_table_indexObjectInt32Boolean)
      BooleanReflection.Emit.ConstructorBuilder::get_IsCompilerContext()
      VoidReflection.Emit.ConstructorBuilder::RejectIfCreated()
      ExceptionReflection.Emit.ConstructorBuilder::not_supported()
      ExceptionReflection.Emit.ConstructorBuilder::not_after_created()
      ExceptionReflection.Emit.ConstructorBuilder::not_created()
    }
}
