// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Reflection.Emit
{
    public class PropertyBuilder : PropertyInfo
    {
      // Fields:
  attrs : PropertyAttributes
  name : String
  type : Type
  parameters : Type[]
  cattrs : CustomAttributeBuilder[]
  def_value : Object
  set_method : MethodBuilder
  get_method : MethodBuilder
  table_idx : Int32
  typeb : TypeBuilder
  returnModReq : Type[]
  returnModOpt : Type[]
  paramModReq : Type[][]
  paramModOpt : Type[][]
      // Properties:
  Attributes : PropertyAttributes
  CanRead : Boolean
  CanWrite : Boolean
  DeclaringType : Type
  Name : String
  PropertyToken : PropertyToken
  PropertyType : Type
  ReflectedType : Type
  Module : Module
      // Events:
      // Methods:
      VoidReflection.Emit.PropertyBuilder::.ctorReflection.Emit.TypeBuilderStringReflection.PropertyAttributesTypeType[]Type[]Type[]Type[][]Type[][])
      VoidReflection.Emit.PropertyBuilder::System.Runtime.InteropServices._PropertyBuilder.GetIDsOfNamesGuid&IntPtrUInt32UInt32IntPtr)
      VoidReflection.Emit.PropertyBuilder::System.Runtime.InteropServices._PropertyBuilder.GetTypeInfoUInt32UInt32IntPtr)
      VoidReflection.Emit.PropertyBuilder::System.Runtime.InteropServices._PropertyBuilder.GetTypeInfoCountUInt32&)
      VoidReflection.Emit.PropertyBuilder::System.Runtime.InteropServices._PropertyBuilder.InvokeUInt32Guid&UInt32Int16IntPtrIntPtrIntPtrIntPtr)
      public Reflection.PropertyAttributesReflection.Emit.PropertyBuilder::get_Attributes()
      public BooleanReflection.Emit.PropertyBuilder::get_CanRead()
      public BooleanReflection.Emit.PropertyBuilder::get_CanWrite()
      public TypeReflection.Emit.PropertyBuilder::get_DeclaringType()
      public StringReflection.Emit.PropertyBuilder::get_Name()
      public Reflection.Emit.PropertyTokenReflection.Emit.PropertyBuilder::get_PropertyToken()
      public TypeReflection.Emit.PropertyBuilder::get_PropertyType()
      public TypeReflection.Emit.PropertyBuilder::get_ReflectedType()
      public VoidReflection.Emit.PropertyBuilder::AddOtherMethodReflection.Emit.MethodBuilder)
      public Reflection.MethodInfo[]Reflection.Emit.PropertyBuilder::GetAccessorsBoolean)
      public Object[]Reflection.Emit.PropertyBuilder::GetCustomAttributesBoolean)
      public Object[]Reflection.Emit.PropertyBuilder::GetCustomAttributesTypeBoolean)
      public Reflection.MethodInfoReflection.Emit.PropertyBuilder::GetGetMethodBoolean)
      public Reflection.ParameterInfo[]Reflection.Emit.PropertyBuilder::GetIndexParameters()
      public Reflection.MethodInfoReflection.Emit.PropertyBuilder::GetSetMethodBoolean)
      public ObjectReflection.Emit.PropertyBuilder::GetValueObjectObject[])
      public ObjectReflection.Emit.PropertyBuilder::GetValueObjectReflection.BindingFlagsReflection.BinderObject[]Globalization.CultureInfo)
      public BooleanReflection.Emit.PropertyBuilder::IsDefinedTypeBoolean)
      public VoidReflection.Emit.PropertyBuilder::SetConstantObject)
      public VoidReflection.Emit.PropertyBuilder::SetCustomAttributeReflection.Emit.CustomAttributeBuilder)
      public VoidReflection.Emit.PropertyBuilder::SetCustomAttributeReflection.ConstructorInfoByte[])
      public VoidReflection.Emit.PropertyBuilder::SetGetMethodReflection.Emit.MethodBuilder)
      public VoidReflection.Emit.PropertyBuilder::SetSetMethodReflection.Emit.MethodBuilder)
      public VoidReflection.Emit.PropertyBuilder::SetValueObjectObjectObject[])
      public VoidReflection.Emit.PropertyBuilder::SetValueObjectObjectReflection.BindingFlagsReflection.BinderObject[]Globalization.CultureInfo)
      public Reflection.ModuleReflection.Emit.PropertyBuilder::get_Module()
      ExceptionReflection.Emit.PropertyBuilder::not_supported()
    }
}
