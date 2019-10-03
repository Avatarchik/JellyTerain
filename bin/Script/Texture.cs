using System;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[UsedByNativeCode]
	public class Texture : Object
	{
		public static int masterTextureLimit
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static AnisotropicFiltering anisotropicFiltering
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public virtual int width
		{
			get
			{
				return Internal_GetWidth(this);
			}
			set
			{
				throw new Exception("not implemented");
			}
		}

		public virtual int height
		{
			get
			{
				return Internal_GetHeight(this);
			}
			set
			{
				throw new Exception("not implemented");
			}
		}

		public virtual TextureDimension dimension
		{
			get
			{
				return Internal_GetDimension(this);
			}
			set
			{
				throw new Exception("not implemented");
			}
		}

		public FilterMode filterMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public int anisoLevel
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public TextureWrapMode wrapMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public float mipMapBias
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public Vector2 texelSize
		{
			get
			{
				INTERNAL_get_texelSize(out Vector2 value);
				return value;
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern void SetGlobalAnisotropicFilteringLimits(int forcedMin, int globalMax);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int Internal_GetWidth(Texture t);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern int Internal_GetHeight(Texture t);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern TextureDimension Internal_GetDimension(Texture t);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_texelSize(out Vector2 value);

		public IntPtr GetNativeTexturePtr()
		{
			INTERNAL_CALL_GetNativeTexturePtr(this, out IntPtr value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_GetNativeTexturePtr(Texture self, out IntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[Obsolete("Use GetNativeTexturePtr instead.")]
		[GeneratedByOldBindingsGenerator]
		public extern int GetNativeTextureID();
	}
}
