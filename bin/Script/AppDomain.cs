// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System
{
    public class AppDomain : MarshalByRefObject
    {
      // Fields:
  _mono_app_domain : IntPtr
  _process_guid : String
  type_resolve_in_progress : Hashtable
  assembly_resolve_in_progress : Hashtable
  assembly_resolve_in_progress_refonly : Hashtable
  _evidence : Evidence
  _granted : PermissionSet
  _principalPolicy : PrincipalPolicy
  _principal : IPrincipal
  default_domain : AppDomain
  _domain_manager : AppDomainManager
  _activation : ActivationContext
  _applicationIdentity : ApplicationIdentity
  AssemblyLoad : AssemblyLoadEventHandler
  AssemblyResolve : ResolveEventHandler
  DomainUnload : EventHandler
  ProcessExit : EventHandler
  ResourceResolve : ResolveEventHandler
  TypeResolve : ResolveEventHandler
  UnhandledException : UnhandledExceptionEventHandler
  ReflectionOnlyAssemblyResolve : ResolveEventHandler
      // Properties:
  SetupInformationNoCopy : AppDomainSetup
  SetupInformation : AppDomainSetup
  BaseDirectory : String
  RelativeSearchPath : String
  DynamicDirectory : String
  ShadowCopyFiles : Boolean
  FriendlyName : String
  Evidence : Evidence
  DefaultPrincipal : IPrincipal
  GrantedPermissionSet : PermissionSet
  CurrentDomain : AppDomain
  DefaultDomain : AppDomain
  DomainManager : AppDomainManager
  ActivationContext : ActivationContext
  ApplicationIdentity : ApplicationIdentity
  Id : Int32
      // Events:
      AssemblyLoad : AssemblyLoadEventHandler
      AssemblyResolve : ResolveEventHandler
      DomainUnload : EventHandler
      ProcessExit : EventHandler
      ResourceResolve : ResolveEventHandler
      TypeResolve : ResolveEventHandler
      UnhandledException : UnhandledExceptionEventHandler
      ReflectionOnlyAssemblyResolve : ResolveEventHandler
      // Methods:
      VoidAppDomain::.ctor()
      public VoidAppDomain::add_AssemblyLoadAssemblyLoadEventHandler)
      public VoidAppDomain::remove_AssemblyLoadAssemblyLoadEventHandler)
      public VoidAppDomain::add_AssemblyResolveResolveEventHandler)
      public VoidAppDomain::remove_AssemblyResolveResolveEventHandler)
      public VoidAppDomain::add_DomainUnloadEventHandler)
      public VoidAppDomain::remove_DomainUnloadEventHandler)
      public VoidAppDomain::add_ProcessExitEventHandler)
      public VoidAppDomain::remove_ProcessExitEventHandler)
      public VoidAppDomain::add_ResourceResolveResolveEventHandler)
      public VoidAppDomain::remove_ResourceResolveResolveEventHandler)
      public VoidAppDomain::add_TypeResolveResolveEventHandler)
      public VoidAppDomain::remove_TypeResolveResolveEventHandler)
      public VoidAppDomain::add_UnhandledExceptionUnhandledExceptionEventHandler)
      public VoidAppDomain::remove_UnhandledExceptionUnhandledExceptionEventHandler)
      public VoidAppDomain::add_ReflectionOnlyAssemblyResolveResolveEventHandler)
      public VoidAppDomain::remove_ReflectionOnlyAssemblyResolveResolveEventHandler)
      AppDomainSetupAppDomain::getSetup()
      AppDomainSetupAppDomain::get_SetupInformationNoCopy()
      public AppDomainSetupAppDomain::get_SetupInformation()
      public StringAppDomain::get_BaseDirectory()
      public StringAppDomain::get_RelativeSearchPath()
      public StringAppDomain::get_DynamicDirectory()
      public BooleanAppDomain::get_ShadowCopyFiles()
      StringAppDomain::getFriendlyName()
      public StringAppDomain::get_FriendlyName()
      public Security.Policy.EvidenceAppDomain::get_Evidence()
      Security.Principal.IPrincipalAppDomain::get_DefaultPrincipal()
      Security.PermissionSetAppDomain::get_GrantedPermissionSet()
      AppDomainAppDomain::getCurDomain()
      public AppDomainAppDomain::get_CurrentDomain()
      AppDomainAppDomain::getRootDomain()
      AppDomainAppDomain::get_DefaultDomain()
      public VoidAppDomain::AppendPrivatePathString)
      public VoidAppDomain::ClearPrivatePath()
      public VoidAppDomain::ClearShadowCopyPath()
      public Runtime.Remoting.ObjectHandleAppDomain::CreateInstanceStringString)
      public Runtime.Remoting.ObjectHandleAppDomain::CreateInstanceStringStringObject[])
      public Runtime.Remoting.ObjectHandleAppDomain::CreateInstanceStringStringBooleanReflection.BindingFlagsReflection.BinderObject[]Globalization.CultureInfoObject[]Security.Policy.Evidence)
      public ObjectAppDomain::CreateInstanceAndUnwrapStringString)
      public ObjectAppDomain::CreateInstanceAndUnwrapStringStringObject[])
      public ObjectAppDomain::CreateInstanceAndUnwrapStringStringBooleanReflection.BindingFlagsReflection.BinderObject[]Globalization.CultureInfoObject[]Security.Policy.Evidence)
      public Runtime.Remoting.ObjectHandleAppDomain::CreateInstanceFromStringString)
      public Runtime.Remoting.ObjectHandleAppDomain::CreateInstanceFromStringStringObject[])
      public Runtime.Remoting.ObjectHandleAppDomain::CreateInstanceFromStringStringBooleanReflection.BindingFlagsReflection.BinderObject[]Globalization.CultureInfoObject[]Security.Policy.Evidence)
      public ObjectAppDomain::CreateInstanceFromAndUnwrapStringString)
      public ObjectAppDomain::CreateInstanceFromAndUnwrapStringStringObject[])
      public ObjectAppDomain::CreateInstanceFromAndUnwrapStringStringBooleanReflection.BindingFlagsReflection.BinderObject[]Globalization.CultureInfoObject[]Security.Policy.Evidence)
      public Reflection.Emit.AssemblyBuilderAppDomain::DefineDynamicAssemblyReflection.AssemblyNameReflection.Emit.AssemblyBuilderAccess)
      public Reflection.Emit.AssemblyBuilderAppDomain::DefineDynamicAssemblyReflection.AssemblyNameReflection.Emit.AssemblyBuilderAccessSecurity.Policy.Evidence)
      public Reflection.Emit.AssemblyBuilderAppDomain::DefineDynamicAssemblyReflection.AssemblyNameReflection.Emit.AssemblyBuilderAccessString)
      public Reflection.Emit.AssemblyBuilderAppDomain::DefineDynamicAssemblyReflection.AssemblyNameReflection.Emit.AssemblyBuilderAccessStringSecurity.Policy.Evidence)
      public Reflection.Emit.AssemblyBuilderAppDomain::DefineDynamicAssemblyReflection.AssemblyNameReflection.Emit.AssemblyBuilderAccessSecurity.PermissionSetSecurity.PermissionSetSecurity.PermissionSet)
      public Reflection.Emit.AssemblyBuilderAppDomain::DefineDynamicAssemblyReflection.AssemblyNameReflection.Emit.AssemblyBuilderAccessSecurity.Policy.EvidenceSecurity.PermissionSetSecurity.PermissionSetSecurity.PermissionSet)
      public Reflection.Emit.AssemblyBuilderAppDomain::DefineDynamicAssemblyReflection.AssemblyNameReflection.Emit.AssemblyBuilderAccessStringSecurity.PermissionSetSecurity.PermissionSetSecurity.PermissionSet)
      public Reflection.Emit.AssemblyBuilderAppDomain::DefineDynamicAssemblyReflection.AssemblyNameReflection.Emit.AssemblyBuilderAccessStringSecurity.Policy.EvidenceSecurity.PermissionSetSecurity.PermissionSetSecurity.PermissionSet)
      public Reflection.Emit.AssemblyBuilderAppDomain::DefineDynamicAssemblyReflection.AssemblyNameReflection.Emit.AssemblyBuilderAccessStringSecurity.Policy.EvidenceSecurity.PermissionSetSecurity.PermissionSetSecurity.PermissionSetBoolean)
      public Reflection.Emit.AssemblyBuilderAppDomain::DefineDynamicAssemblyReflection.AssemblyNameReflection.Emit.AssemblyBuilderAccessStringSecurity.Policy.EvidenceSecurity.PermissionSetSecurity.PermissionSetSecurity.PermissionSetBooleanCollections.Generic.IEnumerable`1<System.Reflection.Emit.CustomAttributeBuilder>)
      public Reflection.Emit.AssemblyBuilderAppDomain::DefineDynamicAssemblyReflection.AssemblyNameReflection.Emit.AssemblyBuilderAccessCollections.Generic.IEnumerable`1<System.Reflection.Emit.CustomAttributeBuilder>)
      Reflection.Emit.AssemblyBuilderAppDomain::DefineInternalDynamicAssemblyReflection.AssemblyNameReflection.Emit.AssemblyBuilderAccess)
      public VoidAppDomain::DoCallBackCrossAppDomainDelegate)
      public Int32AppDomain::ExecuteAssemblyString)
      public Int32AppDomain::ExecuteAssemblyStringSecurity.Policy.Evidence)
      public Int32AppDomain::ExecuteAssemblyStringSecurity.Policy.EvidenceString[])
      public Int32AppDomain::ExecuteAssemblyStringSecurity.Policy.EvidenceString[]Byte[]Configuration.Assemblies.AssemblyHashAlgorithm)
      Int32AppDomain::ExecuteAssemblyInternalReflection.AssemblyString[])
      Int32AppDomain::ExecuteAssemblyReflection.AssemblyString[])
      Reflection.Assembly[]AppDomain::GetAssembliesBoolean)
      public Reflection.Assembly[]AppDomain::GetAssemblies()
      public ObjectAppDomain::GetDataString)
      public TypeAppDomain::GetType()
      public ObjectAppDomain::InitializeLifetimeService()
      Reflection.AssemblyAppDomain::LoadAssemblyStringSecurity.Policy.EvidenceBoolean)
      public Reflection.AssemblyAppDomain::LoadReflection.AssemblyName)
      Reflection.AssemblyAppDomain::LoadSatelliteReflection.AssemblyNameBoolean)
      public Reflection.AssemblyAppDomain::LoadReflection.AssemblyNameSecurity.Policy.Evidence)
      public Reflection.AssemblyAppDomain::LoadString)
      public Reflection.AssemblyAppDomain::LoadStringSecurity.Policy.Evidence)
      Reflection.AssemblyAppDomain::LoadStringSecurity.Policy.EvidenceBoolean)
      public Reflection.AssemblyAppDomain::LoadByte[])
      public Reflection.AssemblyAppDomain::LoadByte[]Byte[])
      Reflection.AssemblyAppDomain::LoadAssemblyRawByte[]Byte[]Security.Policy.EvidenceBoolean)
      public Reflection.AssemblyAppDomain::LoadByte[]Byte[]Security.Policy.Evidence)
      Reflection.AssemblyAppDomain::LoadByte[]Byte[]Security.Policy.EvidenceBoolean)
      public VoidAppDomain::SetAppDomainPolicySecurity.Policy.PolicyLevel)
      public VoidAppDomain::SetCachePathString)
      public VoidAppDomain::SetPrincipalPolicySecurity.Principal.PrincipalPolicy)
      public VoidAppDomain::SetShadowCopyFiles()
      public VoidAppDomain::SetShadowCopyPathString)
      public VoidAppDomain::SetThreadPrincipalSecurity.Principal.IPrincipal)
      AppDomainAppDomain::InternalSetDomainByIDInt32)
      AppDomainAppDomain::InternalSetDomainAppDomain)
      VoidAppDomain::InternalPushDomainRefAppDomain)
      VoidAppDomain::InternalPushDomainRefByIDInt32)
      VoidAppDomain::InternalPopDomainRef()
      Runtime.Remoting.Contexts.ContextAppDomain::InternalSetContextRuntime.Remoting.Contexts.Context)
      Runtime.Remoting.Contexts.ContextAppDomain::InternalGetContext()
      Runtime.Remoting.Contexts.ContextAppDomain::InternalGetDefaultContext()
      StringAppDomain::InternalGetProcessGuidString)
      ObjectAppDomain::InvokeInDomainAppDomainReflection.MethodInfoObjectObject[])
      ObjectAppDomain::InvokeInDomainByIDInt32Reflection.MethodInfoObjectObject[])
      StringAppDomain::GetProcessGuid()
      public AppDomainAppDomain::CreateDomainString)
      public AppDomainAppDomain::CreateDomainStringSecurity.Policy.Evidence)
      AppDomainAppDomain::createDomainStringAppDomainSetup)
      public AppDomainAppDomain::CreateDomainStringSecurity.Policy.EvidenceAppDomainSetup)
      public AppDomainAppDomain::CreateDomainStringSecurity.Policy.EvidenceStringStringBoolean)
      AppDomainSetupAppDomain::CreateDomainSetupStringStringBoolean)
      BooleanAppDomain::InternalIsFinalizingForUnloadInt32)
      public BooleanAppDomain::IsFinalizingForUnload()
      VoidAppDomain::InternalUnloadInt32)
      Int32AppDomain::getDomainID()
      public VoidAppDomain::UnloadAppDomain)
      public VoidAppDomain::SetDataStringObject)
      public VoidAppDomain::SetDataStringObjectSecurity.IPermission)
      public Int32AppDomain::GetCurrentThreadId()
      public StringAppDomain::ToString()
      VoidAppDomain::ValidateAssemblyNameString)
      VoidAppDomain::DoAssemblyLoadReflection.Assembly)
      Reflection.AssemblyAppDomain::DoAssemblyResolveStringBoolean)
      Reflection.AssemblyAppDomain::DoTypeResolveObject)
      VoidAppDomain::DoDomainUnload()
      VoidAppDomain::ProcessMessageInDomainByte[]Runtime.Remoting.Messaging.CADMethodCallMessageByte[]&Runtime.Remoting.Messaging.CADMethodReturnMessage&)
      public AppDomainManagerAppDomain::get_DomainManager()
      public ActivationContextAppDomain::get_ActivationContext()
      public ApplicationIdentityAppDomain::get_ApplicationIdentity()
      public Int32AppDomain::get_Id()
      public StringAppDomain::ApplyPolicyString)
      public AppDomainAppDomain::CreateDomainStringSecurity.Policy.EvidenceStringStringBooleanAppDomainInitializerString[])
      public Int32AppDomain::ExecuteAssemblyByNameString)
      public Int32AppDomain::ExecuteAssemblyByNameStringSecurity.Policy.Evidence)
      public Int32AppDomain::ExecuteAssemblyByNameStringSecurity.Policy.EvidenceString[])
      public Int32AppDomain::ExecuteAssemblyByNameReflection.AssemblyNameSecurity.Policy.EvidenceString[])
      public BooleanAppDomain::IsDefaultAppDomain()
      public Reflection.Assembly[]AppDomain::ReflectionOnlyGetAssemblies()
    }
}
