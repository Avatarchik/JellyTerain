// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System
{
    public class Type : MemberInfo
    {
      // Fields:
  DefaultBindingFlags : BindingFlags
  _impl : RuntimeTypeHandle
  public Delimiter : Char
  public EmptyTypes : Type[]
  public FilterAttribute : MemberFilter
  public FilterName : MemberFilter
  public FilterNameIgnoreCase : MemberFilter
  public Missing : Object
      // Properties:
  Assembly : Assembly
  AssemblyQualifiedName : String
  Attributes : TypeAttributes
  BaseType : Type
  DeclaringType : Type
  DefaultBinder : Binder
  FullName : String
  GUID : Guid
  HasElementType : Boolean
  IsAbstract : Boolean
  IsAnsiClass : Boolean
  IsArray : Boolean
  IsAutoClass : Boolean
  IsAutoLayout : Boolean
  IsByRef : Boolean
  IsClass : Boolean
  IsCOMObject : Boolean
  IsContextful : Boolean
  IsEnum : Boolean
  IsExplicitLayout : Boolean
  IsImport : Boolean
  IsInterface : Boolean
  IsLayoutSequential : Boolean
  IsMarshalByRef : Boolean
  IsNestedAssembly : Boolean
  IsNestedFamANDAssem : Boolean
  IsNestedFamily : Boolean
  IsNestedFamORAssem : Boolean
  IsNestedPrivate : Boolean
  IsNestedPublic : Boolean
  IsNotPublic : Boolean
  IsPointer : Boolean
  IsPrimitive : Boolean
  IsPublic : Boolean
  IsSealed : Boolean
  IsSerializable : Boolean
  IsSpecialName : Boolean
  IsUnicodeClass : Boolean
  IsValueType : Boolean
  MemberType : MemberTypes
  Module : Module
  Namespace : String
  ReflectedType : Type
  TypeHandle : RuntimeTypeHandle
  TypeInitializer : ConstructorInfo
  UnderlyingSystemType : Type
  IsSystemType : Boolean
  ContainsGenericParameters : Boolean
  IsGenericTypeDefinition : Boolean
  IsGenericType : Boolean
  IsGenericParameter : Boolean
  IsNested : Boolean
  IsVisible : Boolean
  GenericParameterPosition : Int32
  GenericParameterAttributes : GenericParameterAttributes
  DeclaringMethod : MethodBase
  StructLayoutAttribute : StructLayoutAttribute
  IsUserType : Boolean
      // Events:
      // Methods:
      VoidType::.ctor()
      VoidType::.cctor()
      VoidType::System.Runtime.InteropServices._Type.GetIDsOfNamesGuid&IntPtrUInt32UInt32IntPtr)
      VoidType::System.Runtime.InteropServices._Type.GetTypeInfoUInt32UInt32IntPtr)
      VoidType::System.Runtime.InteropServices._Type.GetTypeInfoCountUInt32&)
      VoidType::System.Runtime.InteropServices._Type.InvokeUInt32Guid&UInt32Int16IntPtrIntPtrIntPtrIntPtr)
      BooleanType::FilterName_implReflection.MemberInfoObject)
      BooleanType::FilterNameIgnoreCase_implReflection.MemberInfoObject)
      BooleanType::FilterAttribute_implReflection.MemberInfoObject)
      public Reflection.AssemblyType::get_Assembly()
      public StringType::get_AssemblyQualifiedName()
      public Reflection.TypeAttributesType::get_Attributes()
      public TypeType::get_BaseType()
      public TypeType::get_DeclaringType()
      public Reflection.BinderType::get_DefaultBinder()
      public StringType::get_FullName()
      public GuidType::get_GUID()
      public BooleanType::get_HasElementType()
      public BooleanType::get_IsAbstract()
      public BooleanType::get_IsAnsiClass()
      public BooleanType::get_IsArray()
      public BooleanType::get_IsAutoClass()
      public BooleanType::get_IsAutoLayout()
      public BooleanType::get_IsByRef()
      public BooleanType::get_IsClass()
      public BooleanType::get_IsCOMObject()
      public BooleanType::get_IsContextful()
      public BooleanType::get_IsEnum()
      public BooleanType::get_IsExplicitLayout()
      public BooleanType::get_IsImport()
      public BooleanType::get_IsInterface()
      public BooleanType::get_IsLayoutSequential()
      public BooleanType::get_IsMarshalByRef()
      public BooleanType::get_IsNestedAssembly()
      public BooleanType::get_IsNestedFamANDAssem()
      public BooleanType::get_IsNestedFamily()
      public BooleanType::get_IsNestedFamORAssem()
      public BooleanType::get_IsNestedPrivate()
      public BooleanType::get_IsNestedPublic()
      public BooleanType::get_IsNotPublic()
      public BooleanType::get_IsPointer()
      public BooleanType::get_IsPrimitive()
      public BooleanType::get_IsPublic()
      public BooleanType::get_IsSealed()
      public BooleanType::get_IsSerializable()
      public BooleanType::get_IsSpecialName()
      public BooleanType::get_IsUnicodeClass()
      public BooleanType::get_IsValueType()
      public Reflection.MemberTypesType::get_MemberType()
      public Reflection.ModuleType::get_Module()
      public StringType::get_Namespace()
      public TypeType::get_ReflectedType()
      public RuntimeTypeHandleType::get_TypeHandle()
      public Reflection.ConstructorInfoType::get_TypeInitializer()
      public TypeType::get_UnderlyingSystemType()
      public BooleanType::EqualsObject)
      public BooleanType::EqualsType)
      BooleanType::EqualsInternalType)
      TypeType::internal_from_handleIntPtr)
      TypeType::internal_from_nameStringBooleanBoolean)
      public TypeType::GetTypeString)
      public TypeType::GetTypeStringBoolean)
      public TypeType::GetTypeStringBooleanBoolean)
      public Type[]Type::GetTypeArrayObject[])
      TypeCodeType::GetTypeCodeInternalType)
      public TypeCodeType::GetTypeCodeType)
      public TypeType::GetTypeFromCLSIDGuid)
      public TypeType::GetTypeFromCLSIDGuidBoolean)
      public TypeType::GetTypeFromCLSIDGuidString)
      public TypeType::GetTypeFromCLSIDGuidStringBoolean)
      public TypeType::GetTypeFromHandleRuntimeTypeHandle)
      public TypeType::GetTypeFromProgIDString)
      public TypeType::GetTypeFromProgIDStringBoolean)
      public TypeType::GetTypeFromProgIDStringString)
      public TypeType::GetTypeFromProgIDStringStringBoolean)
      public RuntimeTypeHandleType::GetTypeHandleObject)
      BooleanType::type_is_subtype_ofTypeTypeBoolean)
      BooleanType::type_is_assignable_fromTypeType)
      public TypeType::GetType()
      public BooleanType::IsSubclassOfType)
      public Type[]Type::FindInterfacesReflection.TypeFilterObject)
      public TypeType::GetInterfaceString)
      public TypeType::GetInterfaceStringBoolean)
      VoidType::GetInterfaceMapDataTypeTypeReflection.MethodInfo[]&Reflection.MethodInfo[]&)
      public Reflection.InterfaceMappingType::GetInterfaceMapType)
      public Type[]Type::GetInterfaces()
      public BooleanType::IsAssignableFromType)
      public BooleanType::IsInstanceOfTypeObject)
      public Int32Type::GetArrayRank()
      public TypeType::GetElementType()
      public Reflection.EventInfoType::GetEventString)
      public Reflection.EventInfoType::GetEventStringReflection.BindingFlags)
      public Reflection.EventInfo[]Type::GetEvents()
      public Reflection.EventInfo[]Type::GetEventsReflection.BindingFlags)
      public Reflection.FieldInfoType::GetFieldString)
      public Reflection.FieldInfoType::GetFieldStringReflection.BindingFlags)
      public Reflection.FieldInfo[]Type::GetFields()
      public Reflection.FieldInfo[]Type::GetFieldsReflection.BindingFlags)
      public Int32Type::GetHashCode()
      public Reflection.MemberInfo[]Type::GetMemberString)
      public Reflection.MemberInfo[]Type::GetMemberStringReflection.BindingFlags)
      public Reflection.MemberInfo[]Type::GetMemberStringReflection.MemberTypesReflection.BindingFlags)
      public Reflection.MemberInfo[]Type::GetMembers()
      public Reflection.MemberInfo[]Type::GetMembersReflection.BindingFlags)
      public Reflection.MethodInfoType::GetMethodString)
      public Reflection.MethodInfoType::GetMethodStringReflection.BindingFlags)
      public Reflection.MethodInfoType::GetMethodStringType[])
      public Reflection.MethodInfoType::GetMethodStringType[]Reflection.ParameterModifier[])
      public Reflection.MethodInfoType::GetMethodStringReflection.BindingFlagsReflection.BinderType[]Reflection.ParameterModifier[])
      public Reflection.MethodInfoType::GetMethodStringReflection.BindingFlagsReflection.BinderReflection.CallingConventionsType[]Reflection.ParameterModifier[])
      Reflection.MethodInfoType::GetMethodImplStringReflection.BindingFlagsReflection.BinderReflection.CallingConventionsType[]Reflection.ParameterModifier[])
      Reflection.MethodInfoType::GetMethodImplInternalStringReflection.BindingFlagsReflection.BinderReflection.CallingConventionsType[]Reflection.ParameterModifier[])
      Reflection.MethodInfoType::GetMethodReflection.MethodInfo)
      Reflection.ConstructorInfoType::GetConstructorReflection.ConstructorInfo)
      Reflection.FieldInfoType::GetFieldReflection.FieldInfo)
      public Reflection.MethodInfo[]Type::GetMethods()
      public Reflection.MethodInfo[]Type::GetMethodsReflection.BindingFlags)
      public TypeType::GetNestedTypeString)
      public TypeType::GetNestedTypeStringReflection.BindingFlags)
      public Type[]Type::GetNestedTypes()
      public Type[]Type::GetNestedTypesReflection.BindingFlags)
      public Reflection.PropertyInfo[]Type::GetProperties()
      public Reflection.PropertyInfo[]Type::GetPropertiesReflection.BindingFlags)
      public Reflection.PropertyInfoType::GetPropertyString)
      public Reflection.PropertyInfoType::GetPropertyStringReflection.BindingFlags)
      public Reflection.PropertyInfoType::GetPropertyStringType)
      public Reflection.PropertyInfoType::GetPropertyStringType[])
      public Reflection.PropertyInfoType::GetPropertyStringTypeType[])
      public Reflection.PropertyInfoType::GetPropertyStringTypeType[]Reflection.ParameterModifier[])
      public Reflection.PropertyInfoType::GetPropertyStringReflection.BindingFlagsReflection.BinderTypeType[]Reflection.ParameterModifier[])
      Reflection.PropertyInfoType::GetPropertyImplStringReflection.BindingFlagsReflection.BinderTypeType[]Reflection.ParameterModifier[])
      Reflection.PropertyInfoType::GetPropertyImplInternalStringReflection.BindingFlagsReflection.BinderTypeType[]Reflection.ParameterModifier[])
      Reflection.ConstructorInfoType::GetConstructorImplReflection.BindingFlagsReflection.BinderReflection.CallingConventionsType[]Reflection.ParameterModifier[])
      Reflection.TypeAttributesType::GetAttributeFlagsImpl()
      BooleanType::HasElementTypeImpl()
      BooleanType::IsArrayImpl()
      BooleanType::IsByRefImpl()
      BooleanType::IsCOMObjectImpl()
      BooleanType::IsPointerImpl()
      BooleanType::IsPrimitiveImpl()
      BooleanType::IsArrayImplType)
      BooleanType::IsValueTypeImpl()
      BooleanType::IsContextfulImpl()
      BooleanType::IsMarshalByRefImpl()
      public Reflection.ConstructorInfoType::GetConstructorType[])
      public Reflection.ConstructorInfoType::GetConstructorReflection.BindingFlagsReflection.BinderType[]Reflection.ParameterModifier[])
      public Reflection.ConstructorInfoType::GetConstructorReflection.BindingFlagsReflection.BinderReflection.CallingConventionsType[]Reflection.ParameterModifier[])
      public Reflection.ConstructorInfo[]Type::GetConstructors()
      public Reflection.ConstructorInfo[]Type::GetConstructorsReflection.BindingFlags)
      public Reflection.MemberInfo[]Type::GetDefaultMembers()
      public Reflection.MemberInfo[]Type::FindMembersReflection.MemberTypesReflection.BindingFlagsReflection.MemberFilterObject)
      public ObjectType::InvokeMemberStringReflection.BindingFlagsReflection.BinderObjectObject[])
      public ObjectType::InvokeMemberStringReflection.BindingFlagsReflection.BinderObjectObject[]Globalization.CultureInfo)
      public ObjectType::InvokeMemberStringReflection.BindingFlagsReflection.BinderObjectObject[]Reflection.ParameterModifier[]Globalization.CultureInfoString[])
      public StringType::ToString()
      BooleanType::get_IsSystemType()
      public Type[]Type::GetGenericArguments()
      public BooleanType::get_ContainsGenericParameters()
      public BooleanType::get_IsGenericTypeDefinition()
      TypeType::GetGenericTypeDefinition_impl()
      public TypeType::GetGenericTypeDefinition()
      public BooleanType::get_IsGenericType()
      TypeType::MakeGenericTypeTypeType[])
      public TypeType::MakeGenericTypeType[])
      public BooleanType::get_IsGenericParameter()
      public BooleanType::get_IsNested()
      public BooleanType::get_IsVisible()
      Int32Type::GetGenericParameterPosition()
      public Int32Type::get_GenericParameterPosition()
      Reflection.GenericParameterAttributesType::GetGenericParameterAttributes()
      public Reflection.GenericParameterAttributesType::get_GenericParameterAttributes()
      Type[]Type::GetGenericParameterConstraints_impl()
      public Type[]Type::GetGenericParameterConstraints()
      public Reflection.MethodBaseType::get_DeclaringMethod()
      TypeType::make_array_typeInt32)
      public TypeType::MakeArrayType()
      public TypeType::MakeArrayTypeInt32)
      TypeType::make_byref_type()
      public TypeType::MakeByRefType()
      public TypeType::MakePointerType()
      public TypeType::ReflectionOnlyGetTypeStringBooleanBoolean)
      VoidType::GetPackingInt32&Int32&)
      public Runtime.InteropServices.StructLayoutAttributeType::get_StructLayoutAttribute()
      Object[]Type::GetPseudoCustomAttributes()
      BooleanType::get_IsUserType()
    }
}
