using System.Collections;
using System.Collections.Specialized;

namespace System.Net
{
	/// <summary>Manages the authentication modules called during the client authentication process.</summary>
	public class AuthenticationManager
	{
		private static ArrayList modules;

		private static object locker = new object();

		private static ICredentialPolicy credential_policy = null;

		/// <summary>Gets or sets the credential policy to be used for resource requests made using the <see cref="T:System.Net.HttpWebRequest" /> class.</summary>
		/// <returns>An object that implements the <see cref="T:System.Net.ICredentialPolicy" /> interface that determines whether credentials are sent with requests. The default value is null.</returns>
		public static ICredentialPolicy CredentialPolicy
		{
			get
			{
				return credential_policy;
			}
			set
			{
				credential_policy = value;
			}
		}

		/// <summary>Gets the dictionary that contains Service Principal Names (SPNs) that are used to identify hosts during Kerberos authentication for requests made using <see cref="T:System.Net.WebRequest" /> and its derived classes.</summary>
		/// <returns>A writable <see cref="T:System.Collections.Specialized.StringDictionary" /> that contains the SPN values for keys composed of host information. </returns>
		[MonoTODO]
		public static StringDictionary CustomTargetNameDictionary
		{
			get
			{
				throw GetMustImplement();
			}
		}

		/// <summary>Gets a list of authentication modules that are registered with the authentication manager.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that enables the registered authentication modules to be read.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public static IEnumerator RegisteredModules
		{
			get
			{
				EnsureModules();
				return modules.GetEnumerator();
			}
		}

		private AuthenticationManager()
		{
		}

		private static void EnsureModules()
		{
			lock (locker)
			{
				if (modules == null)
				{
					modules = new ArrayList();
					modules.Add(new BasicClient());
					modules.Add(new DigestClient());
				}
			}
		}

		private static Exception GetMustImplement()
		{
			return new NotImplementedException();
		}

		internal static void Clear()
		{
			EnsureModules();
			lock (modules)
			{
				modules.Clear();
			}
		}

		/// <summary>Calls each registered authentication module to find the first module that can respond to the authentication request.</summary>
		/// <returns>An instance of the <see cref="T:System.Net.Authorization" /> class containing the result of the authorization attempt. If there is no authentication module to respond to the challenge, this method returns null.</returns>
		/// <param name="challenge">The challenge returned by the Internet resource. </param>
		/// <param name="request">The <see cref="T:System.Net.WebRequest" /> that initiated the authentication challenge. </param>
		/// <param name="credentials">The <see cref="T:System.Net.ICredentials" /> associated with this request. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="challenge" /> is null.-or- <paramref name="request" /> is null.-or- <paramref name="credentials" /> is null. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public static Authorization Authenticate(string challenge, WebRequest request, ICredentials credentials)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}
			if (credentials == null)
			{
				throw new ArgumentNullException("credentials");
			}
			if (challenge == null)
			{
				throw new ArgumentNullException("challenge");
			}
			return DoAuthenticate(challenge, request, credentials);
		}

		private static Authorization DoAuthenticate(string challenge, WebRequest request, ICredentials credentials)
		{
			EnsureModules();
			lock (modules)
			{
				foreach (IAuthenticationModule module in modules)
				{
					Authorization authorization = module.Authenticate(challenge, request, credentials);
					if (authorization != null)
					{
						authorization.Module = module;
						return authorization;
					}
				}
			}
			return null;
		}

		/// <summary>Preauthenticates a request.</summary>
		/// <returns>An instance of the <see cref="T:System.Net.Authorization" /> class if the request can be preauthenticated; otherwise, null. If <paramref name="credentials" /> is null, this method returns null.</returns>
		/// <param name="request">A <see cref="T:System.Net.WebRequest" /> to an Internet resource. </param>
		/// <param name="credentials">The <see cref="T:System.Net.ICredentials" /> associated with the request. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="request" /> is null. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public static Authorization PreAuthenticate(WebRequest request, ICredentials credentials)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}
			if (credentials == null)
			{
				return null;
			}
			EnsureModules();
			lock (modules)
			{
				foreach (IAuthenticationModule module in modules)
				{
					Authorization authorization = module.PreAuthenticate(request, credentials);
					if (authorization != null)
					{
						authorization.Module = module;
						return authorization;
					}
				}
			}
			return null;
		}

		/// <summary>Registers an authentication module with the authentication manager.</summary>
		/// <param name="authenticationModule">The <see cref="T:System.Net.IAuthenticationModule" /> to register with the authentication manager. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="authenticationModule" /> is null. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static void Register(IAuthenticationModule authenticationModule)
		{
			if (authenticationModule == null)
			{
				throw new ArgumentNullException("authenticationModule");
			}
			DoUnregister(authenticationModule.AuthenticationType, throwEx: false);
			lock (modules)
			{
				modules.Add(authenticationModule);
			}
		}

		/// <summary>Removes the specified authentication module from the list of registered modules.</summary>
		/// <param name="authenticationModule">The <see cref="T:System.Net.IAuthenticationModule" /> to remove from the list of registered modules. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="authenticationModule" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The specified <see cref="T:System.Net.IAuthenticationModule" /> is not registered. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static void Unregister(IAuthenticationModule authenticationModule)
		{
			if (authenticationModule == null)
			{
				throw new ArgumentNullException("authenticationModule");
			}
			DoUnregister(authenticationModule.AuthenticationType, throwEx: true);
		}

		/// <summary>Removes authentication modules with the specified authentication scheme from the list of registered modules.</summary>
		/// <param name="authenticationScheme">The authentication scheme of the module to remove. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="authenticationScheme" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">A module for this authentication scheme is not registered. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public static void Unregister(string authenticationScheme)
		{
			if (authenticationScheme == null)
			{
				throw new ArgumentNullException("authenticationScheme");
			}
			DoUnregister(authenticationScheme, throwEx: true);
		}

		private static void DoUnregister(string authenticationScheme, bool throwEx)
		{
			EnsureModules();
			lock (modules)
			{
				IAuthenticationModule authenticationModule = null;
				foreach (IAuthenticationModule module in modules)
				{
					string authenticationType = module.AuthenticationType;
					if (string.Compare(authenticationType, authenticationScheme, ignoreCase: true) == 0)
					{
						authenticationModule = module;
						break;
					}
				}
				if (authenticationModule == null)
				{
					if (throwEx)
					{
						throw new InvalidOperationException("Scheme not registered.");
					}
				}
				else
				{
					modules.Remove(authenticationModule);
				}
			}
		}
	}
}
