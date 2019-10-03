// Class info from System.dll
// 
using UnityEngine;

namespace System.Net
{
    public class HttpListenerContext : Object
    {
      // Fields:
  request : HttpListenerRequest
  response : HttpListenerResponse
  user : IPrincipal
  cnc : HttpConnection
  error : String
  err_status : Int32
  Listener : HttpListener
      // Properties:
  ErrorStatus : Int32
  ErrorMessage : String
  HaveError : Boolean
  Connection : HttpConnection
  Request : HttpListenerRequest
  Response : HttpListenerResponse
  User : IPrincipal
      // Events:
      // Methods:
      VoidNet.HttpListenerContext::.ctorNet.HttpConnection)
      Int32Net.HttpListenerContext::get_ErrorStatus()
      VoidNet.HttpListenerContext::set_ErrorStatusInt32)
      StringNet.HttpListenerContext::get_ErrorMessage()
      VoidNet.HttpListenerContext::set_ErrorMessageString)
      BooleanNet.HttpListenerContext::get_HaveError()
      Net.HttpConnectionNet.HttpListenerContext::get_Connection()
      public Net.HttpListenerRequestNet.HttpListenerContext::get_Request()
      public Net.HttpListenerResponseNet.HttpListenerContext::get_Response()
      public Security.Principal.IPrincipalNet.HttpListenerContext::get_User()
      VoidNet.HttpListenerContext::ParseAuthenticationNet.AuthenticationSchemes)
      Security.Principal.IPrincipalNet.HttpListenerContext::ParseBasicAuthenticationString)
    }
}
