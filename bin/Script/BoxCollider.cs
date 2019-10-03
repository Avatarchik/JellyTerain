using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class BoxCollider : Collider
	{
		public Vector3 center
		{
			get
			{
				INTERNAL_get_center(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_center(ref value);
			}
		}

		public Vector3 size
		{
			get
			{
				INTERNAL_get_size(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_size(ref value);
			}
		}

		[Obsolete("use BoxCollider.size instead.")]
		public Vector3 extents
		{
			get
			{
				return size * 0.5f;
			}
			set
			{
				size = value * 2f;
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_center(out Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_center(ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_size(out Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_size(ref Vector3 value);
	}
}
