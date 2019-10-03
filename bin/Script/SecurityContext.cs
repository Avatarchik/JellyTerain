// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Security
{
    public class SecurityContext : Object
    {
      // Fields:
  _capture : Boolean
  _winid : IntPtr
  _stack : CompressedStack
  _suppressFlowWindowsIdentity : Boolean
  _suppressFlow : Boolean
      // Properties:
  FlowSuppressed : Boolean
  WindowsIdentityFlowSuppressed : Boolean
  CompressedStack : CompressedStack
  IdentityToken : IntPtr
      // Events:
      // Methods:
      VoidSecurity.SecurityContext::.ctor()
      VoidSecurity.SecurityContext::.ctorSecurity.SecurityContext)
      public Security.SecurityContextSecurity.SecurityContext::CreateCopy()
      public Security.SecurityContextSecurity.SecurityContext::Capture()
      BooleanSecurity.SecurityContext::get_FlowSuppressed()
      VoidSecurity.SecurityContext::set_FlowSuppressedBoolean)
      BooleanSecurity.SecurityContext::get_WindowsIdentityFlowSuppressed()
      VoidSecurity.SecurityContext::set_WindowsIdentityFlowSuppressedBoolean)
      Threading.CompressedStackSecurity.SecurityContext::get_CompressedStack()
      VoidSecurity.SecurityContext::set_CompressedStackThreading.CompressedStack)
      IntPtrSecurity.SecurityContext::get_IdentityToken()
      VoidSecurity.SecurityContext::set_IdentityTokenIntPtr)
      public BooleanSecurity.SecurityContext::IsFlowSuppressed()
      public BooleanSecurity.SecurityContext::IsWindowsIdentityFlowSuppressed()
      public VoidSecurity.SecurityContext::RestoreFlow()
      public VoidSecurity.SecurityContext::RunSecurity.SecurityContextThreading.ContextCallbackObject)
      public Threading.AsyncFlowControlSecurity.SecurityContext::SuppressFlow()
      public Threading.AsyncFlowControlSecurity.SecurityContext::SuppressFlowWindowsIdentity()
    }
}
