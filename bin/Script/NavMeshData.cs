using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.AI
{
	public sealed class NavMeshData : Object
	{
		public Bounds sourceBounds
		{
			get
			{
				INTERNAL_get_sourceBounds(out Bounds value);
				return value;
			}
		}

		public Vector3 position
		{
			get
			{
				INTERNAL_get_position(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_position(ref value);
			}
		}

		public Quaternion rotation
		{
			get
			{
				INTERNAL_get_rotation(out Quaternion value);
				return value;
			}
			set
			{
				INTERNAL_set_rotation(ref value);
			}
		}

		public NavMeshData()
		{
			Internal_Create(this, 0);
		}

		public NavMeshData(int agentTypeID)
		{
			Internal_Create(this, agentTypeID);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void Internal_Create([Writable] NavMeshData mono, int agentTypeID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_sourceBounds(out Bounds value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_position(out Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_position(ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_rotation(out Quaternion value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_set_rotation(ref Quaternion value);
	}
}
