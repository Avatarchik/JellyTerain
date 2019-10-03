using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class CubemapArray : Texture
	{
		public int cubemapCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public TextureFormat format
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public CubemapArray(int faceSize, int cubemapCount, TextureFormat format, bool mipmap)
		{
			Internal_Create(this, faceSize, cubemapCount, format, mipmap, linear: false);
		}

		public CubemapArray(int faceSize, int cubemapCount, TextureFormat format, bool mipmap, bool linear)
		{
			Internal_Create(this, faceSize, cubemapCount, format, mipmap, linear);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void Apply([DefaultValue("true")] bool updateMipmaps, [DefaultValue("false")] bool makeNoLongerReadable);

		[ExcludeFromDocs]
		public void Apply(bool updateMipmaps)
		{
			bool makeNoLongerReadable = false;
			Apply(updateMipmaps, makeNoLongerReadable);
		}

		[ExcludeFromDocs]
		public void Apply()
		{
			bool makeNoLongerReadable = false;
			bool updateMipmaps = true;
			Apply(updateMipmaps, makeNoLongerReadable);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void Internal_Create([Writable] CubemapArray mono, int faceSize, int cubemapCount, TextureFormat format, bool mipmap, bool linear);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetPixels(Color[] colors, CubemapFace face, int arrayElement, [DefaultValue("0")] int miplevel);

		[ExcludeFromDocs]
		public void SetPixels(Color[] colors, CubemapFace face, int arrayElement)
		{
			int miplevel = 0;
			SetPixels(colors, face, arrayElement, miplevel);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetPixels32(Color32[] colors, CubemapFace face, int arrayElement, [DefaultValue("0")] int miplevel);

		[ExcludeFromDocs]
		public void SetPixels32(Color32[] colors, CubemapFace face, int arrayElement)
		{
			int miplevel = 0;
			SetPixels32(colors, face, arrayElement, miplevel);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern Color[] GetPixels(CubemapFace face, int arrayElement, [DefaultValue("0")] int miplevel);

		[ExcludeFromDocs]
		public Color[] GetPixels(CubemapFace face, int arrayElement)
		{
			int miplevel = 0;
			return GetPixels(face, arrayElement, miplevel);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern Color32[] GetPixels32(CubemapFace face, int arrayElement, [DefaultValue("0")] int miplevel);

		[ExcludeFromDocs]
		public Color32[] GetPixels32(CubemapFace face, int arrayElement)
		{
			int miplevel = 0;
			return GetPixels32(face, arrayElement, miplevel);
		}
	}
}
