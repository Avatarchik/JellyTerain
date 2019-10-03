using Mono.Security.Protocol.Tls;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace System.Net
{
	internal sealed class HttpConnection
	{
		private enum InputState
		{
			RequestLine,
			Headers
		}

		private enum LineState
		{
			None,
			CR,
			LF
		}

		private const int BufferSize = 8192;

		private Socket sock;

		private Stream stream;

		private EndPointListener epl;

		private MemoryStream ms;

		private byte[] buffer;

		private HttpListenerContext context;

		private StringBuilder current_line;

		private ListenerPrefix prefix;

		private RequestStream i_stream;

		private ResponseStream o_stream;

		private bool chunked;

		private int chunked_uses;

		private bool context_bound;

		private bool secure;

		private AsymmetricAlgorithm key;

		private InputState input_state;

		private LineState line_state;

		private int position;

		public int ChunkedUses => chunked_uses;

		public IPEndPoint LocalEndPoint => (IPEndPoint)sock.LocalEndPoint;

		public IPEndPoint RemoteEndPoint => (IPEndPoint)sock.RemoteEndPoint;

		public bool IsSecure => secure;

		public ListenerPrefix Prefix
		{
			get
			{
				return prefix;
			}
			set
			{
				prefix = value;
			}
		}

		public HttpConnection(Socket sock, EndPointListener epl, bool secure, X509Certificate2 cert, AsymmetricAlgorithm key)
		{
			this.sock = sock;
			this.epl = epl;
			this.secure = secure;
			this.key = key;
			if (!secure)
			{
				stream = new NetworkStream(sock, owns_socket: false);
			}
			else
			{
				SslServerStream sslServerStream = new SslServerStream(new NetworkStream(sock, owns_socket: false), cert, clientCertificateRequired: false, ownsStream: false);
				SslServerStream sslServerStream2 = sslServerStream;
				sslServerStream2.PrivateKeyCertSelectionDelegate = (PrivateKeySelectionCallback)Delegate.Combine(sslServerStream2.PrivateKeyCertSelectionDelegate, new PrivateKeySelectionCallback(OnPVKSelection));
				stream = sslServerStream;
			}
			Init();
		}

		private AsymmetricAlgorithm OnPVKSelection(X509Certificate certificate, string targetHost)
		{
			return key;
		}

		private void Init()
		{
			context_bound = false;
			i_stream = null;
			o_stream = null;
			prefix = null;
			chunked = false;
			ms = new MemoryStream();
			position = 0;
			input_state = InputState.RequestLine;
			line_state = LineState.None;
			context = new HttpListenerContext(this);
		}

		public void BeginReadRequest()
		{
			if (buffer == null)
			{
				buffer = new byte[8192];
			}
			try
			{
				stream.BeginRead(buffer, 0, 8192, OnRead, this);
			}
			catch
			{
				CloseSocket();
			}
		}

		public RequestStream GetRequestStream(bool chunked, long contentlength)
		{
			if (i_stream == null)
			{
				byte[] array = ms.GetBuffer();
				int num = (int)ms.Length;
				ms = null;
				if (chunked)
				{
					this.chunked = true;
					context.Response.SendChunked = true;
					i_stream = new ChunkedInputStream(context, stream, array, position, num - position);
				}
				else
				{
					i_stream = new RequestStream(stream, array, position, num - position, contentlength);
				}
			}
			return i_stream;
		}

		public ResponseStream GetResponseStream()
		{
			if (o_stream == null)
			{
				bool ignore_errors = context.Listener?.IgnoreWriteExceptions ?? true;
				o_stream = new ResponseStream(stream, context.Response, ignore_errors);
			}
			return o_stream;
		}

		private void OnRead(IAsyncResult ares)
		{
			HttpConnection state = (HttpConnection)ares.AsyncState;
			int num = -1;
			try
			{
				num = stream.EndRead(ares);
				ms.Write(buffer, 0, num);
				if (ms.Length > 32768)
				{
					SendError("Bad request", 400);
					Close(force_close: true);
					return;
				}
			}
			catch
			{
				if (ms != null && ms.Length > 0)
				{
					SendError();
				}
				if (sock != null)
				{
					CloseSocket();
				}
				return;
				IL_009f:;
			}
			if (num == 0)
			{
				CloseSocket();
			}
			else if (ProcessInput(ms))
			{
				if (!context.HaveError)
				{
					context.Request.FinishInitialization();
				}
				if (context.HaveError)
				{
					SendError();
					Close(force_close: true);
					return;
				}
				if (!epl.BindContext(context))
				{
					SendError("Invalid host", 400);
					Close(force_close: true);
				}
				context_bound = true;
			}
			else
			{
				stream.BeginRead(buffer, 0, 8192, OnRead, state);
			}
		}

		private bool ProcessInput(MemoryStream ms)
		{
			byte[] array = ms.GetBuffer();
			int num = (int)ms.Length;
			int used = 0;
			string text;
			try
			{
				text = ReadLine(array, position, num - position, ref used);
				position += used;
			}
			catch (Exception)
			{
				context.ErrorMessage = "Bad request";
				context.ErrorStatus = 400;
				return true;
				IL_0066:;
			}
			while (text != null)
			{
				if (text == string.Empty)
				{
					if (input_state != 0)
					{
						current_line = null;
						ms = null;
						return true;
					}
				}
				else
				{
					if (input_state == InputState.RequestLine)
					{
						context.Request.SetRequestLine(text);
						input_state = InputState.Headers;
					}
					else
					{
						try
						{
							context.Request.AddHeader(text);
						}
						catch (Exception ex2)
						{
							context.ErrorMessage = ex2.Message;
							context.ErrorStatus = 400;
							return true;
							IL_010c:;
						}
					}
					if (context.HaveError)
					{
						return true;
					}
					if (position >= num)
					{
						break;
					}
					try
					{
						text = ReadLine(array, position, num - position, ref used);
						position += used;
					}
					catch (Exception)
					{
						context.ErrorMessage = "Bad request";
						context.ErrorStatus = 400;
						return true;
						IL_0189:;
					}
				}
				if (text == null)
				{
					break;
				}
			}
			if (used == num)
			{
				ms.SetLength(0L);
				position = 0;
			}
			return false;
		}

		private string ReadLine(byte[] buffer, int offset, int len, ref int used)
		{
			if (current_line == null)
			{
				current_line = new StringBuilder();
			}
			int num = offset + len;
			used = 0;
			for (int i = offset; i < num; i++)
			{
				if (line_state == LineState.LF)
				{
					break;
				}
				used++;
				byte b = buffer[i];
				switch (b)
				{
				case 13:
					line_state = LineState.CR;
					break;
				case 10:
					line_state = LineState.LF;
					break;
				default:
					current_line.Append((char)b);
					break;
				}
			}
			string result = null;
			if (line_state == LineState.LF)
			{
				line_state = LineState.None;
				result = current_line.ToString();
				current_line.Length = 0;
			}
			return result;
		}

		public void SendError(string msg, int status)
		{
			try
			{
				HttpListenerResponse response = context.Response;
				response.StatusCode = status;
				response.ContentType = "text/html";
				string statusDescription = HttpListenerResponse.GetStatusDescription(status);
				string s = (msg == null) ? $"<h1>{statusDescription}</h1>" : $"<h1>{statusDescription} ({msg})</h1>";
				byte[] bytes = context.Response.ContentEncoding.GetBytes(s);
				response.Close(bytes, willBlock: false);
			}
			catch
			{
			}
		}

		public void SendError()
		{
			SendError(context.ErrorMessage, context.ErrorStatus);
		}

		private void Unbind()
		{
			if (context_bound)
			{
				epl.UnbindContext(context);
				context_bound = false;
			}
		}

		public void Close()
		{
			Close(force_close: false);
		}

		private void CloseSocket()
		{
			if (sock != null)
			{
				try
				{
					sock.Close();
				}
				catch
				{
				}
				finally
				{
					sock = null;
				}
			}
		}

		internal void Close(bool force_close)
		{
			if (sock != null)
			{
				Stream responseStream = GetResponseStream();
				responseStream.Close();
				o_stream = null;
			}
			if (sock == null)
			{
				return;
			}
			force_close |= (context.Request.Headers["connection"] == "close");
			if (!force_close)
			{
				int statusCode = context.Response.StatusCode;
				bool flag = statusCode == 400 || statusCode == 408 || statusCode == 411 || statusCode == 413 || statusCode == 414 || statusCode == 500 || statusCode == 503;
				force_close |= (context.Request.ProtocolVersion <= HttpVersion.Version10);
			}
			if (!force_close && context.Request.FlushInput())
			{
				if (chunked && !context.Response.ForceCloseChunked)
				{
					chunked_uses++;
					Unbind();
					Init();
					BeginReadRequest();
				}
				else
				{
					Unbind();
					Init();
					BeginReadRequest();
				}
			}
			else
			{
				Socket socket = sock;
				sock = null;
				try
				{
					socket?.Shutdown(SocketShutdown.Both);
				}
				catch
				{
				}
				finally
				{
					socket?.Close();
				}
				Unbind();
			}
		}
	}
}
