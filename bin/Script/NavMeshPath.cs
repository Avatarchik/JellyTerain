using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.AI
{
	[StructLayout(LayoutKind.Sequential)]
	[MovedFrom("UnityEngine")]
	public sealed class NavMeshPath
	{
		internal IntPtr m_Ptr;

		internal Vector3[] m_corners;

		public Vector3[] corners
		{
			get
			{
				CalculateCorners();
				return m_corners;
			}
		}

		public NavMeshPathStatus status
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern NavMeshPath();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		private extern void DestroyNavMeshPath();

		~NavMeshPath()
		{
			DestroyNavMeshPath();
			m_Ptr = IntPtr.Zero;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern int GetCornersNonAlloc(Vector3[] results);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern Vector3[] CalculateCornersInternal();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void ClearCornersInternal();

		public void ClearCorners()
		{
			ClearCornersInternal();
			m_corners = null;
		}

		private void CalculateCorners()
		{
			if (m_corners == null)
			{
				m_corners = CalculateCornersInternal();
			}
		}
	}
}
