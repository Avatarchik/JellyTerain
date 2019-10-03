// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Security
{
    public class SecurityManager : Object
    {
      // Fields:
  _lockObject : Object
  _hierarchy : ArrayList
  _unmanagedCode : IPermission
  _declsecCache : Hashtable
  _level : PolicyLevel
  _execution : SecurityPermission
      // Properties:
  CheckExecutionRights : Boolean
  SecurityEnabled : Boolean
  Hierarchy : IEnumerator
  ResolvingPolicyLevel : PolicyLevel
  UnmanagedCode : IPermission
      // Events:
      // Methods:
      VoidSecurity.SecurityManager::.cctor()
      public BooleanSecurity.SecurityManager::get_CheckExecutionRights()
      public VoidSecurity.SecurityManager::set_CheckExecutionRightsBoolean)
      public BooleanSecurity.SecurityManager::get_SecurityEnabled()
      public VoidSecurity.SecurityManager::set_SecurityEnabledBoolean)
      public VoidSecurity.SecurityManager::GetZoneAndOriginCollections.ArrayList&Collections.ArrayList&)
      public BooleanSecurity.SecurityManager::IsGrantedSecurity.IPermission)
      BooleanSecurity.SecurityManager::IsGrantedReflection.AssemblySecurity.IPermission)
      Security.IPermissionSecurity.SecurityManager::CheckPermissionSetReflection.AssemblySecurity.PermissionSetBoolean)
      Security.IPermissionSecurity.SecurityManager::CheckPermissionSetAppDomainSecurity.PermissionSet)
      public Security.Policy.PolicyLevelSecurity.SecurityManager::LoadPolicyLevelFromFileStringSecurity.PolicyLevelType)
      public Security.Policy.PolicyLevelSecurity.SecurityManager::LoadPolicyLevelFromStringStringSecurity.PolicyLevelType)
      public Collections.IEnumeratorSecurity.SecurityManager::PolicyHierarchy()
      public Security.PermissionSetSecurity.SecurityManager::ResolvePolicySecurity.Policy.Evidence)
      public Security.PermissionSetSecurity.SecurityManager::ResolvePolicySecurity.Policy.Evidence[])
      public Security.PermissionSetSecurity.SecurityManager::ResolveSystemPolicySecurity.Policy.Evidence)
      public Security.PermissionSetSecurity.SecurityManager::ResolvePolicySecurity.Policy.EvidenceSecurity.PermissionSetSecurity.PermissionSetSecurity.PermissionSetSecurity.PermissionSet&)
      public Collections.IEnumeratorSecurity.SecurityManager::ResolvePolicyGroupsSecurity.Policy.Evidence)
      public VoidSecurity.SecurityManager::SavePolicy()
      public VoidSecurity.SecurityManager::SavePolicyLevelSecurity.Policy.PolicyLevel)
      Collections.IEnumeratorSecurity.SecurityManager::get_Hierarchy()
      VoidSecurity.SecurityManager::InitializePolicyHierarchy()
      BooleanSecurity.SecurityManager::ResolvePolicyLevelSecurity.PermissionSet&Security.Policy.PolicyLevelSecurity.Policy.Evidence)
      VoidSecurity.SecurityManager::ResolveIdentityPermissionsSecurity.PermissionSetSecurity.Policy.Evidence)
      Security.Policy.PolicyLevelSecurity.SecurityManager::get_ResolvingPolicyLevel()
      VoidSecurity.SecurityManager::set_ResolvingPolicyLevelSecurity.Policy.PolicyLevel)
      Security.PermissionSetSecurity.SecurityManager::DecodeIntPtrInt32)
      Security.PermissionSetSecurity.SecurityManager::DecodeByte[])
      Security.IPermissionSecurity.SecurityManager::get_UnmanagedCode()
      BooleanSecurity.SecurityManager::GetLinkDemandSecurityReflection.MethodBaseSecurity.RuntimeDeclSecurityActions*Security.RuntimeDeclSecurityActions*)
      VoidSecurity.SecurityManager::ReflectedLinkDemandInvokeReflection.MethodBase)
      BooleanSecurity.SecurityManager::ReflectedLinkDemandQueryReflection.MethodBase)
      BooleanSecurity.SecurityManager::LinkDemandReflection.AssemblySecurity.RuntimeDeclSecurityActions*Security.RuntimeDeclSecurityActions*)
      BooleanSecurity.SecurityManager::LinkDemandFullTrustReflection.Assembly)
      BooleanSecurity.SecurityManager::LinkDemandUnmanagedReflection.Assembly)
      VoidSecurity.SecurityManager::LinkDemandSecurityExceptionInt32IntPtr)
      VoidSecurity.SecurityManager::InheritanceDemandSecurityExceptionInt32Reflection.AssemblyTypeReflection.MethodInfo)
      VoidSecurity.SecurityManager::ThrowExceptionException)
      BooleanSecurity.SecurityManager::InheritanceDemandAppDomainReflection.AssemblySecurity.RuntimeDeclSecurityActions*)
      VoidSecurity.SecurityManager::DemandUnmanaged()
      VoidSecurity.SecurityManager::InternalDemandIntPtrInt32)
      VoidSecurity.SecurityManager::InternalDemandChoiceIntPtrInt32)
    }
}
