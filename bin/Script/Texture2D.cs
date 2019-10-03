using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class Texture2D : Texture
	{
		[Flags]
		public enum EXRFlags
		{
			None = 0x0,
			OutputAsFloat = 0x1,
			CompressZIP = 0x2,
			CompressRLE = 0x4,
			CompressPIZ = 0x8
		}

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

		public static Texture2D whiteTexture
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static Texture2D blackTexture
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public Texture2D(int width, int height)
		{
			Internal_Create(this, width, height, TextureFormat.RGBA32, mipmap: true, linear: false, IntPtr.Zero);
		}

		public Texture2D(int width, int height, TextureFormat format, bool mipmap)
		{
			Internal_Create(this, width, height, format, mipmap, linear: false, IntPtr.Zero);
		}

		public Texture2D(int width, int height, TextureFormat format, bool mipmap, bool linear)
		{
			Internal_Create(this, width, height, format, mipmap, linear, IntPtr.Zero);
		}

		internal Texture2D(int width, int height, TextureFormat format, bool mipmap, bool linear, IntPtr nativeTex)
		{
			Internal_Create(this, width, height, format, mipmap, linear, nativeTex);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void Internal_Create([Writable] Texture2D mono, int width, int height, TextureFormat format, bool mipmap, bool linear, IntPtr nativeTex);

		public static Texture2D CreateExternalTexture(int width, int height, TextureFormat format, bool mipmap, bool linear, IntPtr nativeTex)
		{
			return new Texture2D(width, height, format, mipmap, linear, nativeTex);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void UpdateExternalTexture(IntPtr nativeTex);

		public void SetPixel(int x, int y, Color color)
		{
			INTERNAL_CALL_SetPixel(this, x, y, ref color);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetPixel(Texture2D self, int x, int y, ref Color color);

		public Color GetPixel(int x, int y)
		{
			INTERNAL_CALL_GetPixel(this, x, y, out Color value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetPixel(Texture2D self, int x, int y, out Color value);

		public Color GetPixelBilinear(float u, float v)
		{
			INTERNAL_CALL_GetPixelBilinear(this, u, v, out Color value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetPixelBilinear(Texture2D self, float u, float v, out Color value);

		[ExcludeFromDocs]
		public void SetPixels(Color[] colors)
		{
			int miplevel = 0;
			SetPixels(colors, miplevel);
		}

		public void SetPixels(Color[] colors, [DefaultValue("0")] int miplevel)
		{
			int num = width >> miplevel;
			if (num < 1)
			{
				num = 1;
			}
			int num2 = height >> miplevel;
			if (num2 < 1)
			{
				num2 = 1;
			}
			SetPixels(0, 0, num, num2, colors, miplevel);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetPixels(int x, int y, int blockWidth, int blockHeight, Color[] colors, [DefaultValue("0")] int miplevel);

		[ExcludeFromDocs]
		public void SetPixels(int x, int y, int blockWidth, int blockHeight, Color[] colors)
		{
			int miplevel = 0;
			SetPixels(x, y, blockWidth, blockHeight, colors, miplevel);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetAllPixels32(Color32[] colors, int miplevel);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void SetBlockOfPixels32(int x, int y, int blockWidth, int blockHeight, Color32[] colors, int miplevel);

		[ExcludeFromDocs]
		public void SetPixels32(Color32[] colors)
		{
			int miplevel = 0;
			SetPixels32(colors, miplevel);
		}

		public void SetPixels32(Color32[] colors, [DefaultValue("0")] int miplevel)
		{
			SetAllPixels32(colors, miplevel);
		}

		[ExcludeFromDocs]
		public void SetPixels32(int x, int y, int blockWidth, int blockHeight, Color32[] colors)
		{
			int miplevel = 0;
			SetPixels32(x, y, blockWidth, blockHeight, colors, miplevel);
		}

		public void SetPixels32(int x, int y, int blockWidth, int blockHeight, Color32[] colors, [DefaultValue("0")] int miplevel)
		{
			SetBlockOfPixels32(x, y, blockWidth, blockHeight, colors, miplevel);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern bool LoadImage(byte[] data, [DefaultValue("false")] bool markNonReadable);

		[ExcludeFromDocs]
		public bool LoadImage(byte[] data)
		{
			bool markNonReadable = false;
			return LoadImage(data, markNonReadable);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void LoadRawTextureData_ImplArray(byte[] data);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void LoadRawTextureData_ImplPointer(IntPtr data, int size);

		public void LoadRawTextureData(byte[] data)
		{
			LoadRawTextureData_ImplArray(data);
		}

		public void LoadRawTextureData(IntPtr data, int size)
		{
			LoadRawTextureData_ImplPointer(data, size);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern byte[] GetRawTextureData();

		[ExcludeFromDocs]
		public Color[] GetPixels()
		{
			int miplevel = 0;
			return GetPixels(miplevel);
		}

		public Color[] GetPixels([DefaultValue("0")] int miplevel)
		{
			int num = width >> miplevel;
			if (num < 1)
			{
				num = 1;
			}
			int num2 = height >> miplevel;
			if (num2 < 1)
			{
				num2 = 1;
			}
			return GetPixels(0, 0, num, num2, miplevel);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern Color[] GetPixels(int x, int y, int blockWidth, int blockHeight, [DefaultValue("0")] int miplevel);

		[ExcludeFromDocs]
		public Color[] GetPixels(int x, int y, int blockWidth, int blockHeight)
		{
			int miplevel = 0;
			return GetPixels(x, y, blockWidth, blockHeight, miplevel);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern Color32[] GetPixels32([DefaultValue("0")] int miplevel);

		[ExcludeFromDocs]
		public Color32[] GetPixels32()
		{
			int miplevel = 0;
			return GetPixels32(miplevel);
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
		public extern bool Resize(int width, int height, TextureFormat format, bool hasMipMap);

		public bool Resize(int width, int height)
		{
			return Internal_ResizeWH(width, height);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern bool Internal_ResizeWH(int width, int height);

		public void Compress(bool highQuality)
		{
			INTERNAL_CALL_Compress(this, highQuality);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_Compress(Texture2D self, bool highQuality);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern Rect[] PackTextures(Texture2D[] textures, int padding, [DefaultValue("2048")] int maximumAtlasSize, [DefaultValue("false")] bool makeNoLongerReadable);

		[ExcludeFromDocs]
		public Rect[] PackTextures(Texture2D[] textures, int padding, int maximumAtlasSize)
		{
			bool makeNoLongerReadable = false;
			return PackTextures(textures, padding, maximumAtlasSize, makeNoLongerReadable);
		}

		[ExcludeFromDocs]
		public Rect[] PackTextures(Texture2D[] textures, int padding)
		{
			bool makeNoLongerReadable = false;
			int maximumAtlasSize = 2048;
			return PackTextures(textures, padding, maximumAtlasSize, makeNoLongerReadable);
		}

		public static bool GenerateAtlas(Vector2[] sizes, int padding, int atlasSize, List<Rect> results)
		{
			if (sizes == null)
			{
				throw new ArgumentException("sizes array can not be null");
			}
			if (results == null)
			{
				throw new ArgumentException("results list cannot be null");
			}
			if (padding < 0)
			{
				throw new ArgumentException("padding can not be negative");
			}
			if (atlasSize <= 0)
			{
				throw new ArgumentException("atlas size must be positive");
			}
			results.Clear();
			if (sizes.Length == 0)
			{
				return true;
			}
			GenerateAtlasInternal(sizes, padding, atlasSize, results);
			return results.Count != 0;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void GenerateAtlasInternal(Vector2[] sizes, int padding, int atlasSize, object resultList);

		public void ReadPixels(Rect source, int destX, int destY, [DefaultValue("true")] bool recalculateMipMaps)
		{
			INTERNAL_CALL_ReadPixels(this, ref source, destX, destY, recalculateMipMaps);
		}

		[ExcludeFromDocs]
		public void ReadPixels(Rect source, int destX, int destY)
		{
			bool recalculateMipMaps = true;
			INTERNAL_CALL_ReadPixels(this, ref source, destX, destY, recalculateMipMaps);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_ReadPixels(Texture2D self, ref Rect source, int destX, int destY, bool recalculateMipMaps);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern byte[] EncodeToPNG();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern byte[] EncodeToJPG(int quality);

		public byte[] EncodeToJPG()
		{
			return EncodeToJPG(75);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern byte[] EncodeToEXR([DefaultValue("EXRFlags.None")] EXRFlags flags);

		[ExcludeFromDocs]
		public byte[] EncodeToEXR()
		{
			EXRFlags flags = EXRFlags.None;
			return EncodeToEXR(flags);
		}
	}
}
