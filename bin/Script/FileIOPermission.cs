// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Security.Permissions
{
    public class FileIOPermission : CodeAccessPermission
    {
      // Fields:
  version : Int32
  BadPathNameCharacters : Char[]
  BadFileNameCharacters : Char[]
  m_Unrestricted : Boolean
  m_AllFilesAccess : FileIOPermissionAccess
  m_AllLocalFilesAccess : FileIOPermissionAccess
  readList : ArrayList
  writeList : ArrayList
  appendList : ArrayList
  pathList : ArrayList
      // Properties:
  AllFiles : FileIOPermissionAccess
  AllLocalFiles : FileIOPermissionAccess
      // Events:
      // Methods:
      public VoidSecurity.Permissions.FileIOPermission::.ctorSecurity.Permissions.PermissionState)
      public VoidSecurity.Permissions.FileIOPermission::.ctorSecurity.Permissions.FileIOPermissionAccessString)
      public VoidSecurity.Permissions.FileIOPermission::.ctorSecurity.Permissions.FileIOPermissionAccessString[])
      public VoidSecurity.Permissions.FileIOPermission::.ctorSecurity.Permissions.FileIOPermissionAccessSecurity.AccessControl.AccessControlActionsString)
      public VoidSecurity.Permissions.FileIOPermission::.ctorSecurity.Permissions.FileIOPermissionAccessSecurity.AccessControl.AccessControlActionsString[])
      VoidSecurity.Permissions.FileIOPermission::.cctor()
      Int32Security.Permissions.FileIOPermission::System.Security.Permissions.IBuiltInPermission.GetTokenIndex()
      VoidSecurity.Permissions.FileIOPermission::CreateLists()
      public Security.Permissions.FileIOPermissionAccessSecurity.Permissions.FileIOPermission::get_AllFiles()
      public VoidSecurity.Permissions.FileIOPermission::set_AllFilesSecurity.Permissions.FileIOPermissionAccess)
      public Security.Permissions.FileIOPermissionAccessSecurity.Permissions.FileIOPermission::get_AllLocalFiles()
      public VoidSecurity.Permissions.FileIOPermission::set_AllLocalFilesSecurity.Permissions.FileIOPermissionAccess)
      public VoidSecurity.Permissions.FileIOPermission::AddPathListSecurity.Permissions.FileIOPermissionAccessString)
      public VoidSecurity.Permissions.FileIOPermission::AddPathListSecurity.Permissions.FileIOPermissionAccessString[])
      VoidSecurity.Permissions.FileIOPermission::AddPathInternalSecurity.Permissions.FileIOPermissionAccessString)
      public Security.IPermissionSecurity.Permissions.FileIOPermission::Copy()
      public VoidSecurity.Permissions.FileIOPermission::FromXmlSecurity.SecurityElement)
      public String[]Security.Permissions.FileIOPermission::GetPathListSecurity.Permissions.FileIOPermissionAccess)
      public Security.IPermissionSecurity.Permissions.FileIOPermission::IntersectSecurity.IPermission)
      public BooleanSecurity.Permissions.FileIOPermission::IsSubsetOfSecurity.IPermission)
      public BooleanSecurity.Permissions.FileIOPermission::IsUnrestricted()
      public VoidSecurity.Permissions.FileIOPermission::SetPathListSecurity.Permissions.FileIOPermissionAccessString)
      public VoidSecurity.Permissions.FileIOPermission::SetPathListSecurity.Permissions.FileIOPermissionAccessString[])
      public Security.SecurityElementSecurity.Permissions.FileIOPermission::ToXml()
      public Security.IPermissionSecurity.Permissions.FileIOPermission::UnionSecurity.IPermission)
      public BooleanSecurity.Permissions.FileIOPermission::EqualsObject)
      public Int32Security.Permissions.FileIOPermission::GetHashCode()
      BooleanSecurity.Permissions.FileIOPermission::IsEmpty()
      Security.Permissions.FileIOPermissionSecurity.Permissions.FileIOPermission::CastSecurity.IPermission)
      VoidSecurity.Permissions.FileIOPermission::ThrowInvalidFlagSecurity.Permissions.FileIOPermissionAccessBoolean)
      VoidSecurity.Permissions.FileIOPermission::ThrowIfInvalidPathString)
      VoidSecurity.Permissions.FileIOPermission::ThrowIfInvalidPathString[])
      VoidSecurity.Permissions.FileIOPermission::ClearSecurity.Permissions.FileIOPermissionAccess)
      BooleanSecurity.Permissions.FileIOPermission::KeyIsSubsetOfCollections.IListCollections.IList)
      VoidSecurity.Permissions.FileIOPermission::UnionKeysCollections.IListString[])
      VoidSecurity.Permissions.FileIOPermission::IntersectKeysCollections.IListCollections.IListCollections.IList)
    }
}
