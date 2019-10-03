// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Security
{
    public class SecurityException : SystemException
    {
      // Fields:
  permissionState : String
  permissionType : Type
  _granted : String
  _refused : String
  _demanded : Object
  _firstperm : IPermission
  _method : MethodInfo
  _evidence : Evidence
  _action : SecurityAction
  _denyset : Object
  _permitset : Object
  _assembly : AssemblyName
  _url : String
  _zone : SecurityZone
      // Properties:
  Action : SecurityAction
  DenySetInstance : Object
  FailedAssemblyInfo : AssemblyName
  Method : MethodInfo
  PermitOnlySetInstance : Object
  Url : String
  Zone : SecurityZone
  Demanded : Object
  FirstPermissionThatFailed : IPermission
  PermissionState : String
  PermissionType : Type
  GrantedSet : String
  RefusedSet : String
      // Events:
      // Methods:
      public VoidSecurity.SecurityException::.ctor()
      public VoidSecurity.SecurityException::.ctorString)
      VoidSecurity.SecurityException::.ctorRuntime.Serialization.SerializationInfoRuntime.Serialization.StreamingContext)
      public VoidSecurity.SecurityException::.ctorStringException)
      public VoidSecurity.SecurityException::.ctorStringType)
      public VoidSecurity.SecurityException::.ctorStringTypeString)
      VoidSecurity.SecurityException::.ctorStringSecurity.PermissionSetSecurity.PermissionSet)
      public VoidSecurity.SecurityException::.ctorStringObjectObjectReflection.MethodInfoObjectSecurity.IPermission)
      public VoidSecurity.SecurityException::.ctorStringReflection.AssemblyNameSecurity.PermissionSetSecurity.PermissionSetReflection.MethodInfoSecurity.Permissions.SecurityActionObjectSecurity.IPermissionSecurity.Policy.Evidence)
      public Security.Permissions.SecurityActionSecurity.SecurityException::get_Action()
      public VoidSecurity.SecurityException::set_ActionSecurity.Permissions.SecurityAction)
      public ObjectSecurity.SecurityException::get_DenySetInstance()
      public VoidSecurity.SecurityException::set_DenySetInstanceObject)
      public Reflection.AssemblyNameSecurity.SecurityException::get_FailedAssemblyInfo()
      public VoidSecurity.SecurityException::set_FailedAssemblyInfoReflection.AssemblyName)
      public Reflection.MethodInfoSecurity.SecurityException::get_Method()
      public VoidSecurity.SecurityException::set_MethodReflection.MethodInfo)
      public ObjectSecurity.SecurityException::get_PermitOnlySetInstance()
      public VoidSecurity.SecurityException::set_PermitOnlySetInstanceObject)
      public StringSecurity.SecurityException::get_Url()
      public VoidSecurity.SecurityException::set_UrlString)
      public Security.SecurityZoneSecurity.SecurityException::get_Zone()
      public VoidSecurity.SecurityException::set_ZoneSecurity.SecurityZone)
      public ObjectSecurity.SecurityException::get_Demanded()
      public VoidSecurity.SecurityException::set_DemandedObject)
      public Security.IPermissionSecurity.SecurityException::get_FirstPermissionThatFailed()
      public VoidSecurity.SecurityException::set_FirstPermissionThatFailedSecurity.IPermission)
      public StringSecurity.SecurityException::get_PermissionState()
      public VoidSecurity.SecurityException::set_PermissionStateString)
      public TypeSecurity.SecurityException::get_PermissionType()
      public VoidSecurity.SecurityException::set_PermissionTypeType)
      public StringSecurity.SecurityException::get_GrantedSet()
      public VoidSecurity.SecurityException::set_GrantedSetString)
      public StringSecurity.SecurityException::get_RefusedSet()
      public VoidSecurity.SecurityException::set_RefusedSetString)
      public VoidSecurity.SecurityException::GetObjectDataRuntime.Serialization.SerializationInfoRuntime.Serialization.StreamingContext)
      public StringSecurity.SecurityException::ToString()
    }
}
