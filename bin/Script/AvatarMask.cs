using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine
{
	[MovedFrom("UnityEditor.Animations", true)]
	public sealed class AvatarMask : Object
	{
		[Obsolete("AvatarMask.humanoidBodyPartCount is deprecated. Use AvatarMaskBodyPart.LastBodyPart instead.")]
		private int humanoidBodyPartCount => 13;

		public int transformCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		internal bool hasFeetIK
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public AvatarMask()
		{
			Internal_Create(this);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void Internal_Create([Writable] AvatarMask mono);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern bool GetHumanoidBodyPartActive(AvatarMaskBodyPart index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetHumanoidBodyPartActive(AvatarMaskBodyPart index, bool value);

		[ExcludeFromDocs]
		public void AddTransformPath(Transform transform)
		{
			bool recursive = true;
			AddTransformPath(transform, recursive);
		}

		public void AddTransformPath(Transform transform, [DefaultValue("true")] bool recursive)
		{
			if (transform == null)
			{
				throw new ArgumentNullException("transform");
			}
			Internal_AddTransformPath(transform, recursive);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void Internal_AddTransformPath(Transform transform, bool recursive);

		[ExcludeFromDocs]
		public void RemoveTransformPath(Transform transform)
		{
			bool recursive = true;
			RemoveTransformPath(transform, recursive);
		}

		public void RemoveTransformPath(Transform transform, [DefaultValue("true")] bool recursive)
		{
			if (transform == null)
			{
				throw new ArgumentNullException("transform");
			}
			Internal_RemoveTransformPath(transform, recursive);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void Internal_RemoveTransformPath(Transform transform, bool recursive);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern string GetTransformPath(int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetTransformPath(int index, string path);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern bool GetTransformActive(int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetTransformActive(int index, bool value);

		internal void Copy(AvatarMask other)
		{
			for (AvatarMaskBodyPart avatarMaskBodyPart = AvatarMaskBodyPart.Root; avatarMaskBodyPart < AvatarMaskBodyPart.LastBodyPart; avatarMaskBodyPart++)
			{
				SetHumanoidBodyPartActive(avatarMaskBodyPart, other.GetHumanoidBodyPartActive(avatarMaskBodyPart));
			}
			transformCount = other.transformCount;
			for (int i = 0; i < other.transformCount; i++)
			{
				SetTransformPath(i, other.GetTransformPath(i));
				SetTransformActive(i, other.GetTransformActive(i));
			}
		}
	}
}
