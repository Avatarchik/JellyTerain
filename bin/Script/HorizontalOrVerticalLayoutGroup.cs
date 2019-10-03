namespace UnityEngine.UI
{
	public abstract class HorizontalOrVerticalLayoutGroup : LayoutGroup
	{
		[SerializeField]
		protected float m_Spacing = 0f;

		[SerializeField]
		protected bool m_ChildForceExpandWidth = true;

		[SerializeField]
		protected bool m_ChildForceExpandHeight = true;

		[SerializeField]
		protected bool m_ChildControlWidth = true;

		[SerializeField]
		protected bool m_ChildControlHeight = true;

		public float spacing
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

		public bool childForceExpandWidth
		{
			get
			{
				return m_ChildForceExpandWidth;
			}
			set
			{
				SetProperty(ref m_ChildForceExpandWidth, value);
			}
		}

		public bool childForceExpandHeight
		{
			get
			{
				return m_ChildForceExpandHeight;
			}
			set
			{
				SetProperty(ref m_ChildForceExpandHeight, value);
			}
		}

		public bool childControlWidth
		{
			get
			{
				return m_ChildControlWidth;
			}
			set
			{
				SetProperty(ref m_ChildControlWidth, value);
			}
		}

		public bool childControlHeight
		{
			get
			{
				return m_ChildControlHeight;
			}
			set
			{
				SetProperty(ref m_ChildControlHeight, value);
			}
		}

		protected void CalcAlongAxis(int axis, bool isVertical)
		{
			float num = (axis != 0) ? base.padding.vertical : base.padding.horizontal;
			bool controlSize = (axis != 0) ? m_ChildControlHeight : m_ChildControlWidth;
			bool childForceExpand = (axis != 0) ? childForceExpandHeight : childForceExpandWidth;
			float num2 = num;
			float num3 = num;
			float num4 = 0f;
			bool flag = isVertical ^ (axis == 1);
			for (int i = 0; i < base.rectChildren.Count; i++)
			{
				RectTransform child = base.rectChildren[i];
				GetChildSizes(child, axis, controlSize, childForceExpand, out float min, out float preferred, out float flexible);
				if (flag)
				{
					num2 = Mathf.Max(min + num, num2);
					num3 = Mathf.Max(preferred + num, num3);
					num4 = Mathf.Max(flexible, num4);
				}
				else
				{
					num2 += min + spacing;
					num3 += preferred + spacing;
					num4 += flexible;
				}
			}
			if (!flag && base.rectChildren.Count > 0)
			{
				num2 -= spacing;
				num3 -= spacing;
			}
			num3 = Mathf.Max(num2, num3);
			SetLayoutInputForAxis(num2, num3, num4, axis);
		}

		protected void SetChildrenAlongAxis(int axis, bool isVertical)
		{
			float num = base.rectTransform.rect.size[axis];
			bool flag = (axis != 0) ? m_ChildControlHeight : m_ChildControlWidth;
			bool childForceExpand = (axis != 0) ? childForceExpandHeight : childForceExpandWidth;
			float alignmentOnAxis = GetAlignmentOnAxis(axis);
			if (isVertical ^ (axis == 1))
			{
				float value = num - (float)((axis != 0) ? base.padding.vertical : base.padding.horizontal);
				for (int i = 0; i < base.rectChildren.Count; i++)
				{
					RectTransform rectTransform = base.rectChildren[i];
					GetChildSizes(rectTransform, axis, flag, childForceExpand, out float min, out float preferred, out float flexible);
					float num2 = Mathf.Clamp(value, min, (!(flexible > 0f)) ? preferred : num);
					float startOffset = GetStartOffset(axis, num2);
					if (flag)
					{
						SetChildAlongAxis(rectTransform, axis, startOffset, num2);
						continue;
					}
					float num3 = (num2 - rectTransform.sizeDelta[axis]) * alignmentOnAxis;
					SetChildAlongAxis(rectTransform, axis, startOffset + num3);
				}
				return;
			}
			float num4 = (axis != 0) ? base.padding.top : base.padding.left;
			if (GetTotalFlexibleSize(axis) == 0f && GetTotalPreferredSize(axis) < num)
			{
				num4 = GetStartOffset(axis, GetTotalPreferredSize(axis) - (float)((axis != 0) ? base.padding.vertical : base.padding.horizontal));
			}
			float t = 0f;
			if (GetTotalMinSize(axis) != GetTotalPreferredSize(axis))
			{
				t = Mathf.Clamp01((num - GetTotalMinSize(axis)) / (GetTotalPreferredSize(axis) - GetTotalMinSize(axis)));
			}
			float num5 = 0f;
			if (num > GetTotalPreferredSize(axis) && GetTotalFlexibleSize(axis) > 0f)
			{
				num5 = (num - GetTotalPreferredSize(axis)) / GetTotalFlexibleSize(axis);
			}
			for (int j = 0; j < base.rectChildren.Count; j++)
			{
				RectTransform rectTransform2 = base.rectChildren[j];
				GetChildSizes(rectTransform2, axis, flag, childForceExpand, out float min2, out float preferred2, out float flexible2);
				float num6 = Mathf.Lerp(min2, preferred2, t);
				num6 += flexible2 * num5;
				if (flag)
				{
					SetChildAlongAxis(rectTransform2, axis, num4, num6);
				}
				else
				{
					float num7 = (num6 - rectTransform2.sizeDelta[axis]) * alignmentOnAxis;
					SetChildAlongAxis(rectTransform2, axis, num4 + num7);
				}
				num4 += num6 + spacing;
			}
		}

		private void GetChildSizes(RectTransform child, int axis, bool controlSize, bool childForceExpand, out float min, out float preferred, out float flexible)
		{
			if (!controlSize)
			{
				min = child.sizeDelta[axis];
				preferred = min;
				flexible = 0f;
			}
			else
			{
				min = LayoutUtility.GetMinSize(child, axis);
				preferred = LayoutUtility.GetPreferredSize(child, axis);
				flexible = LayoutUtility.GetFlexibleSize(child, axis);
			}
			if (childForceExpand)
			{
				flexible = Mathf.Max(flexible, 1f);
			}
		}
	}
}
