using UnityEngine.Scripting;

namespace UnityEngine
{
	internal sealed class GUIScrollGroup : GUILayoutGroup
	{
		public float calcMinWidth;

		public float calcMaxWidth;

		public float calcMinHeight;

		public float calcMaxHeight;

		public float clientWidth;

		public float clientHeight;

		public bool allowHorizontalScroll = true;

		public bool allowVerticalScroll = true;

		public bool needsHorizontalScrollbar;

		public bool needsVerticalScrollbar;

		public GUIStyle horizontalScrollbar;

		public GUIStyle verticalScrollbar;

		[RequiredByNativeCode]
		public GUIScrollGroup()
		{
		}

		public override void CalcWidth()
		{
			float minWidth = base.minWidth;
			float maxWidth = base.maxWidth;
			if (allowHorizontalScroll)
			{
				base.minWidth = 0f;
				base.maxWidth = 0f;
			}
			base.CalcWidth();
			calcMinWidth = base.minWidth;
			calcMaxWidth = base.maxWidth;
			if (allowHorizontalScroll)
			{
				if (base.minWidth > 32f)
				{
					base.minWidth = 32f;
				}
				if (minWidth != 0f)
				{
					base.minWidth = minWidth;
				}
				if (maxWidth != 0f)
				{
					base.maxWidth = maxWidth;
					stretchWidth = 0;
				}
			}
		}

		public override void SetHorizontal(float x, float width)
		{
			float num = (!needsVerticalScrollbar) ? width : (width - verticalScrollbar.fixedWidth - (float)verticalScrollbar.margin.left);
			if (allowHorizontalScroll && num < calcMinWidth)
			{
				needsHorizontalScrollbar = true;
				minWidth = calcMinWidth;
				maxWidth = calcMaxWidth;
				base.SetHorizontal(x, calcMinWidth);
				rect.width = width;
				clientWidth = calcMinWidth;
				return;
			}
			needsHorizontalScrollbar = false;
			if (allowHorizontalScroll)
			{
				minWidth = calcMinWidth;
				maxWidth = calcMaxWidth;
			}
			base.SetHorizontal(x, num);
			rect.width = width;
			clientWidth = num;
		}

		public override void CalcHeight()
		{
			float minHeight = base.minHeight;
			float maxHeight = base.maxHeight;
			if (allowVerticalScroll)
			{
				base.minHeight = 0f;
				base.maxHeight = 0f;
			}
			base.CalcHeight();
			calcMinHeight = base.minHeight;
			calcMaxHeight = base.maxHeight;
			if (needsHorizontalScrollbar)
			{
				float num = horizontalScrollbar.fixedHeight + (float)horizontalScrollbar.margin.top;
				base.minHeight += num;
				base.maxHeight += num;
			}
			if (allowVerticalScroll)
			{
				if (base.minHeight > 32f)
				{
					base.minHeight = 32f;
				}
				if (minHeight != 0f)
				{
					base.minHeight = minHeight;
				}
				if (maxHeight != 0f)
				{
					base.maxHeight = maxHeight;
					stretchHeight = 0;
				}
			}
		}

		public override void SetVertical(float y, float height)
		{
			float num = height;
			if (needsHorizontalScrollbar)
			{
				num -= horizontalScrollbar.fixedHeight + (float)horizontalScrollbar.margin.top;
			}
			if (allowVerticalScroll && num < calcMinHeight)
			{
				if (!needsHorizontalScrollbar && !needsVerticalScrollbar)
				{
					clientWidth = rect.width - verticalScrollbar.fixedWidth - (float)verticalScrollbar.margin.left;
					if (clientWidth < calcMinWidth)
					{
						clientWidth = calcMinWidth;
					}
					float width = rect.width;
					SetHorizontal(rect.x, clientWidth);
					CalcHeight();
					rect.width = width;
				}
				float minHeight = base.minHeight;
				float maxHeight = base.maxHeight;
				base.minHeight = calcMinHeight;
				base.maxHeight = calcMaxHeight;
				base.SetVertical(y, calcMinHeight);
				base.minHeight = minHeight;
				base.maxHeight = maxHeight;
				rect.height = height;
				clientHeight = calcMinHeight;
			}
			else
			{
				if (allowVerticalScroll)
				{
					base.minHeight = calcMinHeight;
					base.maxHeight = calcMaxHeight;
				}
				base.SetVertical(y, num);
				rect.height = height;
				clientHeight = num;
			}
		}
	}
}
