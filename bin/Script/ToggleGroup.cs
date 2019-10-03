using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	[AddComponentMenu("UI/Toggle Group", 32)]
	[DisallowMultipleComponent]
	public class ToggleGroup : UIBehaviour
	{
		[SerializeField]
		private bool m_AllowSwitchOff = false;

		private List<Toggle> m_Toggles = new List<Toggle>();

		public bool allowSwitchOff
		{
			get
			{
				return m_AllowSwitchOff;
			}
			set
			{
				m_AllowSwitchOff = value;
			}
		}

		protected ToggleGroup()
		{
		}

		private void ValidateToggleIsInGroup(Toggle toggle)
		{
			if (toggle == null || !m_Toggles.Contains(toggle))
			{
				throw new ArgumentException(string.Format("Toggle {0} is not part of ToggleGroup {1}", new object[2]
				{
					toggle,
					this
				}));
			}
		}

		public void NotifyToggleOn(Toggle toggle)
		{
			ValidateToggleIsInGroup(toggle);
			for (int i = 0; i < m_Toggles.Count; i++)
			{
				if (!(m_Toggles[i] == toggle))
				{
					m_Toggles[i].isOn = false;
				}
			}
		}

		public void UnregisterToggle(Toggle toggle)
		{
			if (m_Toggles.Contains(toggle))
			{
				m_Toggles.Remove(toggle);
			}
		}

		public void RegisterToggle(Toggle toggle)
		{
			if (!m_Toggles.Contains(toggle))
			{
				m_Toggles.Add(toggle);
			}
		}

		public bool AnyTogglesOn()
		{
			return m_Toggles.Find((Toggle x) => x.isOn) != null;
		}

		public IEnumerable<Toggle> ActiveToggles()
		{
			return from x in m_Toggles
				where x.isOn
				select x;
		}

		public void SetAllTogglesOff()
		{
			bool allowSwitchOff = m_AllowSwitchOff;
			m_AllowSwitchOff = true;
			for (int i = 0; i < m_Toggles.Count; i++)
			{
				m_Toggles[i].isOn = false;
			}
			m_AllowSwitchOff = allowSwitchOff;
		}
	}
}
