using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
	[AddComponentMenu("Event/Graphic Raycaster")]
	[RequireComponent(typeof(Canvas))]
	public class GraphicRaycaster : BaseRaycaster
	{
		public enum BlockingObjects
		{
			None,
			TwoD,
			ThreeD,
			All
		}

		protected const int kNoEventMaskSet = -1;

		[FormerlySerializedAs("ignoreReversedGraphics")]
		[SerializeField]
		private bool m_IgnoreReversedGraphics = true;

		[FormerlySerializedAs("blockingObjects")]
		[SerializeField]
		private BlockingObjects m_BlockingObjects = BlockingObjects.None;

		[SerializeField]
		protected LayerMask m_BlockingMask = -1;

		private Canvas m_Canvas;

		[NonSerialized]
		private List<Graphic> m_RaycastResults = new List<Graphic>();

		[NonSerialized]
		private static readonly List<Graphic> s_SortedGraphics = new List<Graphic>();

		public override int sortOrderPriority
		{
			get
			{
				if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
				{
					return canvas.sortingOrder;
				}
				return base.sortOrderPriority;
			}
		}

		public override int renderOrderPriority
		{
			get
			{
				if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
				{
					return canvas.rootCanvas.renderOrder;
				}
				return base.renderOrderPriority;
			}
		}

		public bool ignoreReversedGraphics
		{
			get
			{
				return m_IgnoreReversedGraphics;
			}
			set
			{
				m_IgnoreReversedGraphics = value;
			}
		}

		public BlockingObjects blockingObjects
		{
			get
			{
				return m_BlockingObjects;
			}
			set
			{
				m_BlockingObjects = value;
			}
		}

		private Canvas canvas
		{
			get
			{
				if (m_Canvas != null)
				{
					return m_Canvas;
				}
				m_Canvas = GetComponent<Canvas>();
				return m_Canvas;
			}
		}

		public override Camera eventCamera
		{
			get
			{
				if (canvas.renderMode == RenderMode.ScreenSpaceOverlay || (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera == null))
				{
					return null;
				}
				return (!(canvas.worldCamera != null)) ? Camera.main : canvas.worldCamera;
			}
		}

		protected GraphicRaycaster()
		{
		}

		public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
		{
			if (canvas == null)
			{
				return;
			}
			int num = (canvas.renderMode != 0 && (bool)eventCamera) ? eventCamera.targetDisplay : canvas.targetDisplay;
			Vector3 vector = Display.RelativeMouseAt(eventData.position);
			if (vector != Vector3.zero)
			{
				int num2 = (int)vector.z;
				if (num2 != num)
				{
					return;
				}
			}
			else
			{
				vector = eventData.position;
			}
			Vector2 vector2;
			if (eventCamera == null)
			{
				float num3 = Screen.width;
				float num4 = Screen.height;
				if (num > 0 && num < Display.displays.Length)
				{
					num3 = Display.displays[num].systemWidth;
					num4 = Display.displays[num].systemHeight;
				}
				vector2 = new Vector2(vector.x / num3, vector.y / num4);
			}
			else
			{
				vector2 = eventCamera.ScreenToViewportPoint(vector);
			}
			if (vector2.x < 0f || vector2.x > 1f || vector2.y < 0f || vector2.y > 1f)
			{
				return;
			}
			float num5 = float.MaxValue;
			Ray r = default(Ray);
			if (eventCamera != null)
			{
				r = eventCamera.ScreenPointToRay(vector);
			}
			if (canvas.renderMode != 0 && blockingObjects != 0)
			{
				float num6 = 100f;
				if (eventCamera != null)
				{
					num6 = eventCamera.farClipPlane - eventCamera.nearClipPlane;
				}
				if ((blockingObjects == BlockingObjects.ThreeD || blockingObjects == BlockingObjects.All) && ReflectionMethodsCache.Singleton.raycast3D != null && ReflectionMethodsCache.Singleton.raycast3D(r, out RaycastHit hit, num6, m_BlockingMask))
				{
					num5 = hit.distance;
				}
				if ((blockingObjects == BlockingObjects.TwoD || blockingObjects == BlockingObjects.All) && ReflectionMethodsCache.Singleton.raycast2D != null)
				{
					RaycastHit2D raycastHit2D = ReflectionMethodsCache.Singleton.raycast2D(r.origin, r.direction, num6, m_BlockingMask);
					if ((bool)raycastHit2D.collider)
					{
						num5 = raycastHit2D.fraction * num6;
					}
				}
			}
			m_RaycastResults.Clear();
			Raycast(canvas, eventCamera, vector, m_RaycastResults);
			for (int i = 0; i < m_RaycastResults.Count; i++)
			{
				GameObject gameObject = m_RaycastResults[i].gameObject;
				bool flag = true;
				if (ignoreReversedGraphics)
				{
					if (eventCamera == null)
					{
						Vector3 rhs = gameObject.transform.rotation * Vector3.forward;
						flag = (Vector3.Dot(Vector3.forward, rhs) > 0f);
					}
					else
					{
						Vector3 lhs = eventCamera.transform.rotation * Vector3.forward;
						Vector3 rhs2 = gameObject.transform.rotation * Vector3.forward;
						flag = (Vector3.Dot(lhs, rhs2) > 0f);
					}
				}
				if (!flag)
				{
					continue;
				}
				float num7 = 0f;
				if (eventCamera == null || canvas.renderMode == RenderMode.ScreenSpaceOverlay)
				{
					num7 = 0f;
				}
				else
				{
					Transform transform = gameObject.transform;
					Vector3 forward = transform.forward;
					num7 = Vector3.Dot(forward, transform.position - r.origin) / Vector3.Dot(forward, r.direction);
					if (num7 < 0f)
					{
						continue;
					}
				}
				if (!(num7 >= num5))
				{
					RaycastResult raycastResult = default(RaycastResult);
					raycastResult.gameObject = gameObject;
					raycastResult.module = this;
					raycastResult.distance = num7;
					raycastResult.screenPosition = vector;
					raycastResult.index = resultAppendList.Count;
					raycastResult.depth = m_RaycastResults[i].depth;
					raycastResult.sortingLayer = canvas.sortingLayerID;
					raycastResult.sortingOrder = canvas.sortingOrder;
					RaycastResult item = raycastResult;
					resultAppendList.Add(item);
				}
			}
		}

		private static void Raycast(Canvas canvas, Camera eventCamera, Vector2 pointerPosition, List<Graphic> results)
		{
			IList<Graphic> graphicsForCanvas = GraphicRegistry.GetGraphicsForCanvas(canvas);
			for (int i = 0; i < graphicsForCanvas.Count; i++)
			{
				Graphic graphic = graphicsForCanvas[i];
				if (!graphic.canvasRenderer.cull && graphic.depth != -1 && graphic.raycastTarget && RectTransformUtility.RectangleContainsScreenPoint(graphic.rectTransform, pointerPosition, eventCamera) && graphic.Raycast(pointerPosition, eventCamera))
				{
					s_SortedGraphics.Add(graphic);
				}
			}
			s_SortedGraphics.Sort((Graphic g1, Graphic g2) => g2.depth.CompareTo(g1.depth));
			for (int j = 0; j < s_SortedGraphics.Count; j++)
			{
				results.Add(s_SortedGraphics[j]);
			}
			s_SortedGraphics.Clear();
		}
	}
}
