// Class info from mscorlib.dll
// 
using UnityEngine;

namespace Microsoft.Win32
{
    class KeyHandler : Object
    {
      // Fields:
  key_to_handler : Hashtable
  dir_to_handler : Hashtable
  public Dir : String
  values : Hashtable
  file : String
  dirty : Boolean
  <>f__switch$map1 : Dictionary`2
      // Properties:
  ValueCount : Int32
  IsMarkedForDeletion : Boolean
  UserStore : String
  MachineStore : String
      // Events:
      // Methods:
      Void Microsoft.Win32.KeyHandler::.ctor(Microsoft.Win32.RegistryKeyString)
      Void Microsoft.Win32.KeyHandler::.cctor()
      public Void Microsoft.Win32.KeyHandler::Load()
      Void Microsoft.Win32.KeyHandler::LoadKeySecurity.SecurityElement)
      public Microsoft.Win32.RegistryKey Microsoft.Win32.KeyHandler::Ensure(Microsoft.Win32.RegistryKeyStringBoolean)
      public Microsoft.Win32.RegistryKey Microsoft.Win32.KeyHandler::Probe(Microsoft.Win32.RegistryKeyStringBoolean)
      String Microsoft.Win32.KeyHandler::CombineName(Microsoft.Win32.RegistryKeyString)
      public Microsoft.Win32.KeyHandler Microsoft.Win32.KeyHandler::Lookup(Microsoft.Win32.RegistryKeyBoolean)
      public Void Microsoft.Win32.KeyHandler::Drop(Microsoft.Win32.RegistryKey)
      public Void Microsoft.Win32.KeyHandler::DropString)
      public Object Microsoft.Win32.KeyHandler::GetValueString,Microsoft.Win32.RegistryValueOptions)
      public Void Microsoft.Win32.KeyHandler::SetValueStringObject)
      public String[] Microsoft.Win32.KeyHandler::GetValueNames()
      public Void Microsoft.Win32.KeyHandler::SetValueStringObject,Microsoft.Win32.RegistryValueKind)
      Void Microsoft.Win32.KeyHandler::SetDirty()
      public Void Microsoft.Win32.KeyHandler::DirtyTimeoutObject)
      public Void Microsoft.Win32.KeyHandler::Flush()
      public Boolean Microsoft.Win32.KeyHandler::ValueExistsString)
      public Int32 Microsoft.Win32.KeyHandler::get_ValueCount()
      public Boolean Microsoft.Win32.KeyHandler::get_IsMarkedForDeletion()
      public Void Microsoft.Win32.KeyHandler::RemoveValueString)
      Void Microsoft.Win32.KeyHandler::Finalize()
      Void Microsoft.Win32.KeyHandler::Save()
      Void Microsoft.Win32.KeyHandler::AssertNotMarkedForDeletion()
      String Microsoft.Win32.KeyHandler::get_UserStore()
      String Microsoft.Win32.KeyHandler::get_MachineStore()
    }
}
