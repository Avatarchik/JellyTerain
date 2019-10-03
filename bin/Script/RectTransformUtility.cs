using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class RectTransformUtility
	{
		private static Vector3[] s_Corners = new Vector3[4];

		private RectTransformUtility()
		{
		}

		public static bool RectangleContainsScreenPoint(RectTransform rect, Vector2 screenPoint)
		{
			return RectangleContainsScreenPoint(rect, screenPoint, null);
		}

		public static bool ScreenPointToWorldPointInRectangle(RectTransform rect, Vector2 screenPoint, Camera cam, out Vector3 worldPoint)
		{
			worldPoint = Vector2.zero;
			Ray ray = ScreenPointToRay(cam, screenPoint);
			if (!new Plane(rect.rotation * Vector3.back, rect.position).Raycast(ray, out float enter))
			{
				return false;
			}
			worldPoint = ray.GetPoint(enter);
			return true;
		}

		public static bool ScreenPointToLocalPointInRectangle(RectTransform rect, Vector2 screenPoint, Camera cam, out Vector2 localPoint)
		{
			localPoint = Vector2.zero;
			if (ScreenPointToWorldPointInRectangle(rect, screenPoint, cam, out Vector3 worldPoint))
			{
				localPoint = rect.InverseTransformPoint(worldPoint);
				return true;
			}
			return false;
		}

		public static Ray ScreenPointToRay(Camera cam, Vector2 screenPos)
		{
			if (cam != null)
			{
				return cam.ScreenPointToRay(screenPos);
			}
			Vector3 origin = screenPos;
			origin.z -= 100f;
			return new Ray(origin, Vector3.forward);
		}

		public static Vector2 WorldToScreenPoint(Camera cam, Vector3 worldPoint)
		{
			if (cam == null)
			{
				return new Vector2(worldPoint.x, worldPoint.y);
			}
			return cam.WorldToScreenPoint(worldPoint);
		}

		public static Bounds CalculateRelativeRectTransformBounds(Transform root, Transform child)
		{
			RectTransform[] componentsInChildren = child.GetComponentsInChildren<RectTransform>(includeInactive: false);
			if (componentsInChildren.Length > 0)
			{
				Vector3 vector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
				Vector3 vector2 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
				Matrix4x4 worldToLocalMatrix = root.worldToLocalMatrix;
				int i = 0;
				for (int num = componentsInChildren.Length; i < num; i++)
				{
					componentsInChildren[i].GetWorldCorners(s_Corners);
					for (int j = 0; j < 4; j++)
					{
						Vector3 lhs = worldToLocalMatrix.MultiplyPoint3x4(s_Corners[j]);
						vector = Vector3.Min(lhs, vector);
						vector2 = Vector3.Max(lhs, vector2);
					}
				}
				Bounds result = new Bounds(vector, Vector3.zero);
				result.Encapsulate(vector2);
				return result;
			}
			return new Bounds(Vector3.zero, Vector3.zero);
		}

		public static Bounds CalculateRelativeRectTransformBounds(Transform trans)
		{
			return CalculateRelativeRectTransformBounds(trans, trans);
		}

		public static void FlipLayoutOnAxis(RectTransform rect, int axis, bool keepPositioning, bool recursive)
		{
			if (rect == null)
			{
				return;
			}
			if (recursive)
			{
				for (int i = 0; i < rect.childCount; i++)
				{
					RectTransform rectTransform = rect.GetChild(i) as RectTransform;
					if (rectTransform != null)
					{
						FlipLayoutOnAxis(rectTransform, axis, keepPositioning: false, recursive: true);
					}
				}
			}
			Vector2 pivot = rect.pivot;
			pivot[axis] = 1f - pivot[axis];
			rect.pivot = pivot;
			if (!keepPositioning)
			{
				Vector2 anchoredPosition = rect.anchoredPosition;
				anchoredPosition[axis] = 0f - anchoredPosition[axis];
				rect.anchoredPosition = anchoredPosition;
				Vector2 anchorMin = rect.anchorMin;
				Vector2 anchorMax = rect.anchorMax;
				float num = anchorMin[axis];
				anchorMin[axis] = 1f - anchorMax[axis];
				anchorMax[axis] = 1f - num;
				rect.anchorMin = anchorMin;
				rect.anchorMax = anchorMax;
			}
		}

		public static void FlipLayoutAxes(RectTransform rect, bool keepPositioning, bool recursive)
		{
			if (rect == null)
			{
				return;
			}
			if (recursive)
			{
				for (int i = 0; i < rect.childCount; i++)
				{
					RectTransform rectTransform = rect.GetChild(i) as RectTransform;
					if (rectTransform != null)
					{
						FlipLayoutAxes(rectTransform, keepPositioning: false, recursive: true);
					}
				}
			}
			rect.pivot = GetTransposed(rect.pivot);
			rect.sizeDelta = GetTransposed(rect.sizeDelta);
			if (!keepPositioning)
			{
				rect.anchoredPosition = GetTransposed(rect.anchoredPosition);
				rect.anchorMin = GetTransposed(rect.anchorMin);
				rect.anchorMax = GetTransposed(rect.anchorMax);
			}
		}

		private static Vector2 GetTransposed(Vector2 input)
		{
			return new Vector2(input.y, input.x);
		}

		public static bool RectangleContainsScreenPoint(RectTransform rect, Vector2 screenPoint, Camera cam)
		{
			return INTERNAL_CALL_RectangleContainsScreenPoint(rect, ref screenPoint, cam);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern bool INTERNAL_CALL_RectangleContainsScreenPoint(RectTransform rect, ref Vector2 screenPoint, Camera cam);

		public static Vector2 PixelAdjustPoint(Vector2 point, Transform elementTransform, Canvas canvas)
		{
			INTERNAL_CALL_PixelAdjustPoint(ref point, elementTransform, canvas, out Vector2 value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_PixelAdjustPoint(ref Vector2 point, Transform elementTransform, Canvas canvas, out Vector2 value);

		public static Rect PixelAdjustRect(RectTransform rectTransform, Canvas canvas)
		{
			INTERNAL_CALL_PixelAdjustRect(rectTransform, canvas, out Rect value);
			return value;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private static extern void INTERNAL_CALL_PixelAdjustRect(RectTransform rectTransform, Canvas canvas, out Rect value);
	}
}
