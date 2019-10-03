// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.IO
{
    public class DriveInfo : Object
    {
      // Fields:
  _drive_type : _DriveType
  drive_format : String
  path : String
      // Properties:
  AvailableFreeSpace : Int64
  TotalFreeSpace : Int64
  TotalSize : Int64
  VolumeLabel : String
  DriveFormat : String
  DriveType : DriveType
  Name : String
  RootDirectory : DirectoryInfo
  IsReady : Boolean
      // Events:
      // Methods:
      VoidIO.DriveInfo::.ctorIO.DriveInfo/_DriveTypeStringString)
      public VoidIO.DriveInfo::.ctorString)
      VoidIO.DriveInfo::System.Runtime.Serialization.ISerializable.GetObjectDataRuntime.Serialization.SerializationInfoRuntime.Serialization.StreamingContext)
      VoidIO.DriveInfo::GetDiskFreeSpaceStringUInt64&UInt64&UInt64&)
      public Int64IO.DriveInfo::get_AvailableFreeSpace()
      public Int64IO.DriveInfo::get_TotalFreeSpace()
      public Int64IO.DriveInfo::get_TotalSize()
      public StringIO.DriveInfo::get_VolumeLabel()
      public VoidIO.DriveInfo::set_VolumeLabelString)
      public StringIO.DriveInfo::get_DriveFormat()
      public IO.DriveTypeIO.DriveInfo::get_DriveType()
      public StringIO.DriveInfo::get_Name()
      public IO.DirectoryInfoIO.DriveInfo::get_RootDirectory()
      public BooleanIO.DriveInfo::get_IsReady()
      IO.StreamReaderIO.DriveInfo::TryOpenString)
      IO.DriveInfo[]IO.DriveInfo::LinuxGetDrives()
      IO.DriveInfo[]IO.DriveInfo::UnixGetDrives()
      IO.DriveInfo[]IO.DriveInfo::WindowsGetDrives()
      public IO.DriveInfo[]IO.DriveInfo::GetDrives()
      public StringIO.DriveInfo::ToString()
      BooleanIO.DriveInfo::GetDiskFreeSpaceInternalStringUInt64&UInt64&UInt64&IO.MonoIOError&)
      UInt32IO.DriveInfo::GetDriveTypeInternalString)
    }
}
