using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine.Scripting;
using UnityEngineInternal;

namespace UnityEngine.Networking
{
	[StructLayout(LayoutKind.Sequential)]
	public sealed class UnityWebRequest : IDisposable
	{
		internal enum UnityWebRequestMethod
		{
			Get,
			Post,
			Put,
			Head,
			Custom
		}

		internal enum UnityWebRequestError
		{
			OK,
			Unknown,
			SDKError,
			UnsupportedProtocol,
			MalformattedUrl,
			CannotResolveProxy,
			CannotResolveHost,
			CannotConnectToHost,
			AccessDenied,
			GenericHTTPError,
			WriteError,
			ReadError,
			OutOfMemory,
			Timeout,
			HTTPPostError,
			SSLCannotConnect,
			Aborted,
			TooManyRedirects,
			ReceivedNoData,
			SSLNotSupported,
			FailedToSendData,
			FailedToReceiveData,
			SSLCertificateError,
			SSLCipherNotAvailable,
			SSLCACertError,
			UnrecognizedContentEncoding,
			LoginFailed,
			SSLShutdownFailed,
			NoInternetConnection
		}

		[NonSerialized]
		internal IntPtr m_Ptr;

		public const string kHttpVerbGET = "GET";

		public const string kHttpVerbHEAD = "HEAD";

		public const string kHttpVerbPOST = "POST";

		public const string kHttpVerbPUT = "PUT";

		public const string kHttpVerbCREATE = "CREATE";

		public const string kHttpVerbDELETE = "DELETE";

		public bool disposeDownloadHandlerOnDispose
		{
			get;
			set;
		}

		public bool disposeUploadHandlerOnDispose
		{
			get;
			set;
		}

		public string method
		{
			get
			{
				switch (InternalGetMethod())
				{
				case 0:
					return "GET";
				case 1:
					return "POST";
				case 2:
					return "PUT";
				case 3:
					return "HEAD";
				default:
					return InternalGetCustomMethod();
				}
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentException("Cannot set a UnityWebRequest's method to an empty or null string");
				}
				switch (value.ToUpper())
				{
				case "GET":
					InternalSetMethod(UnityWebRequestMethod.Get);
					break;
				case "POST":
					InternalSetMethod(UnityWebRequestMethod.Post);
					break;
				case "PUT":
					InternalSetMethod(UnityWebRequestMethod.Put);
					break;
				case "HEAD":
					InternalSetMethod(UnityWebRequestMethod.Head);
					break;
				default:
					InternalSetCustomMethod(value.ToUpper());
					break;
				}
			}
		}

		public string error
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public bool useHttpContinue
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public string url
		{
			get
			{
				return InternalGetUrl();
			}
			set
			{
				string localUrl = "http://localhost/";
				InternalSetUrl(WebRequestUtils.MakeInitialUrl(value, localUrl));
			}
		}

		public long responseCode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public float uploadProgress
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public bool isModifiable
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public bool isDone
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public bool isError
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public float downloadProgress
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public ulong uploadedBytes
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public ulong downloadedBytes
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public int redirectLimit
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public bool chunkedTransfer
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public UploadHandler uploadHandler
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public DownloadHandler downloadHandler
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public int timeout
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public UnityWebRequest()
		{
			InternalCreate();
			InternalSetDefaults();
		}

		public UnityWebRequest(string url)
		{
			InternalCreate();
			InternalSetDefaults();
			this.url = url;
		}

		public UnityWebRequest(string url, string method)
		{
			InternalCreate();
			InternalSetDefaults();
			this.url = url;
			this.method = method;
		}

		public UnityWebRequest(string url, string method, DownloadHandler downloadHandler, UploadHandler uploadHandler)
		{
			InternalCreate();
			InternalSetDefaults();
			this.url = url;
			this.method = method;
			this.downloadHandler = downloadHandler;
			this.uploadHandler = uploadHandler;
		}

		public static UnityWebRequest Get(string uri)
		{
			return new UnityWebRequest(uri, "GET", new DownloadHandlerBuffer(), null);
		}

		public static UnityWebRequest Delete(string uri)
		{
			return new UnityWebRequest(uri, "DELETE");
		}

