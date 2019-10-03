using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine.Serialization;

namespace UnityEngine.EventSystems
{
	[AddComponentMenu("Event/Event System")]
	public class EventSystem : UIBehaviour
	{
		private List<BaseInputModule> m_SystemInputModules = new List<BaseInputModule>();

		private BaseInputModule m_CurrentInputModule;

		[SerializeField]
		[FormerlySerializedAs("m_Selected")]
		private GameObject m_FirstSelected;

		[SerializeField]
		private bool m_sendNavigationEvents = true;

		[SerializeField]
		private int m_DragThreshold = 5;

		private GameObject m_CurrentSelected;

		private bool m_HasFocus = false;

		private bool m_SelectionGuard;

		private BaseEventData m_DummyData;

		private static readonly Comparison<RaycastResult> s_RaycastComparer = RaycastComparer;

		[CompilerGenerated]
		private static Comparison<RaycastResult> _003C_003Ef__mg_0024cache0;

		public static EventSystem current
		{
			get;
			set;
		}

		public bool sendNavigationEvents
		{
			get
			{
				return m_sendNavigationEvents;
			}
			set
			{
				m_sendNavigationEvents = value;
			}
		}

		public int pixelDragThreshold
		{
			get
			{
				return m_DragThreshold;
			}
			set
			{
				m_DragThreshold = value;
			}
		}

		public BaseInputModule currentInputModule => m_CurrentInputModule;

		public GameObject firstSelectedGameObject
		{
			get
			{
				return m_FirstSelected;
			}
			set
			{
				m_FirstSelected = value;
			}
		}

		public GameObject currentSelectedGameObject => m_CurrentSelected;

		[Obsolete("lastSelectedGameObject is no longer supported")]
		public GameObject lastSelectedGameObject => null;

		public bool isFocused => m_HasFocus;

		public bool alreadySelecting => m_SelectionGuard;

		private BaseEventData baseEventDataCache
		{
			get
			{
				if (m_DummyData == null)
				{
					m_DummyData = new BaseEventData(this);
				}
				return m_DummyData;
			}
		}

		protected EventSystem()
		{
		}

		public void UpdateModules()
		{
			GetComponents(m_SystemInputModules);
			for (int num = m_SystemInputModules.Count - 1; num >= 0; num--)
			{
				if (!m_SystemInputModules[num] || !m_SystemInputModules[num].IsActive())
				{
					m_SystemInputModules.RemoveAt(num);
				}
			}
		}

		public void SetSelectedGameObject(GameObject selected, BaseEventData pointer)
		{
			if (m_SelectionGuard)
			{
				Debug.LogError("Attempting to select " + selected + "while already selecting an object.");
				return;
			}
			m_SelectionGuard = true;
			if (selected == m_CurrentSelected)
			{
				m_SelectionGuard = false;
				return;
			}
			ExecuteEvents.Execute(m_CurrentSelected, pointer, ExecuteEvents.deselectHandler);
			m_CurrentSelected = selected;
			ExecuteEvents.Execute(m_CurrentSelected, pointer, ExecuteEvents.selectHandler);
			m_SelectionGuard = false;
		}

		public void SetSelectedGameObject(GameObject selected)
		{
			SetSelectedGameObject(selected, baseEventDataCache);
		}

