// Class info from System.dll
// 
using UnityEngine;

namespace System.Net
{
    public class HttpListener : Object
    {
      // Fields:
  auth_schemes : AuthenticationSchemes
  prefixes : HttpListenerPrefixCollection
  auth_selector : AuthenticationSchemeSelector
  realm : String
  ignore_write_exceptions : Boolean
  unsafe_ntlm_auth : Boolean
  listening : Boolean
  disposed : Boolean
  registry : Hashtable
  ctx_queue : ArrayList
  wait_queue : ArrayList
      // Properties:
  AuthenticationSchemes : AuthenticationSchemes
  AuthenticationSchemeSelectorDelegate : AuthenticationSchemeSelector
  IgnoreWriteExceptions : Boolean
  IsListening : Boolean
  IsSupported : Boolean
  Prefixes : HttpListenerPrefixCollection
  Realm : String
  UnsafeConnectionNtlmAuthentication : Boolean
      // Events:
      // Methods:
      public VoidNet.HttpListener::.ctor()
      VoidNet.HttpListener::System.IDisposable.Dispose()
      public Net.AuthenticationSchemesNet.HttpListener::get_AuthenticationSchemes()
      public VoidNet.HttpListener::set_AuthenticationSchemesNet.AuthenticationSchemes)
      public Net.AuthenticationSchemeSelectorNet.HttpListener::get_AuthenticationSchemeSelectorDelegate()
      public VoidNet.HttpListener::set_AuthenticationSchemeSelectorDelegateNet.AuthenticationSchemeSelector)
      public BooleanNet.HttpListener::get_IgnoreWriteExceptions()
      public VoidNet.HttpListener::set_IgnoreWriteExceptionsBoolean)
      public BooleanNet.HttpListener::get_IsListening()
      public BooleanNet.HttpListener::get_IsSupported()
      public Net.HttpListenerPrefixCollectionNet.HttpListener::get_Prefixes()
      public StringNet.HttpListener::get_Realm()
      public VoidNet.HttpListener::set_RealmString)
      public BooleanNet.HttpListener::get_UnsafeConnectionNtlmAuthentication()
      public VoidNet.HttpListener::set_UnsafeConnectionNtlmAuthenticationBoolean)
      public VoidNet.HttpListener::Abort()
      public VoidNet.HttpListener::Close()
      VoidNet.HttpListener::CloseBoolean)
      VoidNet.HttpListener::CleanupBoolean)
      public IAsyncResultNet.HttpListener::BeginGetContextAsyncCallbackObject)
      public Net.HttpListenerContextNet.HttpListener::EndGetContextIAsyncResult)
      Net.AuthenticationSchemesNet.HttpListener::SelectAuthenticationSchemeNet.HttpListenerContext)
      public Net.HttpListenerContextNet.HttpListener::GetContext()
      public VoidNet.HttpListener::Start()
      public VoidNet.HttpListener::Stop()
      VoidNet.HttpListener::CheckDisposed()
      Net.HttpListenerContextNet.HttpListener::GetContextFromQueue()
      VoidNet.HttpListener::RegisterContextNet.HttpListenerContext)
      VoidNet.HttpListener::UnregisterContextNet.HttpListenerContext)
    }
}
