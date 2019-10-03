using System;
using System.Collections.Generic;

namespace UnityEngine.UI
{
	[AddComponentMenu("UI/Text", 10)]
	public class Text : MaskableGraphic, ILayoutElement
	{
		[SerializeField]
		private FontData m_FontData = FontData.defaultFontData;

		[TextArea(3, 10)]
		[SerializeField]
		protected string m_Text = string.Empty;

		private TextGenerator m_TextCache;

		private TextGenerator m_TextCacheForLayout;

		protected static Material s_DefaultText = null;

		[NonSerialized]
		protected bool m_DisableFontTextureRebuiltCallback = false;

		private readonly UIVertex[] m_TempVerts = new UIVertex[4];

		public TextGenerator cachedTextGenerator => m_TextCache ?? (m_TextCache = ((m_Text.Length == 0) ? new TextGenerator() : new TextGenerator(m_Text.Length)));

		public TextGenerator cachedTextGeneratorForLayout => m_TextCacheForLayout ?? (m_TextCacheForLayout = new TextGenerator());

		public override Texture mainTexture
		{
			get
			{
				if (font != null && font.material != null && font.material.mainTexture != null)
				{
					return font.material.mainTexture;
				}
				if (m_Material != null)
				{
					return m_Material.mainTexture;
				}
				return base.mainTexture;
			}
		}

		public Font font
		{
			get
			{
				return m_FontData.font;
			}
			set
			{
				if (!(m_FontData.font == value))
				{
					FontUpdateTracker.UntrackText(this);
					m_FontData.font = value;
					FontUpdateTracker.TrackText(this);
					SetAllDirty();
				}
			}
		}

		public virtual string text
		{
			get
			{
				return m_Text;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					if (!string.IsNullOrEmpty(m_Text))
					{
						m_Text = "";
						SetVerticesDirty();
					}
				}
				else if (m_Text != value)
				{
					m_Text = value;
					SetVerticesDirty();
					SetLayoutDirty();
				}
			}
		}

		public bool supportRichText
		{
			get
			{
				return m_FontData.richText;
			}
			set
			{
				if (m_FontData.richText != value)
				{
					m_FontData.richText = value;
					SetVerticesDirty();
					SetLayoutDirty();
				}
			}
		}

		public bool resizeTextForBestFit
		{
			get
			{
				return m_FontData.bestFit;
			}
			set
			{
				if (m_FontData.bestFit != value)
				{
					m_FontData.bestFit = value;
					SetVerticesDirty();
					SetLayoutDirty();
				}
			}
		}

		public int resizeTextMinSize
		{
			get
			{
				return m_FontData.minSize;
			}
			set
			{
				if (m_FontData.minSize != value)
				{
					m_FontData.minSize = value;
					SetVerticesDirty();
					SetLayoutDirty();
				}
			}
		}

		public int resizeTextMaxSize
		{
			get
			{
				return m_FontData.maxSize;
			}
			set
			{
				if (m_FontData.maxSize != value)
				{
					m_FontData.maxSize = value;
					SetVerticesDirty();
					SetLayoutDirty();
				}
			}
		}

		public TextAnchor alignment
		{
			get
			{
				return m_FontData.alignment;
			}
			set
			{
				if (m_FontData.alignment != value)
				{
					m_FontData.alignment = value;
					SetVerticesDirty();
					SetLayoutDirty();
				}
			}
		}

		public bool alignByGeometry
		{
			get
			{
				return m_FontData.alignByGeometry;
			}
			set
			{
				if (m_FontData.alignByGeometry != value)
				{
					m_FontData.alignByGeometry = value;
					SetVerticesDirty();
				}
			}
		}

		public int fontSize
		{
			get
			{
				return m_FontData.fontSize;
			}
			set
			{
				if (m_FontData.fontSize != value)
				{
					m_FontData.fontSize = value;
					SetVerticesDirty();
					SetLayoutDirty();
				}
			}
		}

		public HorizontalWrapMode horizontalOverflow
		{
			get
			{
				return m_FontData.horizontalOverflow;
			}
			set
			{
				if (m_FontData.horizontalOverflow != value)
				{
					m_FontData.horizontalOverflow = value;
					SetVerticesDirty();
					SetLayoutDirty();
				}
			}
		}

		public VerticalWrapMode verticalOverflow
		{
			get
			{
				return m_FontData.verticalOverflow;
			}
			set
			{
				if (m_FontData.verticalOverflow != value)
				{
					m_FontData.verticalOverflow = value;
					SetVerticesDirty();
					SetLayoutDirty();
				}
			}
		}

		public float lineSpacing
		{
			get
			{
				return m_FontData.lineSpacing;
			}
			set
			{
				if (m_FontData.lineSpacing != value)
				{
					m_FontData.lineSpacing = value;
					SetVerticesDirty();
					SetLayoutDirty();
				}
			}
		}

		public FontStyle fontStyle
		{
			get
			{
				return m_FontData.fontStyle;
			}
			set
			{
				if (m_FontData.fontStyle != value)
				{
					m_FontData.fontStyle = value;
					SetVerticesDirty();
					SetLayoutDirty();
				}
			}
		}

		public float pixelsPerUnit
		{
			get
			{
				Canvas canvas = base.canvas;
				if (!canvas)
				{
					return 1f;
				}
				if (!font || font.dynamic)
				{
					return canvas.scaleFactor;
				}
				if (m_FontData.fontSize <= 0 || font.fontSize <= 0)
				{
					return 1f;
				}
				return (float)font.fontSize / (float)m_FontData.fontSize;
			}
		}

		public virtual float minWidth => 0f;

