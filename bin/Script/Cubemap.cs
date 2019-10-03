using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class Cubemap : Texture
	{
		public int mipmapCount
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

		public Cubemap(int size, TextureFormat format, bool mipmap)
		{
			Internal_Create(this, size, format, mipmap);
		}

		public void SetPixel(CubemapFace face, int x, int y, Color color)
		{
			INTERNAL_CALL_SetPixel(this, face, x, y, ref color);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetPixel(Cubemap self, CubemapFace face, int x, int y, ref Color color);

		public Color GetPixel(CubemapFace face, int x, int y)
		{
			INTERNAL_CALL_GetPixel(this, face, x, y, out Color value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetPixel(Cubemap self, CubemapFace face, int x, int y, out Color value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern Color[] GetPixels(CubemapFace face, [DefaultValue("0")] int miplevel);

		[ExcludeFromDocs]
		public Color[] GetPixels(CubemapFace face)
		{
			int miplevel = 0;
			return GetPixels(face, miplevel);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetPixels(Color[] colors, CubemapFace face, [DefaultValue("0")] int miplevel);

		[ExcludeFromDocs]
		public void SetPixels(Color[] colors, CubemapFace face)
		{
			int miplevel = 0;
			SetPixels(colors, face, miplevel);
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
		private static extern void Internal_Create([Writable] Cubemap mono, int size, TextureFormat format, bool mipmap);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SmoothEdges([DefaultValue("1")] int smoothRegionWidthInPixels);

		[ExcludeFromDocs]
		public void SmoothEdges()
		{
			int smoothRegionWidthInPixels = 1;
			SmoothEdges(smoothRegionWidthInPixels);
		}
	}
}
