using Mono.Security.Protocol.Tls;
using System.IO;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace System.Net.Security
{
	/// <summary>Provides a stream used for client-server communication that uses the Secure Socket Layer (SSL) security protocol to authenticate the server and optionally the client.</summary>
	[MonoTODO("Non-X509Certificate2 certificate is not supported")]
	public class SslStream : AuthenticatedStream
	{
		private SslStreamBase ssl_stream;

		private RemoteCertificateValidationCallback validation_callback;

		private LocalCertificateSelectionCallback selection_callback;

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether the underlying stream is readable.</summary>
		/// <returns>true if authentication has occurred and the underlying stream is readable; otherwise false.</returns>
		public override bool CanRead => base.InnerStream.CanRead;

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether the underlying stream is seekable.</summary>
		/// <returns>This property always returns false.</returns>
		public override bool CanSeek => base.InnerStream.CanSeek;

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether the underlying stream supports time-outs.</summary>
		/// <returns>true if the underlying stream supports time-outs; otherwise, false.</returns>
		public override bool CanTimeout => base.InnerStream.CanTimeout;

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether the underlying stream is writable.</summary>
		/// <returns>true if authentication has occurred and the underlying stream is writable; otherwise false.</returns>
		public override bool CanWrite => base.InnerStream.CanWrite;

		/// <summary>Gets the length of the underlying stream.</summary>
		/// <returns>A <see cref="T:System.Int64" />.</returns>
		/// <exception cref="T:System.NotSupportedException">Getting the value of this property is not supported when the underlying stream is a <see cref="T:System.Net.Sockets.NetworkStream" />.</exception>
		public override long Length => base.InnerStream.Length;

		/// <summary>Gets or sets the current position in the underlying stream.</summary>
		/// <returns>A <see cref="T:System.Int64" />.</returns>
		/// <exception cref="T:System.NotSupportedException">Setting this property is not supported.-or-Getting the value of this property is not supported when the underlying stream is a <see cref="T:System.Net.Sockets.NetworkStream" />.</exception>
		public override long Position
		{
			get
			{
				return base.InnerStream.Position;
			}
			set
			{
				throw new NotSupportedException("This stream does not support seek operations");
			}
		}

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether authentication was successful.</summary>
		/// <returns>true if successful authentication occurred; otherwise, false.</returns>
		public override bool IsAuthenticated => ssl_stream != null;

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether this <see cref="T:System.Net.Security.SslStream" /> uses data encryption.</summary>
		/// <returns>true if data is encrypted before being transmitted over the network and decrypted when it reaches the remote endpoint; otherwise false.</returns>
		public override bool IsEncrypted => IsAuthenticated;

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether both server and client have been authenticated.</summary>
		/// <returns>true if the server has been authenticated; otherwise false.</returns>
		public override bool IsMutuallyAuthenticated => IsAuthenticated && ((!IsServer) ? (LocalCertificate != null) : (RemoteCertificate != null));

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether the local side of the connection used by this <see cref="T:System.Net.Security.SslStream" /> was authenticated as the server.</summary>
		/// <returns>true if the local endpoint was successfully authenticated as the server side of the authenticated connection; otherwise false.</returns>
		public override bool IsServer => ssl_stream is SslServerStream;

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether the data sent using this stream is signed.</summary>
		/// <returns>true if the data is signed before being transmitted; otherwise false.</returns>
		public override bool IsSigned => IsAuthenticated;

		/// <summary>Gets or sets the amount of time a read operation blocks waiting for data.</summary>
		/// <returns>A <see cref="T:System.Int32" /> that specifies the amount of time that elapses before a synchronous read operation fails.</returns>
		public override int ReadTimeout
		{
			get
			{
				return base.InnerStream.ReadTimeout;
			}
			set
			{
				base.InnerStream.ReadTimeout = value;
			}
		}

		/// <summary>Gets or sets the amount of time a write operation blocks waiting for data.</summary>
		/// <returns>A <see cref="T:System.Int32" /> that specifies the amount of time that elapses before a synchronous write operation fails. </returns>
		public override int WriteTimeout
		{
			get
			{
				return base.InnerStream.WriteTimeout;
			}
			set
			{
				base.InnerStream.WriteTimeout = value;
			}
		}

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether the certificate revocation list is checked during the certificate validation process.</summary>
		/// <returns>true if the certificate revocation list is checked; otherwise, false.</returns>
		public virtual bool CheckCertRevocationStatus
		{
			get
			{
				if (!IsAuthenticated)
				{
					return false;
				}
				return ssl_stream.CheckCertRevocationStatus;
			}
		}

		/// <summary>Gets a value that identifies the bulk encryption algorithm used by this <see cref="T:System.Net.Security.SslStream" />.</summary>
		/// <returns>A <see cref="T:System.Security.Authentication.CipherAlgorithmType" /> value.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Net.Security.SslStream.CipherAlgorithm" /> property was accessed before the completion of the authentication process or the authentication process failed.</exception>
		public virtual System.Security.Authentication.CipherAlgorithmType CipherAlgorithm
		{
			get
			{
				CheckConnectionAuthenticated();
				switch (ssl_stream.CipherAlgorithm)
				{
				case Mono.Security.Protocol.Tls.CipherAlgorithmType.Des:
					return System.Security.Authentication.CipherAlgorithmType.Des;
				case Mono.Security.Protocol.Tls.CipherAlgorithmType.None:
					return System.Security.Authentication.CipherAlgorithmType.None;
				case Mono.Security.Protocol.Tls.CipherAlgorithmType.Rc2:
					return System.Security.Authentication.CipherAlgorithmType.Rc2;
				case Mono.Security.Protocol.Tls.CipherAlgorithmType.Rc4:
					return System.Security.Authentication.CipherAlgorithmType.Rc4;
				case Mono.Security.Protocol.Tls.CipherAlgorithmType.TripleDes:
					return System.Security.Authentication.CipherAlgorithmType.TripleDes;
				case Mono.Security.Protocol.Tls.CipherAlgorithmType.Rijndael:
					switch (ssl_stream.CipherStrength)
					{
					case 128:
						return System.Security.Authentication.CipherAlgorithmType.Aes128;
					case 192:
						return System.Security.Authentication.CipherAlgorithmType.Aes192;
					case 256:
						return System.Security.Authentication.CipherAlgorithmType.Aes256;
					}
					break;
				}
				throw new InvalidOperationException("Not supported cipher algorithm is in use. It is likely a bug in SslStream.");
			}
		}

		/// <summary>Gets a value that identifies the strength of the cipher algorithm used by this <see cref="T:System.Net.Security.SslStream" />.</summary>
		/// <returns>An <see cref="T:System.Int32" /> value that specifies the strength of the algorithm, in bits.</returns>
		public virtual int CipherStrength
		{
			get
			{
				CheckConnectionAuthenticated();
				return ssl_stream.CipherStrength;
			}
		}

		/// <summary>Gets the algorithm used for generating message authentication codes (MACs).</summary>
		/// <returns>A <see cref="T:System.Security.Authentication.HashAlgorithmType" /> value.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Net.Security.SslStream.HashAlgorithm" /> property was accessed before the completion of the authentication process or the authentication process failed.</exception>
		public virtual System.Security.Authentication.HashAlgorithmType HashAlgorithm
		{
			get
			{
				CheckConnectionAuthenticated();
				switch (ssl_stream.HashAlgorithm)
				{
				case Mono.Security.Protocol.Tls.HashAlgorithmType.Md5:
					return System.Security.Authentication.HashAlgorithmType.Md5;
				case Mono.Security.Protocol.Tls.HashAlgorithmType.None:
					return System.Security.Authentication.HashAlgorithmType.None;
				case Mono.Security.Protocol.Tls.HashAlgorithmType.Sha1:
					return System.Security.Authentication.HashAlgorithmType.Sha1;
				default:
					throw new InvalidOperationException("Not supported hash algorithm is in use. It is likely a bug in SslStream.");
				}
			}
		}

		/// <summary>Gets a value that identifies the strength of the hash algorithm used by this instance.</summary>
		/// <returns>An <see cref="T:System.Int32" /> value that specifies the strength of the <see cref="T:System.Security.Authentication.HashAlgorithmType" /> algorithm, in bits. Valid values are 128 or 160.</returns>
		public virtual int HashStrength
		{
			get
			{
				CheckConnectionAuthenticated();
				return ssl_stream.HashStrength;
			}
		}

		/// <summary>Gets the key exchange algorithm used by this <see cref="T:System.Net.Security.SslStream" />.</summary>
		/// <returns>An <see cref="T:System.Security.Authentication.ExchangeAlgorithmType" /> value.</returns>
		public virtual System.Security.Authentication.ExchangeAlgorithmType KeyExchangeAlgorithm
		{
			get
			{
				CheckConnectionAuthenticated();
				switch (ssl_stream.KeyExchangeAlgorithm)
				{
				case Mono.Security.Protocol.Tls.ExchangeAlgorithmType.DiffieHellman:
					return System.Security.Authentication.ExchangeAlgorithmType.DiffieHellman;
				case Mono.Security.Protocol.Tls.ExchangeAlgorithmType.None:
					return System.Security.Authentication.ExchangeAlgorithmType.None;
				case Mono.Security.Protocol.Tls.ExchangeAlgorithmType.RsaKeyX:
					return System.Security.Authentication.ExchangeAlgorithmType.RsaKeyX;
				case Mono.Security.Protocol.Tls.ExchangeAlgorithmType.RsaSign:
					return System.Security.Authentication.ExchangeAlgorithmType.RsaSign;
				default:
					throw new InvalidOperationException("Not supported exchange algorithm is in use. It is likely a bug in SslStream.");
				}
			}
		}

		/// <summary>Gets a value that identifies the strength of the key exchange algorithm used by this instance.</summary>
		/// <returns>An <see cref="T:System.Int32" /> value that specifies the strength of the <see cref="T:System.Security.Authentication.ExchangeAlgorithmType" /> algorithm, in bits.</returns>
		public virtual int KeyExchangeStrength
		{
			get
			{
				CheckConnectionAuthenticated();
				return ssl_stream.KeyExchangeStrength;
			}
		}

		/// <summary>Gets the certificate used to authenticate the local endpoint.</summary>
		/// <returns>An X509Certificate object that represents the certificate supplied for authentication or null if no certificate was supplied.</returns>
		/// <exception cref="T:System.InvalidOperationException">Authentication failed or has not occurred.</exception>
		public virtual X509Certificate LocalCertificate
		{
			get
			{
				CheckConnectionAuthenticated();
				return (!IsServer) ? ((SslClientStream)ssl_stream).SelectedClientCertificate : ssl_stream.ServerCertificate;
			}
		}

		/// <summary>Gets the certificate used to authenticate the remote endpoint.</summary>
		/// <returns>An X509Certificate object that represents the certificate supplied for authentication or null if no certificate was supplied.</returns>
		/// <exception cref="T:System.InvalidOperationException">Authentication failed or has not occurred.</exception>
		public virtual X509Certificate RemoteCertificate
		{
			get
			{
				CheckConnectionAuthenticated();
				return IsServer ? ((SslServerStream)ssl_stream).ClientCertificate : ssl_stream.ServerCertificate;
			}
		}

		/// <summary>Gets a value that indicates the security protocol used to authenticate this connection.</summary>
		/// <returns>The <see cref="T:System.Security.Authentication.SslProtocols" /> value that represents the protocol used for authentication.</returns>
		public virtual SslProtocols SslProtocol
		{
			get
			{
				CheckConnectionAuthenticated();
				switch (ssl_stream.SecurityProtocol)
				{
				case Mono.Security.Protocol.Tls.SecurityProtocolType.Default:
					return SslProtocols.Default;
				case Mono.Security.Protocol.Tls.SecurityProtocolType.Ssl2:
					return SslProtocols.Ssl2;
				case Mono.Security.Protocol.Tls.SecurityProtocolType.Ssl3:
					return SslProtocols.Ssl3;
				case Mono.Security.Protocol.Tls.SecurityProtocolType.Tls:
					return SslProtocols.Tls;
				default:
					throw new InvalidOperationException("Not supported SSL/TLS protocol is in use. It is likely a bug in SslStream.");
				}
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Security.SslStream" /> class using the specified <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="innerStream">A <see cref="T:System.IO.Stream" /> object used by the <see cref="T:System.Net.Security.SslStream" /> for sending and receiving data.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="innerStream" /> is not readable.-or-<paramref name="innerStream" /> is not writable.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="innerStream" /> is null.-or-<paramref name="innerStream" /> is equal to <see cref="F:System.IO.Stream.Null" />.</exception>
		public SslStream(Stream innerStream)
			: this(innerStream, leaveStreamOpen: false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Security.SslStream" /> class using the specified <see cref="T:System.IO.Stream" /> and stream closure behavior.</summary>
		/// <param name="innerStream">Initializes a new instance of the <see cref="T:System.Net.Security.SslStream" /> class using the specified <see cref="T:System.IO.Stream" /> and stream closure behavior.</param>
		/// <param name="leaveInnerStreamOpen">Initializes a new instance of the <see cref="T:System.Net.Security.SslStream" /> class using the specified <see cref="T:System.IO.Stream" /> and stream closure behavior.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="innerStream" /> is not readable.-or-<paramref name="innerStream" /> is not writable.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="innerStream" /> is null.-or-<paramref name="innerStream" /> is equal to <see cref="F:System.IO.Stream.Null" />.</exception>
		public SslStream(Stream innerStream, bool leaveStreamOpen)
			: base(innerStream, leaveStreamOpen)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Security.SslStream" /> class using the specified <see cref="T:System.IO.Stream" />, stream closure behavior and certificate validation delegate.</summary>
		/// <param name="innerStream">Initializes a new instance of the <see cref="T:System.Net.Security.SslStream" /> class using the specified <see cref="T:System.IO.Stream" /> and stream closure behavior.</param>
		/// <param name="leaveInnerStreamOpen">Initializes a new instance of the <see cref="T:System.Net.Security.SslStream" /> class using the specified <see cref="T:System.IO.Stream" /> and stream closure behavior.</param>
		/// <param name="userCertificateValidationCallback">A <see cref="T:System.Net.Security.RemoteCertificateValidationCallback" /> delegate responsible for validating the certificate supplied by the remote party.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="innerStream" /> is not readable.-or-<paramref name="innerStream" /> is not writable.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="innerStream" /> is null.-or-<paramref name="innerStream" /> is equal to <see cref="F:System.IO.Stream.Null" />.</exception>
		[MonoTODO("certValidationCallback is not passed X509Chain and SslPolicyErrors correctly")]
		public SslStream(Stream innerStream, bool leaveStreamOpen, RemoteCertificateValidationCallback certValidationCallback)
			: this(innerStream, leaveStreamOpen, certValidationCallback, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Security.SslStream" /> class using the specified <see cref="T:System.IO.Stream" />, stream closure behavior, certificate validation delegate and certificate selection delegate.</summary>
		/// <param name="innerStream">Initializes a new instance of the <see cref="T:System.Net.Security.SslStream" /> class using the specified <see cref="T:System.IO.Stream" /> and stream closure behavior.</param>
		/// <param name="leaveInnerStreamOpen">Initializes a new instance of the <see cref="T:System.Net.Security.SslStream" /> class using the specified <see cref="T:System.IO.Stream" /> and stream closure behavior.</param>
		/// <param name="userCertificateValidationCallback">A <see cref="T:System.Net.Security.RemoteCertificateValidationCallback" /> delegate responsible for validating the certificate supplied by the remote party.</param>
		/// <param name="userCertificateSelectionCallback">A <see cref="T:System.Net.Security.LocalCertificateSelectionCallback" /> delegate responsible for selecting the certificate used for authentication.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="innerStream" /> is not readable.-or-<paramref name="innerStream" /> is not writable.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="innerStream" /> is null.-or-<paramref name="innerStream" /> is equal to <see cref="F:System.IO.Stream.Null" />.</exception>
		[MonoTODO("certValidationCallback is not passed X509Chain and SslPolicyErrors correctly")]
		public SslStream(Stream innerStream, bool leaveStreamOpen, RemoteCertificateValidationCallback certValidationCallback, LocalCertificateSelectionCallback certSelectionCallback)
			: base(innerStream, leaveStreamOpen)
		{
			validation_callback = certValidationCallback;
			selection_callback = certSelectionCallback;
		}

		private X509Certificate OnCertificateSelection(X509CertificateCollection clientCerts, X509Certificate serverCert, string targetHost, X509CertificateCollection serverRequestedCerts)
		{
			string[] array = new string[serverRequestedCerts?.Count ?? 0];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = serverRequestedCerts[i].GetIssuerName();
			}
			return selection_callback(this, targetHost, clientCerts, serverCert, array);
		}

		/// <summary>Called by clients to begin an asynchronous operation to authenticate the server and optionally the client.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> object that indicates the status of the asynchronous operation. </returns>
		/// <param name="targetHost">The name of the server that shares this <see cref="T:System.Net.Security.SslStream" />.</param>
		/// <param name="asyncCallback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the authentication is complete. </param>
		/// <param name="asyncState">A user-defined object that contains information about the operation. This object is passed to the <paramref name="asyncCallback" /> delegate when the operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="targetHost" /> is null.</exception>
		/// <exception cref="T:System.Security.Authentication.AuthenticationException">The authentication failed and left this object in an unusable state.</exception>
		/// <exception cref="T:System.InvalidOperationException">Authentication has already occurred.-or-Server authentication using this <see cref="T:System.Net.Security.SslStream" /> was tried previously.-or- Authentication is already in progress.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
		public virtual IAsyncResult BeginAuthenticateAsClient(string targetHost, AsyncCallback asyncCallback, object asyncState)
		{
			return BeginAuthenticateAsClient(targetHost, new X509CertificateCollection(), SslProtocols.Tls, checkCertificateRevocation: false, asyncCallback, asyncState);
		}

		/// <summary>Called by clients to begin an asynchronous operation to authenticate the server and optionally the client using the specified certificates and security protocol.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> object that indicates the status of the asynchronous operation. </returns>
		/// <param name="targetHost">The name of the server that shares this <see cref="T:System.Net.Security.SslStream" />.</param>
		/// <param name="clientCertificates">The <see cref="T:System.Security.Cryptography.X509Certificates.X509CertificateCollection" /> containing client certificates.</param>
		/// <param name="enabledSslProtocols">The <see cref="T:System.Security.Authentication.SslProtocols" /> value that represents the protocol used for authentication.</param>
		/// <param name="checkCertificateRevocation">A <see cref="T:System.Boolean" /> value that specifies whether the certificate revocation list is checked during authentication.</param>
		/// <param name="asyncCallback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the authentication is complete. </param>
		/// <param name="asyncState">A user-defined object that contains information about the operation. This object is passed to the <paramref name="asyncCallback" /> delegate when the operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="targetHost" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="enabledSslProtocols" /> is not a valid <see cref="T:System.Security.Authentication.SslProtocols" />  value.</exception>
		/// <exception cref="T:System.Security.Authentication.AuthenticationException">The authentication failed and left this object in an unusable state.</exception>
		/// <exception cref="T:System.InvalidOperationException">Authentication has already occurred.-or-Server authentication using this <see cref="T:System.Net.Security.SslStream" /> was tried previously.-or- Authentication is already in progress.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
		public virtual IAsyncResult BeginAuthenticateAsClient(string targetHost, X509CertificateCollection clientCertificates, SslProtocols sslProtocolType, bool checkCertificateRevocation, AsyncCallback asyncCallback, object asyncState)
		{
			if (IsAuthenticated)
			{
				throw new InvalidOperationException("This SslStream is already authenticated");
			}
			SslClientStream sslClientStream = new SslClientStream(base.InnerStream, targetHost, !base.LeaveInnerStreamOpen, GetMonoSslProtocol(sslProtocolType), clientCertificates);
			sslClientStream.CheckCertRevocationStatus = checkCertificateRevocation;
			sslClientStream.PrivateKeyCertSelectionDelegate = delegate(X509Certificate cert, string host)
			{
				string certHashString = cert.GetCertHashString();
				foreach (X509Certificate clientCertificate in clientCertificates)
				{
					if (!(clientCertificate.GetCertHashString() != certHashString))
					{
						X509Certificate2 x509Certificate2 = clientCertificate as X509Certificate2;
						x509Certificate2 = (x509Certificate2 ?? new X509Certificate2(clientCertificate));
						return x509Certificate2.PrivateKey;
					}
				}
				return null;
			};
			if (validation_callback != null)
			{
				sslClientStream.ServerCertValidationDelegate = delegate(X509Certificate cert, int[] certErrors)
				{
					X509Chain x509Chain = new X509Chain();
					X509Certificate2 x509Certificate = cert as X509Certificate2;
					if (x509Certificate == null)
					{
						x509Certificate = new X509Certificate2(cert);
					}
					if (!ServicePointManager.CheckCertificateRevocationList)
					{
						x509Chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
					}
					SslPolicyErrors sslPolicyErrors = SslPolicyErrors.None;
					for (int i = 0; i < certErrors.Length; i++)
					{
						switch (certErrors[i])
						{
						case -2146762490:
							sslPolicyErrors |= SslPolicyErrors.RemoteCertificateNotAvailable;
							break;
						case -2146762481:
							sslPolicyErrors |= SslPolicyErrors.RemoteCertificateNameMismatch;
							break;
						default:
							sslPolicyErrors |= SslPolicyErrors.RemoteCertificateChainErrors;
							break;
						}
					}
					x509Chain.Build(x509Certificate);
					X509ChainStatus[] chainStatus = x509Chain.ChainStatus;
					for (int j = 0; j < chainStatus.Length; j++)
					{
						X509ChainStatus x509ChainStatus = chainStatus[j];
						if (x509ChainStatus.Status != 0)
						{
							sslPolicyErrors = (((x509ChainStatus.Status & X509ChainStatusFlags.PartialChain) == X509ChainStatusFlags.NoError) ? (sslPolicyErrors | SslPolicyErrors.RemoteCertificateChainErrors) : (sslPolicyErrors | SslPolicyErrors.RemoteCertificateNotAvailable));
						}
					}
					return validation_callback(this, cert, x509Chain, sslPolicyErrors);
				};
			}
			if (selection_callback != null)
			{
				sslClientStream.ClientCertSelectionDelegate = OnCertificateSelection;
			}
			ssl_stream = sslClientStream;
			return BeginWrite(new byte[0], 0, 0, asyncCallback, asyncState);
		}

		/// <summary>Begins an asynchronous read operation that reads data from the stream and stores it in the specified array.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> object that indicates the status of the asynchronous operation. </returns>
		/// <param name="buffer">A <see cref="T:System.Byte" /> array that receives the bytes read from the stream.</param>
		/// <param name="offset">The zero-based location in <paramref name="buffer" /> at which to begin storing the data read from this stream.</param>
		/// <param name="count">The maximum number of bytes to read from the stream.</param>
		/// <param name="asyncCallback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the read operation is complete. </param>
		/// <param name="asyncState">A user-defined object that contains information about the read operation. This object is passed to the <paramref name="asyncCallback" /> delegate when the operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="offset" />
		///   <paramref name="&lt;" />
		///   <paramref name="0" />.<paramref name="-or-" /><paramref name="offset" /> &gt; the length of <paramref name="buffer" />.-or-<paramref name="offset" /> + count &gt; the length of <paramref name="buffer" />.</exception>
		/// <exception cref="T:System.IO.IOException">The read operation failed.-or-Encryption is in use, but the data could not be decrypted.</exception>
		/// <exception cref="T:System.NotSupportedException">There is already a read operation in progress.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
		/// <exception cref="T:System.InvalidOperationException">Authentication has not occurred.</exception>
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback asyncCallback, object asyncState)
		{
			CheckConnectionAuthenticated();
			return ssl_stream.BeginRead(buffer, offset, count, asyncCallback, asyncState);
		}

		/// <summary>Called by servers to begin an asynchronous operation to authenticate the client and optionally the server in a client-server connection.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> object indicating the status of the asynchronous operation. </returns>
		/// <param name="serverCertificate">The X509Certificate used to authenticate the server.</param>
		/// <param name="asyncCallback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the authentication is complete.</param>
		/// <param name="asyncState">A user-defined object that contains information about the operation. This object is passed to the <paramref name="asyncCallback" /> delegate when the operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="serverCertificate" /> is null.</exception>
		/// <exception cref="T:System.Security.Authentication.AuthenticationException">The authentication failed and left this object in an unusable state.</exception>
		/// <exception cref="T:System.InvalidOperationException">Authentication has already occurred.-or-Client authentication using this <see cref="T:System.Net.Security.SslStream" /> was tried previously.-or- Authentication is already in progress.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The <see cref="Overload:System.Net.Security.SslStream.BeginAuthenticateAsServer" /> method is not supported on Windows 95, Windows 98, or Windows Millennium.</exception>
		public virtual IAsyncResult BeginAuthenticateAsServer(X509Certificate serverCertificate, AsyncCallback callback, object asyncState)
		{
			return BeginAuthenticateAsServer(serverCertificate, clientCertificateRequired: false, SslProtocols.Tls, checkCertificateRevocation: false, callback, asyncState);
		}

		/// <summary>Called by servers to begin an asynchronous operation to authenticate the server and optionally the client using the specified certificates, requirements and security protocol.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> object that indicates the status of the asynchronous operation. </returns>
		/// <param name="serverCertificate">The X509Certificate used to authenticate the server.</param>
		/// <param name="clientCertificateRequired">A <see cref="T:System.Boolean" /> value that specifies whether the client must supply a certificate for authentication.</param>
		/// <param name="enabledSslProtocols">The <see cref="T:System.Security.Authentication.SslProtocols" />  value that represents the protocol used for authentication.</param>
		/// <param name="checkCertificateRevocation">A <see cref="T:System.Boolean" /> value that specifies whether the certificate revocation list is checked during authentication.</param>
		/// <param name="asyncCallback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the authentication is complete. </param>
		/// <param name="asyncState">A user-defined object that contains information about the operation. This object is passed to the <paramref name="asyncCallback" /> delegate when the operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="serverCertificate" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="enabledSslProtocols" /> is not a valid <see cref="T:System.Security.Authentication.SslProtocols" /> value.</exception>
		/// <exception cref="T:System.Security.Authentication.AuthenticationException">The authentication failed and left this object in an unusable state.</exception>
		/// <exception cref="T:System.InvalidOperationException">Authentication has already occurred.-or-Server authentication using this <see cref="T:System.Net.Security.SslStream" /> was tried previously.-or- Authentication is already in progress.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The <see cref="Overload:System.Net.Security.SslStream.BeginAuthenticateAsServer" /> method is not supported on Windows 95, Windows 98, or Windows Millennium.</exception>
		public virtual IAsyncResult BeginAuthenticateAsServer(X509Certificate serverCertificate, bool clientCertificateRequired, SslProtocols sslProtocolType, bool checkCertificateRevocation, AsyncCallback callback, object asyncState)
		{
			if (IsAuthenticated)
			{
				throw new InvalidOperationException("This SslStream is already authenticated");
			}
			SslServerStream sslServerStream = new SslServerStream(base.InnerStream, serverCertificate, clientCertificateRequired, !base.LeaveInnerStreamOpen, GetMonoSslProtocol(sslProtocolType));
			sslServerStream.CheckCertRevocationStatus = checkCertificateRevocation;
			sslServerStream.PrivateKeyCertSelectionDelegate = ((X509Certificate cert, string targetHost) => ((serverCertificate as X509Certificate2) ?? new X509Certificate2(serverCertificate))?.PrivateKey);
			if (validation_callback != null)
			{
				sslServerStream.ClientCertValidationDelegate = delegate(X509Certificate cert, int[] certErrors)
				{
					X509Chain x509Chain = null;
					if (cert is X509Certificate2)
					{
						x509Chain = new X509Chain();
						x509Chain.Build((X509Certificate2)cert);
					}
					SslPolicyErrors sslPolicyErrors = (certErrors.Length > 0) ? SslPolicyErrors.RemoteCertificateChainErrors : SslPolicyErrors.None;
					return validation_callback(this, cert, x509Chain, sslPolicyErrors);
				};
			}
			ssl_stream = sslServerStream;
			return BeginRead(new byte[0], 0, 0, callback, asyncState);
		}

		private Mono.Security.Protocol.Tls.SecurityProtocolType GetMonoSslProtocol(SslProtocols ms)
		{
			switch (ms)
			{
			case SslProtocols.Ssl2:
				return Mono.Security.Protocol.Tls.SecurityProtocolType.Ssl2;
			case SslProtocols.Ssl3:
				return Mono.Security.Protocol.Tls.SecurityProtocolType.Ssl3;
			case SslProtocols.Tls:
				return Mono.Security.Protocol.Tls.SecurityProtocolType.Tls;
			default:
				return Mono.Security.Protocol.Tls.SecurityProtocolType.Default;
			}
		}

		/// <summary>Begins an asynchronous write operation that writes <see cref="T:System.Byte" />s from the specified buffer to the stream.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> object indicating the status of the asynchronous operation. </returns>
		/// <param name="buffer">A <see cref="T:System.Byte" /> array that supplies the bytes to be written to the stream.</param>
		/// <param name="offset">The zero-based location in <paramref name="buffer" /> at which to begin reading bytes to be written to the stream.</param>
		/// <param name="count">An <see cref="T:System.Int32" /> value that specifies the number of bytes to read from <paramref name="buffer" />.</param>
		/// <param name="asyncCallback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the write operation is complete. </param>
		/// <param name="asyncState">A user-defined object that contains information about the write operation. This object is passed to the <paramref name="asyncCallback" /> delegate when the operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="offset" />
		///   <paramref name="&lt;" />
		///   <paramref name="0" />.<paramref name="-or-" /><paramref name="offset" /> &gt; the length of <paramref name="buffer" />.-or-<paramref name="offset" /> + count &gt; the length of <paramref name="buffer" />.</exception>
		/// <exception cref="T:System.IO.IOException">The write operation failed.</exception>
		/// <exception cref="T:System.NotSupportedException">There is already a write operation in progress.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
		/// <exception cref="T:System.InvalidOperationException">Authentication has not occurred.</exception>
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback asyncCallback, object asyncState)
		{
			CheckConnectionAuthenticated();
			return ssl_stream.BeginWrite(buffer, offset, count, asyncCallback, asyncState);
		}

		/// <summary>Called by clients to authenticate the server and optionally the client in a client-server connection.</summary>
		/// <param name="targetHost">The name of the server that shares this <see cref="T:System.Net.Security.SslStream" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="targetHost" /> is null.</exception>
		/// <exception cref="T:System.Security.Authentication.AuthenticationException">The authentication failed and left this object in an unusable state.</exception>
		/// <exception cref="T:System.InvalidOperationException">Authentication has already occurred.-or-Server authentication using this <see cref="T:System.Net.Security.SslStream" /> was tried previously.-or- Authentication is already in progress.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
		public virtual void AuthenticateAsClient(string targetHost)
		{
			AuthenticateAsClient(targetHost, new X509CertificateCollection(), SslProtocols.Tls, checkCertificateRevocation: false);
		}

		/// <summary>Called by clients to authenticate the server and optionally the client in a client-server connection. The authentication process uses the specified certificate collection and SSL protocol.</summary>
		/// <param name="targetHost">The name of the server that will share this <see cref="T:System.Net.Security.SslStream" />.</param>
		/// <param name="clientCertificates">The <see cref="T:System.Security.Cryptography.X509Certificates.X509CertificateCollection" /> that contains client certificates.</param>
		/// <param name="enabledSslProtocols">The <see cref="T:System.Security.Authentication.SslProtocols" /> value that represents the protocol used for authentication.</param>
		/// <param name="checkCertificateRevocation">A <see cref="T:System.Boolean" /> value that specifies whether the certificate revocation list is checked during authentication.</param>
		public virtual void AuthenticateAsClient(string targetHost, X509CertificateCollection clientCertificates, SslProtocols sslProtocolType, bool checkCertificateRevocation)
		{
			EndAuthenticateAsClient(BeginAuthenticateAsClient(targetHost, clientCertificates, sslProtocolType, checkCertificateRevocation, null, null));
		}

		/// <summary>Called by servers to authenticate the server and optionally the client in a client-server connection using the specified certificate.</summary>
		/// <param name="serverCertificate">The certificate used to authenticate the server.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="serverCertificate" /> is null.</exception>
		/// <exception cref="T:System.Security.Authentication.AuthenticationException">The authentication failed and left this object in an unusable state.</exception>
		/// <exception cref="T:System.InvalidOperationException">Authentication has already occurred.-or-Client authentication using this <see cref="T:System.Net.Security.SslStream" /> was tried previously.-or- Authentication is already in progress.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The <see cref="Overload:System.Net.Security.SslStream.AuthenticateAsServer" /> method is not supported on Windows 95, Windows 98, or Windows Millennium.</exception>
		public virtual void AuthenticateAsServer(X509Certificate serverCertificate)
		{
			AuthenticateAsServer(serverCertificate, clientCertificateRequired: false, SslProtocols.Tls, checkCertificateRevocation: false);
		}

		/// <summary>Called by servers to begin an asynchronous operation to authenticate the server and optionally the client using the specified certificates, requirements and security protocol.</summary>
		/// <param name="serverCertificate">The X509Certificate used to authenticate the server.</param>
		/// <param name="clientCertificateRequired">A <see cref="T:System.Boolean" /> value that specifies whether the client must supply a certificate for authentication.</param>
		/// <param name="enabledSslProtocols">The <see cref="T:System.Security.Authentication.SslProtocols" />  value that represents the protocol used for authentication.</param>
		/// <param name="checkCertificateRevocation">A <see cref="T:System.Boolean" /> value that specifies whether the certificate revocation list is checked during authentication.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="serverCertificate" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="enabledSslProtocols" /> is not a valid <see cref="T:System.Security.Authentication.SslProtocols" /> value.</exception>
		/// <exception cref="T:System.Security.Authentication.AuthenticationException">The authentication failed and left this object in an unusable state.</exception>
		/// <exception cref="T:System.InvalidOperationException">Authentication has already occurred.-or-Client authentication using this <see cref="T:System.Net.Security.SslStream" /> was tried previously.-or- Authentication is already in progress.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The <see cref="Overload:System.Net.Security.SslStream.AuthenticateAsServer" /> method is not supported on Windows 95, Windows 98, or Windows Millennium.</exception>
		public virtual void AuthenticateAsServer(X509Certificate serverCertificate, bool clientCertificateRequired, SslProtocols sslProtocolType, bool checkCertificateRevocation)
		{
			EndAuthenticateAsServer(BeginAuthenticateAsServer(serverCertificate, clientCertificateRequired, sslProtocolType, checkCertificateRevocation, null, null));
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Net.Security.SslStream" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (ssl_stream != null)
				{
					ssl_stream.Dispose();
				}
				ssl_stream = null;
			}
			base.Dispose(disposing);
		}

		/// <summary>Ends a pending asynchronous server authentication operation started with a previous call to <see cref="Overload:System.Net.Security.SslStream.BeginAuthenticateAsServer" />.</summary>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> instance returned by a call to <see cref="Overload:System.Net.Security.SslStream.BeginAuthenticateAsServer" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not created by a call to <see cref="Overload:System.Net.Security.SslStream.BeginAuthenticateAsServer" />.</exception>
		/// <exception cref="T:System.Security.Authentication.AuthenticationException">The authentication failed and left this object in an unusable state.</exception>
		/// <exception cref="T:System.InvalidOperationException">There is no pending server authentication to complete.</exception>
		public virtual void EndAuthenticateAsClient(IAsyncResult asyncResult)
		{
			CheckConnectionAuthenticated();
			if (CanRead)
			{
				ssl_stream.EndRead(asyncResult);
			}
			else
			{
				ssl_stream.EndWrite(asyncResult);
			}
		}

		/// <summary>Ends a pending asynchronous client authentication operation started with a previous call to <see cref="Overload:System.Net.Security.SslStream.BeginAuthenticateAsClient" />.</summary>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> instance returned by a call to <see cref="Overload:System.Net.Security.SslStream.BeginAuthenticateAsClient" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not created by a call to <see cref="Overload:System.Net.Security.SslStream.BeginAuthenticateAsClient" />.</exception>
		/// <exception cref="T:System.Security.Authentication.AuthenticationException">The authentication failed and left this object in an unusable state.</exception>
		/// <exception cref="T:System.InvalidOperationException">There is no pending client authentication to complete.</exception>
		public virtual void EndAuthenticateAsServer(IAsyncResult asyncResult)
		{
			CheckConnectionAuthenticated();
			if (CanRead)
			{
				ssl_stream.EndRead(asyncResult);
			}
			else
			{
				ssl_stream.EndWrite(asyncResult);
			}
		}

		/// <summary>Ends an asynchronous read operation started with a previous call to <see cref="M:System.Net.Security.SslStream.BeginRead(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" />.</summary>
		/// <returns>A <see cref="T:System.Int32" /> value that specifies the number of bytes read from the underlying stream.</returns>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> instance returned by a call to <see cref="M:System.Net.Security.SslStream.BeginRead(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" /></param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not created by a call to <see cref="M:System.Net.Security.SslStream.BeginRead(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">There is no pending read operation to complete.</exception>
		/// <exception cref="T:System.IO.IOException">The read operation failed.</exception>
		/// <exception cref="T:System.InvalidOperationException">Authentication has not occurred.</exception>
		public override int EndRead(IAsyncResult asyncResult)
		{
			CheckConnectionAuthenticated();
			return ssl_stream.EndRead(asyncResult);
		}

		/// <summary>Ends an asynchronous write operation started with a previous call to <see cref="M:System.Net.Security.SslStream.BeginWrite(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" />.</summary>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> instance returned by a call to <see cref="M:System.Net.Security.SslStream.BeginWrite(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" /></param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not created by a call to <see cref="M:System.Net.Security.SslStream.BeginWrite(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">There is no pending write operation to complete.</exception>
		/// <exception cref="T:System.IO.IOException">The write operation failed.</exception>
		/// <exception cref="T:System.InvalidOperationException">Authentication has not occurred.</exception>
		public override void EndWrite(IAsyncResult asyncResult)
		{
			CheckConnectionAuthenticated();
			ssl_stream.EndWrite(asyncResult);
		}

		/// <summary>Causes any buffered data to be written to the underlying device.</summary>
		public override void Flush()
		{
			CheckConnectionAuthenticated();
			base.InnerStream.Flush();
		}

		/// <summary>Reads data from this stream and stores it in the specified array.</summary>
		/// <returns>A <see cref="T:System.Int32" /> value that specifies the number of bytes read. When there is no more data to be read, returns 0.</returns>
		/// <param name="buffer">A <see cref="T:System.Byte" /> array that receives the bytes read from this stream.</param>
		/// <param name="offset">A <see cref="T:System.Int32" /> that contains the zero-based location in <paramref name="buffer" /> at which to begin storing the data read from this stream.</param>
		/// <param name="count">A <see cref="T:System.Int32" /> that contains the maximum number of bytes to read from this stream.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="offset" />
		///   <paramref name="&lt;" />
		///   <paramref name="0" />.<paramref name="-or-" /><paramref name="offset" /> &gt; the length of <paramref name="buffer" />.-or-<paramref name="offset" /> + count &gt; the length of <paramref name="buffer" />.</exception>
		/// <exception cref="T:System.IO.IOException">The read operation failed. Check the inner exception, if present to determine the cause of the failure.</exception>
		/// <exception cref="T:System.NotSupportedException">There is already a read operation in progress.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
		/// <exception cref="T:System.InvalidOperationException">Authentication has not occurred.</exception>
		public override int Read(byte[] buffer, int offset, int count)
		{
			return EndRead(BeginRead(buffer, offset, count, null, null));
		}

		/// <summary>Throws a <see cref="T:System.NotSupportedException" />.</summary>
		/// <returns>Always throws a <see cref="T:System.NotSupportedException" />.</returns>
		/// <param name="offset">This value is ignored.</param>
		/// <param name="origin">This value is ignored.</param>
		/// <exception cref="T:System.NotSupportedException">Seeking is not supported by <see cref="T:System.Net.Security.SslStream" /> objects.</exception>
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException("This stream does not support seek operations");
		}

		/// <summary>Sets the length of the underlying stream.</summary>
		/// <param name="value">An <see cref="T:System.Int64" /> value that specifies the length of the stream.</param>
		public override void SetLength(long value)
		{
			base.InnerStream.SetLength(value);
		}

		/// <summary>Write the specified number of <see cref="T:System.Byte" />s to the underlying stream using the specified buffer and offset.</summary>
		/// <param name="buffer">A <see cref="T:System.Byte" /> array that supplies the bytes written to the stream.</param>
		/// <param name="offset">A <see cref="T:System.Int32" /> that contains the zero-based location in <paramref name="buffer" /> at which to begin reading bytes to be written to the stream.</param>
		/// <param name="count">A <see cref="T:System.Int32" /> that contains the number of bytes to read from <paramref name="buffer" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="offset" />
		///   <paramref name="&lt;" />
		///   <paramref name="0" />.<paramref name="-or-" /><paramref name="offset" /> &gt; the length of <paramref name="buffer" />.-or-<paramref name="offset" /> + count &gt; the length of <paramref name="buffer" />.</exception>
		/// <exception cref="T:System.IO.IOException">The write operation failed.</exception>
		/// <exception cref="T:System.NotSupportedException">There is already a write operation in progress.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
		/// <exception cref="T:System.InvalidOperationException">Authentication has not occurred.</exception>
		public override void Write(byte[] buffer, int offset, int count)
		{
			EndWrite(BeginWrite(buffer, offset, count, null, null));
		}

		/// <summary>Writes the specified data to this stream.</summary>
		/// <param name="buffer">A <see cref="T:System.Byte" /> array that supplies the bytes written to the stream.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is null.</exception>
		/// <exception cref="T:System.IO.IOException">The write operation failed.</exception>
		/// <exception cref="T:System.NotSupportedException">There is already a write operation in progress.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
		/// <exception cref="T:System.InvalidOperationException">Authentication has not occurred.</exception>
		public void Write(byte[] buffer)
		{
			Write(buffer, 0, buffer.Length);
		}

		private void CheckConnectionAuthenticated()
		{
			if (!IsAuthenticated)
			{
				throw new InvalidOperationException("This operation is invalid until it is successfully authenticated");
			}
		}
	}
}
