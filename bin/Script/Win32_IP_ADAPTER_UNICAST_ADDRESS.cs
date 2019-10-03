// Class info from System.dll
// 
using UnityEngine;

namespace System.Net.NetworkInformation
{
    class Win32_IP_ADAPTER_UNICAST_ADDRESS : ValueType
    {
      // Fields:
  public LengthFlags : Win32LengthFlagsUnion
  public Next : IntPtr
  public Address : Win32_SOCKET_ADDRESS
  public PrefixOrigin : PrefixOrigin
  public SuffixOrigin : SuffixOrigin
  public DadState : DuplicateAddressDetectionState
  public ValidLifetime : UInt32
  public PreferredLifetime : UInt32
  public LeaseLifetime : UInt32
  public OnLinkPrefixLength : Byte
      // Properties:
      // Events:
      // Methods:
    }
}