		public static UnityWebRequest Head(string uri)
		{
			return new UnityWebRequest(uri, "HEAD");
		}

		public static UnityWebRequest GetTexture(string uri)
		{
			return GetTexture(uri, nonReadable: false);
		}

		public static UnityWebRequest GetTexture(string uri, bool nonReadable)
		{
			return new UnityWebRequest(uri, "GET", new DownloadHandlerTexture(!nonReadable), null);
		}

		public static UnityWebRequest GetAudioClip(string uri, AudioType audioType)
		{
			Type type = Type.GetType("UnityEngine.Networking.DownloadHandlerAudioClip");
			if (type == null)
			{
				return Get(uri);
			}
			ConstructorInfo constructor = type.GetConstructor(new Type[2]
			{
				typeof(string),
				typeof(AudioType)
			});
			return new UnityWebRequest(uri, "GET", constructor.Invoke(new object[2]
			{
				uri,
				audioType
			}) as DownloadHandler, null);
		}

		public static UnityWebRequest GetAssetBundle(string uri)
		{
			return GetAssetBundle(uri, 0u);
		}

		public static UnityWebRequest GetAssetBundle(string uri, uint crc)
		{
			return new UnityWebRequest(uri, "GET", new DownloadHandlerAssetBundle(uri, crc), null);
		}

		public static UnityWebRequest GetAssetBundle(string uri, uint version, uint crc)
		{
			return new UnityWebRequest(uri, "GET", new DownloadHandlerAssetBundle(uri, version, crc), null);
		}

		public static UnityWebRequest GetAssetBundle(string uri, Hash128 hash, uint crc)
		{
			return new UnityWebRequest(uri, "GET", new DownloadHandlerAssetBundle(uri, hash, crc), null);
		}

		public static UnityWebRequest Put(string uri, byte[] bodyData)
		{
			return new UnityWebRequest(uri, "PUT", new DownloadHandlerBuffer(), new UploadHandlerRaw(bodyData));
		}

		public static UnityWebRequest Put(string uri, string bodyData)
		{
			return new UnityWebRequest(uri, "PUT", new DownloadHandlerBuffer(), new UploadHandlerRaw(Encoding.UTF8.GetBytes(bodyData)));
		}

		public static UnityWebRequest Post(string uri, string postData)
		{
			UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST");
			byte[] data = null;
			if (!string.IsNullOrEmpty(postData))
			{
				string s = WWWTranscoder.URLEncode(postData, Encoding.UTF8);
				data = Encoding.UTF8.GetBytes(s);
			}
			unityWebRequest.uploadHandler = new UploadHandlerRaw(data);
			unityWebRequest.uploadHandler.contentType = "application/x-www-form-urlencoded";
			unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
			return unityWebRequest;
		}

		public static UnityWebRequest Post(string uri, WWWForm formData)
		{
			UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST");
			byte[] array = null;
			if (formData != null)
			{
				array = formData.data;
				if (array.Length == 0)
				{
					array = null;
				}
			}
			unityWebRequest.uploadHandler = new UploadHandlerRaw(array);
			unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
			if (formData != null)
			{
				Dictionary<string, string> headers = formData.headers;
				foreach (KeyValuePair<string, string> item in headers)
				{
					unityWebRequest.SetRequestHeader(item.Key, item.Value);
				}
			}
			return unityWebRequest;
		}

		public static UnityWebRequest Post(string uri, List<IMultipartFormSection> multipartFormSections)
		{
			byte[] boundary = GenerateBoundary();
			return Post(uri, multipartFormSections, boundary);
		}

		public static UnityWebRequest Post(string uri, List<IMultipartFormSection> multipartFormSections, byte[] boundary)
		{
			UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST");
			byte[] data = null;
			if (multipartFormSections != null && multipartFormSections.Count != 0)
			{
				data = SerializeFormSections(multipartFormSections, boundary);
			}
			UploadHandler uploadHandler = new UploadHandlerRaw(data);
			uploadHandler.contentType = "multipart/form-data; boundary=" + Encoding.UTF8.GetString(boundary, 0, boundary.Length);
			unityWebRequest.uploadHandler = uploadHandler;
			unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
			return unityWebRequest;
		}

