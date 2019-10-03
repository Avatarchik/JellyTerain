using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	[AddComponentMenu("Layout/Aspect Ratio Fitter", 142)]
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	[DisallowMultipleComponent]
	public class AspectRatioFitter : UIBehaviour, ILayoutSelfController, ILayoutController
	{
		public enum AspectMode
		{
			None,
			WidthControlsHeight,
			HeightControlsWidth,
			FitInParent,
			EnvelopeParent
		}

		[SerializeField]
		private AspectMode m_AspectMode = AspectMode.None;

		[SerializeField]
		private float m_AspectRatio = 1f;

		[NonSerialized]
		private RectTransform m_Rect;

		private DrivenRectTransformTracker m_Tracker;

		public AspectMode aspectMode
		{
			get
			{
				return m_AspectMode;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_AspectMode, value))
				{
					SetDirty();
				}
			}
		}

		public float aspectRatio
		{
			get
			{
				return m_AspectRatio;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_AspectRatio, value))
				{
					SetDirty();
				}
			}
		}

		private RectTransform rectTransform
		{
			get
			{
				if (m_Rect == null)
				{
					m_Rect = GetComponent<RectTransform>();
				}
				return m_Rect;
			}
		}

		protected AspectRatioFitter()
		{
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			SetDirty();
		}

		protected override void OnDisable()
		{
			m_Tracker.Clear();
			LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
			base.OnDisable();
		}

		protected override void OnRectTransformDimensionsChange()
		{
			UpdateRect();
		}

		private void UpdateRect()
		{
			if (!IsActive())
			{
				return;
			}
			m_Tracker.Clear();
			switch (m_AspectMode)
			{
			case AspectMode.HeightControlsWidth:
				m_Tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDeltaX);
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectTransform.rect.height * m_AspectRatio);
				break;
			case AspectMode.WidthControlsHeight:
				m_Tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDeltaY);
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectTransform.rect.width / m_AspectRatio);
				break;
			case AspectMode.FitInParent:
			case AspectMode.EnvelopeParent:
			{
				m_Tracker.Add(this, rectTransform, DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.AnchorMinX | DrivenTransformProperties.AnchorMinY | DrivenTransformProperties.AnchorMaxX | DrivenTransformProperties.AnchorMaxY | DrivenTransformProperties.SizeDeltaX | DrivenTransformProperties.SizeDeltaY);
				rectTransform.anchorMin = Vector2.zero;
				rectTransform.anchorMax = Vector2.one;
				rectTransform.anchoredPosition = Vector2.zero;
				Vector2 zero = Vector2.zero;
				Vector2 parentSize = GetParentSize();
				if ((parentSize.y * aspectRatio < parentSize.x) ^ (m_AspectMode == AspectMode.FitInParent))
				{
					zero.y = GetSizeDeltaToProduceSize(parentSize.x / aspectRatio, 1);
				}
				else
				{
					zero.x = GetSizeDeltaToProduceSize(parentSize.y * aspectRatio, 0);
				}
				rectTransform.sizeDelta = zero;
				break;
			}
			}
		}

		private float GetSizeDeltaToProduceSize(float size, int axis)
		{
			return size - GetParentSize()[axis] * (rectTransform.anchorMax[axis] - rectTransform.anchorMin[axis]);
		}

		private Vector2 GetParentSize()
		{
			RectTransform rectTransform = this.rectTransform.parent as RectTransform;
			if (!rectTransform)
			{
				return Vector2.zero;
			}
			return rectTransform.rect.size;
		}

		public virtual void SetLayoutHorizontal()
		{
		}

		public virtual void SetLayoutVertical()
		{
		}

		protected void SetDirty()
		{
			UpdateRect();
		}
	}
}
