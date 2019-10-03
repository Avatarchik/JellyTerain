// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Runtime.Remoting.Channels
{
    public class ChannelServices : Object
    {
      // Fields:
  registeredChannels : ArrayList
  delayedClientChannels : ArrayList
  _crossContextSink : CrossContextChannel
  CrossContextUrl : String
  oldStartModeTypes : IList
      // Properties:
  CrossContextChannel : CrossContextChannel
  RegisteredChannels : IChannel[]
      // Events:
      // Methods:
      VoidRuntime.Remoting.Channels.ChannelServices::.ctor()
      VoidRuntime.Remoting.Channels.ChannelServices::.cctor()
      Runtime.Remoting.Contexts.CrossContextChannelRuntime.Remoting.Channels.ChannelServices::get_CrossContextChannel()
      Runtime.Remoting.Messaging.IMessageSinkRuntime.Remoting.Channels.ChannelServices::CreateClientChannelSinkChainStringObjectString&)
      Runtime.Remoting.Messaging.IMessageSinkRuntime.Remoting.Channels.ChannelServices::CreateClientChannelSinkChainRuntime.Remoting.Channels.IChannelSenderStringObject[]String&)
      public Runtime.Remoting.Channels.IChannel[]Runtime.Remoting.Channels.ChannelServices::get_RegisteredChannels()
      public Runtime.Remoting.Channels.IServerChannelSinkRuntime.Remoting.Channels.ChannelServices::CreateServerChannelSinkChainRuntime.Remoting.Channels.IServerChannelSinkProviderRuntime.Remoting.Channels.IChannelReceiver)
      public Runtime.Remoting.Channels.ServerProcessingRuntime.Remoting.Channels.ChannelServices::DispatchMessageRuntime.Remoting.Channels.IServerChannelSinkStackRuntime.Remoting.Messaging.IMessageRuntime.Remoting.Messaging.IMessage&)
      public Runtime.Remoting.Channels.IChannelRuntime.Remoting.Channels.ChannelServices::GetChannelString)
      public Collections.IDictionaryRuntime.Remoting.Channels.ChannelServices::GetChannelSinkPropertiesObject)
      public String[]Runtime.Remoting.Channels.ChannelServices::GetUrlsForObjectMarshalByRefObject)
      public VoidRuntime.Remoting.Channels.ChannelServices::RegisterChannelRuntime.Remoting.Channels.IChannel)
      public VoidRuntime.Remoting.Channels.ChannelServices::RegisterChannelRuntime.Remoting.Channels.IChannelBoolean)
      VoidRuntime.Remoting.Channels.ChannelServices::RegisterChannelConfigRuntime.Remoting.ChannelData)
      ObjectRuntime.Remoting.Channels.ChannelServices::CreateProviderRuntime.Remoting.ProviderData)
      public Runtime.Remoting.Messaging.IMessageRuntime.Remoting.Channels.ChannelServices::SyncDispatchMessageRuntime.Remoting.Messaging.IMessage)
      public Runtime.Remoting.Messaging.IMessageCtrlRuntime.Remoting.Channels.ChannelServices::AsyncDispatchMessageRuntime.Remoting.Messaging.IMessageRuntime.Remoting.Messaging.IMessageSink)
      Runtime.Remoting.Messaging.ReturnMessageRuntime.Remoting.Channels.ChannelServices::CheckIncomingMessageRuntime.Remoting.Messaging.IMessage)
      Runtime.Remoting.Messaging.IMessageRuntime.Remoting.Channels.ChannelServices::CheckReturnMessageRuntime.Remoting.Messaging.IMessageRuntime.Remoting.Messaging.IMessage)
      BooleanRuntime.Remoting.Channels.ChannelServices::IsLocalCallRuntime.Remoting.Messaging.IMessage)
      public VoidRuntime.Remoting.Channels.ChannelServices::UnregisterChannelRuntime.Remoting.Channels.IChannel)
      Object[]Runtime.Remoting.Channels.ChannelServices::GetCurrentChannelInfo()
    }
}
