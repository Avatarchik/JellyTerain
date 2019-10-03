// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Runtime.InteropServices
{
    public class SafeHandle : CriticalFinalizerObject
    {
      // Fields:
  handle : IntPtr
  invalid_handle_value : IntPtr
  refcount : Int32
  owns_handle : Boolean
      // Properties:
  IsClosed : Boolean
  IsInvalid : Boolean
      // Events:
      // Methods:
      VoidRuntime.InteropServices.SafeHandle::.ctor()
      VoidRuntime.InteropServices.SafeHandle::.ctorIntPtrBoolean)
      public VoidRuntime.InteropServices.SafeHandle::Close()
      public VoidRuntime.InteropServices.SafeHandle::DangerousAddRefBoolean&)
      public IntPtrRuntime.InteropServices.SafeHandle::DangerousGetHandle()
      public VoidRuntime.InteropServices.SafeHandle::DangerousRelease()
      public VoidRuntime.InteropServices.SafeHandle::Dispose()
      public VoidRuntime.InteropServices.SafeHandle::SetHandleAsInvalid()
      VoidRuntime.InteropServices.SafeHandle::DisposeBoolean)
      BooleanRuntime.InteropServices.SafeHandle::ReleaseHandle()
      VoidRuntime.InteropServices.SafeHandle::SetHandleIntPtr)
      public BooleanRuntime.InteropServices.SafeHandle::get_IsClosed()
      public BooleanRuntime.InteropServices.SafeHandle::get_IsInvalid()
      VoidRuntime.InteropServices.SafeHandle::Finalize()
    }
}