		public static UnityWebRequest Post(string uri, Dictionary<string, string> formFields)
		{
			UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST");
			byte[] data = null;
			if (formFields != null && formFields.Count != 0)
			{
				data = SerializeSimpleForm(formFields);
			}
			UploadHandler uploadHandler = new UploadHandlerRaw(data);
			uploadHandler.contentType = "application/x-www-form-urlencoded";
			unityWebRequest.uploadHandler = uploadHandler;
			unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
			return unityWebRequest;
		}

		public static byte[] SerializeFormSections(List<IMultipartFormSection> multipartFormSections, byte[] boundary)
		{
			byte[] bytes = Encoding.UTF8.GetBytes("\r\n");
			byte[] bytes2 = WWW.DefaultEncoding.GetBytes("--");
			int num = 0;
			foreach (IMultipartFormSection multipartFormSection in multipartFormSections)
			{
				num += 64 + multipartFormSection.sectionData.Length;
			}
			List<byte> list = new List<byte>(num);
			foreach (IMultipartFormSection multipartFormSection2 in multipartFormSections)
			{
				string str = "form-data";
				string sectionName = multipartFormSection2.sectionName;
				string fileName = multipartFormSection2.fileName;
				if (!string.IsNullOrEmpty(fileName))
				{
					str = "file";
				}
				string str2 = "Content-Disposition: " + str;
				if (!string.IsNullOrEmpty(sectionName))
				{
					str2 = str2 + "; name=\"" + sectionName + "\"";
				}
				if (!string.IsNullOrEmpty(fileName))
				{
					str2 = str2 + "; filename=\"" + fileName + "\"";
				}
				str2 += "\r\n";
				string contentType = multipartFormSection2.contentType;
				if (!string.IsNullOrEmpty(contentType))
				{
					str2 = str2 + "Content-Type: " + contentType + "\r\n";
				}
				list.AddRange(bytes);
				list.AddRange(bytes2);
				list.AddRange(boundary);
				list.AddRange(bytes);
				list.AddRange(Encoding.UTF8.GetBytes(str2));
				list.AddRange(bytes);
				list.AddRange(multipartFormSection2.sectionData);
			}
			list.TrimExcess();
			return list.ToArray();
		}

		public static byte[] GenerateBoundary()
		{
			byte[] array = new byte[40];
			for (int i = 0; i < 40; i++)
			{
				int num = Random.Range(48, 110);
				if (num > 57)
				{
					num += 7;
				}
				if (num > 90)
				{
					num += 6;
				}
				array[i] = (byte)num;
			}
			return array;
		}

