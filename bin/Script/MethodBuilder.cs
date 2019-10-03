// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Reflection.Emit
{
    public class MethodBuilder : MethodInfo
    {
      // Fields:
  mhandle : RuntimeMethodHandle
  rtype : Type
  parameters : Type[]
  attrs : MethodAttributes
  iattrs : MethodImplAttributes
  name : String
  table_idx : Int32
  code : Byte[]
  ilgen : ILGenerator
  type : TypeBuilder
  pinfo : ParameterBuilder[]
  cattrs : CustomAttributeBuilder[]
  override_method : MethodInfo
  pi_dll : String
  pi_entry : String
  charset : CharSet
  extra_flags : UInt32
  native_cc : CallingConvention
  call_conv : CallingConventions
  init_locals : Boolean
  generic_container : IntPtr
  generic_params : GenericTypeParameterBuilder[]
  returnModReq : Type[]
  returnModOpt : Type[]
  paramModReq : Type[][]
  paramModOpt : Type[][]
  permissions : RefEmitPermissionSet[]
  <>f__switch$map1D : Dictionary`2
      // Properties:
  ContainsGenericParameters : Boolean
  InitLocals : Boolean
  TypeBuilder : TypeBuilder
  MethodHandle : RuntimeMethodHandle
  ReturnType : Type
  ReflectedType : Type
  DeclaringType : Type
  Name : String
  Attributes : MethodAttributes
  ReturnTypeCustomAttributes : ICustomAttributeProvider
  CallingConvention : CallingConventions
  Signature : String
  BestFitMapping : Boolean
  ThrowOnUnmappableChar : Boolean
  ExactSpelling : Boolean
  SetLastError : Boolean
  IsGenericMethodDefinition : Boolean
  IsGenericMethod : Boolean
  Module : Module
      // Events:
      // Methods:
      VoidReflection.Emit.MethodBuilder::.ctorReflection.Emit.TypeBuilderStringReflection.MethodAttributesReflection.CallingConventionsTypeType[]Type[]Type[]Type[][]Type[][])
      VoidReflection.Emit.MethodBuilder::.ctorReflection.Emit.TypeBuilderStringReflection.MethodAttributesReflection.CallingConventionsTypeType[]Type[]Type[]Type[][]Type[][]StringStringRuntime.InteropServices.CallingConventionRuntime.InteropServices.CharSet)
      VoidReflection.Emit.MethodBuilder::System.Runtime.InteropServices._MethodBuilder.GetIDsOfNamesGuid&IntPtrUInt32UInt32IntPtr)
      VoidReflection.Emit.MethodBuilder::System.Runtime.InteropServices._MethodBuilder.GetTypeInfoUInt32UInt32IntPtr)
      VoidReflection.Emit.MethodBuilder::System.Runtime.InteropServices._MethodBuilder.GetTypeInfoCountUInt32&)
      VoidReflection.Emit.MethodBuilder::System.Runtime.InteropServices._MethodBuilder.InvokeUInt32Guid&UInt32Int16IntPtrIntPtrIntPtrIntPtr)
      public BooleanReflection.Emit.MethodBuilder::get_ContainsGenericParameters()
      public BooleanReflection.Emit.MethodBuilder::get_InitLocals()
      public VoidReflection.Emit.MethodBuilder::set_InitLocalsBoolean)
      Reflection.Emit.TypeBuilderReflection.Emit.MethodBuilder::get_TypeBuilder()
      public RuntimeMethodHandleReflection.Emit.MethodBuilder::get_MethodHandle()
      public TypeReflection.Emit.MethodBuilder::get_ReturnType()
      public TypeReflection.Emit.MethodBuilder::get_ReflectedType()
      public TypeReflection.Emit.MethodBuilder::get_DeclaringType()
      public StringReflection.Emit.MethodBuilder::get_Name()
      public Reflection.MethodAttributesReflection.Emit.MethodBuilder::get_Attributes()
      public Reflection.ICustomAttributeProviderReflection.Emit.MethodBuilder::get_ReturnTypeCustomAttributes()
      public Reflection.CallingConventionsReflection.Emit.MethodBuilder::get_CallingConvention()
      public StringReflection.Emit.MethodBuilder::get_Signature()
      VoidReflection.Emit.MethodBuilder::set_BestFitMappingBoolean)
      VoidReflection.Emit.MethodBuilder::set_ThrowOnUnmappableCharBoolean)
      VoidReflection.Emit.MethodBuilder::set_ExactSpellingBoolean)
      VoidReflection.Emit.MethodBuilder::set_SetLastErrorBoolean)
      public Reflection.Emit.MethodTokenReflection.Emit.MethodBuilder::GetToken()
      public Reflection.MethodInfoReflection.Emit.MethodBuilder::GetBaseDefinition()
      public Reflection.MethodImplAttributesReflection.Emit.MethodBuilder::GetMethodImplementationFlags()
      public Reflection.ParameterInfo[]Reflection.Emit.MethodBuilder::GetParameters()
      Int32Reflection.Emit.MethodBuilder::GetParameterCount()
      public Reflection.ModuleReflection.Emit.MethodBuilder::GetModule()
      public VoidReflection.Emit.MethodBuilder::CreateMethodBodyByte[]Int32)
      public ObjectReflection.Emit.MethodBuilder::InvokeObjectReflection.BindingFlagsReflection.BinderObject[]Globalization.CultureInfo)
      public BooleanReflection.Emit.MethodBuilder::IsDefinedTypeBoolean)
      public Object[]Reflection.Emit.MethodBuilder::GetCustomAttributesBoolean)
      public Object[]Reflection.Emit.MethodBuilder::GetCustomAttributesTypeBoolean)
      public Reflection.Emit.ILGeneratorReflection.Emit.MethodBuilder::GetILGenerator()
      public Reflection.Emit.ILGeneratorReflection.Emit.MethodBuilder::GetILGeneratorInt32)
      public Reflection.Emit.ParameterBuilderReflection.Emit.MethodBuilder::DefineParameterInt32Reflection.ParameterAttributesString)
      VoidReflection.Emit.MethodBuilder::check_override()
      VoidReflection.Emit.MethodBuilder::fixup()
      VoidReflection.Emit.MethodBuilder::GenerateDebugInfoDiagnostics.SymbolStore.ISymbolWriter)
      public VoidReflection.Emit.MethodBuilder::SetCustomAttributeReflection.Emit.CustomAttributeBuilder)
      public VoidReflection.Emit.MethodBuilder::SetCustomAttributeReflection.ConstructorInfoByte[])
      public VoidReflection.Emit.MethodBuilder::SetImplementationFlagsReflection.MethodImplAttributes)
      public VoidReflection.Emit.MethodBuilder::AddDeclarativeSecuritySecurity.Permissions.SecurityActionSecurity.PermissionSet)
      public VoidReflection.Emit.MethodBuilder::SetMarshalReflection.Emit.UnmanagedMarshal)
      public VoidReflection.Emit.MethodBuilder::SetSymCustomAttributeStringByte[])
      public StringReflection.Emit.MethodBuilder::ToString()
      public BooleanReflection.Emit.MethodBuilder::EqualsObject)
      public Int32Reflection.Emit.MethodBuilder::GetHashCode()
      Int32Reflection.Emit.MethodBuilder::get_next_table_indexObjectInt32Boolean)
      VoidReflection.Emit.MethodBuilder::set_overrideReflection.MethodInfo)
      VoidReflection.Emit.MethodBuilder::RejectIfCreated()
      ExceptionReflection.Emit.MethodBuilder::NotSupported()
      public Reflection.MethodInfoReflection.Emit.MethodBuilder::MakeGenericMethodType[])
      public BooleanReflection.Emit.MethodBuilder::get_IsGenericMethodDefinition()
      public BooleanReflection.Emit.MethodBuilder::get_IsGenericMethod()
      public Reflection.MethodInfoReflection.Emit.MethodBuilder::GetGenericMethodDefinition()
      public Type[]Reflection.Emit.MethodBuilder::GetGenericArguments()
      public Reflection.Emit.GenericTypeParameterBuilder[]Reflection.Emit.MethodBuilder::DefineGenericParametersString[])
      public VoidReflection.Emit.MethodBuilder::SetReturnTypeType)
      public VoidReflection.Emit.MethodBuilder::SetParametersType[])
      public VoidReflection.Emit.MethodBuilder::SetSignatureTypeType[]Type[]Type[]Type[][]Type[][])
      public Reflection.ModuleReflection.Emit.MethodBuilder::get_Module()
    }
}
