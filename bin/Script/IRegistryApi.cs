// Class info from mscorlib.dll
// 
using UnityEngine;

namespace Microsoft.Win32
{
    interface IRegistryApi
    {
      // Fields:
      // Properties:
      // Events:
      // Methods:
      public Microsoft.Win32.RegistryKey Microsoft.Win32.IRegistryApi::CreateSubKey(Microsoft.Win32.RegistryKeyString)
      public Microsoft.Win32.RegistryKey Microsoft.Win32.IRegistryApi::OpenRemoteBaseKey(Microsoft.Win32.RegistryHiveString)
      public Microsoft.Win32.RegistryKey Microsoft.Win32.IRegistryApi::OpenSubKey(Microsoft.Win32.RegistryKeyStringBoolean)
      public Void Microsoft.Win32.IRegistryApi::Flush(Microsoft.Win32.RegistryKey)
      public Void Microsoft.Win32.IRegistryApi::Close(Microsoft.Win32.RegistryKey)
      public Object Microsoft.Win32.IRegistryApi::GetValue(Microsoft.Win32.RegistryKeyStringObject,Microsoft.Win32.RegistryValueOptions)
      public Void Microsoft.Win32.IRegistryApi::SetValue(Microsoft.Win32.RegistryKeyStringObject)
      public Int32 Microsoft.Win32.IRegistryApi::SubKeyCount(Microsoft.Win32.RegistryKey)
      public Int32 Microsoft.Win32.IRegistryApi::ValueCount(Microsoft.Win32.RegistryKey)
      public Void Microsoft.Win32.IRegistryApi::DeleteValue(Microsoft.Win32.RegistryKeyStringBoolean)
      public Void Microsoft.Win32.IRegistryApi::DeleteKey(Microsoft.Win32.RegistryKeyStringBoolean)
      public String[] Microsoft.Win32.IRegistryApi::GetSubKeyNames(Microsoft.Win32.RegistryKey)
      public String[] Microsoft.Win32.IRegistryApi::GetValueNames(Microsoft.Win32.RegistryKey)
      public String Microsoft.Win32.IRegistryApi::ToString(Microsoft.Win32.RegistryKey)
      public Void Microsoft.Win32.IRegistryApi::SetValue(Microsoft.Win32.RegistryKeyStringObject,Microsoft.Win32.RegistryValueKind)
    }
}
