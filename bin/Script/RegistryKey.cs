// Class info from mscorlib.dll
// 
using UnityEngine;

namespace Microsoft.Win32
{
    public class RegistryKey : MarshalByRefObject
    {
      // Fields:
  handle : Object
  hive : Object
  qname : String
  isRemoteRoot : Boolean
  isWritable : Boolean
  RegistryApi : IRegistryApi
      // Properties:
  Name : String
  SubKeyCount : Int32
  ValueCount : Int32
  IsRoot : Boolean
  IsWritable : Boolean
  Hive : RegistryHive
  Handle : Object
      // Events:
      // Methods:
      Void Microsoft.Win32.RegistryKey::.ctor(Microsoft.Win32.RegistryHive)
      Void Microsoft.Win32.RegistryKey::.ctor(Microsoft.Win32.RegistryHiveIntPtrBoolean)
      Void Microsoft.Win32.RegistryKey::.ctorObjectStringBoolean)
      Void Microsoft.Win32.RegistryKey::.cctor()
      Void Microsoft.Win32.RegistryKey::System.IDisposable.Dispose()
      Void Microsoft.Win32.RegistryKey::Finalize()
      public String Microsoft.Win32.RegistryKey::get_Name()
      public Void Microsoft.Win32.RegistryKey::Flush()
      public Void Microsoft.Win32.RegistryKey::Close()
      public Int32 Microsoft.Win32.RegistryKey::get_SubKeyCount()
      public Int32 Microsoft.Win32.RegistryKey::get_ValueCount()
      public Void Microsoft.Win32.RegistryKey::SetValueStringObject)
      public Void Microsoft.Win32.RegistryKey::SetValueStringObject,Microsoft.Win32.RegistryValueKind)
      public Microsoft.Win32.RegistryKey Microsoft.Win32.RegistryKey::OpenSubKeyString)
      public Microsoft.Win32.RegistryKey Microsoft.Win32.RegistryKey::OpenSubKeyStringBoolean)
      public Object Microsoft.Win32.RegistryKey::GetValueString)
      public Object Microsoft.Win32.RegistryKey::GetValueStringObject)
      public Object Microsoft.Win32.RegistryKey::GetValueStringObject,Microsoft.Win32.RegistryValueOptions)
      public Microsoft.Win32.RegistryValueKind Microsoft.Win32.RegistryKey::GetValueKindString)
      public Microsoft.Win32.RegistryKey Microsoft.Win32.RegistryKey::CreateSubKeyString)
      public Microsoft.Win32.RegistryKey Microsoft.Win32.RegistryKey::CreateSubKeyString,Microsoft.Win32.RegistryKeyPermissionCheck)
      public Microsoft.Win32.RegistryKey Microsoft.Win32.RegistryKey::CreateSubKeyString,Microsoft.Win32.RegistryKeyPermissionCheckSecurity.AccessControl.RegistrySecurity)
      public Void Microsoft.Win32.RegistryKey::DeleteSubKeyString)
      public Void Microsoft.Win32.RegistryKey::DeleteSubKeyStringBoolean)
      public Void Microsoft.Win32.RegistryKey::DeleteSubKeyTreeString)
      public Void Microsoft.Win32.RegistryKey::DeleteValueString)
      public Void Microsoft.Win32.RegistryKey::DeleteValueStringBoolean)
      public Security.AccessControl.RegistrySecurity Microsoft.Win32.RegistryKey::GetAccessControl()
      public Security.AccessControl.RegistrySecurity Microsoft.Win32.RegistryKey::GetAccessControlSecurity.AccessControl.AccessControlSections)
      public String[] Microsoft.Win32.RegistryKey::GetSubKeyNames()
      public String[] Microsoft.Win32.RegistryKey::GetValueNames()
      public Microsoft.Win32.RegistryKey Microsoft.Win32.RegistryKey::OpenRemoteBaseKey(Microsoft.Win32.RegistryHiveString)
      public Microsoft.Win32.RegistryKey Microsoft.Win32.RegistryKey::OpenSubKeyString,Microsoft.Win32.RegistryKeyPermissionCheck)
      public Microsoft.Win32.RegistryKey Microsoft.Win32.RegistryKey::OpenSubKeyString,Microsoft.Win32.RegistryKeyPermissionCheckSecurity.AccessControl.RegistryRights)
      public Void Microsoft.Win32.RegistryKey::SetAccessControlSecurity.AccessControl.RegistrySecurity)
      public String Microsoft.Win32.RegistryKey::ToString()
      Boolean Microsoft.Win32.RegistryKey::get_IsRoot()
      Boolean Microsoft.Win32.RegistryKey::get_IsWritable()
      Microsoft.Win32.RegistryHive Microsoft.Win32.RegistryKey::get_Hive()
      Object Microsoft.Win32.RegistryKey::get_Handle()
      Void Microsoft.Win32.RegistryKey::AssertKeyStillValid()
      Void Microsoft.Win32.RegistryKey::AssertKeyNameNotNullString)
      Void Microsoft.Win32.RegistryKey::AssertKeyNameLengthString)
      Void Microsoft.Win32.RegistryKey::DeleteChildKeysAndValues()
      String Microsoft.Win32.RegistryKey::DecodeStringByte[])
      IO.IOException Microsoft.Win32.RegistryKey::CreateMarkedForDeletionException()
      String Microsoft.Win32.RegistryKey::GetHiveName(Microsoft.Win32.RegistryHive)
    }
}
