using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine.Networking
{
	[StructLayout(LayoutKind.Sequential)]
	public class UploadHandler : IDisposable
	{
		[NonSerialized]
		internal IntPtr m_Ptr;

		public byte[] data => GetData();

		public string contentType
		{
			get
			{
				return GetContentType();
			}
			set
			{
				SetContentType(value);
			}
		}

		public float progress => GetProgress();

		internal UploadHandler()
		{
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void InternalCreateRaw(byte[] data);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		private extern void InternalDestroy();

		~UploadHandler()
		{
			InternalDestroy();
		}

		public void Dispose()
		{
			InternalDestroy();
			GC.SuppressFinalize(this);
		}

		internal virtual byte[] GetData()
		{
			return null;
		}

		internal virtual string GetContentType()
		{
			return "text/plain";
		}

		internal virtual void SetContentType(string newContentType)
		{
		}

		internal virtual float GetProgress()
		{
			return 0.5f;
		}
	}
}
