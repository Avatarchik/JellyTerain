// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Reflection
{
    public class Assembly : Object
    {
      // Fields:
  _mono_assembly : IntPtr
  resolve_event_holder : ResolveEventHolder
  _evidence : Evidence
  _minimum : PermissionSet
  _optional : PermissionSet
  _refuse : PermissionSet
  _granted : PermissionSet
  _denied : PermissionSet
  fromByteArray : Boolean
  assemblyName : String
      // Properties:
  CodeBase : String
  EscapedCodeBase : String
  FullName : String
  EntryPoint : MethodInfo
  Evidence : Evidence
  GlobalAssemblyCache : Boolean
  FromByteArray : Boolean
  Location : String
  ImageRuntimeVersion : String
  HostContext : Int64
  ManifestModule : Module
  ReflectionOnly : Boolean
  GrantedPermissionSet : PermissionSet
  DeniedPermissionSet : PermissionSet
      // Events:
      ModuleResolve : ModuleResolveEventHandler
      // Methods:
      VoidReflection.Assembly::.ctor()
      public VoidReflection.Assembly::add_ModuleResolveReflection.ModuleResolveEventHandler)
      public VoidReflection.Assembly::remove_ModuleResolveReflection.ModuleResolveEventHandler)
      StringReflection.Assembly::get_code_baseBoolean)
      StringReflection.Assembly::get_fullname()
      StringReflection.Assembly::get_location()
      StringReflection.Assembly::InternalImageRuntimeVersion()
      StringReflection.Assembly::GetCodeBaseBoolean)
      public StringReflection.Assembly::get_CodeBase()
      public StringReflection.Assembly::get_EscapedCodeBase()
      public StringReflection.Assembly::get_FullName()
      public Reflection.MethodInfoReflection.Assembly::get_EntryPoint()
      public Security.Policy.EvidenceReflection.Assembly::get_Evidence()
      Security.Policy.EvidenceReflection.Assembly::UnprotectedGetEvidence()
      BooleanReflection.Assembly::get_global_assembly_cache()
      public BooleanReflection.Assembly::get_GlobalAssemblyCache()
      VoidReflection.Assembly::set_FromByteArrayBoolean)
      public StringReflection.Assembly::get_Location()
      public StringReflection.Assembly::get_ImageRuntimeVersion()
      public VoidReflection.Assembly::GetObjectDataRuntime.Serialization.SerializationInfoRuntime.Serialization.StreamingContext)
      public BooleanReflection.Assembly::IsDefinedTypeBoolean)
      public Object[]Reflection.Assembly::GetCustomAttributesBoolean)
      public Object[]Reflection.Assembly::GetCustomAttributesTypeBoolean)
      ObjectReflection.Assembly::GetFilesInternalStringBoolean)
      public IO.FileStream[]Reflection.Assembly::GetFiles()
      public IO.FileStream[]Reflection.Assembly::GetFilesBoolean)
      public IO.FileStreamReflection.Assembly::GetFileString)
      IntPtrReflection.Assembly::GetManifestResourceInternalStringInt32&Reflection.Module&)
      public IO.StreamReflection.Assembly::GetManifestResourceStreamString)
      public IO.StreamReflection.Assembly::GetManifestResourceStreamTypeString)
      Type[]Reflection.Assembly::GetTypesBoolean)
      public Type[]Reflection.Assembly::GetTypes()
      public Type[]Reflection.Assembly::GetExportedTypes()
      public TypeReflection.Assembly::GetTypeStringBoolean)
      public TypeReflection.Assembly::GetTypeString)
      TypeReflection.Assembly::InternalGetTypeReflection.ModuleStringBooleanBoolean)
      public TypeReflection.Assembly::GetTypeStringBooleanBoolean)
      VoidReflection.Assembly::InternalGetAssemblyNameStringReflection.AssemblyName)
      VoidReflection.Assembly::FillNameReflection.AssemblyReflection.AssemblyName)
      public Reflection.AssemblyNameReflection.Assembly::GetNameBoolean)
      public Reflection.AssemblyNameReflection.Assembly::GetName()
      Reflection.AssemblyNameReflection.Assembly::UnprotectedGetName()
      public StringReflection.Assembly::ToString()
      public StringReflection.Assembly::CreateQualifiedNameStringString)
      public Reflection.AssemblyReflection.Assembly::GetAssemblyType)
      public Reflection.AssemblyReflection.Assembly::GetEntryAssembly()
      public Reflection.AssemblyReflection.Assembly::GetSatelliteAssemblyGlobalization.CultureInfo)
      public Reflection.AssemblyReflection.Assembly::GetSatelliteAssemblyGlobalization.CultureInfoVersion)
      Reflection.AssemblyReflection.Assembly::GetSatelliteAssemblyNoThrowGlobalization.CultureInfoVersion)
      Reflection.AssemblyReflection.Assembly::GetSatelliteAssemblyGlobalization.CultureInfoVersionBoolean)
      Reflection.AssemblyReflection.Assembly::LoadFromStringBoolean)
      public Reflection.AssemblyReflection.Assembly::LoadFromString)
      public Reflection.AssemblyReflection.Assembly::LoadFromStringSecurity.Policy.Evidence)
      public Reflection.AssemblyReflection.Assembly::LoadFromStringSecurity.Policy.EvidenceByte[]Configuration.Assemblies.AssemblyHashAlgorithm)
      public Reflection.AssemblyReflection.Assembly::LoadFileStringSecurity.Policy.Evidence)
      public Reflection.AssemblyReflection.Assembly::LoadFileString)
      public Reflection.AssemblyReflection.Assembly::LoadString)
      public Reflection.AssemblyReflection.Assembly::LoadStringSecurity.Policy.Evidence)
      public Reflection.AssemblyReflection.Assembly::LoadReflection.AssemblyName)
      public Reflection.AssemblyReflection.Assembly::LoadReflection.AssemblyNameSecurity.Policy.Evidence)
      public Reflection.AssemblyReflection.Assembly::LoadByte[])
      public Reflection.AssemblyReflection.Assembly::LoadByte[]Byte[])
      public Reflection.AssemblyReflection.Assembly::LoadByte[]Byte[]Security.Policy.Evidence)
      public Reflection.AssemblyReflection.Assembly::ReflectionOnlyLoadByte[])
      public Reflection.AssemblyReflection.Assembly::ReflectionOnlyLoadString)
      public Reflection.AssemblyReflection.Assembly::ReflectionOnlyLoadFromString)
      public Reflection.AssemblyReflection.Assembly::LoadWithPartialNameString)
      public Reflection.ModuleReflection.Assembly::LoadModuleStringByte[])
      public Reflection.ModuleReflection.Assembly::LoadModuleStringByte[]Byte[])
      Reflection.AssemblyReflection.Assembly::load_with_partial_nameStringSecurity.Policy.Evidence)
      public Reflection.AssemblyReflection.Assembly::LoadWithPartialNameStringSecurity.Policy.Evidence)
      Reflection.AssemblyReflection.Assembly::LoadWithPartialNameStringSecurity.Policy.EvidenceBoolean)
      public ObjectReflection.Assembly::CreateInstanceString)
      public ObjectReflection.Assembly::CreateInstanceStringBoolean)
      public ObjectReflection.Assembly::CreateInstanceStringBooleanReflection.BindingFlagsReflection.BinderObject[]Globalization.CultureInfoObject[])
      public Reflection.Module[]Reflection.Assembly::GetLoadedModules()
      public Reflection.Module[]Reflection.Assembly::GetLoadedModulesBoolean)
      public Reflection.Module[]Reflection.Assembly::GetModules()
      public Reflection.ModuleReflection.Assembly::GetModuleString)
      Reflection.Module[]Reflection.Assembly::GetModulesInternal()
      public Reflection.Module[]Reflection.Assembly::GetModulesBoolean)
      String[]Reflection.Assembly::GetNamespaces()
      public String[]Reflection.Assembly::GetManifestResourceNames()
      public Reflection.AssemblyReflection.Assembly::GetExecutingAssembly()
      public Reflection.AssemblyReflection.Assembly::GetCallingAssembly()
      public Reflection.AssemblyName[]Reflection.Assembly::GetReferencedAssemblies()
      BooleanReflection.Assembly::GetManifestResourceInfoInternalStringReflection.ManifestResourceInfo)
      public Reflection.ManifestResourceInfoReflection.Assembly::GetManifestResourceInfoString)
      Int32Reflection.Assembly::MonoDebugger_GetMethodTokenReflection.MethodBase)
      public Int64Reflection.Assembly::get_HostContext()
      public Reflection.ModuleReflection.Assembly::get_ManifestModule()
      Reflection.ModuleReflection.Assembly::GetManifestModule()
      Reflection.ModuleReflection.Assembly::GetManifestModuleInternal()
      public BooleanReflection.Assembly::get_ReflectionOnly()
      VoidReflection.Assembly::Resolve()
      Security.PermissionSetReflection.Assembly::get_GrantedPermissionSet()
      Security.PermissionSetReflection.Assembly::get_DeniedPermissionSet()
      BooleanReflection.Assembly::LoadPermissionsReflection.AssemblyIntPtr&Int32&IntPtr&Int32&IntPtr&Int32&)
      VoidReflection.Assembly::LoadAssemblyPermissions()
      TypeReflection.Assembly::System.Runtime.InteropServices._Assembly.GetType()
    }
}
