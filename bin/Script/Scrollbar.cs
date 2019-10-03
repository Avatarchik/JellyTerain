using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	[AddComponentMenu("UI/Scrollbar", 34)]
	[RequireComponent(typeof(RectTransform))]
	public class Scrollbar : Selectable, IBeginDragHandler, IDragHandler, IInitializePotentialDragHandler, ICanvasElement, IEventSystemHandler
	{
		public enum Direction
		{
			LeftToRight,
			RightToLeft,
			BottomToTop,
			TopToBottom
		}

		[Serializable]
		public class ScrollEvent : UnityEvent<float>
		{
		}

		private enum Axis
		{
			Horizontal,
			Vertical
		}

		[SerializeField]
		private RectTransform m_HandleRect;

		[SerializeField]
		private Direction m_Direction = Direction.LeftToRight;

		[Range(0f, 1f)]
		[SerializeField]
		private float m_Value;

		[Range(0f, 1f)]
		[SerializeField]
		private float m_Size = 0.2f;

		[Range(0f, 11f)]
		[SerializeField]
		private int m_NumberOfSteps = 0;

		[Space(6f)]
		[SerializeField]
		private ScrollEvent m_OnValueChanged = new ScrollEvent();

		private RectTransform m_ContainerRect;

		private Vector2 m_Offset = Vector2.zero;

		private DrivenRectTransformTracker m_Tracker;

		private Coroutine m_PointerDownRepeat;

		private bool isPointerDownAndNotDragging = false;

		public RectTransform handleRect
		{
			get
			{
				return m_HandleRect;
			}
			set
			{
				if (SetPropertyUtility.SetClass(ref m_HandleRect, value))
				{
					UpdateCachedReferences();
					UpdateVisuals();
				}
			}
		}

		public Direction direction
		{
			get
			{
				return m_Direction;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_Direction, value))
				{
					UpdateVisuals();
				}
			}
		}

		public float value
		{
			get
			{
				float num = m_Value;
				if (m_NumberOfSteps > 1)
				{
					num = Mathf.Round(num * (float)(m_NumberOfSteps - 1)) / (float)(m_NumberOfSteps - 1);
				}
				return num;
			}
			set
			{
				Set(value);
			}
		}

		public float size
		{
			get
			{
				return m_Size;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_Size, Mathf.Clamp01(value)))
				{
					UpdateVisuals();
				}
			}
		}

		public int numberOfSteps
		{
			get
			{
				return m_NumberOfSteps;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_NumberOfSteps, value))
				{
					Set(m_Value);
					UpdateVisuals();
				}
			}
		}

		public ScrollEvent onValueChanged
		{
			get
			{
				return m_OnValueChanged;
			}
			set
			{
				m_OnValueChanged = value;
			}
		}

		private float stepSize => (m_NumberOfSteps <= 1) ? 0.1f : (1f / (float)(m_NumberOfSteps - 1));

		private Axis axis => (m_Direction != 0 && m_Direction != Direction.RightToLeft) ? Axis.Vertical : Axis.Horizontal;

		private bool reverseValue => m_Direction == Direction.RightToLeft || m_Direction == Direction.TopToBottom;

		protected Scrollbar()
		{
		}

		public virtual void Rebuild(CanvasUpdate executing)
		{
		}

		public virtual void LayoutComplete()
		{
		}

		public virtual void GraphicUpdateComplete()
		{
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			UpdateCachedReferences();
			Set(m_Value, sendCallback: false);
			UpdateVisuals();
		}

		protected override void OnDisable()
		{
			m_Tracker.Clear();
			base.OnDisable();
		}

		private void UpdateCachedReferences()
		{
			if ((bool)m_HandleRect && m_HandleRect.parent != null)
			{
				m_ContainerRect = m_HandleRect.parent.GetComponent<RectTransform>();
			}
			else
			{
				m_ContainerRect = null;
			}
		}

		private void Set(float input)
		{
			Set(input, sendCallback: true);
		}

		private void Set(float input, bool sendCallback)
		{
			float value = m_Value;
			m_Value = Mathf.Clamp01(input);
			if (value != this.value)
			{
				UpdateVisuals();
				if (sendCallback)
				{
					m_OnValueChanged.Invoke(this.value);
				}
			}
		}

		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			if (IsActive())
			{
				UpdateVisuals();
			}
		}

		private void UpdateVisuals()
		{
			m_Tracker.Clear();
			if (m_ContainerRect != null)
			{
				m_Tracker.Add(this, m_HandleRect, DrivenTransformProperties.Anchors);
				Vector2 zero = Vector2.zero;
				Vector2 one = Vector2.one;
				float num = value * (1f - size);
				if (reverseValue)
				{
					zero[(int)axis] = 1f - num - size;
					one[(int)axis] = 1f - num;
				}
				else
				{
					zero[(int)axis] = num;
					one[(int)axis] = num + size;
				}
				m_HandleRect.anchorMin = zero;
				m_HandleRect.anchorMax = one;
			}
		}

		private void UpdateDrag(PointerEventData eventData)
		{
			if (eventData.button != 0 || m_ContainerRect == null || !RectTransformUtility.ScreenPointToLocalPointInRectangle(m_ContainerRect, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
			{
				return;
			}
			Vector2 a = localPoint - m_Offset - m_ContainerRect.rect.position;
			Vector2 vector = a - (m_HandleRect.rect.size - m_HandleRect.sizeDelta) * 0.5f;
			float num = (axis != 0) ? m_ContainerRect.rect.height : m_ContainerRect.rect.width;
			float num2 = num * (1f - size);
			if (!(num2 <= 0f))
			{
				switch (m_Direction)
				{
				case Direction.LeftToRight:
					Set(vector.x / num2);
					break;
				case Direction.RightToLeft:
					Set(1f - vector.x / num2);
					break;
				case Direction.BottomToTop:
					Set(vector.y / num2);
					break;
				case Direction.TopToBottom:
					Set(1f - vector.y / num2);
					break;
				}
			}
		}

		private bool MayDrag(PointerEventData eventData)
		{
			return IsActive() && IsInteractable() && eventData.button == PointerEventData.InputButton.Left;
		}

		public virtual void OnBeginDrag(PointerEventData eventData)
		{
			isPointerDownAndNotDragging = false;
			if (MayDrag(eventData) && !(m_ContainerRect == null))
			{
				m_Offset = Vector2.zero;
				if (RectTransformUtility.RectangleContainsScreenPoint(m_HandleRect, eventData.position, eventData.enterEventCamera) && RectTransformUtility.ScreenPointToLocalPointInRectangle(m_HandleRect, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
				{
					m_Offset = localPoint - m_HandleRect.rect.center;
				}
			}
		}

		public virtual void OnDrag(PointerEventData eventData)
		{
			if (MayDrag(eventData) && m_ContainerRect != null)
			{
				UpdateDrag(eventData);
			}
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			if (MayDrag(eventData))
			{
				base.OnPointerDown(eventData);
				isPointerDownAndNotDragging = true;
				m_PointerDownRepeat = StartCoroutine(ClickRepeat(eventData));
			}
		}

		protected IEnumerator ClickRepeat(PointerEventData eventData)
		{
			while (isPointerDownAndNotDragging)
			{
				if (!RectTransformUtility.RectangleContainsScreenPoint(m_HandleRect, eventData.position, eventData.enterEventCamera) && RectTransformUtility.ScreenPointToLocalPointInRectangle(m_HandleRect, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
				{
					float num = (axis != 0) ? localPoint.y : localPoint.x;
					if (num < 0f)
					{
						value -= size;
					}
					else
					{
						value += size;
					}
				}
				yield return new WaitForEndOfFrame();
			}
			StopCoroutine(m_PointerDownRepeat);
		}

		public override void OnPointerUp(PointerEventData eventData)
		{
			base.OnPointerUp(eventData);
			isPointerDownAndNotDragging = false;
		}

		public override void OnMove(AxisEventData eventData)
		{
			if (!IsActive() || !IsInteractable())
			{
				base.OnMove(eventData);
				return;
			}
			switch (eventData.moveDir)
			{
			case MoveDirection.Left:
				if (axis == Axis.Horizontal && FindSelectableOnLeft() == null)
				{
					Set((!reverseValue) ? (value - stepSize) : (value + stepSize));
				}
				else
				{
					base.OnMove(eventData);
				}
				break;
			case MoveDirection.Right:
				if (axis == Axis.Horizontal && FindSelectableOnRight() == null)
				{
					Set((!reverseValue) ? (value + stepSize) : (value - stepSize));
				}
				else
				{
					base.OnMove(eventData);
				}
				break;
			case MoveDirection.Up:
				if (axis == Axis.Vertical && FindSelectableOnUp() == null)
				{
					Set((!reverseValue) ? (value + stepSize) : (value - stepSize));
				}
				else
				{
					base.OnMove(eventData);
				}
				break;
			case MoveDirection.Down:
				if (axis == Axis.Vertical && FindSelectableOnDown() == null)
				{
					Set((!reverseValue) ? (value - stepSize) : (value + stepSize));
				}
				else
				{
					base.OnMove(eventData);
				}
				break;
			}
		}

		public override Selectable FindSelectableOnLeft()
		{
			if (base.navigation.mode == Navigation.Mode.Automatic && axis == Axis.Horizontal)
			{
				return null;
			}
			return base.FindSelectableOnLeft();
		}

		public override Selectable FindSelectableOnRight()
		{
			if (base.navigation.mode == Navigation.Mode.Automatic && axis == Axis.Horizontal)
			{
				return null;
			}
			return base.FindSelectableOnRight();
		}

		public override Selectable FindSelectableOnUp()
		{
			if (base.navigation.mode == Navigation.Mode.Automatic && axis == Axis.Vertical)
			{
				return null;
			}
			return base.FindSelectableOnUp();
		}

		public override Selectable FindSelectableOnDown()
		{
			if (base.navigation.mode == Navigation.Mode.Automatic && axis == Axis.Vertical)
			{
				return null;
			}
			return base.FindSelectableOnDown();
		}

		public virtual void OnInitializePotentialDrag(PointerEventData eventData)
		{
			eventData.useDragThreshold = false;
		}

		public void SetDirection(Direction direction, bool includeRectLayouts)
		{
			Axis axis = this.axis;
			bool reverseValue = this.reverseValue;
			this.direction = direction;
			if (includeRectLayouts)
			{
				if (this.axis != axis)
				{
					RectTransformUtility.FlipLayoutAxes(base.transform as RectTransform, keepPositioning: true, recursive: true);
				}
				if (this.reverseValue != reverseValue)
				{
					RectTransformUtility.FlipLayoutOnAxis(base.transform as RectTransform, (int)this.axis, keepPositioning: true, recursive: true);
				}
			}
		}

		Transform ICanvasElement.get_transform()
		{
			return base.transform;
		}
	}
}
