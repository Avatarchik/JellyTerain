using System;
using System.Collections.Generic;

namespace UnityEngine.EventSystems
{
	public abstract class BaseRaycaster : UIBehaviour
	{
		public abstract Camera eventCamera
		{
			get;
		}

		[Obsolete("Please use sortOrderPriority and renderOrderPriority", false)]
		public virtual int priority => 0;

		public virtual int sortOrderPriority => int.MinValue;

		public virtual int renderOrderPriority => int.MinValue;

		public abstract void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList);

		public override string ToString()
		{
			return "Name: " + base.gameObject + "\neventCamera: " + eventCamera + "\nsortOrderPriority: " + sortOrderPriority + "\nrenderOrderPriority: " + renderOrderPriority;
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			RaycasterManager.AddRaycaster(this);
		}

		protected override void OnDisable()
		{
			RaycasterManager.RemoveRaycasters(this);
			base.OnDisable();
		}
	}
}
