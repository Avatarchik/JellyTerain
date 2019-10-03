// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Security.Policy
{
    public class PolicyLevel : Object
    {
      // Fields:
  label : String
  root_code_group : CodeGroup
  full_trust_assemblies : ArrayList
  named_permission_sets : ArrayList
  _location : String
  _type : PolicyLevelType
  fullNames : Hashtable
  xml : SecurityElement
      // Properties:
  FullTrustAssemblies : IList
  Label : String
  NamedPermissionSets : IList
  RootCodeGroup : CodeGroup
  StoreLocation : String
  Type : PolicyLevelType
      // Events:
      // Methods:
      VoidSecurity.Policy.PolicyLevel::.ctorStringSecurity.PolicyLevelType)
      VoidSecurity.Policy.PolicyLevel::LoadFromFileString)
      VoidSecurity.Policy.PolicyLevel::LoadFromStringString)
      Security.SecurityElementSecurity.Policy.PolicyLevel::FromStringString)
      public Collections.IListSecurity.Policy.PolicyLevel::get_FullTrustAssemblies()
      public StringSecurity.Policy.PolicyLevel::get_Label()
      public Collections.IListSecurity.Policy.PolicyLevel::get_NamedPermissionSets()
      public Security.Policy.CodeGroupSecurity.Policy.PolicyLevel::get_RootCodeGroup()
      public VoidSecurity.Policy.PolicyLevel::set_RootCodeGroupSecurity.Policy.CodeGroup)
      public StringSecurity.Policy.PolicyLevel::get_StoreLocation()
      public Security.PolicyLevelTypeSecurity.Policy.PolicyLevel::get_Type()
      public VoidSecurity.Policy.PolicyLevel::AddFullTrustAssemblySecurity.Policy.StrongName)
      public VoidSecurity.Policy.PolicyLevel::AddFullTrustAssemblySecurity.Policy.StrongNameMembershipCondition)
      public VoidSecurity.Policy.PolicyLevel::AddNamedPermissionSetSecurity.NamedPermissionSet)
      public Security.NamedPermissionSetSecurity.Policy.PolicyLevel::ChangeNamedPermissionSetStringSecurity.PermissionSet)
      public Security.Policy.PolicyLevelSecurity.Policy.PolicyLevel::CreateAppDomainLevel()
      public VoidSecurity.Policy.PolicyLevel::FromXmlSecurity.SecurityElement)
      public Security.NamedPermissionSetSecurity.Policy.PolicyLevel::GetNamedPermissionSetString)
      public VoidSecurity.Policy.PolicyLevel::Recover()
      public VoidSecurity.Policy.PolicyLevel::RemoveFullTrustAssemblySecurity.Policy.StrongName)
      public VoidSecurity.Policy.PolicyLevel::RemoveFullTrustAssemblySecurity.Policy.StrongNameMembershipCondition)
      public Security.NamedPermissionSetSecurity.Policy.PolicyLevel::RemoveNamedPermissionSetSecurity.NamedPermissionSet)
      public Security.NamedPermissionSetSecurity.Policy.PolicyLevel::RemoveNamedPermissionSetString)
      public VoidSecurity.Policy.PolicyLevel::Reset()
      public Security.Policy.PolicyStatementSecurity.Policy.PolicyLevel::ResolveSecurity.Policy.Evidence)
      public Security.Policy.CodeGroupSecurity.Policy.PolicyLevel::ResolveMatchingCodeGroupsSecurity.Policy.Evidence)
      public Security.SecurityElementSecurity.Policy.PolicyLevel::ToXml()
      VoidSecurity.Policy.PolicyLevel::Save()
      VoidSecurity.Policy.PolicyLevel::CreateDefaultLevelSecurity.PolicyLevelType)
      VoidSecurity.Policy.PolicyLevel::CreateDefaultFullTrustAssemblies()
      VoidSecurity.Policy.PolicyLevel::CreateDefaultNamedPermissionSets()
      StringSecurity.Policy.PolicyLevel::ResolveClassNameString)
      BooleanSecurity.Policy.PolicyLevel::IsFullTrustAssemblyReflection.Assembly)
    }
}
