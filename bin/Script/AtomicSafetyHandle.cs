// Class info from UnityEngine.dll
// 
using UnityEngine;

namespace UnityEngine
{
    class AtomicSafetyHandle : ValueType
    {
      // Fields:
  versionNode : IntPtr
  version : AtomicSafetyHandleVersionMask
      // Properties:
      // Events:
      // Methods:
      UnityEngine.AtomicSafetyHandle UnityEngine.AtomicSafetyHandle::Create()
      Void UnityEngine.AtomicSafetyHandle::Release(UnityEngine.AtomicSafetyHandle)
      Void UnityEngine.AtomicSafetyHandle::PrepareUndisposable(UnityEngine.AtomicSafetyHandle&)
      Void UnityEngine.AtomicSafetyHandle::UseSecondaryVersion(UnityEngine.AtomicSafetyHandle&)
      Void UnityEngine.AtomicSafetyHandle::BumpSecondaryVersion(UnityEngine.AtomicSafetyHandle&)
      Void UnityEngine.AtomicSafetyHandle::EnforceAllBufferJobsHaveCompletedAndRelease(UnityEngine.AtomicSafetyHandle)
      Void UnityEngine.AtomicSafetyHandle::CheckReadAndThrowNoEarlyOut(UnityEngine.AtomicSafetyHandle)
      Void UnityEngine.AtomicSafetyHandle::CheckWriteAndThrowNoEarlyOut(UnityEngine.AtomicSafetyHandle)
      Void UnityEngine.AtomicSafetyHandle::CheckDeallocateAndThrow(UnityEngine.AtomicSafetyHandle)
      Void UnityEngine.AtomicSafetyHandle::CheckReadAndThrow(UnityEngine.AtomicSafetyHandle)
      Void UnityEngine.AtomicSafetyHandle::CheckWriteAndThrow(UnityEngine.AtomicSafetyHandle)
      Void UnityEngine.AtomicSafetyHandle::Create_Injected(UnityEngine.AtomicSafetyHandle&)
      Void UnityEngine.AtomicSafetyHandle::Release_Injected(UnityEngine.AtomicSafetyHandle&)
      Void UnityEngine.AtomicSafetyHandle::EnforceAllBufferJobsHaveCompletedAndRelease_Injected(UnityEngine.AtomicSafetyHandle&)
      Void UnityEngine.AtomicSafetyHandle::CheckReadAndThrowNoEarlyOut_Injected(UnityEngine.AtomicSafetyHandle&)
      Void UnityEngine.AtomicSafetyHandle::CheckWriteAndThrowNoEarlyOut_Injected(UnityEngine.AtomicSafetyHandle&)
      Void UnityEngine.AtomicSafetyHandle::CheckDeallocateAndThrow_Injected(UnityEngine.AtomicSafetyHandle&)
    }
}
