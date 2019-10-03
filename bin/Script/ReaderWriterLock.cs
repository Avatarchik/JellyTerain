// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Threading
{
    public class ReaderWriterLock : CriticalFinalizerObject
    {
      // Fields:
  seq_num : Int32
  state : Int32
  readers : Int32
  writer_queue : LockQueue
  reader_locks : Hashtable
  writer_lock_owner : Int32
      // Properties:
  IsReaderLockHeld : Boolean
  IsWriterLockHeld : Boolean
  WriterSeqNum : Int32
      // Events:
      // Methods:
      public VoidThreading.ReaderWriterLock::.ctor()
      VoidThreading.ReaderWriterLock::Finalize()
      public BooleanThreading.ReaderWriterLock::get_IsReaderLockHeld()
      public BooleanThreading.ReaderWriterLock::get_IsWriterLockHeld()
      public Int32Threading.ReaderWriterLock::get_WriterSeqNum()
      public VoidThreading.ReaderWriterLock::AcquireReaderLockInt32)
      VoidThreading.ReaderWriterLock::AcquireReaderLockInt32Int32)
      public VoidThreading.ReaderWriterLock::AcquireReaderLockTimeSpan)
      public VoidThreading.ReaderWriterLock::AcquireWriterLockInt32)
      VoidThreading.ReaderWriterLock::AcquireWriterLockInt32Int32)
      public VoidThreading.ReaderWriterLock::AcquireWriterLockTimeSpan)
      public BooleanThreading.ReaderWriterLock::AnyWritersSinceInt32)
      public VoidThreading.ReaderWriterLock::DowngradeFromWriterLockThreading.LockCookie&)
      public Threading.LockCookieThreading.ReaderWriterLock::ReleaseLock()
      public VoidThreading.ReaderWriterLock::ReleaseReaderLock()
      VoidThreading.ReaderWriterLock::ReleaseReaderLockInt32Int32)
      public VoidThreading.ReaderWriterLock::ReleaseWriterLock()
      VoidThreading.ReaderWriterLock::ReleaseWriterLockInt32)
      public VoidThreading.ReaderWriterLock::RestoreLockThreading.LockCookie&)
      public Threading.LockCookieThreading.ReaderWriterLock::UpgradeToWriterLockInt32)
      public Threading.LockCookieThreading.ReaderWriterLock::UpgradeToWriterLockTimeSpan)
      Threading.LockCookieThreading.ReaderWriterLock::GetLockCookie()
      BooleanThreading.ReaderWriterLock::HasWriterLock()
      Int32Threading.ReaderWriterLock::CheckTimeoutTimeSpan)
    }
}
