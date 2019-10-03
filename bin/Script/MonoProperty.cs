// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Reflection
{
    class MonoProperty : PropertyInfo
    {
      // Fields:
  klass : IntPtr
  prop : IntPtr
  info : MonoPropertyInfo
  cached : PInfo
  cached_getter : GetterAdapter
      // Properties:
  Attributes : PropertyAttributes
  CanRead : Boolean
  CanWrite : Boolean
  PropertyType : Type
  ReflectedType : Type
  DeclaringType : Type
  Name : String
      // Events:
      // Methods:
      public VoidReflection.MonoProperty::.ctor()
      VoidReflection.MonoProperty::CachePropertyInfoReflection.PInfo)
      public Reflection.PropertyAttributesReflection.MonoProperty::get_Attributes()
      public BooleanReflection.MonoProperty::get_CanRead()
      public BooleanReflection.MonoProperty::get_CanWrite()
      public TypeReflection.MonoProperty::get_PropertyType()
      public TypeReflection.MonoProperty::get_ReflectedType()
      public TypeReflection.MonoProperty::get_DeclaringType()
      public StringReflection.MonoProperty::get_Name()
      public Reflection.MethodInfo[]Reflection.MonoProperty::GetAccessorsBoolean)
      public Reflection.MethodInfoReflection.MonoProperty::GetGetMethodBoolean)
      public Reflection.ParameterInfo[]Reflection.MonoProperty::GetIndexParameters()
      public Reflection.MethodInfoReflection.MonoProperty::GetSetMethodBoolean)
      public BooleanReflection.MonoProperty::IsDefinedTypeBoolean)
      public Object[]Reflection.MonoProperty::GetCustomAttributesBoolean)
      public Object[]Reflection.MonoProperty::GetCustomAttributesTypeBoolean)
      ObjectReflection.MonoProperty::GetterAdapterFrameReflection.MonoProperty/Getter`2<T,R>Object)
      ObjectReflection.MonoProperty::StaticGetterAdapterFrameReflection.MonoProperty/StaticGetter`1<R>Object)
      Reflection.MonoProperty/GetterAdapterReflection.MonoProperty::CreateGetterDelegateReflection.MethodInfo)
      public ObjectReflection.MonoProperty::GetValueObjectObject[])
      public ObjectReflection.MonoProperty::GetValueObjectReflection.BindingFlagsReflection.BinderObject[]Globalization.CultureInfo)
      public VoidReflection.MonoProperty::SetValueObjectObjectReflection.BindingFlagsReflection.BinderObject[]Globalization.CultureInfo)
      public StringReflection.MonoProperty::ToString()
      public Type[]Reflection.MonoProperty::GetOptionalCustomModifiers()
      public Type[]Reflection.MonoProperty::GetRequiredCustomModifiers()
      public VoidReflection.MonoProperty::GetObjectDataRuntime.Serialization.SerializationInfoRuntime.Serialization.StreamingContext)
    }
}
