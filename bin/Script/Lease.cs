// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Runtime.Remoting.Lifetime
{
    class Lease : MarshalByRefObject
    {
      // Fields:
  _leaseExpireTime : DateTime
  _currentState : LeaseState
  _initialLeaseTime : TimeSpan
  _renewOnCallTime : TimeSpan
  _sponsorshipTimeout : TimeSpan
  _sponsors : ArrayList
  _renewingSponsors : Queue
  _renewalDelegate : RenewalDelegate
      // Properties:
  CurrentLeaseTime : TimeSpan
  CurrentState : LeaseState
  InitialLeaseTime : TimeSpan
  RenewOnCallTime : TimeSpan
  SponsorshipTimeout : TimeSpan
      // Events:
      // Methods:
      public VoidRuntime.Remoting.Lifetime.Lease::.ctor()
      public TimeSpanRuntime.Remoting.Lifetime.Lease::get_CurrentLeaseTime()
      public Runtime.Remoting.Lifetime.LeaseStateRuntime.Remoting.Lifetime.Lease::get_CurrentState()
      public VoidRuntime.Remoting.Lifetime.Lease::Activate()
      public TimeSpanRuntime.Remoting.Lifetime.Lease::get_InitialLeaseTime()
      public VoidRuntime.Remoting.Lifetime.Lease::set_InitialLeaseTimeTimeSpan)
      public TimeSpanRuntime.Remoting.Lifetime.Lease::get_RenewOnCallTime()
      public VoidRuntime.Remoting.Lifetime.Lease::set_RenewOnCallTimeTimeSpan)
      public TimeSpanRuntime.Remoting.Lifetime.Lease::get_SponsorshipTimeout()
      public VoidRuntime.Remoting.Lifetime.Lease::set_SponsorshipTimeoutTimeSpan)
      public VoidRuntime.Remoting.Lifetime.Lease::RegisterRuntime.Remoting.Lifetime.ISponsor)
      public VoidRuntime.Remoting.Lifetime.Lease::RegisterRuntime.Remoting.Lifetime.ISponsorTimeSpan)
      public TimeSpanRuntime.Remoting.Lifetime.Lease::RenewTimeSpan)
      public VoidRuntime.Remoting.Lifetime.Lease::UnregisterRuntime.Remoting.Lifetime.ISponsor)
      VoidRuntime.Remoting.Lifetime.Lease::UpdateState()
      VoidRuntime.Remoting.Lifetime.Lease::CheckNextSponsor()
      VoidRuntime.Remoting.Lifetime.Lease::ProcessSponsorResponseObjectBoolean)
    }
}
