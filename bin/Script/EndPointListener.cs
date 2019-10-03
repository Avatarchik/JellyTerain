using Mono.Security.Authenticode;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace System.Net
{
	internal sealed class EndPointListener
	{
		private IPEndPoint endpoint;

		private Socket sock;

		private Hashtable prefixes;

		private ArrayList unhandled;

		private ArrayList all;

		private X509Certificate2 cert;

		private AsymmetricAlgorithm key;

		private bool secure;

		public EndPointListener(IPAddress addr, int port, bool secure)
		{
			if (secure)
			{
				this.secure = secure;
				LoadCertificateAndKey(addr, port);
			}
			endpoint = new IPEndPoint(addr, port);
			sock = new Socket(addr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			sock.Bind(endpoint);
			sock.Listen(500);
			sock.BeginAccept(OnAccept, this);
			prefixes = new Hashtable();
		}

		private void LoadCertificateAndKey(IPAddress addr, int port)
		{
			try
			{
				string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
				string path = Path.Combine(folderPath, ".mono");
				path = Path.Combine(path, "httplistener");
				string fileName = Path.Combine(path, $"{port}.cer");
				string filename = Path.Combine(path, $"{port}.pvk");
				cert = new X509Certificate2(fileName);
				key = PrivateKey.CreateFromFile(filename).RSA;
			}
			catch
			{
			}
		}

		private static void OnAccept(IAsyncResult ares)
		{
			EndPointListener endPointListener = (EndPointListener)ares.AsyncState;
			Socket socket = null;
			try
			{
				socket = endPointListener.sock.EndAccept(ares);
			}
			catch
			{
			}
			finally
			{
				try
				{
					endPointListener.sock.BeginAccept(OnAccept, endPointListener);
				}
				catch
				{
					if (socket != null)
					{
						try
						{
							socket.Close();
						}
						catch
						{
						}
						socket = null;
					}
				}
			}
			if (socket != null)
			{
				if (endPointListener.secure && (endPointListener.cert == null || endPointListener.key == null))
				{
					socket.Close();
					return;
				}
				HttpConnection httpConnection = new HttpConnection(socket, endPointListener, endPointListener.secure, endPointListener.cert, endPointListener.key);
				httpConnection.BeginReadRequest();
			}
		}

		public bool BindContext(HttpListenerContext context)
		{
			HttpListenerRequest request = context.Request;
			ListenerPrefix prefix;
			HttpListener httpListener = SearchListener(request.UserHostName, request.Url, out prefix);
			if (httpListener == null)
			{
				return false;
			}
			context.Listener = httpListener;
			context.Connection.Prefix = prefix;
			httpListener.RegisterContext(context);
			return true;
		}

		public void UnbindContext(HttpListenerContext context)
		{
			if (context != null && context.Request != null)
			{
				HttpListenerRequest request = context.Request;
				SearchListener(request.UserHostName, request.Url, out ListenerPrefix _)?.UnregisterContext(context);
			}
		}

		private HttpListener SearchListener(string host, Uri uri, out ListenerPrefix prefix)
		{
			prefix = null;
			if (uri == null)
			{
				return null;
			}
			if (host != null)
			{
				int num = host.IndexOf(':');
				if (num >= 0)
				{
					host = host.Substring(0, num);
				}
			}
			string text = HttpUtility.UrlDecode(uri.AbsolutePath);
			string text2 = (text[text.Length - 1] != '/') ? (text + "/") : text;
			HttpListener result = null;
			int num2 = -1;
			lock (prefixes)
			{
				if (host != null && host != string.Empty)
				{
					foreach (ListenerPrefix key2 in prefixes.Keys)
					{
						string path = key2.Path;
						if (path.Length >= num2 && key2.Host == host && (text.StartsWith(path) || text2.StartsWith(path)))
						{
							num2 = path.Length;
							result = (HttpListener)prefixes[key2];
							prefix = key2;
						}
					}
					if (num2 != -1)
					{
						return result;
					}
				}
				result = MatchFromList(host, text, unhandled, out prefix);
				if (result != null)
				{
					return result;
				}
				result = MatchFromList(host, text, all, out prefix);
				if (result != null)
				{
					return result;
				}
			}
			return null;
		}

		private HttpListener MatchFromList(string host, string path, ArrayList list, out ListenerPrefix prefix)
		{
			prefix = null;
			if (list == null)
			{
				return null;
			}
			HttpListener result = null;
			int num = -1;
			foreach (ListenerPrefix item in list)
			{
				string path2 = item.Path;
				if (path2.Length >= num && path.StartsWith(path2))
				{
					num = path2.Length;
					result = item.Listener;
					prefix = item;
				}
			}
			return result;
		}

		private void AddSpecial(ArrayList coll, ListenerPrefix prefix)
		{
			if (coll != null)
			{
				foreach (ListenerPrefix item in coll)
				{
					if (item.Path == prefix.Path)
					{
						throw new HttpListenerException(400, "Prefix already in use.");
					}
				}
				coll.Add(prefix);
			}
		}

		private void RemoveSpecial(ArrayList coll, ListenerPrefix prefix)
		{
			if (coll == null)
			{
				return;
			}
			int count = coll.Count;
			int num = 0;
			while (true)
			{
				if (num < count)
				{
					ListenerPrefix listenerPrefix = (ListenerPrefix)coll[num];
					if (listenerPrefix.Path == prefix.Path)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			coll.RemoveAt(num);
			CheckIfRemove();
		}

		private void CheckIfRemove()
		{
			if (prefixes.Count <= 0 && (unhandled == null || unhandled.Count <= 0) && (all == null || all.Count <= 0))
			{
				EndPointManager.RemoveEndPoint(this, endpoint);
			}
		}

		public void Close()
		{
			sock.Close();
		}

		public void AddPrefix(ListenerPrefix prefix, HttpListener listener)
		{
			lock (prefixes)
			{
				if (prefix.Host == "*")
				{
					if (unhandled == null)
					{
						unhandled = new ArrayList();
					}
					prefix.Listener = listener;
					AddSpecial(unhandled, prefix);
				}
				else if (prefix.Host == "+")
				{
					if (all == null)
					{
						all = new ArrayList();
					}
					prefix.Listener = listener;
					AddSpecial(all, prefix);
				}
				else if (prefixes.ContainsKey(prefix))
				{
					HttpListener httpListener = (HttpListener)prefixes[prefix];
					if (httpListener != listener)
					{
						throw new HttpListenerException(400, "There's another listener for " + prefix);
					}
				}
				else
				{
					prefixes[prefix] = listener;
				}
			}
		}

		public void RemovePrefix(ListenerPrefix prefix, HttpListener listener)
		{
			lock (prefixes)
			{
				if (prefix.Host == "*")
				{
					RemoveSpecial(unhandled, prefix);
				}
				else if (prefix.Host == "+")
				{
					RemoveSpecial(all, prefix);
				}
				else if (prefixes.ContainsKey(prefix))
				{
					prefixes.Remove(prefix);
					CheckIfRemove();
				}
			}
		}
	}
}
