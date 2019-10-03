// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Runtime.Remoting
{
    public class RemotingConfiguration : Object
    {
      // Fields:
  applicationID : String
  applicationName : String
  processGuid : String
  defaultConfigRead : Boolean
  defaultDelayedConfigRead : Boolean
  _errorMode : String
  wellKnownClientEntries : Hashtable
  activatedClientEntries : Hashtable
  wellKnownServiceEntries : Hashtable
  activatedServiceEntries : Hashtable
  channelTemplates : Hashtable
  clientProviderTemplates : Hashtable
  serverProviderTemplates : Hashtable
      // Properties:
  ApplicationId : String
  ApplicationName : String
  CustomErrorsMode : CustomErrorsModes
  ProcessId : String
      // Events:
      // Methods:
      VoidRuntime.Remoting.RemotingConfiguration::.cctor()
      public StringRuntime.Remoting.RemotingConfiguration::get_ApplicationId()
      public StringRuntime.Remoting.RemotingConfiguration::get_ApplicationName()
      public VoidRuntime.Remoting.RemotingConfiguration::set_ApplicationNameString)
      public Runtime.Remoting.CustomErrorsModesRuntime.Remoting.RemotingConfiguration::get_CustomErrorsMode()
      public VoidRuntime.Remoting.RemotingConfiguration::set_CustomErrorsModeRuntime.Remoting.CustomErrorsModes)
      public StringRuntime.Remoting.RemotingConfiguration::get_ProcessId()
      public VoidRuntime.Remoting.RemotingConfiguration::ConfigureStringBoolean)
      public VoidRuntime.Remoting.RemotingConfiguration::ConfigureString)
      VoidRuntime.Remoting.RemotingConfiguration::ReadConfigFileString)
      VoidRuntime.Remoting.RemotingConfiguration::LoadDefaultDelayedChannels()
      public Runtime.Remoting.ActivatedClientTypeEntry[]Runtime.Remoting.RemotingConfiguration::GetRegisteredActivatedClientTypes()
      public Runtime.Remoting.ActivatedServiceTypeEntry[]Runtime.Remoting.RemotingConfiguration::GetRegisteredActivatedServiceTypes()
      public Runtime.Remoting.WellKnownClientTypeEntry[]Runtime.Remoting.RemotingConfiguration::GetRegisteredWellKnownClientTypes()
      public Runtime.Remoting.WellKnownServiceTypeEntry[]Runtime.Remoting.RemotingConfiguration::GetRegisteredWellKnownServiceTypes()
      public BooleanRuntime.Remoting.RemotingConfiguration::IsActivationAllowedType)
      public Runtime.Remoting.ActivatedClientTypeEntryRuntime.Remoting.RemotingConfiguration::IsRemotelyActivatedClientTypeType)
      public Runtime.Remoting.ActivatedClientTypeEntryRuntime.Remoting.RemotingConfiguration::IsRemotelyActivatedClientTypeStringString)
      public Runtime.Remoting.WellKnownClientTypeEntryRuntime.Remoting.RemotingConfiguration::IsWellKnownClientTypeType)
      public Runtime.Remoting.WellKnownClientTypeEntryRuntime.Remoting.RemotingConfiguration::IsWellKnownClientTypeStringString)
      public VoidRuntime.Remoting.RemotingConfiguration::RegisterActivatedClientTypeRuntime.Remoting.ActivatedClientTypeEntry)
      public VoidRuntime.Remoting.RemotingConfiguration::RegisterActivatedClientTypeTypeString)
      public VoidRuntime.Remoting.RemotingConfiguration::RegisterActivatedServiceTypeRuntime.Remoting.ActivatedServiceTypeEntry)
      public VoidRuntime.Remoting.RemotingConfiguration::RegisterActivatedServiceTypeType)
      public VoidRuntime.Remoting.RemotingConfiguration::RegisterWellKnownClientTypeTypeString)
      public VoidRuntime.Remoting.RemotingConfiguration::RegisterWellKnownClientTypeRuntime.Remoting.WellKnownClientTypeEntry)
      public VoidRuntime.Remoting.RemotingConfiguration::RegisterWellKnownServiceTypeTypeStringRuntime.Remoting.WellKnownObjectMode)
      public VoidRuntime.Remoting.RemotingConfiguration::RegisterWellKnownServiceTypeRuntime.Remoting.WellKnownServiceTypeEntry)
      VoidRuntime.Remoting.RemotingConfiguration::RegisterChannelTemplateRuntime.Remoting.ChannelData)
      VoidRuntime.Remoting.RemotingConfiguration::RegisterClientProviderTemplateRuntime.Remoting.ProviderData)
      VoidRuntime.Remoting.RemotingConfiguration::RegisterServerProviderTemplateRuntime.Remoting.ProviderData)
      VoidRuntime.Remoting.RemotingConfiguration::RegisterChannelsCollections.ArrayListBoolean)
      VoidRuntime.Remoting.RemotingConfiguration::RegisterTypesCollections.ArrayList)
      public BooleanRuntime.Remoting.RemotingConfiguration::CustomErrorsEnabledBoolean)
      VoidRuntime.Remoting.RemotingConfiguration::SetCustomErrorsModeString)
    }
}
