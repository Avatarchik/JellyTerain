// Class info from System.dll
// 
using UnityEngine;

namespace System.Security.Cryptography.X509Certificates
{
    public class X509BasicConstraintsExtension : X509Extension
    {
      // Fields:
  oid : String
  friendlyName : String
  _certificateAuthority : Boolean
  _hasPathLengthConstraint : Boolean
  _pathLengthConstraint : Int32
  _status : AsnDecodeStatus
      // Properties:
  CertificateAuthority : Boolean
  HasPathLengthConstraint : Boolean
  PathLengthConstraint : Int32
      // Events:
      // Methods:
      public VoidSecurity.Cryptography.X509Certificates.X509BasicConstraintsExtension::.ctor()
      public VoidSecurity.Cryptography.X509Certificates.X509BasicConstraintsExtension::.ctorSecurity.Cryptography.AsnEncodedDataBoolean)
      public VoidSecurity.Cryptography.X509Certificates.X509BasicConstraintsExtension::.ctorBooleanBooleanInt32Boolean)
      public BooleanSecurity.Cryptography.X509Certificates.X509BasicConstraintsExtension::get_CertificateAuthority()
      public BooleanSecurity.Cryptography.X509Certificates.X509BasicConstraintsExtension::get_HasPathLengthConstraint()
      public Int32Security.Cryptography.X509Certificates.X509BasicConstraintsExtension::get_PathLengthConstraint()
      public VoidSecurity.Cryptography.X509Certificates.X509BasicConstraintsExtension::CopyFromSecurity.Cryptography.AsnEncodedData)
      Security.Cryptography.AsnDecodeStatusSecurity.Cryptography.X509Certificates.X509BasicConstraintsExtension::DecodeByte[])
      Byte[]Security.Cryptography.X509Certificates.X509BasicConstraintsExtension::Encode()
      StringSecurity.Cryptography.X509Certificates.X509BasicConstraintsExtension::ToStringBoolean)
    }
}
