// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Security.Permissions
{
    public class IsolatedStoragePermission : CodeAccessPermission
    {
      // Fields:
  version : Int32
  m_userQuota : Int64
  m_machineQuota : Int64
  m_expirationDays : Int64
  m_permanentData : Boolean
  m_allowed : IsolatedStorageContainment
      // Properties:
  UserQuota : Int64
  UsageAllowed : IsolatedStorageContainment
      // Events:
      // Methods:
      VoidSecurity.Permissions.IsolatedStoragePermission::.ctorSecurity.Permissions.PermissionState)
      public Int64Security.Permissions.IsolatedStoragePermission::get_UserQuota()
      public VoidSecurity.Permissions.IsolatedStoragePermission::set_UserQuotaInt64)
      public Security.Permissions.IsolatedStorageContainmentSecurity.Permissions.IsolatedStoragePermission::get_UsageAllowed()
      public VoidSecurity.Permissions.IsolatedStoragePermission::set_UsageAllowedSecurity.Permissions.IsolatedStorageContainment)
      public BooleanSecurity.Permissions.IsolatedStoragePermission::IsUnrestricted()
      public Security.SecurityElementSecurity.Permissions.IsolatedStoragePermission::ToXml()
      public VoidSecurity.Permissions.IsolatedStoragePermission::FromXmlSecurity.SecurityElement)
      BooleanSecurity.Permissions.IsolatedStoragePermission::IsEmpty()
    }
}
