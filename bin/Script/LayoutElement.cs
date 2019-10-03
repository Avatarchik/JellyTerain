using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	[AddComponentMenu("Layout/Layout Element", 140)]
	[RequireComponent(typeof(RectTransform))]
	[ExecuteInEditMode]
	public class LayoutElement : UIBehaviour, ILayoutElement, ILayoutIgnorer
	{
		[SerializeField]
		private bool m_IgnoreLayout = false;

		[SerializeField]
		private float m_MinWidth = -1f;

		[SerializeField]
		private float m_MinHeight = -1f;

		[SerializeField]
		private float m_PreferredWidth = -1f;

		[SerializeField]
		private float m_PreferredHeight = -1f;

		[SerializeField]
		private float m_FlexibleWidth = -1f;

		[SerializeField]
		private float m_FlexibleHeight = -1f;

		public virtual bool ignoreLayout
		{
			get
			{
				return m_IgnoreLayout;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_IgnoreLayout, value))
				{
					SetDirty();
				}
			}
		}

		public virtual float minWidth
		{
			get
			{
				return m_MinWidth;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_MinWidth, value))
				{
					SetDirty();
				}
			}
		}

		public virtual float minHeight
		{
			get
			{
				return m_MinHeight;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_MinHeight, value))
				{
					SetDirty();
				}
			}
		}

		public virtual float preferredWidth
		{
			get
			{
				return m_PreferredWidth;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_PreferredWidth, value))
				{
					SetDirty();
				}
			}
		}

		public virtual float preferredHeight
		{
			get
			{
				return m_PreferredHeight;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_PreferredHeight, value))
				{
					SetDirty();
				}
			}
		}

		public virtual float flexibleWidth
		{
			get
			{
				return m_FlexibleWidth;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_FlexibleWidth, value))
				{
					SetDirty();
				}
			}
		}

		public virtual float flexibleHeight
		{
			get
			{
				return m_FlexibleHeight;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_FlexibleHeight, value))
				{
					SetDirty();
				}
			}
		}

		public virtual int layoutPriority => 1;

		protected LayoutElement()
		{
		}

		public virtual void CalculateLayoutInputHorizontal()
		{
		}

		public virtual void CalculateLayoutInputVertical()
		{
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			SetDirty();
		}

		protected override void OnTransformParentChanged()
		{
			SetDirty();
		}

		protected override void OnDisable()
		{
			SetDirty();
			base.OnDisable();
		}

		protected override void OnDidApplyAnimationProperties()
		{
			SetDirty();
		}

		protected override void OnBeforeTransformParentChanged()
		{
			SetDirty();
		}

		protected void SetDirty()
		{
			if (IsActive())
			{
				LayoutRebuilder.MarkLayoutForRebuild(base.transform as RectTransform);
			}
		}
	}
}
