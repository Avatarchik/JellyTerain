// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System
{
    public class Environment : Object
    {
      // Fields:
  mono_corlib_version : Int32
  os : OperatingSystem
      // Properties:
  CommandLine : String
  CurrentDirectory : String
  ExitCode : Int32
  HasShutdownStarted : Boolean
  EmbeddingHostName : String
  SocketSecurityEnabled : Boolean
  UnityWebSecurityEnabled : Boolean
  MachineName : String
  NewLine : String
  Platform : PlatformID
  OSVersion : OperatingSystem
  StackTrace : String
  TickCount : Int32
  UserDomainName : String
  UserInteractive : Boolean
  UserName : String
  Version : Version
  WorkingSet : Int64
  ProcessorCount : Int32
  IsRunningOnWindows : Boolean
      // Events:
      // Methods:
      public StringEnvironment::get_CommandLine()
      public StringEnvironment::get_CurrentDirectory()
      public VoidEnvironment::set_CurrentDirectoryString)
      public Int32Environment::get_ExitCode()
      public VoidEnvironment::set_ExitCodeInt32)
      public BooleanEnvironment::get_HasShutdownStarted()
      public StringEnvironment::get_EmbeddingHostName()
      public BooleanEnvironment::get_SocketSecurityEnabled()
      public BooleanEnvironment::get_UnityWebSecurityEnabled()
      public StringEnvironment::get_MachineName()
      public StringEnvironment::get_NewLine()
      PlatformIDEnvironment::get_Platform()
      StringEnvironment::GetOSVersionString()
      public OperatingSystemEnvironment::get_OSVersion()
      public StringEnvironment::get_StackTrace()
      public Int32Environment::get_TickCount()
      public StringEnvironment::get_UserDomainName()
      public BooleanEnvironment::get_UserInteractive()
      public StringEnvironment::get_UserName()
      public VersionEnvironment::get_Version()
      public Int64Environment::get_WorkingSet()
      public VoidEnvironment::ExitInt32)
      public StringEnvironment::ExpandEnvironmentVariablesString)
      public String[]Environment::GetCommandLineArgs()
      StringEnvironment::internalGetEnvironmentVariableString)
      public StringEnvironment::GetEnvironmentVariableString)
      Collections.HashtableEnvironment::GetEnvironmentVariablesNoCase()
      public Collections.IDictionaryEnvironment::GetEnvironmentVariables()
      StringEnvironment::GetWindowsFolderPathInt32)
      public StringEnvironment::GetFolderPathEnvironment/SpecialFolder)
      StringEnvironment::ReadXdgUserDirStringStringStringString)
      StringEnvironment::InternalGetFolderPathEnvironment/SpecialFolder)
      public String[]Environment::GetLogicalDrives()
      VoidEnvironment::internalBroadcastSettingChange()
      public StringEnvironment::GetEnvironmentVariableStringEnvironmentVariableTarget)
      public Collections.IDictionaryEnvironment::GetEnvironmentVariablesEnvironmentVariableTarget)
      public VoidEnvironment::SetEnvironmentVariableStringString)
      public VoidEnvironment::SetEnvironmentVariableStringStringEnvironmentVariableTarget)
      VoidEnvironment::InternalSetEnvironmentVariableStringString)
      public VoidEnvironment::FailFastString)
      public Int32Environment::get_ProcessorCount()
      BooleanEnvironment::get_IsRunningOnWindows()
      String[]Environment::GetLogicalDrivesInternal()
      String[]Environment::GetEnvironmentVariableNames()
      StringEnvironment::GetMachineConfigPath()
      StringEnvironment::internalGetHome()
    }
}
