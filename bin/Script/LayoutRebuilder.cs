using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Events;

namespace UnityEngine.UI
{
	public class LayoutRebuilder : ICanvasElement
	{
		private RectTransform m_ToRebuild;

		private int m_CachedHashFromTransform;

		private static ObjectPool<LayoutRebuilder> s_Rebuilders;

		[CompilerGenerated]
		private static RectTransform.ReapplyDrivenProperties _003C_003Ef__mg_0024cache0;

		public Transform transform => m_ToRebuild;

		static LayoutRebuilder()
		{
			s_Rebuilders = new ObjectPool<LayoutRebuilder>(null, delegate(LayoutRebuilder x)
			{
				x.Clear();
			});
			RectTransform.reapplyDrivenProperties += ReapplyDrivenProperties;
		}

		private void Initialize(RectTransform controller)
		{
			m_ToRebuild = controller;
			m_CachedHashFromTransform = controller.GetHashCode();
		}

		private void Clear()
		{
			m_ToRebuild = null;
			m_CachedHashFromTransform = 0;
		}

		private static void ReapplyDrivenProperties(RectTransform driven)
		{
			MarkLayoutForRebuild(driven);
		}

		public bool IsDestroyed()
		{
			return m_ToRebuild == null;
		}

		private static void StripDisabledBehavioursFromList(List<Component> components)
		{
			components.RemoveAll((Component e) => e is Behaviour && !((Behaviour)e).isActiveAndEnabled);
		}

		public static void ForceRebuildLayoutImmediate(RectTransform layoutRoot)
		{
			LayoutRebuilder layoutRebuilder = s_Rebuilders.Get();
			layoutRebuilder.Initialize(layoutRoot);
			layoutRebuilder.Rebuild(CanvasUpdate.Layout);
			s_Rebuilders.Release(layoutRebuilder);
		}

		public void Rebuild(CanvasUpdate executing)
		{
			if (executing == CanvasUpdate.Layout)
			{
				PerformLayoutCalculation(m_ToRebuild, delegate(Component e)
				{
					(e as ILayoutElement).CalculateLayoutInputHorizontal();
				});
				PerformLayoutControl(m_ToRebuild, delegate(Component e)
				{
					(e as ILayoutController).SetLayoutHorizontal();
				});
				PerformLayoutCalculation(m_ToRebuild, delegate(Component e)
				{
					(e as ILayoutElement).CalculateLayoutInputVertical();
				});
				PerformLayoutControl(m_ToRebuild, delegate(Component e)
				{
					(e as ILayoutController).SetLayoutVertical();
				});
			}
		}

		private void PerformLayoutControl(RectTransform rect, UnityAction<Component> action)
		{
			if (rect == null)
			{
				return;
			}
			List<Component> list = ListPool<Component>.Get();
			rect.GetComponents(typeof(ILayoutController), list);
			StripDisabledBehavioursFromList(list);
			if (list.Count > 0)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] is ILayoutSelfController)
					{
						action(list[i]);
					}
				}
				for (int j = 0; j < list.Count; j++)
				{
					if (!(list[j] is ILayoutSelfController))
					{
						action(list[j]);
					}
				}
				for (int k = 0; k < rect.childCount; k++)
				{
					PerformLayoutControl(rect.GetChild(k) as RectTransform, action);
				}
			}
			ListPool<Component>.Release(list);
		}

		private void PerformLayoutCalculation(RectTransform rect, UnityAction<Component> action)
		{
			if (rect == null)
			{
				return;
			}
			List<Component> list = ListPool<Component>.Get();
			rect.GetComponents(typeof(ILayoutElement), list);
			StripDisabledBehavioursFromList(list);
			if (list.Count > 0 || (bool)rect.GetComponent(typeof(ILayoutGroup)))
			{
				for (int i = 0; i < rect.childCount; i++)
				{
					PerformLayoutCalculation(rect.GetChild(i) as RectTransform, action);
				}
				for (int j = 0; j < list.Count; j++)
				{
					action(list[j]);
				}
			}
			ListPool<Component>.Release(list);
		}

		public static void MarkLayoutForRebuild(RectTransform rect)
		{
			if (rect == null)
			{
				return;
			}
			List<Component> list = ListPool<Component>.Get();
			RectTransform rectTransform = rect;
			while (true)
			{
				RectTransform rectTransform2 = rectTransform.parent as RectTransform;
				if (!ValidLayoutGroup(rectTransform2, list))
				{
					break;
				}
				rectTransform = rectTransform2;
			}
			if (rectTransform == rect && !ValidController(rectTransform, list))
			{
				ListPool<Component>.Release(list);
				return;
			}
			MarkLayoutRootForRebuild(rectTransform);
			ListPool<Component>.Release(list);
		}

		private static bool ValidLayoutGroup(RectTransform parent, List<Component> comps)
		{
			if (parent == null)
			{
				return false;
			}
			parent.GetComponents(typeof(ILayoutGroup), comps);
			StripDisabledBehavioursFromList(comps);
			return comps.Count > 0;
		}

		private static bool ValidController(RectTransform layoutRoot, List<Component> comps)
		{
			if (layoutRoot == null)
			{
				return false;
			}
			layoutRoot.GetComponents(typeof(ILayoutController), comps);
			StripDisabledBehavioursFromList(comps);
			return comps.Count > 0;
		}

		private static void MarkLayoutRootForRebuild(RectTransform controller)
		{
			if (!(controller == null))
			{
				LayoutRebuilder layoutRebuilder = s_Rebuilders.Get();
				layoutRebuilder.Initialize(controller);
				if (!CanvasUpdateRegistry.TryRegisterCanvasElementForLayoutRebuild(layoutRebuilder))
				{
					s_Rebuilders.Release(layoutRebuilder);
				}
			}
		}

		public void LayoutComplete()
		{
			s_Rebuilders.Release(this);
		}

		public void GraphicUpdateComplete()
		{
		}

		public override int GetHashCode()
		{
			return m_CachedHashFromTransform;
		}

		public override bool Equals(object obj)
		{
			return obj.GetHashCode() == GetHashCode();
		}

		public override string ToString()
		{
			return "(Layout Rebuilder for) " + m_ToRebuild;
		}
	}
}
