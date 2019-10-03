// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Reflection.Emit
{
    public class AssemblyBuilder : Assembly
    {
      // Fields:
  COMPILER_ACCESS : AssemblyBuilderAccess
  dynamic_assembly : UIntPtr
  entry_point : MethodInfo
  modules : ModuleBuilder[]
  name : String
  dir : String
  cattrs : CustomAttributeBuilder[]
  resources : MonoResource[]
  public_key : Byte[]
  version : String
  culture : String
  algid : UInt32
  flags : UInt32
  pekind : PEFileKinds
  delay_sign : Boolean
  access : UInt32
  loaded_modules : Module[]
  win32_resources : MonoWin32Resource[]
  permissions_minimum : RefEmitPermissionSet[]
  permissions_optional : RefEmitPermissionSet[]
  permissions_refused : RefEmitPermissionSet[]
  peKind : PortableExecutableKinds
  machine : ImageFileMachine
  corlib_internal : Boolean
  type_forwarders : Type[]
  pktoken : Byte[]
  corlib_object_type : Type
  corlib_value_type : Type
  corlib_enum_type : Type
  corlib_void_type : Type
  resource_writers : ArrayList
  version_res : Win32VersionResource
  created : Boolean
  is_module_only : Boolean
  sn : StrongName
  native_resource : NativeResourceType
  is_compiler_context : Boolean
  versioninfo_culture : String
  manifest_module : ModuleBuilder
      // Properties:
  CodeBase : String
  EntryPoint : MethodInfo
  Location : String
  ImageRuntimeVersion : String
  ReflectionOnly : Boolean
  IsCompilerContext : Boolean
  IsSave : Boolean
  IsRun : Boolean
  AssemblyDir : String
  IsModuleOnly : Boolean
      // Events:
      // Methods:
      VoidReflection.Emit.AssemblyBuilder::.ctorReflection.AssemblyNameStringReflection.Emit.AssemblyBuilderAccessBoolean)
      VoidReflection.Emit.AssemblyBuilder::System.Runtime.InteropServices._AssemblyBuilder.GetIDsOfNamesGuid&IntPtrUInt32UInt32IntPtr)
      VoidReflection.Emit.AssemblyBuilder::System.Runtime.InteropServices._AssemblyBuilder.GetTypeInfoUInt32UInt32IntPtr)
      VoidReflection.Emit.AssemblyBuilder::System.Runtime.InteropServices._AssemblyBuilder.GetTypeInfoCountUInt32&)
      VoidReflection.Emit.AssemblyBuilder::System.Runtime.InteropServices._AssemblyBuilder.InvokeUInt32Guid&UInt32Int16IntPtrIntPtrIntPtrIntPtr)
      VoidReflection.Emit.AssemblyBuilder::basic_initReflection.Emit.AssemblyBuilder)
      public StringReflection.Emit.AssemblyBuilder::get_CodeBase()
      public Reflection.MethodInfoReflection.Emit.AssemblyBuilder::get_EntryPoint()
      public StringReflection.Emit.AssemblyBuilder::get_Location()
      public StringReflection.Emit.AssemblyBuilder::get_ImageRuntimeVersion()
      public BooleanReflection.Emit.AssemblyBuilder::get_ReflectionOnly()
      public VoidReflection.Emit.AssemblyBuilder::AddResourceFileStringString)
      public VoidReflection.Emit.AssemblyBuilder::AddResourceFileStringStringReflection.ResourceAttributes)
      VoidReflection.Emit.AssemblyBuilder::AddResourceFileStringStringReflection.ResourceAttributesBoolean)
      VoidReflection.Emit.AssemblyBuilder::AddPermissionRequestsSecurity.PermissionSetSecurity.PermissionSetSecurity.PermissionSet)
      VoidReflection.Emit.AssemblyBuilder::EmbedResourceFileStringString)
      VoidReflection.Emit.AssemblyBuilder::EmbedResourceFileStringStringReflection.ResourceAttributes)
      VoidReflection.Emit.AssemblyBuilder::EmbedResourceStringByte[]Reflection.ResourceAttributes)
      VoidReflection.Emit.AssemblyBuilder::AddTypeForwarderType)
      public Reflection.Emit.ModuleBuilderReflection.Emit.AssemblyBuilder::DefineDynamicModuleString)
      public Reflection.Emit.ModuleBuilderReflection.Emit.AssemblyBuilder::DefineDynamicModuleStringBoolean)
      public Reflection.Emit.ModuleBuilderReflection.Emit.AssemblyBuilder::DefineDynamicModuleStringString)
      public Reflection.Emit.ModuleBuilderReflection.Emit.AssemblyBuilder::DefineDynamicModuleStringStringBoolean)
      Reflection.Emit.ModuleBuilderReflection.Emit.AssemblyBuilder::DefineDynamicModuleStringStringBooleanBoolean)
      Reflection.ModuleReflection.Emit.AssemblyBuilder::InternalAddModuleString)
      Reflection.ModuleReflection.Emit.AssemblyBuilder::AddModuleString)
      public Resources.IResourceWriterReflection.Emit.AssemblyBuilder::DefineResourceStringStringString)
      public Resources.IResourceWriterReflection.Emit.AssemblyBuilder::DefineResourceStringStringStringReflection.ResourceAttributes)
      VoidReflection.Emit.AssemblyBuilder::AddUnmanagedResourceResources.Win32Resource)
      public VoidReflection.Emit.AssemblyBuilder::DefineUnmanagedResourceByte[])
      public VoidReflection.Emit.AssemblyBuilder::DefineUnmanagedResourceString)
      public VoidReflection.Emit.AssemblyBuilder::DefineVersionInfoResource()
      public VoidReflection.Emit.AssemblyBuilder::DefineVersionInfoResourceStringStringStringStringString)
      VoidReflection.Emit.AssemblyBuilder::DefineIconResourceString)
      VoidReflection.Emit.AssemblyBuilder::DefineVersionInfoResourceImplString)
      public Reflection.Emit.ModuleBuilderReflection.Emit.AssemblyBuilder::GetDynamicModuleString)
      public Type[]Reflection.Emit.AssemblyBuilder::GetExportedTypes()
      public IO.FileStreamReflection.Emit.AssemblyBuilder::GetFileString)
      public IO.FileStream[]Reflection.Emit.AssemblyBuilder::GetFilesBoolean)
      Reflection.Module[]Reflection.Emit.AssemblyBuilder::GetModulesInternal()
      Type[]Reflection.Emit.AssemblyBuilder::GetTypesBoolean)
      public Reflection.ManifestResourceInfoReflection.Emit.AssemblyBuilder::GetManifestResourceInfoString)
      public String[]Reflection.Emit.AssemblyBuilder::GetManifestResourceNames()
      public IO.StreamReflection.Emit.AssemblyBuilder::GetManifestResourceStreamString)
      public IO.StreamReflection.Emit.AssemblyBuilder::GetManifestResourceStreamTypeString)
      BooleanReflection.Emit.AssemblyBuilder::get_IsCompilerContext()
      BooleanReflection.Emit.AssemblyBuilder::get_IsSave()
      BooleanReflection.Emit.AssemblyBuilder::get_IsRun()
      StringReflection.Emit.AssemblyBuilder::get_AssemblyDir()
      BooleanReflection.Emit.AssemblyBuilder::get_IsModuleOnly()
      VoidReflection.Emit.AssemblyBuilder::set_IsModuleOnlyBoolean)
      Reflection.ModuleReflection.Emit.AssemblyBuilder::GetManifestModule()
      public VoidReflection.Emit.AssemblyBuilder::SaveStringReflection.PortableExecutableKindsReflection.ImageFileMachine)
      public VoidReflection.Emit.AssemblyBuilder::SaveString)
      public VoidReflection.Emit.AssemblyBuilder::SetEntryPointReflection.MethodInfo)
      public VoidReflection.Emit.AssemblyBuilder::SetEntryPointReflection.MethodInfoReflection.Emit.PEFileKinds)
      public VoidReflection.Emit.AssemblyBuilder::SetCustomAttributeReflection.Emit.CustomAttributeBuilder)
      public VoidReflection.Emit.AssemblyBuilder::SetCustomAttributeReflection.ConstructorInfoByte[])
      VoidReflection.Emit.AssemblyBuilder::SetCorlibTypeBuildersTypeTypeType)
      VoidReflection.Emit.AssemblyBuilder::SetCorlibTypeBuildersTypeTypeTypeType)
      ExceptionReflection.Emit.AssemblyBuilder::not_supported()
      VoidReflection.Emit.AssemblyBuilder::check_name_and_filenameStringStringBoolean)
      StringReflection.Emit.AssemblyBuilder::create_assembly_versionString)
      StringReflection.Emit.AssemblyBuilder::GetCultureStringString)
      Reflection.AssemblyNameReflection.Emit.AssemblyBuilder::UnprotectedGetName()
    }
}
