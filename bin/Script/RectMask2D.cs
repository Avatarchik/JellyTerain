using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	[AddComponentMenu("UI/Rect Mask 2D", 13)]
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(RectTransform))]
	public class RectMask2D : UIBehaviour, IClipper, ICanvasRaycastFilter
	{
		[NonSerialized]
		private readonly RectangularVertexClipper m_VertexClipper = new RectangularVertexClipper();

		[NonSerialized]
		private RectTransform m_RectTransform;

		[NonSerialized]
		private HashSet<IClippable> m_ClipTargets = new HashSet<IClippable>();

		[NonSerialized]
		private bool m_ShouldRecalculateClipRects;

		[NonSerialized]
		private List<RectMask2D> m_Clippers = new List<RectMask2D>();

		[NonSerialized]
		private Rect m_LastClipRectCanvasSpace;

		[NonSerialized]
		private bool m_LastValidClipRect;

		[NonSerialized]
		private bool m_ForceClip;

		public Rect canvasRect
		{
			get
			{
				Canvas c = null;
				List<Canvas> list = ListPool<Canvas>.Get();
				base.gameObject.GetComponentsInParent(includeInactive: false, list);
				if (list.Count > 0)
				{
					c = list[list.Count - 1];
				}
				ListPool<Canvas>.Release(list);
				return m_VertexClipper.GetCanvasRect(rectTransform, c);
			}
		}

		public RectTransform rectTransform => m_RectTransform ?? (m_RectTransform = GetComponent<RectTransform>());

		protected RectMask2D()
		{
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			m_ShouldRecalculateClipRects = true;
			ClipperRegistry.Register(this);
			MaskUtilities.Notify2DMaskStateChanged(this);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			m_ClipTargets.Clear();
			m_Clippers.Clear();
			ClipperRegistry.Unregister(this);
			MaskUtilities.Notify2DMaskStateChanged(this);
		}

		public virtual bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
		{
			if (!base.isActiveAndEnabled)
			{
				return true;
			}
			return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, sp, eventCamera);
		}

		public virtual void PerformClipping()
		{
			if (m_ShouldRecalculateClipRects)
			{
				MaskUtilities.GetRectMasksForClip(this, m_Clippers);
				m_ShouldRecalculateClipRects = false;
			}
			bool validRect = true;
			Rect rect = Clipping.FindCullAndClipWorldRect(m_Clippers, out validRect);
			bool flag = rect != m_LastClipRectCanvasSpace;
			if (flag || m_ForceClip)
			{
				foreach (IClippable clipTarget in m_ClipTargets)
				{
					clipTarget.SetClipRect(rect, validRect);
				}
				m_LastClipRectCanvasSpace = rect;
				m_LastValidClipRect = validRect;
			}
			foreach (IClippable clipTarget2 in m_ClipTargets)
			{
				MaskableGraphic maskableGraphic = clipTarget2 as MaskableGraphic;
				if (!(maskableGraphic != null) || maskableGraphic.canvasRenderer.hasMoved || flag)
				{
					clipTarget2.Cull(m_LastClipRectCanvasSpace, m_LastValidClipRect);
				}
			}
		}

		public void AddClippable(IClippable clippable)
		{
			if (clippable != null)
			{
				m_ShouldRecalculateClipRects = true;
				if (!m_ClipTargets.Contains(clippable))
				{
					m_ClipTargets.Add(clippable);
				}
				m_ForceClip = true;
			}
		}

		public void RemoveClippable(IClippable clippable)
		{
			if (clippable != null)
			{
				m_ShouldRecalculateClipRects = true;
				clippable.SetClipRect(default(Rect), validRect: false);
				m_ClipTargets.Remove(clippable);
				m_ForceClip = true;
			}
		}

		protected override void OnTransformParentChanged()
		{
			base.OnTransformParentChanged();
			m_ShouldRecalculateClipRects = true;
		}

		protected override void OnCanvasHierarchyChanged()
		{
			base.OnCanvasHierarchyChanged();
			m_ShouldRecalculateClipRects = true;
		}
	}
}
