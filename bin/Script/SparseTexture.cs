using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class SparseTexture : Texture
	{
		public int tileWidth
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public int tileHeight
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public bool isCreated
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public SparseTexture(int width, int height, TextureFormat format, int mipCount)
		{
			Internal_Create(this, width, height, format, mipCount, linear: false);
		}

		public SparseTexture(int width, int height, TextureFormat format, int mipCount, bool linear)
		{
			Internal_Create(this, width, height, format, mipCount, linear);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void Internal_Create([Writable] SparseTexture mono, int width, int height, TextureFormat format, int mipCount, bool linear);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void UpdateTile(int tileX, int tileY, int miplevel, Color32[] data);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void UpdateTileRaw(int tileX, int tileY, int miplevel, byte[] data);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void UnloadTile(int tileX, int tileY, int miplevel);
	}
}
