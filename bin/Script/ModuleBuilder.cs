// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Reflection.Emit
{
    public class ModuleBuilder : Module
    {
      // Fields:
  dynamic_image : UIntPtr
  num_types : Int32
  types : TypeBuilder[]
  cattrs : CustomAttributeBuilder[]
  guid : Byte[]
  table_idx : Int32
  assemblyb : AssemblyBuilder
  global_methods : MethodBuilder[]
  global_fields : FieldBuilder[]
  is_main : Boolean
  resources : MonoResource[]
  global_type : TypeBuilder
  global_type_created : Type
  name_cache : Hashtable
  us_string_cache : Hashtable
  table_indexes : Int32[]
  transient : Boolean
  token_gen : ModuleBuilderTokenGenerator
  resource_writers : Hashtable
  symbolWriter : ISymbolWriter
  type_modifiers : Char[]
      // Properties:
  FullyQualifiedName : String
  FileName : String
  IsMain : Boolean
      // Events:
      // Methods:
      VoidReflection.Emit.ModuleBuilder::.ctorReflection.Emit.AssemblyBuilderStringStringBooleanBoolean)
      VoidReflection.Emit.ModuleBuilder::.cctor()
      VoidReflection.Emit.ModuleBuilder::System.Runtime.InteropServices._ModuleBuilder.GetIDsOfNamesGuid&IntPtrUInt32UInt32IntPtr)
      VoidReflection.Emit.ModuleBuilder::System.Runtime.InteropServices._ModuleBuilder.GetTypeInfoUInt32UInt32IntPtr)
      VoidReflection.Emit.ModuleBuilder::System.Runtime.InteropServices._ModuleBuilder.GetTypeInfoCountUInt32&)
      VoidReflection.Emit.ModuleBuilder::System.Runtime.InteropServices._ModuleBuilder.InvokeUInt32Guid&UInt32Int16IntPtrIntPtrIntPtrIntPtr)
      VoidReflection.Emit.ModuleBuilder::basic_initReflection.Emit.ModuleBuilder)
      VoidReflection.Emit.ModuleBuilder::set_wrappers_typeReflection.Emit.ModuleBuilderType)
      public StringReflection.Emit.ModuleBuilder::get_FullyQualifiedName()
      public BooleanReflection.Emit.ModuleBuilder::IsTransient()
      public VoidReflection.Emit.ModuleBuilder::CreateGlobalFunctions()
      public Reflection.Emit.FieldBuilderReflection.Emit.ModuleBuilder::DefineInitializedDataStringByte[]Reflection.FieldAttributes)
      public Reflection.Emit.FieldBuilderReflection.Emit.ModuleBuilder::DefineUninitializedDataStringInt32Reflection.FieldAttributes)
      VoidReflection.Emit.ModuleBuilder::addGlobalMethodReflection.Emit.MethodBuilder)
      public Reflection.Emit.MethodBuilderReflection.Emit.ModuleBuilder::DefineGlobalMethodStringReflection.MethodAttributesTypeType[])
      public Reflection.Emit.MethodBuilderReflection.Emit.ModuleBuilder::DefineGlobalMethodStringReflection.MethodAttributesReflection.CallingConventionsTypeType[])
      public Reflection.Emit.MethodBuilderReflection.Emit.ModuleBuilder::DefineGlobalMethodStringReflection.MethodAttributesReflection.CallingConventionsTypeType[]Type[]Type[]Type[][]Type[][])
      public Reflection.Emit.MethodBuilderReflection.Emit.ModuleBuilder::DefinePInvokeMethodStringStringReflection.MethodAttributesReflection.CallingConventionsTypeType[]Runtime.InteropServices.CallingConventionRuntime.InteropServices.CharSet)
      public Reflection.Emit.MethodBuilderReflection.Emit.ModuleBuilder::DefinePInvokeMethodStringStringStringReflection.MethodAttributesReflection.CallingConventionsTypeType[]Runtime.InteropServices.CallingConventionRuntime.InteropServices.CharSet)
      public Reflection.Emit.TypeBuilderReflection.Emit.ModuleBuilder::DefineTypeString)
      public Reflection.Emit.TypeBuilderReflection.Emit.ModuleBuilder::DefineTypeStringReflection.TypeAttributes)
      public Reflection.Emit.TypeBuilderReflection.Emit.ModuleBuilder::DefineTypeStringReflection.TypeAttributesType)
      VoidReflection.Emit.ModuleBuilder::AddTypeReflection.Emit.TypeBuilder)
      Reflection.Emit.TypeBuilderReflection.Emit.ModuleBuilder::DefineTypeStringReflection.TypeAttributesTypeType[]Reflection.Emit.PackingSizeInt32)
      VoidReflection.Emit.ModuleBuilder::RegisterTypeNameReflection.Emit.TypeBuilderString)
      Reflection.Emit.TypeBuilderReflection.Emit.ModuleBuilder::GetRegisteredTypeString)
      public Reflection.Emit.TypeBuilderReflection.Emit.ModuleBuilder::DefineTypeStringReflection.TypeAttributesTypeType[])
      public Reflection.Emit.TypeBuilderReflection.Emit.ModuleBuilder::DefineTypeStringReflection.TypeAttributesTypeInt32)
      public Reflection.Emit.TypeBuilderReflection.Emit.ModuleBuilder::DefineTypeStringReflection.TypeAttributesTypeReflection.Emit.PackingSize)
      public Reflection.Emit.TypeBuilderReflection.Emit.ModuleBuilder::DefineTypeStringReflection.TypeAttributesTypeReflection.Emit.PackingSizeInt32)
      public Reflection.MethodInfoReflection.Emit.ModuleBuilder::GetArrayMethodTypeStringReflection.CallingConventionsTypeType[])
      public Reflection.Emit.EnumBuilderReflection.Emit.ModuleBuilder::DefineEnumStringReflection.TypeAttributesType)
      public TypeReflection.Emit.ModuleBuilder::GetTypeString)
      public TypeReflection.Emit.ModuleBuilder::GetTypeStringBoolean)
      Reflection.Emit.TypeBuilderReflection.Emit.ModuleBuilder::search_in_arrayReflection.Emit.TypeBuilder[]Int32String)
      Reflection.Emit.TypeBuilderReflection.Emit.ModuleBuilder::search_nested_in_arrayReflection.Emit.TypeBuilder[]Int32String)
      TypeReflection.Emit.ModuleBuilder::create_modified_typeReflection.Emit.TypeBuilderString)
      Reflection.Emit.TypeBuilderReflection.Emit.ModuleBuilder::GetMaybeNestedReflection.Emit.TypeBuilderString)
      public TypeReflection.Emit.ModuleBuilder::GetTypeStringBooleanBoolean)
      Int32Reflection.Emit.ModuleBuilder::get_next_table_indexObjectInt32Boolean)
      public VoidReflection.Emit.ModuleBuilder::SetCustomAttributeReflection.Emit.CustomAttributeBuilder)
      public VoidReflection.Emit.ModuleBuilder::SetCustomAttributeReflection.ConstructorInfoByte[])
      public Diagnostics.SymbolStore.ISymbolWriterReflection.Emit.ModuleBuilder::GetSymWriter()
      public Diagnostics.SymbolStore.ISymbolDocumentWriterReflection.Emit.ModuleBuilder::DefineDocumentStringGuidGuidGuid)
      public Type[]Reflection.Emit.ModuleBuilder::GetTypes()
      public Resources.IResourceWriterReflection.Emit.ModuleBuilder::DefineResourceStringStringReflection.ResourceAttributes)
      public Resources.IResourceWriterReflection.Emit.ModuleBuilder::DefineResourceStringString)
      public VoidReflection.Emit.ModuleBuilder::DefineUnmanagedResourceByte[])
      public VoidReflection.Emit.ModuleBuilder::DefineUnmanagedResourceString)
      public VoidReflection.Emit.ModuleBuilder::DefineManifestResourceStringIO.StreamReflection.ResourceAttributes)
      public VoidReflection.Emit.ModuleBuilder::SetSymCustomAttributeStringByte[])
      public VoidReflection.Emit.ModuleBuilder::SetUserEntryPointReflection.MethodInfo)
      public Reflection.Emit.MethodTokenReflection.Emit.ModuleBuilder::GetMethodTokenReflection.MethodInfo)
      public Reflection.Emit.MethodTokenReflection.Emit.ModuleBuilder::GetArrayMethodTokenTypeStringReflection.CallingConventionsTypeType[])
      public Reflection.Emit.MethodTokenReflection.Emit.ModuleBuilder::GetConstructorTokenReflection.ConstructorInfo)
      public Reflection.Emit.FieldTokenReflection.Emit.ModuleBuilder::GetFieldTokenReflection.FieldInfo)
      public Reflection.Emit.SignatureTokenReflection.Emit.ModuleBuilder::GetSignatureTokenByte[]Int32)
      public Reflection.Emit.SignatureTokenReflection.Emit.ModuleBuilder::GetSignatureTokenReflection.Emit.SignatureHelper)
      public Reflection.Emit.StringTokenReflection.Emit.ModuleBuilder::GetStringConstantString)
      public Reflection.Emit.TypeTokenReflection.Emit.ModuleBuilder::GetTypeTokenType)
      public Reflection.Emit.TypeTokenReflection.Emit.ModuleBuilder::GetTypeTokenString)
      Int32Reflection.Emit.ModuleBuilder::getUSIndexReflection.Emit.ModuleBuilderString)
      Int32Reflection.Emit.ModuleBuilder::getTokenReflection.Emit.ModuleBuilderObject)
      Int32Reflection.Emit.ModuleBuilder::getMethodTokenReflection.Emit.ModuleBuilderReflection.MethodInfoType[])
      Int32Reflection.Emit.ModuleBuilder::GetTokenString)
      Int32Reflection.Emit.ModuleBuilder::GetTokenReflection.MemberInfo)
      Int32Reflection.Emit.ModuleBuilder::GetTokenReflection.MethodInfoType[])
      Int32Reflection.Emit.ModuleBuilder::GetTokenReflection.Emit.SignatureHelper)
      VoidReflection.Emit.ModuleBuilder::RegisterTokenObjectInt32)
      Reflection.Emit.TokenGeneratorReflection.Emit.ModuleBuilder::GetTokenGenerator()
      VoidReflection.Emit.ModuleBuilder::build_metadataReflection.Emit.ModuleBuilder)
      VoidReflection.Emit.ModuleBuilder::WriteToFileIntPtr)
      VoidReflection.Emit.ModuleBuilder::Save()
      StringReflection.Emit.ModuleBuilder::get_FileName()
      VoidReflection.Emit.ModuleBuilder::set_IsMainBoolean)
      VoidReflection.Emit.ModuleBuilder::CreateGlobalType()
      GuidReflection.Emit.ModuleBuilder::GetModuleVersionId()
      GuidReflection.Emit.ModuleBuilder::Mono_GetGuidReflection.Emit.ModuleBuilder)
    }
}