		private static int RaycastComparer(RaycastResult lhs, RaycastResult rhs)
		{
			if (lhs.module != rhs.module)
			{
				if (lhs.module.eventCamera != null && rhs.module.eventCamera != null && lhs.module.eventCamera.depth != rhs.module.eventCamera.depth)
				{
					if (lhs.module.eventCamera.depth < rhs.module.eventCamera.depth)
					{
						return 1;
					}
					if (lhs.module.eventCamera.depth == rhs.module.eventCamera.depth)
					{
						return 0;
					}
					return -1;
				}
				if (lhs.module.sortOrderPriority != rhs.module.sortOrderPriority)
				{
					return rhs.module.sortOrderPriority.CompareTo(lhs.module.sortOrderPriority);
				}
				if (lhs.module.renderOrderPriority != rhs.module.renderOrderPriority)
				{
					return rhs.module.renderOrderPriority.CompareTo(lhs.module.renderOrderPriority);
				}
			}
			if (lhs.sortingLayer != rhs.sortingLayer)
			{
				int layerValueFromID = SortingLayer.GetLayerValueFromID(rhs.sortingLayer);
				int layerValueFromID2 = SortingLayer.GetLayerValueFromID(lhs.sortingLayer);
				return layerValueFromID.CompareTo(layerValueFromID2);
			}
			if (lhs.sortingOrder != rhs.sortingOrder)
			{
				return rhs.sortingOrder.CompareTo(lhs.sortingOrder);
			}
			if (lhs.depth != rhs.depth)
			{
				return rhs.depth.CompareTo(lhs.depth);
			}
			if (lhs.distance != rhs.distance)
			{
				return lhs.distance.CompareTo(rhs.distance);
			}
			return lhs.index.CompareTo(rhs.index);
		}

		public void RaycastAll(PointerEventData eventData, List<RaycastResult> raycastResults)
		{
			raycastResults.Clear();
			List<BaseRaycaster> raycasters = RaycasterManager.GetRaycasters();
			for (int i = 0; i < raycasters.Count; i++)
			{
				BaseRaycaster baseRaycaster = raycasters[i];
				if (!(baseRaycaster == null) && baseRaycaster.IsActive())
				{
					baseRaycaster.Raycast(eventData, raycastResults);
				}
			}
			raycastResults.Sort(s_RaycastComparer);
		}

		public bool IsPointerOverGameObject()
		{
			return IsPointerOverGameObject(-1);
		}

		public bool IsPointerOverGameObject(int pointerId)
		{
			if (m_CurrentInputModule == null)
			{
				return false;
			}
			return m_CurrentInputModule.IsPointerOverGameObject(pointerId);
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (current == null)
			{
				current = this;
			}
		}

		protected override void OnDisable()
		{
			if (m_CurrentInputModule != null)
			{
				m_CurrentInputModule.DeactivateModule();
				m_CurrentInputModule = null;
			}
			if (current == this)
			{
				current = null;
			}
			base.OnDisable();
		}

		private void TickModules()
		{
			for (int i = 0; i < m_SystemInputModules.Count; i++)
			{
				if (m_SystemInputModules[i] != null)
				{
					m_SystemInputModules[i].UpdateModule();
				}
			}
		}

		protected virtual void OnApplicationFocus(bool hasFocus)
		{
			m_HasFocus = !hasFocus;
		}

		protected virtual void Update()
		{
			if (current != this)
			{
				return;
			}
			TickModules();
			bool flag = false;
			for (int i = 0; i < m_SystemInputModules.Count; i++)
			{
				BaseInputModule baseInputModule = m_SystemInputModules[i];
				if (baseInputModule.IsModuleSupported() && baseInputModule.ShouldActivateModule())
				{
					if (m_CurrentInputModule != baseInputModule)
					{
						ChangeEventModule(baseInputModule);
						flag = true;
					}
					break;
				}
			}
			if (m_CurrentInputModule == null)
			{
				for (int j = 0; j < m_SystemInputModules.Count; j++)
				{
					BaseInputModule baseInputModule2 = m_SystemInputModules[j];
					if (baseInputModule2.IsModuleSupported())
					{
						ChangeEventModule(baseInputModule2);
						flag = true;
						break;
					}
				}
			}
			if (!flag && m_CurrentInputModule != null)
			{
				m_CurrentInputModule.Process();
			}
		}

		private void ChangeEventModule(BaseInputModule module)
		{
			if (!(m_CurrentInputModule == module))
			{
				if (m_CurrentInputModule != null)
				{
					m_CurrentInputModule.DeactivateModule();
				}
				if (module != null)
				{
					module.ActivateModule();
				}
				m_CurrentInputModule = module;
			}
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<b>Selected:</b>" + currentSelectedGameObject);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.AppendLine((!(m_CurrentInputModule != null)) ? "No module" : m_CurrentInputModule.ToString());
			return stringBuilder.ToString();
		}
	}
}
