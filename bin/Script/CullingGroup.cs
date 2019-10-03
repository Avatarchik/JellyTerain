using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[StructLayout(LayoutKind.Sequential)]
	public sealed class CullingGroup : IDisposable
	{
		public delegate void StateChanged(CullingGroupEvent sphere);

		internal IntPtr m_Ptr;

		private StateChanged m_OnStateChanged = null;

		public StateChanged onStateChanged
		{
			get
			{
				return m_OnStateChanged;
			}
			set
			{
				m_OnStateChanged = value;
			}
		}

		public bool enabled
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public Camera targetCamera
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			set;
		}

		public CullingGroup()
		{
			Init();
		}

		~CullingGroup()
		{
			if (m_Ptr != IntPtr.Zero)
			{
				FinalizerFailure();
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void Dispose();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetBoundingSpheres(BoundingSphere[] array);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetBoundingSphereCount(int count);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void EraseSwapBack(int index);

		public static void EraseSwapBack<T>(int index, T[] myArray, ref int size)
		{
			size--;
			myArray[index] = myArray[size];
		}

		public int QueryIndices(bool visible, int[] result, int firstIndex)
		{
			return QueryIndices(visible, -1, CullingQueryOptions.IgnoreDistance, result, firstIndex);
		}

		public int QueryIndices(int distanceIndex, int[] result, int firstIndex)
		{
			return QueryIndices(visible: false, distanceIndex, CullingQueryOptions.IgnoreVisibility, result, firstIndex);
		}

		public int QueryIndices(bool visible, int distanceIndex, int[] result, int firstIndex)
		{
			return QueryIndices(visible, distanceIndex, CullingQueryOptions.Normal, result, firstIndex);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern int QueryIndices(bool visible, int distanceIndex, CullingQueryOptions options, int[] result, int firstIndex);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern bool IsVisible(int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern int GetDistance(int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetBoundingDistances(float[] distances);

		public void SetDistanceReferencePoint(Vector3 point)
		{
			INTERNAL_CALL_SetDistanceReferencePoint(this, ref point);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_SetDistanceReferencePoint(CullingGroup self, ref Vector3 point);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SetDistanceReferencePoint(Transform transform);

		[SecuritySafeCritical]
		[RequiredByNativeCode]
		private unsafe static void SendEvents(CullingGroup cullingGroup, IntPtr eventsPtr, int count)
		{
			CullingGroupEvent* ptr = (CullingGroupEvent*)eventsPtr.ToPointer();
			if (cullingGroup.m_OnStateChanged != null)
			{
				for (int i = 0; i < count; i++)
				{
					cullingGroup.m_OnStateChanged(ptr[i]);
				}
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void Init();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		private extern void FinalizerFailure();
	}
}
