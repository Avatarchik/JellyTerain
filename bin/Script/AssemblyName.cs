// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Reflection
{
    public class AssemblyName : Object
    {
      // Fields:
  name : String
  codebase : String
  major : Int32
  minor : Int32
  build : Int32
  revision : Int32
  cultureinfo : CultureInfo
  flags : AssemblyNameFlags
  hashalg : AssemblyHashAlgorithm
  keypair : StrongNameKeyPair
  publicKey : Byte[]
  keyToken : Byte[]
  versioncompat : AssemblyVersionCompatibility
  version : Version
  processor_architecture : ProcessorArchitecture
      // Properties:
  ProcessorArchitecture : ProcessorArchitecture
  Name : String
  CodeBase : String
  EscapedCodeBase : String
  CultureInfo : CultureInfo
  Flags : AssemblyNameFlags
  FullName : String
  HashAlgorithm : AssemblyHashAlgorithm
  KeyPair : StrongNameKeyPair
  Version : Version
  VersionCompatibility : AssemblyVersionCompatibility
  IsPublicKeyValid : Boolean
      // Events:
      // Methods:
      public VoidReflection.AssemblyName::.ctor()
      public VoidReflection.AssemblyName::.ctorString)
      VoidReflection.AssemblyName::.ctorRuntime.Serialization.SerializationInfoRuntime.Serialization.StreamingContext)
      VoidReflection.AssemblyName::System.Runtime.InteropServices._AssemblyName.GetIDsOfNamesGuid&IntPtrUInt32UInt32IntPtr)
      VoidReflection.AssemblyName::System.Runtime.InteropServices._AssemblyName.GetTypeInfoUInt32UInt32IntPtr)
      VoidReflection.AssemblyName::System.Runtime.InteropServices._AssemblyName.GetTypeInfoCountUInt32&)
      VoidReflection.AssemblyName::System.Runtime.InteropServices._AssemblyName.InvokeUInt32Guid&UInt32Int16IntPtrIntPtrIntPtrIntPtr)
      BooleanReflection.AssemblyName::ParseNameReflection.AssemblyNameString)
      public Reflection.ProcessorArchitectureReflection.AssemblyName::get_ProcessorArchitecture()
      public VoidReflection.AssemblyName::set_ProcessorArchitectureReflection.ProcessorArchitecture)
      public StringReflection.AssemblyName::get_Name()
      public VoidReflection.AssemblyName::set_NameString)
      public StringReflection.AssemblyName::get_CodeBase()
      public VoidReflection.AssemblyName::set_CodeBaseString)
      public StringReflection.AssemblyName::get_EscapedCodeBase()
      public Globalization.CultureInfoReflection.AssemblyName::get_CultureInfo()
      public VoidReflection.AssemblyName::set_CultureInfoGlobalization.CultureInfo)
      public Reflection.AssemblyNameFlagsReflection.AssemblyName::get_Flags()
      public VoidReflection.AssemblyName::set_FlagsReflection.AssemblyNameFlags)
      public StringReflection.AssemblyName::get_FullName()
      public Configuration.Assemblies.AssemblyHashAlgorithmReflection.AssemblyName::get_HashAlgorithm()
      public VoidReflection.AssemblyName::set_HashAlgorithmConfiguration.Assemblies.AssemblyHashAlgorithm)
      public Reflection.StrongNameKeyPairReflection.AssemblyName::get_KeyPair()
      public VoidReflection.AssemblyName::set_KeyPairReflection.StrongNameKeyPair)
      public VersionReflection.AssemblyName::get_Version()
      public VoidReflection.AssemblyName::set_VersionVersion)
      public Configuration.Assemblies.AssemblyVersionCompatibilityReflection.AssemblyName::get_VersionCompatibility()
      public VoidReflection.AssemblyName::set_VersionCompatibilityConfiguration.Assemblies.AssemblyVersionCompatibility)
      public StringReflection.AssemblyName::ToString()
      public Byte[]Reflection.AssemblyName::GetPublicKey()
      public Byte[]Reflection.AssemblyName::GetPublicKeyToken()
      BooleanReflection.AssemblyName::get_IsPublicKeyValid()
      Byte[]Reflection.AssemblyName::InternalGetPublicKeyToken()
      Byte[]Reflection.AssemblyName::ComputePublicKeyToken()
      public BooleanReflection.AssemblyName::ReferenceMatchesDefinitionReflection.AssemblyNameReflection.AssemblyName)
      public VoidReflection.AssemblyName::SetPublicKeyByte[])
      public VoidReflection.AssemblyName::SetPublicKeyTokenByte[])
      public VoidReflection.AssemblyName::GetObjectDataRuntime.Serialization.SerializationInfoRuntime.Serialization.StreamingContext)
      public ObjectReflection.AssemblyName::Clone()
      public VoidReflection.AssemblyName::OnDeserializationObject)
      public Reflection.AssemblyNameReflection.AssemblyName::GetAssemblyNameString)
    }
}
