using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace UnityEngine.EventSystems
{
	[AddComponentMenu("Event/Physics Raycaster")]
	[RequireComponent(typeof(Camera))]
	public class PhysicsRaycaster : BaseRaycaster
	{
		protected const int kNoEventMaskSet = -1;

		protected Camera m_EventCamera;

		[SerializeField]
		protected LayerMask m_EventMask = -1;

		public override Camera eventCamera
		{
			get
			{
				if (m_EventCamera == null)
				{
					m_EventCamera = GetComponent<Camera>();
				}
				return m_EventCamera ?? Camera.main;
			}
		}

		public virtual int depth => (!(eventCamera != null)) ? 16777215 : ((int)eventCamera.depth);

		public int finalEventMask => (!(eventCamera != null)) ? (-1) : (eventCamera.cullingMask & (int)m_EventMask);

		public LayerMask eventMask
		{
			get
			{
				return m_EventMask;
			}
			set
			{
				m_EventMask = value;
			}
		}

		protected PhysicsRaycaster()
		{
		}

		protected void ComputeRayAndDistance(PointerEventData eventData, out Ray ray, out float distanceToClipPlane)
		{
			ray = eventCamera.ScreenPointToRay(eventData.position);
			Vector3 direction = ray.direction;
			float z = direction.z;
			distanceToClipPlane = ((!Mathf.Approximately(0f, z)) ? Mathf.Abs((eventCamera.farClipPlane - eventCamera.nearClipPlane) / z) : float.PositiveInfinity);
		}

		public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
		{
			if (eventCamera == null)
			{
				return;
			}
			ComputeRayAndDistance(eventData, out Ray ray, out float distanceToClipPlane);
			if (ReflectionMethodsCache.Singleton.raycast3DAll == null)
			{
				return;
			}
			RaycastHit[] array = ReflectionMethodsCache.Singleton.raycast3DAll(ray, distanceToClipPlane, finalEventMask);
			if (array.Length > 1)
			{
				Array.Sort(array, (RaycastHit r1, RaycastHit r2) => r1.distance.CompareTo(r2.distance));
			}
			if (array.Length != 0)
			{
				int i = 0;
				for (int num = array.Length; i < num; i++)
				{
					RaycastResult raycastResult = default(RaycastResult);
					raycastResult.gameObject = array[i].collider.gameObject;
					raycastResult.module = this;
					raycastResult.distance = array[i].distance;
					raycastResult.worldPosition = array[i].point;
					raycastResult.worldNormal = array[i].normal;
					raycastResult.screenPosition = eventData.position;
					raycastResult.index = resultAppendList.Count;
					raycastResult.sortingLayer = 0;
					raycastResult.sortingOrder = 0;
					RaycastResult item = raycastResult;
					resultAppendList.Add(item);
				}
			}
		}
	}
}
