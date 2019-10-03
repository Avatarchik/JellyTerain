// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Threading
{
    public class Thread : CriticalFinalizerObject
    {
      // Fields:
  lock_thread_id : Int32
  system_thread_handle : IntPtr
  cached_culture_info : Object
  unused0 : IntPtr
  threadpool_thread : Boolean
  name : IntPtr
  name_len : Int32
  state : ThreadState
  abort_exc : Object
  abort_state_handle : Int32
  thread_id : Int64
  start_notify : IntPtr
  stack_ptr : IntPtr
  static_data : UIntPtr
  jit_data : IntPtr
  lock_data : IntPtr
  current_appcontext : Object
  stack_size : Int32
  start_obj : Object
  appdomain_refs : IntPtr
  interruption_requested : Int32
  suspend_event : IntPtr
  suspended_event : IntPtr
  resume_event : IntPtr
  synch_cs : IntPtr
  serialized_culture_info : IntPtr
  serialized_culture_info_len : Int32
  serialized_ui_culture_info : IntPtr
  serialized_ui_culture_info_len : Int32
  thread_dump_requested : Boolean
  end_stack : IntPtr
  thread_interrupt_requested : Boolean
  apartment_state : Byte
  critical_region_level : Int32 modreq(System.Runtime.CompilerServices.IsVolatile)
  small_id : Int32
  manage_callback : IntPtr
  pending_exception : Object
  ec_to_set : ExecutionContext
  interrupt_on_stop : IntPtr
  unused3 : IntPtr
  unused4 : IntPtr
  unused5 : IntPtr
  unused6 : IntPtr
  local_slots : Object[]
  _ec : ExecutionContext
  threadstart : MulticastDelegate
  managed_id : Int32
  _principal : IPrincipal
  datastorehash : Hashtable
  datastore_lock : Object
  in_currentculture : Boolean
  culture_lock : Object
      // Properties:
  CurrentContext : Context
  CurrentPrincipal : IPrincipal
  CurrentThread : Thread
  CurrentThreadId : Int32
  ApartmentState : ApartmentState
  CurrentCulture : CultureInfo
  CurrentUICulture : CultureInfo
  IsThreadPoolThread : Boolean
  IsThreadPoolThreadInternal : Boolean
  IsAlive : Boolean
  IsBackground : Boolean
  Name : String
  Priority : ThreadPriority
  ThreadState : ThreadState
  ExecutionContext : ExecutionContext
  ManagedThreadId : Int32
      // Events:
      // Methods:
      public VoidThreading.Thread::.ctorThreading.ThreadStart)
      public VoidThreading.Thread::.ctorThreading.ThreadStartInt32)
      public VoidThreading.Thread::.ctorThreading.ParameterizedThreadStart)
      public VoidThreading.Thread::.ctorThreading.ParameterizedThreadStartInt32)
      VoidThreading.Thread::.cctor()
      VoidThreading.Thread::System.Runtime.InteropServices._Thread.GetIDsOfNamesGuid&IntPtrUInt32UInt32IntPtr)
      VoidThreading.Thread::System.Runtime.InteropServices._Thread.GetTypeInfoUInt32UInt32IntPtr)
      VoidThreading.Thread::System.Runtime.InteropServices._Thread.GetTypeInfoCountUInt32&)
      VoidThreading.Thread::System.Runtime.InteropServices._Thread.InvokeUInt32Guid&UInt32Int16IntPtrIntPtrIntPtrIntPtr)
      public Runtime.Remoting.Contexts.ContextThreading.Thread::get_CurrentContext()
      public Security.Principal.IPrincipalThreading.Thread::get_CurrentPrincipal()
      public VoidThreading.Thread::set_CurrentPrincipalSecurity.Principal.IPrincipal)
      Threading.ThreadThreading.Thread::CurrentThread_internal()
      public Threading.ThreadThreading.Thread::get_CurrentThread()
      Int32Threading.Thread::get_CurrentThreadId()
      VoidThreading.Thread::InitDataStoreHash()
      public LocalDataStoreSlotThreading.Thread::AllocateNamedDataSlotString)
      public VoidThreading.Thread::FreeNamedDataSlotString)
      public LocalDataStoreSlotThreading.Thread::AllocateDataSlot()
      public ObjectThreading.Thread::GetDataLocalDataStoreSlot)
      public VoidThreading.Thread::SetDataLocalDataStoreSlotObject)
      VoidThreading.Thread::FreeLocalSlotValuesInt32Boolean)
      public LocalDataStoreSlotThreading.Thread::GetNamedDataSlotString)
      public AppDomainThreading.Thread::GetDomain()
      public Int32Threading.Thread::GetDomainID()
      VoidThreading.Thread::ResetAbort_internal()
      public VoidThreading.Thread::ResetAbort()
      VoidThreading.Thread::Sleep_internalInt32)
      public VoidThreading.Thread::SleepInt32)
      public VoidThreading.Thread::SleepTimeSpan)
      IntPtrThreading.Thread::Thread_internalMulticastDelegate)
      VoidThreading.Thread::Thread_init()
      public Threading.ApartmentStateThreading.Thread::get_ApartmentState()
      public VoidThreading.Thread::set_ApartmentStateThreading.ApartmentState)
      Globalization.CultureInfoThreading.Thread::GetCachedCurrentCulture()
      Byte[]Threading.Thread::GetSerializedCurrentCulture()
      VoidThreading.Thread::SetCachedCurrentCultureGlobalization.CultureInfo)
      VoidThreading.Thread::SetSerializedCurrentCultureByte[])
      Globalization.CultureInfoThreading.Thread::GetCachedCurrentUICulture()
      Byte[]Threading.Thread::GetSerializedCurrentUICulture()
      VoidThreading.Thread::SetCachedCurrentUICultureGlobalization.CultureInfo)
      VoidThreading.Thread::SetSerializedCurrentUICultureByte[])
      public Globalization.CultureInfoThreading.Thread::get_CurrentCulture()
      public VoidThreading.Thread::set_CurrentCultureGlobalization.CultureInfo)
      public Globalization.CultureInfoThreading.Thread::get_CurrentUICulture()
      public VoidThreading.Thread::set_CurrentUICultureGlobalization.CultureInfo)
      public BooleanThreading.Thread::get_IsThreadPoolThread()
      BooleanThreading.Thread::get_IsThreadPoolThreadInternal()
      VoidThreading.Thread::set_IsThreadPoolThreadInternalBoolean)
      public BooleanThreading.Thread::get_IsAlive()
      public BooleanThreading.Thread::get_IsBackground()
      public VoidThreading.Thread::set_IsBackgroundBoolean)
      StringThreading.Thread::GetName_internal()
      VoidThreading.Thread::SetName_internalString)
      public StringThreading.Thread::get_Name()
      public VoidThreading.Thread::set_NameString)
      public Threading.ThreadPriorityThreading.Thread::get_Priority()
      public VoidThreading.Thread::set_PriorityThreading.ThreadPriority)
      public Threading.ThreadStateThreading.Thread::get_ThreadState()
      VoidThreading.Thread::Abort_internalObject)
      public VoidThreading.Thread::Abort()
      public VoidThreading.Thread::AbortObject)
      ObjectThreading.Thread::GetAbortExceptionState()
      VoidThreading.Thread::Interrupt_internal()
      public VoidThreading.Thread::Interrupt()
      BooleanThreading.Thread::Join_internalInt32IntPtr)
      public VoidThreading.Thread::Join()
      public BooleanThreading.Thread::JoinInt32)
      public BooleanThreading.Thread::JoinTimeSpan)
      public VoidThreading.Thread::MemoryBarrier()
      VoidThreading.Thread::Resume_internal()
      public VoidThreading.Thread::Resume()
      VoidThreading.Thread::SpinWait_nop()
      public VoidThreading.Thread::SpinWaitInt32)
      public VoidThreading.Thread::Start()
      VoidThreading.Thread::Suspend_internal()
      public VoidThreading.Thread::Suspend()
      VoidThreading.Thread::Thread_free_internalIntPtr)
      VoidThreading.Thread::Finalize()
      VoidThreading.Thread::SetStateThreading.ThreadState)
      VoidThreading.Thread::ClrStateThreading.ThreadState)
      Threading.ThreadStateThreading.Thread::GetState()
      public ByteThreading.Thread::VolatileReadByte&)
      public DoubleThreading.Thread::VolatileReadDouble&)
      public Int16Threading.Thread::VolatileReadInt16&)
      public Int32Threading.Thread::VolatileReadInt32&)
      public Int64Threading.Thread::VolatileReadInt64&)
      public IntPtrThreading.Thread::VolatileReadIntPtr&)
      public ObjectThreading.Thread::VolatileReadObject&)
      public SByteThreading.Thread::VolatileReadSByte&)
      public SingleThreading.Thread::VolatileReadSingle&)
      public UInt16Threading.Thread::VolatileReadUInt16&)
      public UInt32Threading.Thread::VolatileReadUInt32&)
      public UInt64Threading.Thread::VolatileReadUInt64&)
      public UIntPtrThreading.Thread::VolatileReadUIntPtr&)
      public VoidThreading.Thread::VolatileWriteByte&Byte)
      public VoidThreading.Thread::VolatileWriteDouble&Double)
      public VoidThreading.Thread::VolatileWriteInt16&Int16)
      public VoidThreading.Thread::VolatileWriteInt32&Int32)
      public VoidThreading.Thread::VolatileWriteInt64&Int64)
      public VoidThreading.Thread::VolatileWriteIntPtr&IntPtr)
      public VoidThreading.Thread::VolatileWriteObject&Object)
      public VoidThreading.Thread::VolatileWriteSByte&SByte)
      public VoidThreading.Thread::VolatileWriteSingle&Single)
      public VoidThreading.Thread::VolatileWriteUInt16&UInt16)
      public VoidThreading.Thread::VolatileWriteUInt32&UInt32)
      public VoidThreading.Thread::VolatileWriteUInt64&UInt64)
      public VoidThreading.Thread::VolatileWriteUIntPtr&UIntPtr)
      Int32Threading.Thread::GetNewManagedId()
      Int32Threading.Thread::GetNewManagedId_internal()
      public Threading.ExecutionContextThreading.Thread::get_ExecutionContext()
      public Int32Threading.Thread::get_ManagedThreadId()
      public VoidThreading.Thread::BeginCriticalRegion()
      public VoidThreading.Thread::EndCriticalRegion()
      public VoidThreading.Thread::BeginThreadAffinity()
      public VoidThreading.Thread::EndThreadAffinity()
      public Threading.ApartmentStateThreading.Thread::GetApartmentState()
      public VoidThreading.Thread::SetApartmentStateThreading.ApartmentState)
      public BooleanThreading.Thread::TrySetApartmentStateThreading.ApartmentState)
      public Int32Threading.Thread::GetHashCode()
      public VoidThreading.Thread::StartObject)
      public Threading.CompressedStackThreading.Thread::GetCompressedStack()
      public VoidThreading.Thread::SetCompressedStackThreading.CompressedStack)
    }
}
