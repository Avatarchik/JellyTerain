using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine.Networking
{
	[StructLayout(LayoutKind.Sequential)]
	public sealed class DownloadHandlerTexture : DownloadHandler
	{
		private Texture2D mTexture;

		private bool mHasTexture;

		private bool mNonReadable;

		public Texture2D texture => InternalGetTexture();

		public DownloadHandlerTexture()
		{
			InternalCreateTexture(readable: true);
		}

		public DownloadHandlerTexture(bool readable)
		{
			InternalCreateTexture(readable);
			mNonReadable = !readable;
		}

		protected override byte[] GetData()
		{
			return InternalGetData();
		}

		private Texture2D InternalGetTexture()
		{
			if (mHasTexture)
			{
				if (mTexture == null)
				{
					mTexture = new Texture2D(2, 2);
					mTexture.LoadImage(GetData(), mNonReadable);
				}
			}
			else if (mTexture == null)
			{
				mTexture = InternalGetTextureNative();
				mHasTexture = true;
			}
			return mTexture;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern Texture2D InternalGetTextureNative();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern byte[] InternalGetData();

		public static Texture2D GetContent(UnityWebRequest www)
		{
			return DownloadHandler.GetCheckedDownloader<DownloadHandlerTexture>(www).texture;
		}
	}
}
