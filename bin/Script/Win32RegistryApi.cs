// Class info from mscorlib.dll
// 
using UnityEngine;

namespace Microsoft.Win32
{
    class Win32RegistryApi : Object
    {
      // Fields:
  OpenRegKeyRead : Int32
  OpenRegKeyWrite : Int32
  Int32ByteSize : Int32
  BufferMaxLength : Int32
  NativeBytesPerCharacter : Int32
      // Properties:
      // Events:
      // Methods:
      public Void Microsoft.Win32.Win32RegistryApi::.ctor()
      Int32 Microsoft.Win32.Win32RegistryApi::RegCreateKeyIntPtrStringIntPtr&)
      Int32 Microsoft.Win32.Win32RegistryApi::RegCloseKeyIntPtr)
      Int32 Microsoft.Win32.Win32RegistryApi::RegConnectRegistryStringIntPtrIntPtr&)
      Int32 Microsoft.Win32.Win32RegistryApi::RegFlushKeyIntPtr)
      Int32 Microsoft.Win32.Win32RegistryApi::RegOpenKeyExIntPtrStringIntPtrInt32IntPtr&)
      Int32 Microsoft.Win32.Win32RegistryApi::RegDeleteKeyIntPtrString)
      Int32 Microsoft.Win32.Win32RegistryApi::RegDeleteValueIntPtrString)
      Int32 Microsoft.Win32.Win32RegistryApi::RegEnumKeyIntPtrInt32Text.StringBuilderInt32)
      Int32 Microsoft.Win32.Win32RegistryApi::RegEnumValueIntPtrInt32Text.StringBuilderInt32&IntPtr,Microsoft.Win32.RegistryValueKind&IntPtrIntPtr)
      Int32 Microsoft.Win32.Win32RegistryApi::RegSetValueExIntPtrStringIntPtr,Microsoft.Win32.RegistryValueKindStringInt32)
      Int32 Microsoft.Win32.Win32RegistryApi::RegSetValueExIntPtrStringIntPtr,Microsoft.Win32.RegistryValueKindByte[]Int32)
      Int32 Microsoft.Win32.Win32RegistryApi::RegSetValueExIntPtrStringIntPtr,Microsoft.Win32.RegistryValueKindInt32&Int32)
      Int32 Microsoft.Win32.Win32RegistryApi::RegQueryValueExIntPtrStringIntPtr,Microsoft.Win32.RegistryValueKind&IntPtrInt32&)
      Int32 Microsoft.Win32.Win32RegistryApi::RegQueryValueExIntPtrStringIntPtr,Microsoft.Win32.RegistryValueKind&Byte[]Int32&)
      Int32 Microsoft.Win32.Win32RegistryApi::RegQueryValueExIntPtrStringIntPtr,Microsoft.Win32.RegistryValueKind&Int32&Int32&)
      IntPtr Microsoft.Win32.Win32RegistryApi::GetHandle(Microsoft.Win32.RegistryKey)
      Boolean Microsoft.Win32.Win32RegistryApi::IsHandleValid(Microsoft.Win32.RegistryKey)
      public Object Microsoft.Win32.Win32RegistryApi::GetValue(Microsoft.Win32.RegistryKeyStringObject,Microsoft.Win32.RegistryValueOptions)
      public Void Microsoft.Win32.Win32RegistryApi::SetValue(Microsoft.Win32.RegistryKeyStringObject,Microsoft.Win32.RegistryValueKind)
      public Void Microsoft.Win32.Win32RegistryApi::SetValue(Microsoft.Win32.RegistryKeyStringObject)
      Int32 Microsoft.Win32.Win32RegistryApi::GetBinaryValue(Microsoft.Win32.RegistryKeyString,Microsoft.Win32.RegistryValueKindByte[]&Int32)
      public Int32 Microsoft.Win32.Win32RegistryApi::SubKeyCount(Microsoft.Win32.RegistryKey)
      public Int32 Microsoft.Win32.Win32RegistryApi::ValueCount(Microsoft.Win32.RegistryKey)
      public Microsoft.Win32.RegistryKey Microsoft.Win32.Win32RegistryApi::OpenRemoteBaseKey(Microsoft.Win32.RegistryHiveString)
      public Microsoft.Win32.RegistryKey Microsoft.Win32.Win32RegistryApi::OpenSubKey(Microsoft.Win32.RegistryKeyStringBoolean)
      public Void Microsoft.Win32.Win32RegistryApi::Flush(Microsoft.Win32.RegistryKey)
      public Void Microsoft.Win32.Win32RegistryApi::Close(Microsoft.Win32.RegistryKey)
      public Microsoft.Win32.RegistryKey Microsoft.Win32.Win32RegistryApi::CreateSubKey(Microsoft.Win32.RegistryKeyString)
      public Void Microsoft.Win32.Win32RegistryApi::DeleteKey(Microsoft.Win32.RegistryKeyStringBoolean)
      public Void Microsoft.Win32.Win32RegistryApi::DeleteValue(Microsoft.Win32.RegistryKeyStringBoolean)
      public String[] Microsoft.Win32.Win32RegistryApi::GetSubKeyNames(Microsoft.Win32.RegistryKey)
      public String[] Microsoft.Win32.Win32RegistryApi::GetValueNames(Microsoft.Win32.RegistryKey)
      Void Microsoft.Win32.Win32RegistryApi::GenerateExceptionInt32)
      public String Microsoft.Win32.Win32RegistryApi::ToString(Microsoft.Win32.RegistryKey)
      String Microsoft.Win32.Win32RegistryApi::CombineName(Microsoft.Win32.RegistryKeyString)
    }
}
