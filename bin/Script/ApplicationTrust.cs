// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Security.Policy
{
    public class ApplicationTrust : Object
    {
      // Fields:
  _appid : ApplicationIdentity
  _defaultPolicy : PolicyStatement
  _xtranfo : Object
  _trustrun : Boolean
  _persist : Boolean
  fullTrustAssemblies : IList`1
      // Properties:
  ApplicationIdentity : ApplicationIdentity
  DefaultGrantSet : PolicyStatement
  ExtraInfo : Object
  IsApplicationTrustedToRun : Boolean
  Persist : Boolean
      // Events:
      // Methods:
      public VoidSecurity.Policy.ApplicationTrust::.ctor()
      public VoidSecurity.Policy.ApplicationTrust::.ctorApplicationIdentity)
      VoidSecurity.Policy.ApplicationTrust::.ctorSecurity.PermissionSetCollections.Generic.IEnumerable`1<System.Security.Policy.StrongName>)
      public ApplicationIdentitySecurity.Policy.ApplicationTrust::get_ApplicationIdentity()
      public VoidSecurity.Policy.ApplicationTrust::set_ApplicationIdentityApplicationIdentity)
      public Security.Policy.PolicyStatementSecurity.Policy.ApplicationTrust::get_DefaultGrantSet()
      public VoidSecurity.Policy.ApplicationTrust::set_DefaultGrantSetSecurity.Policy.PolicyStatement)
      public ObjectSecurity.Policy.ApplicationTrust::get_ExtraInfo()
      public VoidSecurity.Policy.ApplicationTrust::set_ExtraInfoObject)
      public BooleanSecurity.Policy.ApplicationTrust::get_IsApplicationTrustedToRun()
      public VoidSecurity.Policy.ApplicationTrust::set_IsApplicationTrustedToRunBoolean)
      public BooleanSecurity.Policy.ApplicationTrust::get_Persist()
      public VoidSecurity.Policy.ApplicationTrust::set_PersistBoolean)
      public VoidSecurity.Policy.ApplicationTrust::FromXmlSecurity.SecurityElement)
      public Security.SecurityElementSecurity.Policy.ApplicationTrust::ToXml()
      Security.Policy.PolicyStatementSecurity.Policy.ApplicationTrust::GetDefaultGrantSet()
    }
}
