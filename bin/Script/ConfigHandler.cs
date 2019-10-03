// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Runtime.Remoting
{
    class ConfigHandler : Object
    {
      // Fields:
  typeEntries : ArrayList
  channelInstances : ArrayList
  currentChannel : ChannelData
  currentProviderData : Stack
  currentClientUrl : String
  appName : String
  currentXmlPath : String
  onlyDelayedChannels : Boolean
  <>f__switch$map27 : Dictionary`2
  <>f__switch$map28 : Dictionary`2
      // Properties:
      // Events:
      // Methods:
      public VoidRuntime.Remoting.ConfigHandler::.ctorBoolean)
      VoidRuntime.Remoting.ConfigHandler::ValidatePathStringString[])
      BooleanRuntime.Remoting.ConfigHandler::CheckPathString)
      public VoidRuntime.Remoting.ConfigHandler::OnStartParsing(Mono.Xml.SmallXmlParser)
      public VoidRuntime.Remoting.ConfigHandler::OnProcessingInstructionStringString)
      public VoidRuntime.Remoting.ConfigHandler::OnIgnorableWhitespaceString)
      public VoidRuntime.Remoting.ConfigHandler::OnStartElementString,Mono.Xml.SmallXmlParser/IAttrList)
      public VoidRuntime.Remoting.ConfigHandler::ParseElementString,Mono.Xml.SmallXmlParser/IAttrList)
      public VoidRuntime.Remoting.ConfigHandler::OnEndElementString)
      VoidRuntime.Remoting.ConfigHandler::ReadCustomProviderDataString,Mono.Xml.SmallXmlParser/IAttrList)
      VoidRuntime.Remoting.ConfigHandler::ReadLifetine(Mono.Xml.SmallXmlParser/IAttrList)
      TimeSpanRuntime.Remoting.ConfigHandler::ParseTimeString)
      VoidRuntime.Remoting.ConfigHandler::ReadChannel(Mono.Xml.SmallXmlParser/IAttrListBoolean)
      Runtime.Remoting.ProviderDataRuntime.Remoting.ConfigHandler::ReadProviderString,Mono.Xml.SmallXmlParser/IAttrListBoolean)
      VoidRuntime.Remoting.ConfigHandler::ReadClientActivated(Mono.Xml.SmallXmlParser/IAttrList)
      VoidRuntime.Remoting.ConfigHandler::ReadServiceActivated(Mono.Xml.SmallXmlParser/IAttrList)
      VoidRuntime.Remoting.ConfigHandler::ReadClientWellKnown(Mono.Xml.SmallXmlParser/IAttrList)
      VoidRuntime.Remoting.ConfigHandler::ReadServiceWellKnown(Mono.Xml.SmallXmlParser/IAttrList)
      VoidRuntime.Remoting.ConfigHandler::ReadInteropXml(Mono.Xml.SmallXmlParser/IAttrListBoolean)
      VoidRuntime.Remoting.ConfigHandler::ReadPreload(Mono.Xml.SmallXmlParser/IAttrList)
      StringRuntime.Remoting.ConfigHandler::GetNotNull(Mono.Xml.SmallXmlParser/IAttrListString)
      StringRuntime.Remoting.ConfigHandler::ExtractAssemblyString&)
      public VoidRuntime.Remoting.ConfigHandler::OnCharsString)
      public VoidRuntime.Remoting.ConfigHandler::OnEndParsing(Mono.Xml.SmallXmlParser)
    }
}
