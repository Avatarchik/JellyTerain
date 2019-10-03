// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Reflection.Emit
{
    public class GenericTypeParameterBuilder : Type
    {
      // Fields:
  tbuilder : TypeBuilder
  mbuilder : MethodBuilder
  name : String
  index : Int32
  base_type : Type
  iface_constraints : Type[]
  cattrs : CustomAttributeBuilder[]
  attrs : GenericParameterAttributes
      // Properties:
  UnderlyingSystemType : Type
  Assembly : Assembly
  AssemblyQualifiedName : String
  BaseType : Type
  FullName : String
  GUID : Guid
  Name : String
  Namespace : String
  Module : Module
  DeclaringType : Type
  ReflectedType : Type
  TypeHandle : RuntimeTypeHandle
  ContainsGenericParameters : Boolean
  IsGenericParameter : Boolean
  IsGenericType : Boolean
  IsGenericTypeDefinition : Boolean
  GenericParameterAttributes : GenericParameterAttributes
  GenericParameterPosition : Int32
  DeclaringMethod : MethodBase
      // Events:
      // Methods:
      VoidReflection.Emit.GenericTypeParameterBuilder::.ctorReflection.Emit.TypeBuilderReflection.Emit.MethodBuilderStringInt32)
      public VoidReflection.Emit.GenericTypeParameterBuilder::SetBaseTypeConstraintType)
      public VoidReflection.Emit.GenericTypeParameterBuilder::SetInterfaceConstraintsType[])
      public VoidReflection.Emit.GenericTypeParameterBuilder::SetGenericParameterAttributesReflection.GenericParameterAttributes)
      VoidReflection.Emit.GenericTypeParameterBuilder::initialize()
      public BooleanReflection.Emit.GenericTypeParameterBuilder::IsSubclassOfType)
      Reflection.TypeAttributesReflection.Emit.GenericTypeParameterBuilder::GetAttributeFlagsImpl()
      Reflection.ConstructorInfoReflection.Emit.GenericTypeParameterBuilder::GetConstructorImplReflection.BindingFlagsReflection.BinderReflection.CallingConventionsType[]Reflection.ParameterModifier[])
      public Reflection.ConstructorInfo[]Reflection.Emit.GenericTypeParameterBuilder::GetConstructorsReflection.BindingFlags)
      public Reflection.EventInfoReflection.Emit.GenericTypeParameterBuilder::GetEventStringReflection.BindingFlags)
      public Reflection.EventInfo[]Reflection.Emit.GenericTypeParameterBuilder::GetEvents()
      public Reflection.EventInfo[]Reflection.Emit.GenericTypeParameterBuilder::GetEventsReflection.BindingFlags)
      public Reflection.FieldInfoReflection.Emit.GenericTypeParameterBuilder::GetFieldStringReflection.BindingFlags)
      public Reflection.FieldInfo[]Reflection.Emit.GenericTypeParameterBuilder::GetFieldsReflection.BindingFlags)
      public TypeReflection.Emit.GenericTypeParameterBuilder::GetInterfaceStringBoolean)
      public Type[]Reflection.Emit.GenericTypeParameterBuilder::GetInterfaces()
      public Reflection.MemberInfo[]Reflection.Emit.GenericTypeParameterBuilder::GetMembersReflection.BindingFlags)
      public Reflection.MemberInfo[]Reflection.Emit.GenericTypeParameterBuilder::GetMemberStringReflection.MemberTypesReflection.BindingFlags)
      public Reflection.MethodInfo[]Reflection.Emit.GenericTypeParameterBuilder::GetMethodsReflection.BindingFlags)
      Reflection.MethodInfoReflection.Emit.GenericTypeParameterBuilder::GetMethodImplStringReflection.BindingFlagsReflection.BinderReflection.CallingConventionsType[]Reflection.ParameterModifier[])
      public TypeReflection.Emit.GenericTypeParameterBuilder::GetNestedTypeStringReflection.BindingFlags)
      public Type[]Reflection.Emit.GenericTypeParameterBuilder::GetNestedTypesReflection.BindingFlags)
      public Reflection.PropertyInfo[]Reflection.Emit.GenericTypeParameterBuilder::GetPropertiesReflection.BindingFlags)
      Reflection.PropertyInfoReflection.Emit.GenericTypeParameterBuilder::GetPropertyImplStringReflection.BindingFlagsReflection.BinderTypeType[]Reflection.ParameterModifier[])
      BooleanReflection.Emit.GenericTypeParameterBuilder::HasElementTypeImpl()
      public BooleanReflection.Emit.GenericTypeParameterBuilder::IsAssignableFromType)
      public BooleanReflection.Emit.GenericTypeParameterBuilder::IsInstanceOfTypeObject)
      BooleanReflection.Emit.GenericTypeParameterBuilder::IsArrayImpl()
      BooleanReflection.Emit.GenericTypeParameterBuilder::IsByRefImpl()
      BooleanReflection.Emit.GenericTypeParameterBuilder::IsCOMObjectImpl()
      BooleanReflection.Emit.GenericTypeParameterBuilder::IsPointerImpl()
      BooleanReflection.Emit.GenericTypeParameterBuilder::IsPrimitiveImpl()
      BooleanReflection.Emit.GenericTypeParameterBuilder::IsValueTypeImpl()
      public ObjectReflection.Emit.GenericTypeParameterBuilder::InvokeMemberStringReflection.BindingFlagsReflection.BinderObjectObject[]Reflection.ParameterModifier[]Globalization.CultureInfoString[])
      public TypeReflection.Emit.GenericTypeParameterBuilder::GetElementType()
      public TypeReflection.Emit.GenericTypeParameterBuilder::get_UnderlyingSystemType()
      public Reflection.AssemblyReflection.Emit.GenericTypeParameterBuilder::get_Assembly()
      public StringReflection.Emit.GenericTypeParameterBuilder::get_AssemblyQualifiedName()
      public TypeReflection.Emit.GenericTypeParameterBuilder::get_BaseType()
      public StringReflection.Emit.GenericTypeParameterBuilder::get_FullName()
      public GuidReflection.Emit.GenericTypeParameterBuilder::get_GUID()
      public BooleanReflection.Emit.GenericTypeParameterBuilder::IsDefinedTypeBoolean)
      public Object[]Reflection.Emit.GenericTypeParameterBuilder::GetCustomAttributesBoolean)
      public Object[]Reflection.Emit.GenericTypeParameterBuilder::GetCustomAttributesTypeBoolean)
      public Reflection.InterfaceMappingReflection.Emit.GenericTypeParameterBuilder::GetInterfaceMapType)
      public StringReflection.Emit.GenericTypeParameterBuilder::get_Name()
      public StringReflection.Emit.GenericTypeParameterBuilder::get_Namespace()
      public Reflection.ModuleReflection.Emit.GenericTypeParameterBuilder::get_Module()
      public TypeReflection.Emit.GenericTypeParameterBuilder::get_DeclaringType()
      public TypeReflection.Emit.GenericTypeParameterBuilder::get_ReflectedType()
      public RuntimeTypeHandleReflection.Emit.GenericTypeParameterBuilder::get_TypeHandle()
      public Type[]Reflection.Emit.GenericTypeParameterBuilder::GetGenericArguments()
      public TypeReflection.Emit.GenericTypeParameterBuilder::GetGenericTypeDefinition()
      public BooleanReflection.Emit.GenericTypeParameterBuilder::get_ContainsGenericParameters()
      public BooleanReflection.Emit.GenericTypeParameterBuilder::get_IsGenericParameter()
      public BooleanReflection.Emit.GenericTypeParameterBuilder::get_IsGenericType()
      public BooleanReflection.Emit.GenericTypeParameterBuilder::get_IsGenericTypeDefinition()
      public Reflection.GenericParameterAttributesReflection.Emit.GenericTypeParameterBuilder::get_GenericParameterAttributes()
      public Int32Reflection.Emit.GenericTypeParameterBuilder::get_GenericParameterPosition()
      public Type[]Reflection.Emit.GenericTypeParameterBuilder::GetGenericParameterConstraints()
      public Reflection.MethodBaseReflection.Emit.GenericTypeParameterBuilder::get_DeclaringMethod()
      public VoidReflection.Emit.GenericTypeParameterBuilder::SetCustomAttributeReflection.Emit.CustomAttributeBuilder)
      public VoidReflection.Emit.GenericTypeParameterBuilder::SetCustomAttributeReflection.ConstructorInfoByte[])
      ExceptionReflection.Emit.GenericTypeParameterBuilder::not_supported()
      public StringReflection.Emit.GenericTypeParameterBuilder::ToString()
      public BooleanReflection.Emit.GenericTypeParameterBuilder::EqualsObject)
      public Int32Reflection.Emit.GenericTypeParameterBuilder::GetHashCode()
      public TypeReflection.Emit.GenericTypeParameterBuilder::MakeArrayType()
      public TypeReflection.Emit.GenericTypeParameterBuilder::MakeArrayTypeInt32)
      public TypeReflection.Emit.GenericTypeParameterBuilder::MakeByRefType()
      public TypeReflection.Emit.GenericTypeParameterBuilder::MakeGenericTypeType[])
      public TypeReflection.Emit.GenericTypeParameterBuilder::MakePointerType()
    }
}
