using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public class Motion : Object
	{
		public float averageDuration
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public float averageAngularSpeed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public Vector3 averageSpeed
		{
			get
			{
				INTERNAL_get_averageSpeed(out Vector3 value);
				return value;
			}
		}

		public float apparentSpeed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public bool isLooping
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public bool legacy
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public bool isHumanMotion
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		[Obsolete("isAnimatorMotion is not supported anymore. Use !legacy instead.", true)]
		public bool isAnimatorMotion
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void INTERNAL_get_averageSpeed(out Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[Obsolete("ValidateIfRetargetable is not supported anymore. Use isHumanMotion instead.", true)]
		[GeneratedByOldBindingsGenerator]
		public extern bool ValidateIfRetargetable(bool val);
	}
}
