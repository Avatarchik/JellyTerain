using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class HumanPoseHandler : IDisposable
	{
		internal IntPtr m_Ptr;

		public HumanPoseHandler(Avatar avatar, Transform root)
		{
			m_Ptr = IntPtr.Zero;
			if (root == null)
			{
				throw new ArgumentNullException("HumanPoseHandler root Transform is null");
			}
			if (avatar == null)
			{
				throw new ArgumentNullException("HumanPoseHandler avatar is null");
			}
			if (!avatar.isValid)
			{
				throw new ArgumentException("HumanPoseHandler avatar is invalid");
			}
			if (!avatar.isHuman)
			{
				throw new ArgumentException("HumanPoseHandler avatar is not human");
			}
			Internal_HumanPoseHandler(avatar, root);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void Dispose();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void Internal_HumanPoseHandler(Avatar avatar, Transform root);

		private bool Internal_GetHumanPose(ref Vector3 bodyPosition, ref Quaternion bodyRotation, float[] muscles)
		{
			return INTERNAL_CALL_Internal_GetHumanPose(this, ref bodyPosition, ref bodyRotation, muscles);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_Internal_GetHumanPose(HumanPoseHandler self, ref Vector3 bodyPosition, ref Quaternion bodyRotation, float[] muscles);

		public void GetHumanPose(ref HumanPose humanPose)
		{
			humanPose.Init();
			if (!Internal_GetHumanPose(ref humanPose.bodyPosition, ref humanPose.bodyRotation, humanPose.muscles))
			{
				Debug.LogWarning("HumanPoseHandler is not initialized properly");
			}
		}

		private bool Internal_SetHumanPose(ref Vector3 bodyPosition, ref Quaternion bodyRotation, float[] muscles)
		{
			return INTERNAL_CALL_Internal_SetHumanPose(this, ref bodyPosition, ref bodyRotation, muscles);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_Internal_SetHumanPose(HumanPoseHandler self, ref Vector3 bodyPosition, ref Quaternion bodyRotation, float[] muscles);

		public void SetHumanPose(ref HumanPose humanPose)
		{
			humanPose.Init();
			if (!Internal_SetHumanPose(ref humanPose.bodyPosition, ref humanPose.bodyRotation, humanPose.muscles))
			{
				Debug.LogWarning("HumanPoseHandler is not initialized properly");
			}
		}
	}
}
