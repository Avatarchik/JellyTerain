// Class info from System.dll
// 
using UnityEngine;

namespace System.Net.NetworkInformation
{
    class Win32_MIB_IFROW : ValueType
    {
      // Fields:
  MAX_INTERFACE_NAME_LEN : Int32
  MAXLEN_PHYSADDR : Int32
  MAXLEN_IFDESCR : Int32
  public Name : Char[]
  public Index : Int32
  public Type : NetworkInterfaceType
  public Mtu : Int32
  public Speed : UInt32
  public PhysAddrLen : Int32
  public PhysAddr : Byte[]
  public AdminStatus : UInt32
  public OperStatus : UInt32
  public LastChange : UInt32
  public InOctets : Int32
  public InUcastPkts : Int32
  public InNUcastPkts : Int32
  public InDiscards : Int32
  public InErrors : Int32
  public InUnknownProtos : Int32
  public OutOctets : Int32
  public OutUcastPkts : Int32
  public OutNUcastPkts : Int32
  public OutDiscards : Int32
  public OutErrors : Int32
  public OutQLen : Int32
  public DescrLen : Int32
  public Descr : Byte[]
      // Properties:
      // Events:
      // Methods:
    }
}
