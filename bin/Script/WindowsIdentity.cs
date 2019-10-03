// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Security.Principal
{
    public class WindowsIdentity : Object
    {
      // Fields:
  _token : IntPtr
  _type : String
  _account : WindowsAccountType
  _authenticated : Boolean
  _name : String
  _info : SerializationInfo
  invalidWindows : IntPtr
      // Properties:
  AuthenticationType : String
  IsAnonymous : Boolean
  IsAuthenticated : Boolean
  IsGuest : Boolean
  IsSystem : Boolean
  Name : String
  Token : IntPtr
  Groups : IdentityReferenceCollection
  ImpersonationLevel : TokenImpersonationLevel
  Owner : SecurityIdentifier
  User : SecurityIdentifier
  IsPosix : Boolean
      // Events:
      // Methods:
      public VoidSecurity.Principal.WindowsIdentity::.ctorIntPtr)
      public VoidSecurity.Principal.WindowsIdentity::.ctorIntPtrString)
      public VoidSecurity.Principal.WindowsIdentity::.ctorIntPtrStringSecurity.Principal.WindowsAccountType)
      public VoidSecurity.Principal.WindowsIdentity::.ctorIntPtrStringSecurity.Principal.WindowsAccountTypeBoolean)
      public VoidSecurity.Principal.WindowsIdentity::.ctorString)
      public VoidSecurity.Principal.WindowsIdentity::.ctorStringString)
      public VoidSecurity.Principal.WindowsIdentity::.ctorRuntime.Serialization.SerializationInfoRuntime.Serialization.StreamingContext)
      VoidSecurity.Principal.WindowsIdentity::.cctor()
      VoidSecurity.Principal.WindowsIdentity::System.Runtime.Serialization.IDeserializationCallback.OnDeserializationObject)
      VoidSecurity.Principal.WindowsIdentity::System.Runtime.Serialization.ISerializable.GetObjectDataRuntime.Serialization.SerializationInfoRuntime.Serialization.StreamingContext)
      public VoidSecurity.Principal.WindowsIdentity::Dispose()
      VoidSecurity.Principal.WindowsIdentity::DisposeBoolean)
      public Security.Principal.WindowsIdentitySecurity.Principal.WindowsIdentity::GetAnonymous()
      public Security.Principal.WindowsIdentitySecurity.Principal.WindowsIdentity::GetCurrent()
      public Security.Principal.WindowsIdentitySecurity.Principal.WindowsIdentity::GetCurrentBoolean)
      public Security.Principal.WindowsIdentitySecurity.Principal.WindowsIdentity::GetCurrentSecurity.Principal.TokenAccessLevels)
      public Security.Principal.WindowsImpersonationContextSecurity.Principal.WindowsIdentity::Impersonate()
      public Security.Principal.WindowsImpersonationContextSecurity.Principal.WindowsIdentity::ImpersonateIntPtr)
      public StringSecurity.Principal.WindowsIdentity::get_AuthenticationType()
      public BooleanSecurity.Principal.WindowsIdentity::get_IsAnonymous()
      public BooleanSecurity.Principal.WindowsIdentity::get_IsAuthenticated()
      public BooleanSecurity.Principal.WindowsIdentity::get_IsGuest()
      public BooleanSecurity.Principal.WindowsIdentity::get_IsSystem()
      public StringSecurity.Principal.WindowsIdentity::get_Name()
      public IntPtrSecurity.Principal.WindowsIdentity::get_Token()
      public Security.Principal.IdentityReferenceCollectionSecurity.Principal.WindowsIdentity::get_Groups()
      public Security.Principal.TokenImpersonationLevelSecurity.Principal.WindowsIdentity::get_ImpersonationLevel()
      public Security.Principal.SecurityIdentifierSecurity.Principal.WindowsIdentity::get_Owner()
      public Security.Principal.SecurityIdentifierSecurity.Principal.WindowsIdentity::get_User()
      BooleanSecurity.Principal.WindowsIdentity::get_IsPosix()
      VoidSecurity.Principal.WindowsIdentity::SetTokenIntPtr)
      String[]Security.Principal.WindowsIdentity::_GetRolesIntPtr)
      IntPtrSecurity.Principal.WindowsIdentity::GetCurrentToken()
      StringSecurity.Principal.WindowsIdentity::GetTokenNameIntPtr)
      IntPtrSecurity.Principal.WindowsIdentity::GetUserTokenString)
    }
}
