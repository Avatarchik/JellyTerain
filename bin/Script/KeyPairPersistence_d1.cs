// Class info from mscorlib.dll
// 
using UnityEngine;

namespace Mono.Security.Cryptography
{
    class KeyPairPersistence : Object
    {
      // Fields:
  _userPathExists : Boolean
  _userPath : String
  _machinePathExists : Boolean
  _machinePath : String
  _params : CspParameters
  _keyvalue : String
  _filename : String
  _container : String
  lockobj : Object
      // Properties:
  Filename : String
  KeyValue : String
  Parameters : CspParameters
  UserPath : String
  MachinePath : String
  CanChange : Boolean
  UseDefaultKeyContainer : Boolean
  UseMachineKeyStore : Boolean
  ContainerName : String
      // Events:
      // Methods:
      public Void Mono.Security.Cryptography.KeyPairPersistence::.ctorSecurity.Cryptography.CspParameters)
      public Void Mono.Security.Cryptography.KeyPairPersistence::.ctorSecurity.Cryptography.CspParametersString)
      Void Mono.Security.Cryptography.KeyPairPersistence::.cctor()
      public String Mono.Security.Cryptography.KeyPairPersistence::get_Filename()
      public String Mono.Security.Cryptography.KeyPairPersistence::get_KeyValue()
      public Void Mono.Security.Cryptography.KeyPairPersistence::set_KeyValueString)
      public Security.Cryptography.CspParameters Mono.Security.Cryptography.KeyPairPersistence::get_Parameters()
      public Boolean Mono.Security.Cryptography.KeyPairPersistence::Load()
      public Void Mono.Security.Cryptography.KeyPairPersistence::Save()
      public Void Mono.Security.Cryptography.KeyPairPersistence::Remove()
      String Mono.Security.Cryptography.KeyPairPersistence::get_UserPath()
      String Mono.Security.Cryptography.KeyPairPersistence::get_MachinePath()
      Boolean Mono.Security.Cryptography.KeyPairPersistence::_CanSecureString)
      Boolean Mono.Security.Cryptography.KeyPairPersistence::_ProtectUserString)
      Boolean Mono.Security.Cryptography.KeyPairPersistence::_ProtectMachineString)
      Boolean Mono.Security.Cryptography.KeyPairPersistence::_IsUserProtectedString)
      Boolean Mono.Security.Cryptography.KeyPairPersistence::_IsMachineProtectedString)
      Boolean Mono.Security.Cryptography.KeyPairPersistence::CanSecureString)
      Boolean Mono.Security.Cryptography.KeyPairPersistence::ProtectUserString)
      Boolean Mono.Security.Cryptography.KeyPairPersistence::ProtectMachineString)
      Boolean Mono.Security.Cryptography.KeyPairPersistence::IsUserProtectedString)
      Boolean Mono.Security.Cryptography.KeyPairPersistence::IsMachineProtectedString)
      Boolean Mono.Security.Cryptography.KeyPairPersistence::get_CanChange()
      Boolean Mono.Security.Cryptography.KeyPairPersistence::get_UseDefaultKeyContainer()
      Boolean Mono.Security.Cryptography.KeyPairPersistence::get_UseMachineKeyStore()
      String Mono.Security.Cryptography.KeyPairPersistence::get_ContainerName()
      Security.Cryptography.CspParameters Mono.Security.Cryptography.KeyPairPersistence::CopySecurity.Cryptography.CspParameters)
      Void Mono.Security.Cryptography.KeyPairPersistence::FromXmlString)
      String Mono.Security.Cryptography.KeyPairPersistence::ToXml()
    }
}
