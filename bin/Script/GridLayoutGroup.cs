namespace UnityEngine.UI
{
	[AddComponentMenu("Layout/Grid Layout Group", 152)]
	public class GridLayoutGroup : LayoutGroup
	{
		public enum Corner
		{
			UpperLeft,
			UpperRight,
			LowerLeft,
			LowerRight
		}

		public enum Axis
		{
			Horizontal,
			Vertical
		}

		public enum Constraint
		{
			Flexible,
			FixedColumnCount,
			FixedRowCount
		}

		[SerializeField]
		protected Corner m_StartCorner = Corner.UpperLeft;

		[SerializeField]
		protected Axis m_StartAxis = Axis.Horizontal;

		[SerializeField]
		protected Vector2 m_CellSize = new Vector2(100f, 100f);

		[SerializeField]
		protected Vector2 m_Spacing = Vector2.zero;

		[SerializeField]
		protected Constraint m_Constraint = Constraint.Flexible;

		[SerializeField]
		protected int m_ConstraintCount = 2;

		public Corner startCorner
		{
			get
			{
				return m_StartCorner;
			}
			set
			{
				SetProperty(ref m_StartCorner, value);
			}
		}

		public Axis startAxis
		{
			get
			{
				return m_StartAxis;
			}
			set
			{
				SetProperty(ref m_StartAxis, value);
			}
		}

		public Vector2 cellSize
		{
			get
			{
				return m_CellSize;
			}
			set
			{
				SetProperty(ref m_CellSize, value);
			}
		}

		public Vector2 spacing
		{
			get
			{
				return m_Spacing;
			}
			set
			{
				SetProperty(ref m_Spacing, value);
			}
		}

		public Constraint constraint
		{
			get
			{
				return m_Constraint;
			}
			set
			{
				SetProperty(ref m_Constraint, value);
			}
		}

		public int constraintCount
		{
			get
			{
				return m_ConstraintCount;
			}
			set
			{
				SetProperty(ref m_ConstraintCount, Mathf.Max(1, value));
			}
		}

		protected GridLayoutGroup()
		{
		}

		public override void CalculateLayoutInputHorizontal()
		{
			base.CalculateLayoutInputHorizontal();
			int num = 0;
			int num2 = 0;
			if (m_Constraint == Constraint.FixedColumnCount)
			{
				num = (num2 = m_ConstraintCount);
			}
			else if (m_Constraint == Constraint.FixedRowCount)
			{
				num = (num2 = Mathf.CeilToInt((float)base.rectChildren.Count / (float)m_ConstraintCount - 0.001f));
			}
			else
			{
				num = 1;
				num2 = Mathf.CeilToInt(Mathf.Sqrt(base.rectChildren.Count));
			}
			float num3 = base.padding.horizontal;
			Vector2 cellSize = this.cellSize;
			float x = cellSize.x;
			Vector2 spacing = this.spacing;
			float num4 = num3 + (x + spacing.x) * (float)num;
			Vector2 spacing2 = this.spacing;
			float totalMin = num4 - spacing2.x;
			float num5 = base.padding.horizontal;
			Vector2 cellSize2 = this.cellSize;
			float x2 = cellSize2.x;
			Vector2 spacing3 = this.spacing;
			float num6 = num5 + (x2 + spacing3.x) * (float)num2;
			Vector2 spacing4 = this.spacing;
			SetLayoutInputForAxis(totalMin, num6 - spacing4.x, -1f, 0);
		}

		public override void CalculateLayoutInputVertical()
		{
			int num = 0;
			if (m_Constraint == Constraint.FixedColumnCount)
			{
				num = Mathf.CeilToInt((float)base.rectChildren.Count / (float)m_ConstraintCount - 0.001f);
			}
			else if (m_Constraint == Constraint.FixedRowCount)
			{
				num = m_ConstraintCount;
			}
			else
			{
				Vector2 size = base.rectTransform.rect.size;
				float x = size.x;
				float num2 = x - (float)base.padding.horizontal;
				Vector2 spacing = this.spacing;
				float num3 = num2 + spacing.x + 0.001f;
				Vector2 cellSize = this.cellSize;
				float x2 = cellSize.x;
				Vector2 spacing2 = this.spacing;
				int num4 = Mathf.Max(1, Mathf.FloorToInt(num3 / (x2 + spacing2.x)));
				num = Mathf.CeilToInt((float)base.rectChildren.Count / (float)num4);
			}
			float num5 = base.padding.vertical;
			Vector2 cellSize2 = this.cellSize;
			float y = cellSize2.y;
			Vector2 spacing3 = this.spacing;
			float num6 = num5 + (y + spacing3.y) * (float)num;
			Vector2 spacing4 = this.spacing;
			float num7 = num6 - spacing4.y;
			SetLayoutInputForAxis(num7, num7, -1f, 1);
		}

		public override void SetLayoutHorizontal()
		{
			SetCellsAlongAxis(0);
		}

		public override void SetLayoutVertical()
		{
			SetCellsAlongAxis(1);
		}

		private void SetCellsAlongAxis(int axis)
		{
			if (axis == 0)
			{
				for (int i = 0; i < base.rectChildren.Count; i++)
				{
					RectTransform rectTransform = base.rectChildren[i];
					m_Tracker.Add(this, rectTransform, DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.AnchorMinX | DrivenTransformProperties.AnchorMinY | DrivenTransformProperties.AnchorMaxX | DrivenTransformProperties.AnchorMaxY | DrivenTransformProperties.SizeDeltaX | DrivenTransformProperties.SizeDeltaY);
					rectTransform.anchorMin = Vector2.up;
					rectTransform.anchorMax = Vector2.up;
					rectTransform.sizeDelta = this.cellSize;
				}
				return;
			}
			Vector2 size = base.rectTransform.rect.size;
			float x = size.x;
			Vector2 size2 = base.rectTransform.rect.size;
			float y = size2.y;
			int num = 1;
			int num2 = 1;
			if (m_Constraint == Constraint.FixedColumnCount)
			{
				num = m_ConstraintCount;
				num2 = Mathf.CeilToInt((float)base.rectChildren.Count / (float)num - 0.001f);
			}
			else if (m_Constraint == Constraint.FixedRowCount)
			{
				num2 = m_ConstraintCount;
				num = Mathf.CeilToInt((float)base.rectChildren.Count / (float)num2 - 0.001f);
			}
			else
			{
				Vector2 cellSize = this.cellSize;
				float x2 = cellSize.x;
				Vector2 spacing = this.spacing;
				if (x2 + spacing.x <= 0f)
				{
					num = int.MaxValue;
				}
				else
				{
					float num3 = x - (float)base.padding.horizontal;
					Vector2 spacing2 = this.spacing;
					float num4 = num3 + spacing2.x + 0.001f;
					Vector2 cellSize2 = this.cellSize;
					float x3 = cellSize2.x;
					Vector2 spacing3 = this.spacing;
					num = Mathf.Max(1, Mathf.FloorToInt(num4 / (x3 + spacing3.x)));
				}
				Vector2 cellSize3 = this.cellSize;
				float y2 = cellSize3.y;
				Vector2 spacing4 = this.spacing;
				if (y2 + spacing4.y <= 0f)
				{
					num2 = int.MaxValue;
				}
				else
				{
					float num5 = y - (float)base.padding.vertical;
					Vector2 spacing5 = this.spacing;
					float num6 = num5 + spacing5.y + 0.001f;
					Vector2 cellSize4 = this.cellSize;
					float y3 = cellSize4.y;
					Vector2 spacing6 = this.spacing;
					num2 = Mathf.Max(1, Mathf.FloorToInt(num6 / (y3 + spacing6.y)));
				}
			}
			int num7 = (int)startCorner % 2;
			int num8 = (int)startCorner / 2;
			int num9;
			int num10;
			int num11;
			if (startAxis == Axis.Horizontal)
			{
				num9 = num;
				num10 = Mathf.Clamp(num, 1, base.rectChildren.Count);
				num11 = Mathf.Clamp(num2, 1, Mathf.CeilToInt((float)base.rectChildren.Count / (float)num9));
			}
			else
			{
				num9 = num2;
				num11 = Mathf.Clamp(num2, 1, base.rectChildren.Count);
				num10 = Mathf.Clamp(num, 1, Mathf.CeilToInt((float)base.rectChildren.Count / (float)num9));
			}
			float num12 = num10;
			Vector2 cellSize5 = this.cellSize;
			float num13 = num12 * cellSize5.x;
			float num14 = num10 - 1;
			Vector2 spacing7 = this.spacing;
			float x4 = num13 + num14 * spacing7.x;
			float num15 = num11;
			Vector2 cellSize6 = this.cellSize;
			float num16 = num15 * cellSize6.y;
			float num17 = num11 - 1;
			Vector2 spacing8 = this.spacing;
			Vector2 vector = new Vector2(x4, num16 + num17 * spacing8.y);
			Vector2 vector2 = new Vector2(GetStartOffset(0, vector.x), GetStartOffset(1, vector.y));
			for (int j = 0; j < base.rectChildren.Count; j++)
			{
				int num18;
				int num19;
				if (startAxis == Axis.Horizontal)
				{
					num18 = j % num9;
					num19 = j / num9;
				}
				else
				{
					num18 = j / num9;
					num19 = j % num9;
				}
				if (num7 == 1)
				{
					num18 = num10 - 1 - num18;
				}
				if (num8 == 1)
				{
					num19 = num11 - 1 - num19;
				}
				SetChildAlongAxis(base.rectChildren[j], 0, vector2.x + (this.cellSize[0] + this.spacing[0]) * (float)num18, this.cellSize[0]);
				SetChildAlongAxis(base.rectChildren[j], 1, vector2.y + (this.cellSize[1] + this.spacing[1]) * (float)num19, this.cellSize[1]);
			}
		}
	}
}
