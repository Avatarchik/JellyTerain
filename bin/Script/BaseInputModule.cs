using System;
using System.Collections.Generic;

namespace UnityEngine.EventSystems
{
	[RequireComponent(typeof(EventSystem))]
	public abstract class BaseInputModule : UIBehaviour
	{
		[NonSerialized]
		protected List<RaycastResult> m_RaycastResultCache = new List<RaycastResult>();

		private AxisEventData m_AxisEventData;

		private EventSystem m_EventSystem;

		private BaseEventData m_BaseEventData;

		protected BaseInput m_InputOverride;

		private BaseInput m_DefaultInput;

		public BaseInput input
		{
			get
			{
				if (m_InputOverride != null)
				{
					return m_InputOverride;
				}
				if (m_DefaultInput == null)
				{
					BaseInput[] components = GetComponents<BaseInput>();
					BaseInput[] array = components;
					foreach (BaseInput baseInput in array)
					{
						if (baseInput != null && baseInput.GetType() == typeof(BaseInput))
						{
							m_DefaultInput = baseInput;
							break;
						}
					}
					if (m_DefaultInput == null)
					{
						m_DefaultInput = base.gameObject.AddComponent<BaseInput>();
					}
				}
				return m_DefaultInput;
			}
		}

		protected EventSystem eventSystem => m_EventSystem;

		protected override void OnEnable()
		{
			base.OnEnable();
			m_EventSystem = GetComponent<EventSystem>();
			m_EventSystem.UpdateModules();
		}

		protected override void OnDisable()
		{
			m_EventSystem.UpdateModules();
			base.OnDisable();
		}

		public abstract void Process();

		protected static RaycastResult FindFirstRaycast(List<RaycastResult> candidates)
		{
			for (int i = 0; i < candidates.Count; i++)
			{
				if (!(candidates[i].gameObject == null))
				{
					return candidates[i];
				}
			}
			return default(RaycastResult);
		}

		protected static MoveDirection DetermineMoveDirection(float x, float y)
		{
			return DetermineMoveDirection(x, y, 0.6f);
		}

		protected static MoveDirection DetermineMoveDirection(float x, float y, float deadZone)
		{
			if (new Vector2(x, y).sqrMagnitude < deadZone * deadZone)
			{
				return MoveDirection.None;
			}
			if (Mathf.Abs(x) > Mathf.Abs(y))
			{
				if (x > 0f)
				{
					return MoveDirection.Right;
				}
				return MoveDirection.Left;
			}
			if (y > 0f)
			{
				return MoveDirection.Up;
			}
			return MoveDirection.Down;
		}

		protected static GameObject FindCommonRoot(GameObject g1, GameObject g2)
		{
			if (g1 == null || g2 == null)
			{
				return null;
			}
			Transform transform = g1.transform;
			while (transform != null)
			{
				Transform transform2 = g2.transform;
				while (transform2 != null)
				{
					if (transform == transform2)
					{
						return transform.gameObject;
					}
					transform2 = transform2.parent;
				}
				transform = transform.parent;
			}
			return null;
		}

		protected void HandlePointerExitAndEnter(PointerEventData currentPointerData, GameObject newEnterTarget)
		{
			if (newEnterTarget == null || currentPointerData.pointerEnter == null)
			{
				for (int i = 0; i < currentPointerData.hovered.Count; i++)
				{
					ExecuteEvents.Execute(currentPointerData.hovered[i], currentPointerData, ExecuteEvents.pointerExitHandler);
				}
				currentPointerData.hovered.Clear();
				if (newEnterTarget == null)
				{
					currentPointerData.pointerEnter = newEnterTarget;
					return;
				}
			}
			if (currentPointerData.pointerEnter == newEnterTarget && (bool)newEnterTarget)
			{
				return;
			}
			GameObject gameObject = FindCommonRoot(currentPointerData.pointerEnter, newEnterTarget);
			if (currentPointerData.pointerEnter != null)
			{
				Transform transform = currentPointerData.pointerEnter.transform;
				while (transform != null && (!(gameObject != null) || !(gameObject.transform == transform)))
				{
					ExecuteEvents.Execute(transform.gameObject, currentPointerData, ExecuteEvents.pointerExitHandler);
					currentPointerData.hovered.Remove(transform.gameObject);
					transform = transform.parent;
				}
			}
			currentPointerData.pointerEnter = newEnterTarget;
			if (newEnterTarget != null)
			{
				Transform transform2 = newEnterTarget.transform;
				while (transform2 != null && transform2.gameObject != gameObject)
				{
					ExecuteEvents.Execute(transform2.gameObject, currentPointerData, ExecuteEvents.pointerEnterHandler);
					currentPointerData.hovered.Add(transform2.gameObject);
					transform2 = transform2.parent;
				}
			}
		}

		protected virtual AxisEventData GetAxisEventData(float x, float y, float moveDeadZone)
		{
			if (m_AxisEventData == null)
			{
				m_AxisEventData = new AxisEventData(eventSystem);
			}
			m_AxisEventData.Reset();
			m_AxisEventData.moveVector = new Vector2(x, y);
			m_AxisEventData.moveDir = DetermineMoveDirection(x, y, moveDeadZone);
			return m_AxisEventData;
		}

		protected virtual BaseEventData GetBaseEventData()
		{
			if (m_BaseEventData == null)
			{
				m_BaseEventData = new BaseEventData(eventSystem);
			}
			m_BaseEventData.Reset();
			return m_BaseEventData;
		}

		public virtual bool IsPointerOverGameObject(int pointerId)
		{
			return false;
		}

		public virtual bool ShouldActivateModule()
		{
			return base.enabled && base.gameObject.activeInHierarchy;
		}

		public virtual void DeactivateModule()
		{
		}

		public virtual void ActivateModule()
		{
		}

		public virtual void UpdateModule()
		{
		}

		public virtual bool IsModuleSupported()
		{
			return true;
		}
	}
}
