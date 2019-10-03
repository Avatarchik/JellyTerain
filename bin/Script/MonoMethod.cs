// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Reflection
{
    class MonoMethod : MethodInfo
    {
      // Fields:
  mhandle : IntPtr
  name : String
  reftype : Type
      // Properties:
  ReturnParameter : ParameterInfo
  ReturnType : Type
  ReturnTypeCustomAttributes : ICustomAttributeProvider
  MethodHandle : RuntimeMethodHandle
  Attributes : MethodAttributes
  CallingConvention : CallingConventions
  ReflectedType : Type
  DeclaringType : Type
  Name : String
  IsGenericMethodDefinition : Boolean
  IsGenericMethod : Boolean
  ContainsGenericParameters : Boolean
      // Events:
      // Methods:
      VoidReflection.MonoMethod::.ctor()
      VoidReflection.MonoMethod::.ctorRuntimeMethodHandle)
      StringReflection.MonoMethod::get_nameReflection.MethodBase)
      Reflection.MonoMethodReflection.MonoMethod::get_base_definitionReflection.MonoMethod)
      public Reflection.MethodInfoReflection.MonoMethod::GetBaseDefinition()
      public Reflection.ParameterInfoReflection.MonoMethod::get_ReturnParameter()
      public TypeReflection.MonoMethod::get_ReturnType()
      public Reflection.ICustomAttributeProviderReflection.MonoMethod::get_ReturnTypeCustomAttributes()
      public Reflection.MethodImplAttributesReflection.MonoMethod::GetMethodImplementationFlags()
      public Reflection.ParameterInfo[]Reflection.MonoMethod::GetParameters()
      ObjectReflection.MonoMethod::InternalInvokeObjectObject[]Exception&)
      public ObjectReflection.MonoMethod::InvokeObjectReflection.BindingFlagsReflection.BinderObject[]Globalization.CultureInfo)
      public RuntimeMethodHandleReflection.MonoMethod::get_MethodHandle()
      public Reflection.MethodAttributesReflection.MonoMethod::get_Attributes()
      public Reflection.CallingConventionsReflection.MonoMethod::get_CallingConvention()
      public TypeReflection.MonoMethod::get_ReflectedType()
      public TypeReflection.MonoMethod::get_DeclaringType()
      public StringReflection.MonoMethod::get_Name()
      public BooleanReflection.MonoMethod::IsDefinedTypeBoolean)
      public Object[]Reflection.MonoMethod::GetCustomAttributesBoolean)
      public Object[]Reflection.MonoMethod::GetCustomAttributesTypeBoolean)
      Runtime.InteropServices.DllImportAttributeReflection.MonoMethod::GetDllImportAttributeIntPtr)
      Object[]Reflection.MonoMethod::GetPseudoCustomAttributes()
      BooleanReflection.MonoMethod::ShouldPrintFullNameType)
      public StringReflection.MonoMethod::ToString()
      public VoidReflection.MonoMethod::GetObjectDataRuntime.Serialization.SerializationInfoRuntime.Serialization.StreamingContext)
      public Reflection.MethodInfoReflection.MonoMethod::MakeGenericMethodType[])
      Reflection.MethodInfoReflection.MonoMethod::MakeGenericMethod_implType[])
      public Type[]Reflection.MonoMethod::GetGenericArguments()
      Reflection.MethodInfoReflection.MonoMethod::GetGenericMethodDefinition_impl()
      public Reflection.MethodInfoReflection.MonoMethod::GetGenericMethodDefinition()
      public BooleanReflection.MonoMethod::get_IsGenericMethodDefinition()
      public BooleanReflection.MonoMethod::get_IsGenericMethod()
      public BooleanReflection.MonoMethod::get_ContainsGenericParameters()
      public Reflection.MethodBodyReflection.MonoMethod::GetMethodBody()
    }
}
