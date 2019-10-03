// Class info from mscorlib.dll
// 
using UnityEngine;

namespace Mono.Security.X509
{
    class X509StoreManager : Object
    {
      // Fields:
  _userStore : X509Stores
  _machineStore : X509Stores
      // Properties:
  CurrentUser : X509Stores
  LocalMachine : X509Stores
  IntermediateCACertificates : X509CertificateCollection
  IntermediateCACrls : ArrayList
  TrustedRootCertificates : X509CertificateCollection
  TrustedRootCACrls : ArrayList
  UntrustedCertificates : X509CertificateCollection
      // Events:
      // Methods:
      Void Mono.Security.X509.X509StoreManager::.ctor()
      public Mono.Security.X509.X509Stores Mono.Security.X509.X509StoreManager::get_CurrentUser()
      public Mono.Security.X509.X509Stores Mono.Security.X509.X509StoreManager::get_LocalMachine()
      public Mono.Security.X509.X509CertificateCollection Mono.Security.X509.X509StoreManager::get_IntermediateCACertificates()
      public Collections.ArrayList Mono.Security.X509.X509StoreManager::get_IntermediateCACrls()
      public Mono.Security.X509.X509CertificateCollection Mono.Security.X509.X509StoreManager::get_TrustedRootCertificates()
      public Collections.ArrayList Mono.Security.X509.X509StoreManager::get_TrustedRootCACrls()
      public Mono.Security.X509.X509CertificateCollection Mono.Security.X509.X509StoreManager::get_UntrustedCertificates()
    }
}
