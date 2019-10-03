using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class Texture2DArray : Texture
	{
		public int depth
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

		public Texture2DArray(int width, int height, int depth, TextureFormat format, bool mipmap)
		{
			Internal_Create(this, width, height, depth, format, mipmap, linear: false);
		}

		public Texture2DArray(int width, int height, int depth, TextureFormat format, bool mipmap, bool linear)
		{
			Internal_Create(this, width, height, depth, format, mipmap, linear);
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
		private static extern void Internal_Create([Writable] Texture2DArray mono, int width, int height, int depth, TextureFormat format, bool mipmap, bool linear);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetPixels(Color[] colors, int arrayElement, [DefaultValue("0")] int miplevel);

		[ExcludeFromDocs]
		public void SetPixels(Color[] colors, int arrayElement)
		{
			int miplevel = 0;
			SetPixels(colors, arrayElement, miplevel);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetPixels32(Color32[] colors, int arrayElement, [DefaultValue("0")] int miplevel);

		[ExcludeFromDocs]
		public void SetPixels32(Color32[] colors, int arrayElement)
		{
			int miplevel = 0;
			SetPixels32(colors, arrayElement, miplevel);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern Color[] GetPixels(int arrayElement, [DefaultValue("0")] int miplevel);

		[ExcludeFromDocs]
		public Color[] GetPixels(int arrayElement)
		{
			int miplevel = 0;
			return GetPixels(arrayElement, miplevel);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern Color32[] GetPixels32(int arrayElement, [DefaultValue("0")] int miplevel);

		[ExcludeFromDocs]
		public Color32[] GetPixels32(int arrayElement)
		{
			int miplevel = 0;
			return GetPixels32(arrayElement, miplevel);
		}
	}
}
