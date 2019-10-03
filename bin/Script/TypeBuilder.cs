// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Reflection.Emit
{
    public class TypeBuilder : Type
    {
      // Fields:
  public UnspecifiedTypeSize : Int32
  tname : String
  nspace : String
  parent : Type
  nesting_type : Type
  interfaces : Type[]
  num_methods : Int32
  methods : MethodBuilder[]
  ctors : ConstructorBuilder[]
  properties : PropertyBuilder[]
  num_fields : Int32
  fields : FieldBuilder[]
  events : EventBuilder[]
  cattrs : CustomAttributeBuilder[]
  subtypes : TypeBuilder[]
  attrs : TypeAttributes
  table_idx : Int32
  pmodule : ModuleBuilder
  class_size : Int32
  packing_size : PackingSize
  generic_container : IntPtr
  generic_params : GenericTypeParameterBuilder[]
  permissions : RefEmitPermissionSet[]
  created : Type
  fullname : String
  createTypeCalled : Boolean
  underlying_type : Type
  <>f__switch$map1E : Dictionary`2
      // Properties:
  Assembly : Assembly
  AssemblyQualifiedName : String
  BaseType : Type
  DeclaringType : Type
  UnderlyingSystemType : Type
  FullName : String
  GUID : Guid
  Module : Module
  Name : String
  Namespace : String
  PackingSize : PackingSize
  Size : Int32
  ReflectedType : Type
  TypeHandle : RuntimeTypeHandle
  TypeToken : TypeToken
  IsCompilerContext : Boolean
  is_created : Boolean
  ContainsGenericParameters : Boolean
  IsGenericParameter : Boolean
  GenericParameterAttributes : GenericParameterAttributes
  IsGenericTypeDefinition : Boolean
  IsGenericType : Boolean
  GenericParameterPosition : Int32
  DeclaringMethod : MethodBase
      // Events:
      // Methods:
      VoidReflection.Emit.TypeBuilder::.ctorReflection.Emit.ModuleBuilderReflection.TypeAttributesInt32)
      VoidReflection.Emit.TypeBuilder::.ctorReflection.Emit.ModuleBuilderStringReflection.TypeAttributesTypeType[]Reflection.Emit.PackingSizeInt32Type)
      VoidReflection.Emit.TypeBuilder::System.Runtime.InteropServices._TypeBuilder.GetIDsOfNamesGuid&IntPtrUInt32UInt32IntPtr)
      VoidReflection.Emit.TypeBuilder::System.Runtime.InteropServices._TypeBuilder.GetTypeInfoUInt32UInt32IntPtr)
      VoidReflection.Emit.TypeBuilder::System.Runtime.InteropServices._TypeBuilder.GetTypeInfoCountUInt32&)
      VoidReflection.Emit.TypeBuilder::System.Runtime.InteropServices._TypeBuilder.InvokeUInt32Guid&UInt32Int16IntPtrIntPtrIntPtrIntPtr)
      Reflection.TypeAttributesReflection.Emit.TypeBuilder::GetAttributeFlagsImpl()
      VoidReflection.Emit.TypeBuilder::setup_internal_classReflection.Emit.TypeBuilder)
      VoidReflection.Emit.TypeBuilder::create_internal_classReflection.Emit.TypeBuilder)
      VoidReflection.Emit.TypeBuilder::setup_generic_class()
      VoidReflection.Emit.TypeBuilder::create_generic_class()
      Reflection.EventInfoReflection.Emit.TypeBuilder::get_event_infoReflection.Emit.EventBuilder)
      public Reflection.AssemblyReflection.Emit.TypeBuilder::get_Assembly()
      public StringReflection.Emit.TypeBuilder::get_AssemblyQualifiedName()
      public TypeReflection.Emit.TypeBuilder::get_BaseType()
      public TypeReflection.Emit.TypeBuilder::get_DeclaringType()
      public TypeReflection.Emit.TypeBuilder::get_UnderlyingSystemType()
      StringReflection.Emit.TypeBuilder::GetFullName()
      public StringReflection.Emit.TypeBuilder::get_FullName()
      public GuidReflection.Emit.TypeBuilder::get_GUID()
      public Reflection.ModuleReflection.Emit.TypeBuilder::get_Module()
      public StringReflection.Emit.TypeBuilder::get_Name()
      public StringReflection.Emit.TypeBuilder::get_Namespace()
      public Reflection.Emit.PackingSizeReflection.Emit.TypeBuilder::get_PackingSize()
      public Int32Reflection.Emit.TypeBuilder::get_Size()
      public TypeReflection.Emit.TypeBuilder::get_ReflectedType()
      public VoidReflection.Emit.TypeBuilder::AddDeclarativeSecuritySecurity.Permissions.SecurityActionSecurity.PermissionSet)
      public VoidReflection.Emit.TypeBuilder::AddInterfaceImplementationType)
      Reflection.ConstructorInfoReflection.Emit.TypeBuilder::GetConstructorImplReflection.BindingFlagsReflection.BinderReflection.CallingConventionsType[]Reflection.ParameterModifier[])
      public BooleanReflection.Emit.TypeBuilder::IsDefinedTypeBoolean)
      public Object[]Reflection.Emit.TypeBuilder::GetCustomAttributesBoolean)
      public Object[]Reflection.Emit.TypeBuilder::GetCustomAttributesTypeBoolean)
      public Reflection.Emit.TypeBuilderReflection.Emit.TypeBuilder::DefineNestedTypeString)
      public Reflection.Emit.TypeBuilderReflection.Emit.TypeBuilder::DefineNestedTypeStringReflection.TypeAttributes)
      public Reflection.Emit.TypeBuilderReflection.Emit.TypeBuilder::DefineNestedTypeStringReflection.TypeAttributesType)
      Reflection.Emit.TypeBuilderReflection.Emit.TypeBuilder::DefineNestedTypeStringReflection.TypeAttributesTypeType[]Reflection.Emit.PackingSizeInt32)
      public Reflection.Emit.TypeBuilderReflection.Emit.TypeBuilder::DefineNestedTypeStringReflection.TypeAttributesTypeType[])
      public Reflection.Emit.TypeBuilderReflection.Emit.TypeBuilder::DefineNestedTypeStringReflection.TypeAttributesTypeInt32)
      public Reflection.Emit.TypeBuilderReflection.Emit.TypeBuilder::DefineNestedTypeStringReflection.TypeAttributesTypeReflection.Emit.PackingSize)
      public Reflection.Emit.ConstructorBuilderReflection.Emit.TypeBuilder::DefineConstructorReflection.MethodAttributesReflection.CallingConventionsType[])
      public Reflection.Emit.ConstructorBuilderReflection.Emit.TypeBuilder::DefineConstructorReflection.MethodAttributesReflection.CallingConventionsType[]Type[][]Type[][])
      public Reflection.Emit.ConstructorBuilderReflection.Emit.TypeBuilder::DefineDefaultConstructorReflection.MethodAttributes)
      VoidReflection.Emit.TypeBuilder::append_methodReflection.Emit.MethodBuilder)
      public Reflection.Emit.MethodBuilderReflection.Emit.TypeBuilder::DefineMethodStringReflection.MethodAttributesTypeType[])
      public Reflection.Emit.MethodBuilderReflection.Emit.TypeBuilder::DefineMethodStringReflection.MethodAttributesReflection.CallingConventionsTypeType[])
      public Reflection.Emit.MethodBuilderReflection.Emit.TypeBuilder::DefineMethodStringReflection.MethodAttributesReflection.CallingConventionsTypeType[]Type[]Type[]Type[][]Type[][])
      public Reflection.Emit.MethodBuilderReflection.Emit.TypeBuilder::DefinePInvokeMethodStringStringStringReflection.MethodAttributesReflection.CallingConventionsTypeType[]Runtime.InteropServices.CallingConventionRuntime.InteropServices.CharSet)
      public Reflection.Emit.MethodBuilderReflection.Emit.TypeBuilder::DefinePInvokeMethodStringStringStringReflection.MethodAttributesReflection.CallingConventionsTypeType[]Type[]Type[]Type[][]Type[][]Runtime.InteropServices.CallingConventionRuntime.InteropServices.CharSet)
      public Reflection.Emit.MethodBuilderReflection.Emit.TypeBuilder::DefinePInvokeMethodStringStringReflection.MethodAttributesReflection.CallingConventionsTypeType[]Runtime.InteropServices.CallingConventionRuntime.InteropServices.CharSet)
      public Reflection.Emit.MethodBuilderReflection.Emit.TypeBuilder::DefineMethodStringReflection.MethodAttributes)
      public Reflection.Emit.MethodBuilderReflection.Emit.TypeBuilder::DefineMethodStringReflection.MethodAttributesReflection.CallingConventions)
      public VoidReflection.Emit.TypeBuilder::DefineMethodOverrideReflection.MethodInfoReflection.MethodInfo)
      public Reflection.Emit.FieldBuilderReflection.Emit.TypeBuilder::DefineFieldStringTypeReflection.FieldAttributes)
      public Reflection.Emit.FieldBuilderReflection.Emit.TypeBuilder::DefineFieldStringTypeType[]Type[]Reflection.FieldAttributes)
      public Reflection.Emit.PropertyBuilderReflection.Emit.TypeBuilder::DefinePropertyStringReflection.PropertyAttributesTypeType[])
      public Reflection.Emit.PropertyBuilderReflection.Emit.TypeBuilder::DefinePropertyStringReflection.PropertyAttributesTypeType[]Type[]Type[]Type[][]Type[][])
      public Reflection.Emit.ConstructorBuilderReflection.Emit.TypeBuilder::DefineTypeInitializer()
      TypeReflection.Emit.TypeBuilder::create_runtime_classReflection.Emit.TypeBuilder)
      BooleanReflection.Emit.TypeBuilder::is_nested_inType)
      BooleanReflection.Emit.TypeBuilder::has_ctor_method()
      public TypeReflection.Emit.TypeBuilder::CreateType()
      VoidReflection.Emit.TypeBuilder::GenerateDebugInfoDiagnostics.SymbolStore.ISymbolWriter)
      public Reflection.ConstructorInfo[]Reflection.Emit.TypeBuilder::GetConstructorsReflection.BindingFlags)
      Reflection.ConstructorInfo[]Reflection.Emit.TypeBuilder::GetConstructorsInternalReflection.BindingFlags)
      public TypeReflection.Emit.TypeBuilder::GetElementType()
      public Reflection.EventInfoReflection.Emit.TypeBuilder::GetEventStringReflection.BindingFlags)
      public Reflection.EventInfo[]Reflection.Emit.TypeBuilder::GetEvents()
      public Reflection.EventInfo[]Reflection.Emit.TypeBuilder::GetEventsReflection.BindingFlags)
      Reflection.EventInfo[]Reflection.Emit.TypeBuilder::GetEvents_internalReflection.BindingFlags)
      public Reflection.FieldInfoReflection.Emit.TypeBuilder::GetFieldStringReflection.BindingFlags)
      public Reflection.FieldInfo[]Reflection.Emit.TypeBuilder::GetFieldsReflection.BindingFlags)
      public TypeReflection.Emit.TypeBuilder::GetInterfaceStringBoolean)
      public Type[]Reflection.Emit.TypeBuilder::GetInterfaces()
      public Reflection.MemberInfo[]Reflection.Emit.TypeBuilder::GetMemberStringReflection.MemberTypesReflection.BindingFlags)
      public Reflection.MemberInfo[]Reflection.Emit.TypeBuilder::GetMembersReflection.BindingFlags)
      Reflection.MethodInfo[]Reflection.Emit.TypeBuilder::GetMethodsByNameStringReflection.BindingFlagsBooleanType)
      public Reflection.MethodInfo[]Reflection.Emit.TypeBuilder::GetMethodsReflection.BindingFlags)
      Reflection.MethodInfoReflection.Emit.TypeBuilder::GetMethodImplStringReflection.BindingFlagsReflection.BinderReflection.CallingConventionsType[]Reflection.ParameterModifier[])
      public TypeReflection.Emit.TypeBuilder::GetNestedTypeStringReflection.BindingFlags)
      public Type[]Reflection.Emit.TypeBuilder::GetNestedTypesReflection.BindingFlags)
      public Reflection.PropertyInfo[]Reflection.Emit.TypeBuilder::GetPropertiesReflection.BindingFlags)
      Reflection.PropertyInfoReflection.Emit.TypeBuilder::GetPropertyImplStringReflection.BindingFlagsReflection.BinderTypeType[]Reflection.ParameterModifier[])
      BooleanReflection.Emit.TypeBuilder::HasElementTypeImpl()
      public ObjectReflection.Emit.TypeBuilder::InvokeMemberStringReflection.BindingFlagsReflection.BinderObjectObject[]Reflection.ParameterModifier[]Globalization.CultureInfoString[])
      BooleanReflection.Emit.TypeBuilder::IsArrayImpl()
      BooleanReflection.Emit.TypeBuilder::IsByRefImpl()
      BooleanReflection.Emit.TypeBuilder::IsCOMObjectImpl()
      BooleanReflection.Emit.TypeBuilder::IsPointerImpl()
      BooleanReflection.Emit.TypeBuilder::IsPrimitiveImpl()
      BooleanReflection.Emit.TypeBuilder::IsValueTypeImpl()
      public TypeReflection.Emit.TypeBuilder::MakeArrayType()
      public TypeReflection.Emit.TypeBuilder::MakeArrayTypeInt32)
      public TypeReflection.Emit.TypeBuilder::MakeByRefType()
      public TypeReflection.Emit.TypeBuilder::MakeGenericTypeType[])
      public TypeReflection.Emit.TypeBuilder::MakePointerType()
      public RuntimeTypeHandleReflection.Emit.TypeBuilder::get_TypeHandle()
      VoidReflection.Emit.TypeBuilder::SetCharSetReflection.TypeAttributes)
      public VoidReflection.Emit.TypeBuilder::SetCustomAttributeReflection.Emit.CustomAttributeBuilder)
      public VoidReflection.Emit.TypeBuilder::SetCustomAttributeReflection.ConstructorInfoByte[])
      public Reflection.Emit.EventBuilderReflection.Emit.TypeBuilder::DefineEventStringReflection.EventAttributesType)
      public Reflection.Emit.FieldBuilderReflection.Emit.TypeBuilder::DefineInitializedDataStringByte[]Reflection.FieldAttributes)
      public Reflection.Emit.FieldBuilderReflection.Emit.TypeBuilder::DefineUninitializedDataStringInt32Reflection.FieldAttributes)
      public Reflection.Emit.TypeTokenReflection.Emit.TypeBuilder::get_TypeToken()
      public VoidReflection.Emit.TypeBuilder::SetParentType)
      Int32Reflection.Emit.TypeBuilder::get_next_table_indexObjectInt32Boolean)
      public Reflection.InterfaceMappingReflection.Emit.TypeBuilder::GetInterfaceMapType)
      BooleanReflection.Emit.TypeBuilder::get_IsCompilerContext()
      BooleanReflection.Emit.TypeBuilder::get_is_created()
      ExceptionReflection.Emit.TypeBuilder::not_supported()
      VoidReflection.Emit.TypeBuilder::check_not_created()
      VoidReflection.Emit.TypeBuilder::check_created()
      VoidReflection.Emit.TypeBuilder::check_nameStringString)
      public StringReflection.Emit.TypeBuilder::ToString()
      public BooleanReflection.Emit.TypeBuilder::IsAssignableFromType)
      public BooleanReflection.Emit.TypeBuilder::IsSubclassOfType)
      BooleanReflection.Emit.TypeBuilder::IsAssignableToType)
      public BooleanReflection.Emit.TypeBuilder::IsCreated()
      public Type[]Reflection.Emit.TypeBuilder::GetGenericArguments()
      public TypeReflection.Emit.TypeBuilder::GetGenericTypeDefinition()
      public BooleanReflection.Emit.TypeBuilder::get_ContainsGenericParameters()
      public BooleanReflection.Emit.TypeBuilder::get_IsGenericParameter()
      public Reflection.GenericParameterAttributesReflection.Emit.TypeBuilder::get_GenericParameterAttributes()
      public BooleanReflection.Emit.TypeBuilder::get_IsGenericTypeDefinition()
      public BooleanReflection.Emit.TypeBuilder::get_IsGenericType()
      public Int32Reflection.Emit.TypeBuilder::get_GenericParameterPosition()
      public Reflection.MethodBaseReflection.Emit.TypeBuilder::get_DeclaringMethod()
      public Reflection.Emit.GenericTypeParameterBuilder[]Reflection.Emit.TypeBuilder::DefineGenericParametersString[])
      public Reflection.ConstructorInfoReflection.Emit.TypeBuilder::GetConstructorTypeReflection.ConstructorInfo)
      BooleanReflection.Emit.TypeBuilder::IsValidGetMethodTypeType)
      public Reflection.MethodInfoReflection.Emit.TypeBuilder::GetMethodTypeReflection.MethodInfo)
      public Reflection.FieldInfoReflection.Emit.TypeBuilder::GetFieldTypeReflection.FieldInfo)
    }
}
