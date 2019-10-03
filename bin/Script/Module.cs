// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Reflection
{
    public class Module : Object
    {
      // Fields:
  defaultBindingFlags : BindingFlags
  public FilterTypeName : TypeFilter
  public FilterTypeNameIgnoreCase : TypeFilter
  _impl : IntPtr
  assembly : Assembly
  fqname : String
  name : String
  scopename : String
  is_resource : Boolean
  token : Int32
      // Properties:
  Assembly : Assembly
  FullyQualifiedName : String
  Name : String
  ScopeName : String
  ModuleHandle : ModuleHandle
  MetadataToken : Int32
  MDStreamVersion : Int32
  MvId : Guid
  ModuleVersionId : Guid
      // Events:
      // Methods:
      VoidReflection.Module::.ctor()
      VoidReflection.Module::.cctor()
      VoidReflection.Module::System.Runtime.InteropServices._Module.GetIDsOfNamesGuid&IntPtrUInt32UInt32IntPtr)
      VoidReflection.Module::System.Runtime.InteropServices._Module.GetTypeInfoUInt32UInt32IntPtr)
      VoidReflection.Module::System.Runtime.InteropServices._Module.GetTypeInfoCountUInt32&)
      VoidReflection.Module::System.Runtime.InteropServices._Module.InvokeUInt32Guid&UInt32Int16IntPtrIntPtrIntPtrIntPtr)
      public Reflection.AssemblyReflection.Module::get_Assembly()
      public StringReflection.Module::get_FullyQualifiedName()
      public StringReflection.Module::get_Name()
      public StringReflection.Module::get_ScopeName()
      public ModuleHandleReflection.Module::get_ModuleHandle()
      public Int32Reflection.Module::get_MetadataToken()
      public Int32Reflection.Module::get_MDStreamVersion()
      Int32Reflection.Module::GetMDStreamVersionIntPtr)
      public Type[]Reflection.Module::FindTypesReflection.TypeFilterObject)
      public Object[]Reflection.Module::GetCustomAttributesBoolean)
      public Object[]Reflection.Module::GetCustomAttributesTypeBoolean)
      public Reflection.FieldInfoReflection.Module::GetFieldString)
      public Reflection.FieldInfoReflection.Module::GetFieldStringReflection.BindingFlags)
      public Reflection.FieldInfo[]Reflection.Module::GetFields()
      public Reflection.MethodInfoReflection.Module::GetMethodString)
      public Reflection.MethodInfoReflection.Module::GetMethodStringType[])
      public Reflection.MethodInfoReflection.Module::GetMethodStringReflection.BindingFlagsReflection.BinderReflection.CallingConventionsType[]Reflection.ParameterModifier[])
      Reflection.MethodInfoReflection.Module::GetMethodImplStringReflection.BindingFlagsReflection.BinderReflection.CallingConventionsType[]Reflection.ParameterModifier[])
      public Reflection.MethodInfo[]Reflection.Module::GetMethods()
      public Reflection.MethodInfo[]Reflection.Module::GetMethodsReflection.BindingFlags)
      public Reflection.FieldInfo[]Reflection.Module::GetFieldsReflection.BindingFlags)
      public VoidReflection.Module::GetObjectDataRuntime.Serialization.SerializationInfoRuntime.Serialization.StreamingContext)
      public TypeReflection.Module::GetTypeString)
      public TypeReflection.Module::GetTypeStringBoolean)
      public TypeReflection.Module::GetTypeStringBooleanBoolean)
      Type[]Reflection.Module::InternalGetTypes()
      public Type[]Reflection.Module::GetTypes()
      public BooleanReflection.Module::IsDefinedTypeBoolean)
      public BooleanReflection.Module::IsResource()
      public StringReflection.Module::ToString()
      GuidReflection.Module::get_MvId()
      public GuidReflection.Module::get_ModuleVersionId()
      public VoidReflection.Module::GetPEKindReflection.PortableExecutableKinds&Reflection.ImageFileMachine&)
      ExceptionReflection.Module::resolve_token_exceptionInt32Reflection.ResolveTokenErrorString)
      IntPtr[]Reflection.Module::ptrs_from_typesType[])
      public Reflection.FieldInfoReflection.Module::ResolveFieldInt32)
      public Reflection.FieldInfoReflection.Module::ResolveFieldInt32Type[]Type[])
      public Reflection.MemberInfoReflection.Module::ResolveMemberInt32)
      public Reflection.MemberInfoReflection.Module::ResolveMemberInt32Type[]Type[])
      public Reflection.MethodBaseReflection.Module::ResolveMethodInt32)
      public Reflection.MethodBaseReflection.Module::ResolveMethodInt32Type[]Type[])
      public StringReflection.Module::ResolveStringInt32)
      public TypeReflection.Module::ResolveTypeInt32)
      public TypeReflection.Module::ResolveTypeInt32Type[]Type[])
      public Byte[]Reflection.Module::ResolveSignatureInt32)
      TypeReflection.Module::MonoDebugger_ResolveTypeReflection.ModuleInt32)
      GuidReflection.Module::Mono_GetGuidReflection.Module)
      GuidReflection.Module::GetModuleVersionId()
      BooleanReflection.Module::filter_by_type_nameTypeObject)
      BooleanReflection.Module::filter_by_type_name_ignore_caseTypeObject)
      IntPtrReflection.Module::GetHINSTANCE()
      StringReflection.Module::GetGuidInternal()
      TypeReflection.Module::GetGlobalType()
      IntPtrReflection.Module::ResolveTypeTokenIntPtrInt32IntPtr[]IntPtr[]Reflection.ResolveTokenError&)
      IntPtrReflection.Module::ResolveMethodTokenIntPtrInt32IntPtr[]IntPtr[]Reflection.ResolveTokenError&)
      IntPtrReflection.Module::ResolveFieldTokenIntPtrInt32IntPtr[]IntPtr[]Reflection.ResolveTokenError&)
      StringReflection.Module::ResolveStringTokenIntPtrInt32Reflection.ResolveTokenError&)
      Reflection.MemberInfoReflection.Module::ResolveMemberTokenIntPtrInt32IntPtr[]IntPtr[]Reflection.ResolveTokenError&)
      Byte[]Reflection.Module::ResolveSignatureIntPtrInt32Reflection.ResolveTokenError&)
      VoidReflection.Module::GetPEKindIntPtrReflection.PortableExecutableKinds&Reflection.ImageFileMachine&)
    }
}