		public virtual float preferredWidth
		{
			get
			{
				TextGenerationSettings generationSettings = GetGenerationSettings(Vector2.zero);
				return cachedTextGeneratorForLayout.GetPreferredWidth(m_Text, generationSettings) / pixelsPerUnit;
			}
		}

		public virtual float flexibleWidth => -1f;

		public virtual float minHeight => 0f;

		public virtual float preferredHeight
		{
			get
			{
				Vector2 size = GetPixelAdjustedRect().size;
				TextGenerationSettings generationSettings = GetGenerationSettings(new Vector2(size.x, 0f));
				return cachedTextGeneratorForLayout.GetPreferredHeight(m_Text, generationSettings) / pixelsPerUnit;
			}
		}

		public virtual float flexibleHeight => -1f;

		public virtual int layoutPriority => 0;

		protected Text()
		{
			base.useLegacyMeshGeneration = false;
		}

		public void FontTextureChanged()
		{
			if (!this || m_DisableFontTextureRebuiltCallback)
			{
				return;
			}
			cachedTextGenerator.Invalidate();
			if (IsActive())
			{
				if (CanvasUpdateRegistry.IsRebuildingGraphics() || CanvasUpdateRegistry.IsRebuildingLayout())
				{
					UpdateGeometry();
				}
				else
				{
					SetAllDirty();
				}
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			cachedTextGenerator.Invalidate();
			FontUpdateTracker.TrackText(this);
		}

		protected override void OnDisable()
		{
			FontUpdateTracker.UntrackText(this);
			base.OnDisable();
		}

		protected override void UpdateGeometry()
		{
			if (font != null)
			{
				base.UpdateGeometry();
			}
		}

		internal void AssignDefaultFont()
		{
			font = Resources.GetBuiltinResource<Font>("Arial.ttf");
		}

		public TextGenerationSettings GetGenerationSettings(Vector2 extents)
		{
			TextGenerationSettings result = default(TextGenerationSettings);
			result.generationExtents = extents;
			if (font != null && font.dynamic)
			{
				result.fontSize = m_FontData.fontSize;
				result.resizeTextMinSize = m_FontData.minSize;
				result.resizeTextMaxSize = m_FontData.maxSize;
			}
			result.textAnchor = m_FontData.alignment;
			result.alignByGeometry = m_FontData.alignByGeometry;
			result.scaleFactor = pixelsPerUnit;
			result.color = color;
			result.font = font;
			result.pivot = base.rectTransform.pivot;
			result.richText = m_FontData.richText;
			result.lineSpacing = m_FontData.lineSpacing;
			result.fontStyle = m_FontData.fontStyle;
			result.resizeTextForBestFit = m_FontData.bestFit;
			result.updateBounds = false;
			result.horizontalOverflow = m_FontData.horizontalOverflow;
			result.verticalOverflow = m_FontData.verticalOverflow;
			return result;
		}

		public static Vector2 GetTextAnchorPivot(TextAnchor anchor)
		{
			switch (anchor)
			{
			case TextAnchor.LowerLeft:
				return new Vector2(0f, 0f);
			case TextAnchor.LowerCenter:
				return new Vector2(0.5f, 0f);
			case TextAnchor.LowerRight:
				return new Vector2(1f, 0f);
			case TextAnchor.MiddleLeft:
				return new Vector2(0f, 0.5f);
			case TextAnchor.MiddleCenter:
				return new Vector2(0.5f, 0.5f);
			case TextAnchor.MiddleRight:
				return new Vector2(1f, 0.5f);
			case TextAnchor.UpperLeft:
				return new Vector2(0f, 1f);
			case TextAnchor.UpperCenter:
				return new Vector2(0.5f, 1f);
			case TextAnchor.UpperRight:
				return new Vector2(1f, 1f);
			default:
				return Vector2.zero;
			}
		}

		protected override void OnPopulateMesh(VertexHelper toFill)
		{
			if (font == null)
			{
				return;
			}
			m_DisableFontTextureRebuiltCallback = true;
			Vector2 size = base.rectTransform.rect.size;
			TextGenerationSettings generationSettings = GetGenerationSettings(size);
			cachedTextGenerator.PopulateWithErrors(text, generationSettings, base.gameObject);
			IList<UIVertex> verts = cachedTextGenerator.verts;
			float num = 1f / pixelsPerUnit;
			int num2 = verts.Count - 4;
			UIVertex uIVertex = verts[0];
			float x = uIVertex.position.x;
			UIVertex uIVertex2 = verts[0];
			Vector2 vector = new Vector2(x, uIVertex2.position.y) * num;
			vector = PixelAdjustPoint(vector) - vector;
			toFill.Clear();
			if (vector != Vector2.zero)
			{
				for (int i = 0; i < num2; i++)
				{
					int num3 = i & 3;
					m_TempVerts[num3] = verts[i];
					m_TempVerts[num3].position *= num;
					m_TempVerts[num3].position.x += vector.x;
					m_TempVerts[num3].position.y += vector.y;
					if (num3 == 3)
					{
						toFill.AddUIVertexQuad(m_TempVerts);
					}
				}
			}
			else
			{
				for (int j = 0; j < num2; j++)
				{
					int num4 = j & 3;
					m_TempVerts[num4] = verts[j];
					m_TempVerts[num4].position *= num;
					if (num4 == 3)
					{
						toFill.AddUIVertexQuad(m_TempVerts);
					}
				}
			}
			m_DisableFontTextureRebuiltCallback = false;
		}

		public virtual void CalculateLayoutInputHorizontal()
		{
		}

		public virtual void CalculateLayoutInputVertical()
		{
		}
	}
}