		public static byte[] SerializeSimpleForm(Dictionary<string, string> formFields)
		{
			string text = "";
			foreach (KeyValuePair<string, string> formField in formFields)
			{
				if (text.Length > 0)
				{
					text += "&";
				}
				text = text + Uri.EscapeDataString(formField.Key) + "=" + Uri.EscapeDataString(formField.Value);
			}
			return Encoding.UTF8.GetBytes(text);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void InternalCreate();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		internal extern void InternalDestroy();

		private void InternalSetDefaults()
		{
			disposeDownloadHandlerOnDispose = true;
			disposeUploadHandlerOnDispose = true;
		}

		~UnityWebRequest()
		{
			DisposeHandlers();
			InternalDestroy();
		}

		public void Dispose()
		{
			DisposeHandlers();
			InternalDestroy();
			GC.SuppressFinalize(this);
		}

		private void DisposeHandlers()
		{
			if (disposeDownloadHandlerOnDispose)
			{
				GetDownloadHandler()?.Dispose();
			}
			if (disposeUploadHandlerOnDispose)
			{
				GetUploadHandler()?.Dispose();
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern AsyncOperation InternalBegin();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void InternalAbort();

		public AsyncOperation Send()
		{
			return InternalBegin();
		}

		public void Abort()
		{
			InternalAbort();
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void InternalSetMethod(UnityWebRequestMethod methodType);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void InternalSetCustomMethod(string customMethodName);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern int InternalGetMethod();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern string InternalGetCustomMethod();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern int InternalGetError();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern string InternalGetUrl();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void InternalSetUrl(string url);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern string GetRequestHeader(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void InternalSetRequestHeader(string name, string value);

		public void SetRequestHeader(string name, string value)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException("Cannot set a Request Header with a null or empty name");
			}
			if (value == null)
			{
				throw new ArgumentException("Cannot set a Request header with a null");
			}
			InternalSetRequestHeader(name, value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern string GetResponseHeader(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern string[] InternalGetResponseHeaderKeys();

		public Dictionary<string, string> GetResponseHeaders()
		{
			string[] array = InternalGetResponseHeaderKeys();
			if (array == null)
			{
				return null;
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>(array.Length, StringComparer.OrdinalIgnoreCase);
			for (int i = 0; i < array.Length; i++)
			{
				string responseHeader = GetResponseHeader(array[i]);
				dictionary.Add(array[i], responseHeader);
			}
			return dictionary;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		private extern UploadHandler GetUploadHandler();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		private extern DownloadHandler GetDownloadHandler();

		private static string GetErrorDescription(UnityWebRequestError errorCode)
		{
			switch (errorCode)
			{
			case UnityWebRequestError.OK:
				return "No Error";
			case UnityWebRequestError.SDKError:
				return "Internal Error With Transport Layer";
			case UnityWebRequestError.UnsupportedProtocol:
				return "Specified Transport Protocol is Unsupported";
			case UnityWebRequestError.MalformattedUrl:
				return "URL is Malformatted";
			case UnityWebRequestError.CannotResolveProxy:
				return "Unable to resolve specified proxy server";
			case UnityWebRequestError.CannotResolveHost:
				return "Unable to resolve host specified in URL";
			case UnityWebRequestError.CannotConnectToHost:
				return "Unable to connect to host specified in URL";
			case UnityWebRequestError.AccessDenied:
				return "Remote server denied access to the specified URL";
			case UnityWebRequestError.GenericHTTPError:
				return "Unknown/Generic HTTP Error - Check HTTP Error code";
			case UnityWebRequestError.WriteError:
				return "Error when transmitting request to remote server - transmission terminated prematurely";
			case UnityWebRequestError.ReadError:
				return "Error when reading response from remote server - transmission terminated prematurely";
			case UnityWebRequestError.OutOfMemory:
				return "Out of Memory";
			case UnityWebRequestError.Timeout:
				return "Timeout occurred while waiting for response from remote server";
			case UnityWebRequestError.HTTPPostError:
				return "Error while transmitting HTTP POST body data";
			case UnityWebRequestError.SSLCannotConnect:
				return "Unable to connect to SSL server at remote host";
			case UnityWebRequestError.Aborted:
				return "Request was manually aborted by local code";
			case UnityWebRequestError.TooManyRedirects:
				return "Redirect limit exceeded";
			case UnityWebRequestError.ReceivedNoData:
				return "Received an empty response from remote host";
			case UnityWebRequestError.SSLNotSupported:
				return "SSL connections are not supported on the local machine";
			case UnityWebRequestError.FailedToSendData:
				return "Failed to transmit body data";
			case UnityWebRequestError.FailedToReceiveData:
				return "Failed to receive response body data";
			case UnityWebRequestError.SSLCertificateError:
				return "Failure to authenticate SSL certificate of remote host";
			case UnityWebRequestError.SSLCipherNotAvailable:
				return "SSL cipher received from remote host is not supported on the local machine";
			case UnityWebRequestError.SSLCACertError:
				return "Failure to authenticate Certificate Authority of the SSL certificate received from the remote host";
			case UnityWebRequestError.UnrecognizedContentEncoding:
				return "Remote host returned data with an unrecognized/unparseable content encoding";
			case UnityWebRequestError.LoginFailed:
				return "HTTP authentication failed";
			case UnityWebRequestError.SSLShutdownFailed:
				return "Failure while shutting down SSL connection";
			default:
				return "Unknown error";
			}
		}
	}
}
