// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Reflection.Emit
{
    public class FieldBuilder : FieldInfo
    {
      // Fields:
  attrs : FieldAttributes
  type : Type
  name : String
  def_value : Object
  offset : Int32
  table_idx : Int32
  typeb : TypeBuilder
  rva_data : Byte[]
  cattrs : CustomAttributeBuilder[]
  marshal_info : UnmanagedMarshal
  handle : RuntimeFieldHandle
  modReq : Type[]
  modOpt : Type[]
      // Properties:
  Attributes : FieldAttributes
  DeclaringType : Type
  FieldHandle : RuntimeFieldHandle
  FieldType : Type
  Name : String
  ReflectedType : Type
  UMarshal : UnmanagedMarshal
  Module : Module
      // Events:
      // Methods:
      VoidReflection.Emit.FieldBuilder::.ctorReflection.Emit.TypeBuilderStringTypeReflection.FieldAttributesType[]Type[])
      VoidReflection.Emit.FieldBuilder::System.Runtime.InteropServices._FieldBuilder.GetIDsOfNamesGuid&IntPtrUInt32UInt32IntPtr)
      VoidReflection.Emit.FieldBuilder::System.Runtime.InteropServices._FieldBuilder.GetTypeInfoUInt32UInt32IntPtr)
      VoidReflection.Emit.FieldBuilder::System.Runtime.InteropServices._FieldBuilder.GetTypeInfoCountUInt32&)
      VoidReflection.Emit.FieldBuilder::System.Runtime.InteropServices._FieldBuilder.InvokeUInt32Guid&UInt32Int16IntPtrIntPtrIntPtrIntPtr)
      public Reflection.FieldAttributesReflection.Emit.FieldBuilder::get_Attributes()
      public TypeReflection.Emit.FieldBuilder::get_DeclaringType()
      public RuntimeFieldHandleReflection.Emit.FieldBuilder::get_FieldHandle()
      public TypeReflection.Emit.FieldBuilder::get_FieldType()
      public StringReflection.Emit.FieldBuilder::get_Name()
      public TypeReflection.Emit.FieldBuilder::get_ReflectedType()
      public Object[]Reflection.Emit.FieldBuilder::GetCustomAttributesBoolean)
      public Object[]Reflection.Emit.FieldBuilder::GetCustomAttributesTypeBoolean)
      public Reflection.Emit.FieldTokenReflection.Emit.FieldBuilder::GetToken()
      public ObjectReflection.Emit.FieldBuilder::GetValueObject)
      public BooleanReflection.Emit.FieldBuilder::IsDefinedTypeBoolean)
      Int32Reflection.Emit.FieldBuilder::GetFieldOffset()
      VoidReflection.Emit.FieldBuilder::SetRVADataByte[])
      public VoidReflection.Emit.FieldBuilder::SetConstantObject)
      public VoidReflection.Emit.FieldBuilder::SetCustomAttributeReflection.Emit.CustomAttributeBuilder)
      public VoidReflection.Emit.FieldBuilder::SetCustomAttributeReflection.ConstructorInfoByte[])
      public VoidReflection.Emit.FieldBuilder::SetMarshalReflection.Emit.UnmanagedMarshal)
      public VoidReflection.Emit.FieldBuilder::SetOffsetInt32)
      public VoidReflection.Emit.FieldBuilder::SetValueObjectObjectReflection.BindingFlagsReflection.BinderGlobalization.CultureInfo)
      Reflection.Emit.UnmanagedMarshalReflection.Emit.FieldBuilder::get_UMarshal()
      ExceptionReflection.Emit.FieldBuilder::CreateNotSupportedException()
      VoidReflection.Emit.FieldBuilder::RejectIfCreated()
      public Reflection.ModuleReflection.Emit.FieldBuilder::get_Module()
    }
}
