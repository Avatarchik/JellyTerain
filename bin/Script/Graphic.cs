using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI.CoroutineTween;

namespace UnityEngine.UI
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(CanvasRenderer))]
	[RequireComponent(typeof(RectTransform))]
	[ExecuteInEditMode]
	public abstract class Graphic : UIBehaviour, ICanvasElement
	{
		protected static Material s_DefaultUI = null;

		protected static Texture2D s_WhiteTexture = null;

		[FormerlySerializedAs("m_Mat")]
		[SerializeField]
		protected Material m_Material;

		[SerializeField]
		private Color m_Color = Color.white;

		[SerializeField]
		private bool m_RaycastTarget = true;

		[NonSerialized]
		private RectTransform m_RectTransform;

		[NonSerialized]
		private CanvasRenderer m_CanvasRender;

		[NonSerialized]
		private Canvas m_Canvas;

		[NonSerialized]
		private bool m_VertsDirty;

		[NonSerialized]
		private bool m_MaterialDirty;

		[NonSerialized]
		protected UnityAction m_OnDirtyLayoutCallback;

		[NonSerialized]
		protected UnityAction m_OnDirtyVertsCallback;

		[NonSerialized]
		protected UnityAction m_OnDirtyMaterialCallback;

		[NonSerialized]
		protected static Mesh s_Mesh;

		[NonSerialized]
		private static readonly VertexHelper s_VertexHelper = new VertexHelper();

		[NonSerialized]
		private readonly TweenRunner<ColorTween> m_ColorTweenRunner;

		public static Material defaultGraphicMaterial
		{
			get
			{
				if (s_DefaultUI == null)
				{
					s_DefaultUI = Canvas.GetDefaultCanvasMaterial();
				}
				return s_DefaultUI;
			}
		}

		public virtual Color color
		{
			get
			{
				return m_Color;
			}
			set
			{
				if (SetPropertyUtility.SetColor(ref m_Color, value))
				{
					SetVerticesDirty();
				}
			}
		}

		public virtual bool raycastTarget
		{
			get
			{
				return m_RaycastTarget;
			}
			set
			{
				m_RaycastTarget = value;
			}
		}

		protected bool useLegacyMeshGeneration
		{
			get;
			set;
		}

		public int depth => canvasRenderer.absoluteDepth;

		public RectTransform rectTransform => m_RectTransform ?? (m_RectTransform = GetComponent<RectTransform>());

		public Canvas canvas
		{
			get
			{
				if (m_Canvas == null)
				{
					CacheCanvas();
				}
				return m_Canvas;
			}
		}

		public CanvasRenderer canvasRenderer
		{
			get
			{
				if (m_CanvasRender == null)
				{
					m_CanvasRender = GetComponent<CanvasRenderer>();
				}
				return m_CanvasRender;
			}
		}

		public virtual Material defaultMaterial => defaultGraphicMaterial;

		public virtual Material material
		{
			get
			{
				return (!(m_Material != null)) ? defaultMaterial : m_Material;
			}
			set
			{
				if (!(m_Material == value))
				{
					m_Material = value;
					SetMaterialDirty();
				}
			}
		}

		public virtual Material materialForRendering
		{
			get
			{
				List<Component> list = ListPool<Component>.Get();
				GetComponents(typeof(IMaterialModifier), list);
				Material material = this.material;
				for (int i = 0; i < list.Count; i++)
				{
					material = (list[i] as IMaterialModifier).GetModifiedMaterial(material);
				}
				ListPool<Component>.Release(list);
				return material;
			}
		}

		public virtual Texture mainTexture => s_WhiteTexture;

		protected static Mesh workerMesh
		{
			get
			{
				if (s_Mesh == null)
				{
					s_Mesh = new Mesh();
					s_Mesh.name = "Shared UI Mesh";
					s_Mesh.hideFlags = HideFlags.HideAndDontSave;
				}
				return s_Mesh;
			}
		}

		protected Graphic()
		{
			if (m_ColorTweenRunner == null)
			{
				m_ColorTweenRunner = new TweenRunner<ColorTween>();
			}
			m_ColorTweenRunner.Init(this);
			useLegacyMeshGeneration = true;
		}

		public virtual void SetAllDirty()
		{
			SetLayoutDirty();
			SetVerticesDirty();
			SetMaterialDirty();
		}

		public virtual void SetLayoutDirty()
		{
			if (IsActive())
			{
				LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
				if (m_OnDirtyLayoutCallback != null)
				{
					m_OnDirtyLayoutCallback();
				}
			}
		}

		public virtual void SetVerticesDirty()
		{
			if (IsActive())
			{
				m_VertsDirty = true;
				CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(this);
				if (m_OnDirtyVertsCallback != null)
				{
					m_OnDirtyVertsCallback();
				}
			}
		}

		public virtual void SetMaterialDirty()
		{
			if (IsActive())
			{
				m_MaterialDirty = true;
				CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(this);
				if (m_OnDirtyMaterialCallback != null)
				{
					m_OnDirtyMaterialCallback();
				}
			}
		}

		protected override void OnRectTransformDimensionsChange()
		{
			if (base.gameObject.activeInHierarchy)
			{
				if (CanvasUpdateRegistry.IsRebuildingLayout())
				{
					SetVerticesDirty();
					return;
				}
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}

		protected override void OnBeforeTransformParentChanged()
		{
			GraphicRegistry.UnregisterGraphicForCanvas(canvas, this);
			LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
		}

		protected override void OnTransformParentChanged()
		{
			base.OnTransformParentChanged();
			m_Canvas = null;
			if (IsActive())
			{
				CacheCanvas();
				GraphicRegistry.RegisterGraphicForCanvas(canvas, this);
				SetAllDirty();
			}
		}

		private void CacheCanvas()
		{
			List<Canvas> list = ListPool<Canvas>.Get();
			base.gameObject.GetComponentsInParent(includeInactive: false, list);
			if (list.Count > 0)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].isActiveAndEnabled)
					{
						m_Canvas = list[i];
						break;
					}
				}
			}
			else
			{
				m_Canvas = null;
			}
			ListPool<Canvas>.Release(list);
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			CacheCanvas();
			GraphicRegistry.RegisterGraphicForCanvas(canvas, this);
			if (s_WhiteTexture == null)
			{
				s_WhiteTexture = Texture2D.whiteTexture;
			}
			SetAllDirty();
		}

		protected override void OnDisable()
		{
			GraphicRegistry.UnregisterGraphicForCanvas(canvas, this);
			CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild(this);
			if (canvasRenderer != null)
			{
				canvasRenderer.Clear();
			}
			LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
			base.OnDisable();
		}

		protected override void OnCanvasHierarchyChanged()
		{
			Canvas canvas = m_Canvas;
			m_Canvas = null;
			if (!IsActive())
			{
				return;
			}
			CacheCanvas();
			if (canvas != m_Canvas)
			{
				GraphicRegistry.UnregisterGraphicForCanvas(canvas, this);
				if (IsActive())
				{
					GraphicRegistry.RegisterGraphicForCanvas(this.canvas, this);
				}
			}
		}

		public virtual void Rebuild(CanvasUpdate update)
		{
			if (!canvasRenderer.cull && update == CanvasUpdate.PreRender)
			{
				if (m_VertsDirty)
				{
					UpdateGeometry();
					m_VertsDirty = false;
				}
				if (m_MaterialDirty)
				{
					UpdateMaterial();
					m_MaterialDirty = false;
				}
			}
		}

		public virtual void LayoutComplete()
		{
		}

		public virtual void GraphicUpdateComplete()
		{
		}

		protected virtual void UpdateMaterial()
		{
			if (IsActive())
			{
				canvasRenderer.materialCount = 1;
				canvasRenderer.SetMaterial(materialForRendering, 0);
				canvasRenderer.SetTexture(mainTexture);
			}
		}

		protected virtual void UpdateGeometry()
		{
			if (useLegacyMeshGeneration)
			{
				DoLegacyMeshGeneration();
			}
			else
			{
				DoMeshGeneration();
			}
		}

		private void DoMeshGeneration()
		{
			if (rectTransform != null && rectTransform.rect.width >= 0f && rectTransform.rect.height >= 0f)
			{
				OnPopulateMesh(s_VertexHelper);
			}
			else
			{
				s_VertexHelper.Clear();
			}
			List<Component> list = ListPool<Component>.Get();
			GetComponents(typeof(IMeshModifier), list);
			for (int i = 0; i < list.Count; i++)
			{
				((IMeshModifier)list[i]).ModifyMesh(s_VertexHelper);
			}
			ListPool<Component>.Release(list);
			s_VertexHelper.FillMesh(workerMesh);
			canvasRenderer.SetMesh(workerMesh);
		}

		private void DoLegacyMeshGeneration()
		{
			if (rectTransform != null && rectTransform.rect.width >= 0f && rectTransform.rect.height >= 0f)
			{
				OnPopulateMesh(workerMesh);
			}
			else
			{
				workerMesh.Clear();
			}
			List<Component> list = ListPool<Component>.Get();
			GetComponents(typeof(IMeshModifier), list);
			for (int i = 0; i < list.Count; i++)
			{
				((IMeshModifier)list[i]).ModifyMesh(workerMesh);
			}
			ListPool<Component>.Release(list);
			canvasRenderer.SetMesh(workerMesh);
		}

		[Obsolete("Use OnPopulateMesh instead.", true)]
		protected virtual void OnFillVBO(List<UIVertex> vbo)
		{
		}

		[Obsolete("Use OnPopulateMesh(VertexHelper vh) instead.", false)]
		protected virtual void OnPopulateMesh(Mesh m)
		{
			OnPopulateMesh(s_VertexHelper);
			s_VertexHelper.FillMesh(m);
		}

		protected virtual void OnPopulateMesh(VertexHelper vh)
		{
			Rect pixelAdjustedRect = GetPixelAdjustedRect();
			Vector4 vector = new Vector4(pixelAdjustedRect.x, pixelAdjustedRect.y, pixelAdjustedRect.x + pixelAdjustedRect.width, pixelAdjustedRect.y + pixelAdjustedRect.height);
			Color32 color = this.color;
			vh.Clear();
			vh.AddVert(new Vector3(vector.x, vector.y), color, new Vector2(0f, 0f));
			vh.AddVert(new Vector3(vector.x, vector.w), color, new Vector2(0f, 1f));
			vh.AddVert(new Vector3(vector.z, vector.w), color, new Vector2(1f, 1f));
			vh.AddVert(new Vector3(vector.z, vector.y), color, new Vector2(1f, 0f));
			vh.AddTriangle(0, 1, 2);
			vh.AddTriangle(2, 3, 0);
		}

		protected override void OnDidApplyAnimationProperties()
		{
			SetAllDirty();
		}

		public virtual void SetNativeSize()
		{
		}

		public virtual bool Raycast(Vector2 sp, Camera eventCamera)
		{
			if (!base.isActiveAndEnabled)
			{
				return false;
			}
			Transform transform = base.transform;
			List<Component> list = ListPool<Component>.Get();
			bool flag = false;
			bool flag2 = true;
			while (transform != null)
			{
				transform.GetComponents(list);
				for (int i = 0; i < list.Count; i++)
				{
					Canvas canvas = list[i] as Canvas;
					if (canvas != null && canvas.overrideSorting)
					{
						flag2 = false;
					}
					ICanvasRaycastFilter canvasRaycastFilter = list[i] as ICanvasRaycastFilter;
					if (canvasRaycastFilter == null)
					{
						continue;
					}
					bool flag3 = true;
					CanvasGroup canvasGroup = list[i] as CanvasGroup;
					if (canvasGroup != null)
					{
						if (!flag && canvasGroup.ignoreParentGroups)
						{
							flag = true;
							flag3 = canvasRaycastFilter.IsRaycastLocationValid(sp, eventCamera);
						}
						else if (!flag)
						{
							flag3 = canvasRaycastFilter.IsRaycastLocationValid(sp, eventCamera);
						}
					}
					else
					{
						flag3 = canvasRaycastFilter.IsRaycastLocationValid(sp, eventCamera);
					}
					if (!flag3)
					{
						ListPool<Component>.Release(list);
						return false;
					}
				}
				transform = ((!flag2) ? null : transform.parent);
			}
			ListPool<Component>.Release(list);
			return true;
		}

		public Vector2 PixelAdjustPoint(Vector2 point)
		{
			if (!canvas || canvas.renderMode == RenderMode.WorldSpace || canvas.scaleFactor == 0f || !canvas.pixelPerfect)
			{
				return point;
			}
			return RectTransformUtility.PixelAdjustPoint(point, base.transform, canvas);
		}

		public Rect GetPixelAdjustedRect()
		{
			if (!canvas || canvas.renderMode == RenderMode.WorldSpace || canvas.scaleFactor == 0f || !canvas.pixelPerfect)
			{
				return rectTransform.rect;
			}
			return RectTransformUtility.PixelAdjustRect(rectTransform, canvas);
		}

		public virtual void CrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha)
		{
			CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha, useRGB: true);
		}

		public virtual void CrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha, bool useRGB)
		{
			if (!(canvasRenderer == null) && (useRGB || useAlpha))
			{
				if (canvasRenderer.GetColor().Equals(targetColor))
				{
					m_ColorTweenRunner.StopTween();
					return;
				}
				ColorTween.ColorTweenMode tweenMode = (!useRGB || !useAlpha) ? (useRGB ? ColorTween.ColorTweenMode.RGB : ColorTween.ColorTweenMode.Alpha) : ColorTween.ColorTweenMode.All;
				ColorTween colorTween = default(ColorTween);
				colorTween.duration = duration;
				colorTween.startColor = canvasRenderer.GetColor();
				colorTween.targetColor = targetColor;
				ColorTween info = colorTween;
				info.AddOnChangedCallback(canvasRenderer.SetColor);
				info.ignoreTimeScale = ignoreTimeScale;
				info.tweenMode = tweenMode;
				m_ColorTweenRunner.StartTween(info);
			}
		}

		private static Color CreateColorFromAlpha(float alpha)
		{
			Color black = Color.black;
			black.a = alpha;
			return black;
		}

		public virtual void CrossFadeAlpha(float alpha, float duration, bool ignoreTimeScale)
		{
			CrossFadeColor(CreateColorFromAlpha(alpha), duration, ignoreTimeScale, useAlpha: true, useRGB: false);
		}

		public void RegisterDirtyLayoutCallback(UnityAction action)
		{
			m_OnDirtyLayoutCallback = (UnityAction)Delegate.Combine(m_OnDirtyLayoutCallback, action);
		}

		public void UnregisterDirtyLayoutCallback(UnityAction action)
		{
			m_OnDirtyLayoutCallback = (UnityAction)Delegate.Remove(m_OnDirtyLayoutCallback, action);
		}

		public void RegisterDirtyVerticesCallback(UnityAction action)
		{
			m_OnDirtyVertsCallback = (UnityAction)Delegate.Combine(m_OnDirtyVertsCallback, action);
		}

		public void UnregisterDirtyVerticesCallback(UnityAction action)
		{
			m_OnDirtyVertsCallback = (UnityAction)Delegate.Remove(m_OnDirtyVertsCallback, action);
		}

		public void RegisterDirtyMaterialCallback(UnityAction action)
		{
			m_OnDirtyMaterialCallback = (UnityAction)Delegate.Combine(m_OnDirtyMaterialCallback, action);
		}

		public void UnregisterDirtyMaterialCallback(UnityAction action)
		{
			m_OnDirtyMaterialCallback = (UnityAction)Delegate.Remove(m_OnDirtyMaterialCallback, action);
		}

		Transform ICanvasElement.get_transform()
		{
			return base.transform;
		}
	}
}
