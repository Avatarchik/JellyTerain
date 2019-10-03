// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System
{
    public class AppDomainSetup : Object
    {
      // Fields:
  application_base : String
  application_name : String
  cache_path : String
  configuration_file : String
  dynamic_base : String
  license_file : String
  private_bin_path : String
  private_bin_path_probe : String
  shadow_copy_directories : String
  shadow_copy_files : String
  publisher_policy : Boolean
  path_changed : Boolean
  loader_optimization : LoaderOptimization
  disallow_binding_redirects : Boolean
  disallow_code_downloads : Boolean
  _activationArguments : ActivationArguments
  domain_initializer : AppDomainInitializer
  application_trust : ApplicationTrust
  domain_initializer_args : String[]
  application_trust_xml : SecurityElement
  disallow_appbase_probe : Boolean
  configuration_bytes : Byte[]
      // Properties:
  ApplicationBase : String
  ApplicationName : String
  CachePath : String
  ConfigurationFile : String
  DisallowPublisherPolicy : Boolean
  DynamicBase : String
  LicenseFile : String
  LoaderOptimization : LoaderOptimization
  PrivateBinPath : String
  PrivateBinPathProbe : String
  ShadowCopyDirectories : String
  ShadowCopyFiles : String
  DisallowBindingRedirects : Boolean
  DisallowCodeDownload : Boolean
  ActivationArguments : ActivationArguments
  AppDomainInitializer : AppDomainInitializer
  AppDomainInitializerArguments : String[]
  ApplicationTrust : ApplicationTrust
  DisallowApplicationBaseProbing : Boolean
      // Events:
      // Methods:
      public VoidAppDomainSetup::.ctor()
      VoidAppDomainSetup::.ctorAppDomainSetup)
      public VoidAppDomainSetup::.ctorRuntime.Hosting.ActivationArguments)
      public VoidAppDomainSetup::.ctorActivationContext)
      StringAppDomainSetup::GetAppBaseString)
      public StringAppDomainSetup::get_ApplicationBase()
      public VoidAppDomainSetup::set_ApplicationBaseString)
      public StringAppDomainSetup::get_ApplicationName()
      public VoidAppDomainSetup::set_ApplicationNameString)
      public StringAppDomainSetup::get_CachePath()
      public VoidAppDomainSetup::set_CachePathString)
      public StringAppDomainSetup::get_ConfigurationFile()
      public VoidAppDomainSetup::set_ConfigurationFileString)
      public BooleanAppDomainSetup::get_DisallowPublisherPolicy()
      public VoidAppDomainSetup::set_DisallowPublisherPolicyBoolean)
      public StringAppDomainSetup::get_DynamicBase()
      public VoidAppDomainSetup::set_DynamicBaseString)
      public StringAppDomainSetup::get_LicenseFile()
      public VoidAppDomainSetup::set_LicenseFileString)
      public LoaderOptimizationAppDomainSetup::get_LoaderOptimization()
      public VoidAppDomainSetup::set_LoaderOptimizationLoaderOptimization)
      public StringAppDomainSetup::get_PrivateBinPath()
      public VoidAppDomainSetup::set_PrivateBinPathString)
      public StringAppDomainSetup::get_PrivateBinPathProbe()
      public VoidAppDomainSetup::set_PrivateBinPathProbeString)
      public StringAppDomainSetup::get_ShadowCopyDirectories()
      public VoidAppDomainSetup::set_ShadowCopyDirectoriesString)
      public StringAppDomainSetup::get_ShadowCopyFiles()
      public VoidAppDomainSetup::set_ShadowCopyFilesString)
      public BooleanAppDomainSetup::get_DisallowBindingRedirects()
      public VoidAppDomainSetup::set_DisallowBindingRedirectsBoolean)
      public BooleanAppDomainSetup::get_DisallowCodeDownload()
      public VoidAppDomainSetup::set_DisallowCodeDownloadBoolean)
      public Runtime.Hosting.ActivationArgumentsAppDomainSetup::get_ActivationArguments()
      public VoidAppDomainSetup::set_ActivationArgumentsRuntime.Hosting.ActivationArguments)
      public AppDomainInitializerAppDomainSetup::get_AppDomainInitializer()
      public VoidAppDomainSetup::set_AppDomainInitializerAppDomainInitializer)
      public String[]AppDomainSetup::get_AppDomainInitializerArguments()
      public VoidAppDomainSetup::set_AppDomainInitializerArgumentsString[])
      public Security.Policy.ApplicationTrustAppDomainSetup::get_ApplicationTrust()
      public VoidAppDomainSetup::set_ApplicationTrustSecurity.Policy.ApplicationTrust)
      public BooleanAppDomainSetup::get_DisallowApplicationBaseProbing()
      public VoidAppDomainSetup::set_DisallowApplicationBaseProbingBoolean)
      public Byte[]AppDomainSetup::GetConfigurationBytes()
      public VoidAppDomainSetup::SetConfigurationBytesByte[])
    }
}
