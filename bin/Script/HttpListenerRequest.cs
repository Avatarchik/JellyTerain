// Class info from System.dll
// 
using UnityEngine;

namespace System.Net
{
    public class HttpListenerRequest : Object
    {
      // Fields:
  accept_types : String[]
  content_encoding : Encoding
  content_length : Int64
  cl_set : Boolean
  cookies : CookieCollection
  headers : WebHeaderCollection
  method : String
  input_stream : Stream
  version : Version
  query_string : NameValueCollection
  raw_url : String
  identifier : Guid
  url : Uri
  referrer : Uri
  user_languages : String[]
  context : HttpListenerContext
  is_chunked : Boolean
  _100continue : Byte[]
  no_body_methods : String[]
  separators : Char[]
  <>f__switch$map7 : Dictionary`2
      // Properties:
  AcceptTypes : String[]
  ClientCertificateError : Int32
  ContentEncoding : Encoding
  ContentLength64 : Int64
  ContentType : String
  Cookies : CookieCollection
  HasEntityBody : Boolean
  Headers : NameValueCollection
  HttpMethod : String
  InputStream : Stream
  IsAuthenticated : Boolean
  IsLocal : Boolean
  IsSecureConnection : Boolean
  KeepAlive : Boolean
  LocalEndPoint : IPEndPoint
  ProtocolVersion : Version
  QueryString : NameValueCollection
  RawUrl : String
  RemoteEndPoint : IPEndPoint
  RequestTraceIdentifier : Guid
  Url : Uri
  UrlReferrer : Uri
  UserAgent : String
  UserHostAddress : String
  UserHostName : String
  UserLanguages : String[]
      // Events:
      // Methods:
      VoidNet.HttpListenerRequest::.ctorNet.HttpListenerContext)
      VoidNet.HttpListenerRequest::.cctor()
      VoidNet.HttpListenerRequest::SetRequestLineString)
      VoidNet.HttpListenerRequest::CreateQueryStringString)
      VoidNet.HttpListenerRequest::FinishInitialization()
      StringNet.HttpListenerRequest::UnquoteString)
      VoidNet.HttpListenerRequest::AddHeaderString)
      BooleanNet.HttpListenerRequest::FlushInput()
      public String[]Net.HttpListenerRequest::get_AcceptTypes()
      public Int32Net.HttpListenerRequest::get_ClientCertificateError()
      public Text.EncodingNet.HttpListenerRequest::get_ContentEncoding()
      public Int64Net.HttpListenerRequest::get_ContentLength64()
      public StringNet.HttpListenerRequest::get_ContentType()
      public Net.CookieCollectionNet.HttpListenerRequest::get_Cookies()
      public BooleanNet.HttpListenerRequest::get_HasEntityBody()
      public Collections.Specialized.NameValueCollectionNet.HttpListenerRequest::get_Headers()
      public StringNet.HttpListenerRequest::get_HttpMethod()
      public IO.StreamNet.HttpListenerRequest::get_InputStream()
      public BooleanNet.HttpListenerRequest::get_IsAuthenticated()
      public BooleanNet.HttpListenerRequest::get_IsLocal()
      public BooleanNet.HttpListenerRequest::get_IsSecureConnection()
      public BooleanNet.HttpListenerRequest::get_KeepAlive()
      public Net.IPEndPointNet.HttpListenerRequest::get_LocalEndPoint()
      public VersionNet.HttpListenerRequest::get_ProtocolVersion()
      public Collections.Specialized.NameValueCollectionNet.HttpListenerRequest::get_QueryString()
      public StringNet.HttpListenerRequest::get_RawUrl()
      public Net.IPEndPointNet.HttpListenerRequest::get_RemoteEndPoint()
      public GuidNet.HttpListenerRequest::get_RequestTraceIdentifier()
      public UriNet.HttpListenerRequest::get_Url()
      public UriNet.HttpListenerRequest::get_UrlReferrer()
      public StringNet.HttpListenerRequest::get_UserAgent()
      public StringNet.HttpListenerRequest::get_UserHostAddress()
      public StringNet.HttpListenerRequest::get_UserHostName()
      public String[]Net.HttpListenerRequest::get_UserLanguages()
      public IAsyncResultNet.HttpListenerRequest::BeginGetClientCertificateAsyncCallbackObject)
      public Security.Cryptography.X509Certificates.X509Certificate2Net.HttpListenerRequest::EndGetClientCertificateIAsyncResult)
      public Security.Cryptography.X509Certificates.X509Certificate2Net.HttpListenerRequest::GetClientCertificate()
    }
}
