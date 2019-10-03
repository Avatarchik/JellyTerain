// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Security.AccessControl
{
    public class CommonSecurityDescriptor : GenericSecurityDescriptor
    {
      // Fields:
  isContainer : Boolean
  isDS : Boolean
  flags : ControlFlags
  owner : SecurityIdentifier
  group : SecurityIdentifier
  systemAcl : SystemAcl
  discretionaryAcl : DiscretionaryAcl
      // Properties:
  ControlFlags : ControlFlags
  DiscretionaryAcl : DiscretionaryAcl
  Group : SecurityIdentifier
  IsContainer : Boolean
  IsDiscretionaryAclCanonical : Boolean
  IsDS : Boolean
  IsSystemAclCanonical : Boolean
  Owner : SecurityIdentifier
  SystemAcl : SystemAcl
      // Events:
      // Methods:
      public VoidSecurity.AccessControl.CommonSecurityDescriptor::.ctorBooleanBooleanSecurity.AccessControl.RawSecurityDescriptor)
      public VoidSecurity.AccessControl.CommonSecurityDescriptor::.ctorBooleanBooleanString)
      public VoidSecurity.AccessControl.CommonSecurityDescriptor::.ctorBooleanBooleanByte[]Int32)
      public VoidSecurity.AccessControl.CommonSecurityDescriptor::.ctorBooleanBooleanSecurity.AccessControl.ControlFlagsSecurity.Principal.SecurityIdentifierSecurity.Principal.SecurityIdentifierSecurity.AccessControl.SystemAclSecurity.AccessControl.DiscretionaryAcl)
      public Security.AccessControl.ControlFlagsSecurity.AccessControl.CommonSecurityDescriptor::get_ControlFlags()
      public Security.AccessControl.DiscretionaryAclSecurity.AccessControl.CommonSecurityDescriptor::get_DiscretionaryAcl()
      public VoidSecurity.AccessControl.CommonSecurityDescriptor::set_DiscretionaryAclSecurity.AccessControl.DiscretionaryAcl)
      public Security.Principal.SecurityIdentifierSecurity.AccessControl.CommonSecurityDescriptor::get_Group()
      public VoidSecurity.AccessControl.CommonSecurityDescriptor::set_GroupSecurity.Principal.SecurityIdentifier)
      public BooleanSecurity.AccessControl.CommonSecurityDescriptor::get_IsContainer()
      public BooleanSecurity.AccessControl.CommonSecurityDescriptor::get_IsDiscretionaryAclCanonical()
      public BooleanSecurity.AccessControl.CommonSecurityDescriptor::get_IsDS()
      public BooleanSecurity.AccessControl.CommonSecurityDescriptor::get_IsSystemAclCanonical()
      public Security.Principal.SecurityIdentifierSecurity.AccessControl.CommonSecurityDescriptor::get_Owner()
      public VoidSecurity.AccessControl.CommonSecurityDescriptor::set_OwnerSecurity.Principal.SecurityIdentifier)
      public Security.AccessControl.SystemAclSecurity.AccessControl.CommonSecurityDescriptor::get_SystemAcl()
      public VoidSecurity.AccessControl.CommonSecurityDescriptor::set_SystemAclSecurity.AccessControl.SystemAcl)
      public VoidSecurity.AccessControl.CommonSecurityDescriptor::PurgeAccessControlSecurity.Principal.SecurityIdentifier)
      public VoidSecurity.AccessControl.CommonSecurityDescriptor::PurgeAuditSecurity.Principal.SecurityIdentifier)
      public VoidSecurity.AccessControl.CommonSecurityDescriptor::SetDiscretionaryAclProtectionBooleanBoolean)
      public VoidSecurity.AccessControl.CommonSecurityDescriptor::SetSystemAclProtectionBooleanBoolean)
    }
}
