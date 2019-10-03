using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class Caching
	{
		[Obsolete("this API is not for public use.")]
		public static CacheIndex[] index
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static long spaceFree
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static long maximumAvailableDiskSpace
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static long spaceOccupied
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		[Obsolete("Please use Caching.spaceFree instead")]
		public static int spaceAvailable
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		[Obsolete("Please use Caching.spaceOccupied instead")]
		public static int spaceUsed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static int expirationDelay
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static bool enabled
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static bool compressionEnabled
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public static bool ready
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public static bool Authorize(string name, string domain, long size, string signature)
		{
			return Authorize(name, domain, size, -1, signature);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern bool Authorize(string name, string domain, long size, int expiration, string signature);

		[Obsolete("Size is now specified as a long")]
		public static bool Authorize(string name, string domain, int size, int expiration, string signature)
		{
			return Authorize(name, domain, (long)size, expiration, signature);
		}

		[Obsolete("Size is now specified as a long")]
		public static bool Authorize(string name, string domain, int size, string signature)
		{
			return Authorize(name, domain, (long)size, signature);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public static extern bool CleanCache();

		public static bool CleanCache(int expiration)
		{
			return CleanCacheInternal(expiration);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal static extern bool CleanCacheInternal(int expiration);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[Obsolete("this API is not for public use.")]
		[GeneratedByOldBindingsGenerator]
		public static extern bool CleanNamedCache(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[Obsolete("This function is obsolete and has no effect.")]
		[GeneratedByOldBindingsGenerator]
		public static extern bool DeleteFromCache(string url);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[Obsolete("This function is obsolete and will always return -1. Use IsVersionCached instead.")]
		[GeneratedByOldBindingsGenerator]
		public static extern int GetVersionFromCache(string url);

		public static bool IsVersionCached(string url, int version)
		{
			Hash128 hash = new Hash128(0u, 0u, 0u, (uint)version);
			return IsVersionCached(url, hash);
		}

		public static bool IsVersionCached(string url, Hash128 hash)
		{
			return INTERNAL_CALL_IsVersionCached(url, ref hash);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_IsVersionCached(string url, ref Hash128 hash);

		public static bool MarkAsUsed(string url, int version)
		{
			Hash128 hash = new Hash128(0u, 0u, 0u, (uint)version);
			return MarkAsUsed(url, hash);
		}

		public static bool MarkAsUsed(string url, Hash128 hash)
		{
			return INTERNAL_CALL_MarkAsUsed(url, ref hash);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_MarkAsUsed(string url, ref Hash128 hash);
	}
}
