using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine.Networking
{
	[StructLayout(LayoutKind.Sequential)]
	public sealed class UploadHandlerRaw : UploadHandler
	{
		public UploadHandlerRaw(byte[] data)
		{
			InternalCreateRaw(data);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern string InternalGetContentType();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void InternalSetContentType(string newContentType);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern byte[] InternalGetData();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern float InternalGetProgress();

		internal override string GetContentType()
		{
			return InternalGetContentType();
		}

		internal override void SetContentType(string newContentType)
		{
			InternalSetContentType(newContentType);
		}

		internal override byte[] GetData()
		{
			return InternalGetData();
		}

		internal override float GetProgress()
		{
			return InternalGetProgress();
		}
	}
}
