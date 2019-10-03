// Class info from mscorlib.dll
// 
using UnityEngine;

namespace Mono.Security.X509
{
    class X501 : Object
    {
      // Fields:
  countryName : Byte[]
  organizationName : Byte[]
  organizationalUnitName : Byte[]
  commonName : Byte[]
  localityName : Byte[]
  stateOrProvinceName : Byte[]
  streetAddress : Byte[]
  domainComponent : Byte[]
  userid : Byte[]
  email : Byte[]
  dnQualifier : Byte[]
  title : Byte[]
  surname : Byte[]
  givenName : Byte[]
  initial : Byte[]
  <>f__switch$map10 : Dictionary`2
      // Properties:
      // Events:
      // Methods:
      Void Mono.Security.X509.X501::.ctor()
      Void Mono.Security.X509.X501::.cctor()
      public String Mono.Security.X509.X501::ToString(Mono.Security.ASN1)
      public String Mono.Security.X509.X501::ToString(Mono.Security.ASN1BooleanStringBoolean)
      Void Mono.Security.X509.X501::AppendEntryText.StringBuilder,Mono.Security.ASN1Boolean)
      Mono.Security.X509.X520/AttributeTypeAndValue Mono.Security.X509.X501::GetAttributeFromOidString)
      Boolean Mono.Security.X509.X501::IsOidString)
      Mono.Security.X509.X520/AttributeTypeAndValue Mono.Security.X509.X501::ReadAttributeStringInt32&)
      Boolean Mono.Security.X509.X501::IsHexChar)
      String Mono.Security.X509.X501::ReadHexStringInt32&)
      Int32 Mono.Security.X509.X501::ReadEscapedText.StringBuilderStringInt32)
      Int32 Mono.Security.X509.X501::ReadQuotedText.StringBuilderStringInt32)
      String Mono.Security.X509.X501::ReadValueStringInt32&)
      public Mono.Security.ASN1 Mono.Security.X509.X501::FromStringString)
    }
}
