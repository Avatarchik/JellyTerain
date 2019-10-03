// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Reflection.Emit
{
    public class DynamicMethod : MethodInfo
    {
      // Fields:
  mhandle : RuntimeMethodHandle
  name : String
  returnType : Type
  parameters : Type[]
  attributes : MethodAttributes
  callingConvention : CallingConventions
  module : Module
  skipVisibility : Boolean
  init_locals : Boolean
  ilgen : ILGenerator
  nrefs : Int32
  refs : Object[]
  referenced_by : IntPtr
  owner : Type
  deleg : Delegate
  method : MonoMethod
  pinfo : ParameterBuilder[]
  creating : Boolean
      // Properties:
  Attributes : MethodAttributes
  CallingConvention : CallingConventions
  DeclaringType : Type
  InitLocals : Boolean
  MethodHandle : RuntimeMethodHandle
  Module : Module
  Name : String
  ReflectedType : Type
  ReturnParameter : ParameterInfo
  ReturnType : Type
  ReturnTypeCustomAttributes : ICustomAttributeProvider
      // Events:
      // Methods:
      public VoidReflection.Emit.DynamicMethod::.ctorStringTypeType[]Reflection.Module)
      public VoidReflection.Emit.DynamicMethod::.ctorStringTypeType[]Type)
      public VoidReflection.Emit.DynamicMethod::.ctorStringTypeType[]Reflection.ModuleBoolean)
      public VoidReflection.Emit.DynamicMethod::.ctorStringTypeType[]TypeBoolean)
      public VoidReflection.Emit.DynamicMethod::.ctorStringReflection.MethodAttributesReflection.CallingConventionsTypeType[]TypeBoolean)
      public VoidReflection.Emit.DynamicMethod::.ctorStringReflection.MethodAttributesReflection.CallingConventionsTypeType[]Reflection.ModuleBoolean)
      public VoidReflection.Emit.DynamicMethod::.ctorStringTypeType[])
      public VoidReflection.Emit.DynamicMethod::.ctorStringTypeType[]Boolean)
      VoidReflection.Emit.DynamicMethod::.ctorStringReflection.MethodAttributesReflection.CallingConventionsTypeType[]TypeReflection.ModuleBooleanBoolean)
      VoidReflection.Emit.DynamicMethod::create_dynamic_methodReflection.Emit.DynamicMethod)
      VoidReflection.Emit.DynamicMethod::destroy_dynamic_methodReflection.Emit.DynamicMethod)
      VoidReflection.Emit.DynamicMethod::CreateDynMethod()
      VoidReflection.Emit.DynamicMethod::Finalize()
      public DelegateReflection.Emit.DynamicMethod::CreateDelegateType)
      public DelegateReflection.Emit.DynamicMethod::CreateDelegateTypeObject)
      public Reflection.Emit.ParameterBuilderReflection.Emit.DynamicMethod::DefineParameterInt32Reflection.ParameterAttributesString)
      public Reflection.MethodInfoReflection.Emit.DynamicMethod::GetBaseDefinition()
      public Object[]Reflection.Emit.DynamicMethod::GetCustomAttributesBoolean)
      public Object[]Reflection.Emit.DynamicMethod::GetCustomAttributesTypeBoolean)
      public Reflection.Emit.DynamicILInfoReflection.Emit.DynamicMethod::GetDynamicILInfo()
      public Reflection.Emit.ILGeneratorReflection.Emit.DynamicMethod::GetILGenerator()
      public Reflection.Emit.ILGeneratorReflection.Emit.DynamicMethod::GetILGeneratorInt32)
      public Reflection.MethodImplAttributesReflection.Emit.DynamicMethod::GetMethodImplementationFlags()
      public Reflection.ParameterInfo[]Reflection.Emit.DynamicMethod::GetParameters()
      public ObjectReflection.Emit.DynamicMethod::InvokeObjectReflection.BindingFlagsReflection.BinderObject[]Globalization.CultureInfo)
      public BooleanReflection.Emit.DynamicMethod::IsDefinedTypeBoolean)
      public StringReflection.Emit.DynamicMethod::ToString()
      public Reflection.MethodAttributesReflection.Emit.DynamicMethod::get_Attributes()
      public Reflection.CallingConventionsReflection.Emit.DynamicMethod::get_CallingConvention()
      public TypeReflection.Emit.DynamicMethod::get_DeclaringType()
      public BooleanReflection.Emit.DynamicMethod::get_InitLocals()
      public VoidReflection.Emit.DynamicMethod::set_InitLocalsBoolean)
      public RuntimeMethodHandleReflection.Emit.DynamicMethod::get_MethodHandle()
      public Reflection.ModuleReflection.Emit.DynamicMethod::get_Module()
      public StringReflection.Emit.DynamicMethod::get_Name()
      public TypeReflection.Emit.DynamicMethod::get_ReflectedType()
      public Reflection.ParameterInfoReflection.Emit.DynamicMethod::get_ReturnParameter()
      public TypeReflection.Emit.DynamicMethod::get_ReturnType()
      public Reflection.ICustomAttributeProviderReflection.Emit.DynamicMethod::get_ReturnTypeCustomAttributes()
      VoidReflection.Emit.DynamicMethod::RejectIfCreated()
      Int32Reflection.Emit.DynamicMethod::AddRefObject)
    }
}
