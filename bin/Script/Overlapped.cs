// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Threading
{
    public class Overlapped : Object
    {
      // Fields:
  ares : IAsyncResult
  offsetL : Int32
  offsetH : Int32
  evt : Int32
  evt_ptr : IntPtr
      // Properties:
  AsyncResult : IAsyncResult
  EventHandle : Int32
  EventHandleIntPtr : IntPtr
  OffsetHigh : Int32
  OffsetLow : Int32
      // Events:
      // Methods:
      public VoidThreading.Overlapped::.ctor()
      public VoidThreading.Overlapped::.ctorInt32Int32Int32IAsyncResult)
      public VoidThreading.Overlapped::.ctorInt32Int32IntPtrIAsyncResult)
      public VoidThreading.Overlapped::FreeThreading.NativeOverlapped*)
      public Threading.OverlappedThreading.Overlapped::UnpackThreading.NativeOverlapped*)
      public Threading.NativeOverlapped*Threading.Overlapped::PackThreading.IOCompletionCallback)
      public Threading.NativeOverlapped*Threading.Overlapped::PackThreading.IOCompletionCallbackObject)
      public Threading.NativeOverlapped*Threading.Overlapped::UnsafePackThreading.IOCompletionCallback)
      public Threading.NativeOverlapped*Threading.Overlapped::UnsafePackThreading.IOCompletionCallbackObject)
      public IAsyncResultThreading.Overlapped::get_AsyncResult()
      public VoidThreading.Overlapped::set_AsyncResultIAsyncResult)
      public Int32Threading.Overlapped::get_EventHandle()
      public VoidThreading.Overlapped::set_EventHandleInt32)
      public IntPtrThreading.Overlapped::get_EventHandleIntPtr()
      public VoidThreading.Overlapped::set_EventHandleIntPtrIntPtr)
      public Int32Threading.Overlapped::get_OffsetHigh()
      public VoidThreading.Overlapped::set_OffsetHighInt32)
      public Int32Threading.Overlapped::get_OffsetLow()
      public VoidThreading.Overlapped::set_OffsetLowInt32)
    }
}
