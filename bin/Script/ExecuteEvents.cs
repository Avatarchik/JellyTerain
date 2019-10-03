using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.UI;

namespace UnityEngine.EventSystems
{
	public static class ExecuteEvents
	{
		public delegate void EventFunction<T1>(T1 handler, BaseEventData eventData);

		private static readonly EventFunction<IPointerEnterHandler> s_PointerEnterHandler = Execute;

		private static readonly EventFunction<IPointerExitHandler> s_PointerExitHandler = Execute;

		private static readonly EventFunction<IPointerDownHandler> s_PointerDownHandler = Execute;

		private static readonly EventFunction<IPointerUpHandler> s_PointerUpHandler = Execute;

		private static readonly EventFunction<IPointerClickHandler> s_PointerClickHandler = Execute;

		private static readonly EventFunction<IInitializePotentialDragHandler> s_InitializePotentialDragHandler = Execute;

		private static readonly EventFunction<IBeginDragHandler> s_BeginDragHandler = Execute;

		private static readonly EventFunction<IDragHandler> s_DragHandler = Execute;

		private static readonly EventFunction<IEndDragHandler> s_EndDragHandler = Execute;

		private static readonly EventFunction<IDropHandler> s_DropHandler = Execute;

		private static readonly EventFunction<IScrollHandler> s_ScrollHandler = Execute;

		private static readonly EventFunction<IUpdateSelectedHandler> s_UpdateSelectedHandler = Execute;

		private static readonly EventFunction<ISelectHandler> s_SelectHandler = Execute;

		private static readonly EventFunction<IDeselectHandler> s_DeselectHandler = Execute;

		private static readonly EventFunction<IMoveHandler> s_MoveHandler = Execute;

		private static readonly EventFunction<ISubmitHandler> s_SubmitHandler = Execute;

		private static readonly EventFunction<ICancelHandler> s_CancelHandler = Execute;

		private static readonly ObjectPool<List<IEventSystemHandler>> s_HandlerListPool = new ObjectPool<List<IEventSystemHandler>>(null, delegate(List<IEventSystemHandler> l)
		{
			l.Clear();
		});

		private static readonly List<Transform> s_InternalTransformList = new List<Transform>(30);

		[CompilerGenerated]
		private static EventFunction<IPointerEnterHandler> _003C_003Ef__mg_0024cache0;

		[CompilerGenerated]
		private static EventFunction<IPointerExitHandler> _003C_003Ef__mg_0024cache1;

		[CompilerGenerated]
		private static EventFunction<IPointerDownHandler> _003C_003Ef__mg_0024cache2;

		[CompilerGenerated]
		private static EventFunction<IPointerUpHandler> _003C_003Ef__mg_0024cache3;

		[CompilerGenerated]
		private static EventFunction<IPointerClickHandler> _003C_003Ef__mg_0024cache4;

		[CompilerGenerated]
		private static EventFunction<IInitializePotentialDragHandler> _003C_003Ef__mg_0024cache5;

		[CompilerGenerated]
		private static EventFunction<IBeginDragHandler> _003C_003Ef__mg_0024cache6;

		[CompilerGenerated]
		private static EventFunction<IDragHandler> _003C_003Ef__mg_0024cache7;

		[CompilerGenerated]
		private static EventFunction<IEndDragHandler> _003C_003Ef__mg_0024cache8;

		[CompilerGenerated]
		private static EventFunction<IDropHandler> _003C_003Ef__mg_0024cache9;

		[CompilerGenerated]
		private static EventFunction<IScrollHandler> _003C_003Ef__mg_0024cacheA;

		[CompilerGenerated]
		private static EventFunction<IUpdateSelectedHandler> _003C_003Ef__mg_0024cacheB;

		[CompilerGenerated]
		private static EventFunction<ISelectHandler> _003C_003Ef__mg_0024cacheC;

		[CompilerGenerated]
		private static EventFunction<IDeselectHandler> _003C_003Ef__mg_0024cacheD;

		[CompilerGenerated]
		private static EventFunction<IMoveHandler> _003C_003Ef__mg_0024cacheE;

		[CompilerGenerated]
		private static EventFunction<ISubmitHandler> _003C_003Ef__mg_0024cacheF;

		[CompilerGenerated]
		private static EventFunction<ICancelHandler> _003C_003Ef__mg_0024cache10;

		public static EventFunction<IPointerEnterHandler> pointerEnterHandler => s_PointerEnterHandler;

		public static EventFunction<IPointerExitHandler> pointerExitHandler => s_PointerExitHandler;

		public static EventFunction<IPointerDownHandler> pointerDownHandler => s_PointerDownHandler;

		public static EventFunction<IPointerUpHandler> pointerUpHandler => s_PointerUpHandler;

		public static EventFunction<IPointerClickHandler> pointerClickHandler => s_PointerClickHandler;

		public static EventFunction<IInitializePotentialDragHandler> initializePotentialDrag => s_InitializePotentialDragHandler;

		public static EventFunction<IBeginDragHandler> beginDragHandler => s_BeginDragHandler;

		public static EventFunction<IDragHandler> dragHandler => s_DragHandler;

		public static EventFunction<IEndDragHandler> endDragHandler => s_EndDragHandler;

		public static EventFunction<IDropHandler> dropHandler => s_DropHandler;

		public static EventFunction<IScrollHandler> scrollHandler => s_ScrollHandler;

		public static EventFunction<IUpdateSelectedHandler> updateSelectedHandler => s_UpdateSelectedHandler;

		public static EventFunction<ISelectHandler> selectHandler => s_SelectHandler;

		public static EventFunction<IDeselectHandler> deselectHandler => s_DeselectHandler;

		public static EventFunction<IMoveHandler> moveHandler => s_MoveHandler;

		public static EventFunction<ISubmitHandler> submitHandler => s_SubmitHandler;

		public static EventFunction<ICancelHandler> cancelHandler => s_CancelHandler;

		public static T ValidateEventData<T>(BaseEventData data) where T : class
		{
			if (data as T == null)
			{
				throw new ArgumentException($"Invalid type: {data.GetType()} passed to event expecting {typeof(T)}");
			}
			return data as T;
		}

		private static void Execute(IPointerEnterHandler handler, BaseEventData eventData)
		{
			handler.OnPointerEnter(ValidateEventData<PointerEventData>(eventData));
		}

		private static void Execute(IPointerExitHandler handler, BaseEventData eventData)
		{
			handler.OnPointerExit(ValidateEventData<PointerEventData>(eventData));
		}

		private static void Execute(IPointerDownHandler handler, BaseEventData eventData)
		{
			handler.OnPointerDown(ValidateEventData<PointerEventData>(eventData));
		}

		private static void Execute(IPointerUpHandler handler, BaseEventData eventData)
		{
			handler.OnPointerUp(ValidateEventData<PointerEventData>(eventData));
		}

		private static void Execute(IPointerClickHandler handler, BaseEventData eventData)
		{
			handler.OnPointerClick(ValidateEventData<PointerEventData>(eventData));
		}

		private static void Execute(IInitializePotentialDragHandler handler, BaseEventData eventData)
		{
			handler.OnInitializePotentialDrag(ValidateEventData<PointerEventData>(eventData));
		}

		private static void Execute(IBeginDragHandler handler, BaseEventData eventData)
		{
			handler.OnBeginDrag(ValidateEventData<PointerEventData>(eventData));
		}

		private static void Execute(IDragHandler handler, BaseEventData eventData)
		{
			handler.OnDrag(ValidateEventData<PointerEventData>(eventData));
		}

		private static void Execute(IEndDragHandler handler, BaseEventData eventData)
		{
			handler.OnEndDrag(ValidateEventData<PointerEventData>(eventData));
		}

		private static void Execute(IDropHandler handler, BaseEventData eventData)
		{
			handler.OnDrop(ValidateEventData<PointerEventData>(eventData));
		}

		private static void Execute(IScrollHandler handler, BaseEventData eventData)
		{
			handler.OnScroll(ValidateEventData<PointerEventData>(eventData));
		}

		private static void Execute(IUpdateSelectedHandler handler, BaseEventData eventData)
		{
			handler.OnUpdateSelected(eventData);
		}

		private static void Execute(ISelectHandler handler, BaseEventData eventData)
		{
			handler.OnSelect(eventData);
		}

		private static void Execute(IDeselectHandler handler, BaseEventData eventData)
		{
			handler.OnDeselect(eventData);
		}

		private static void Execute(IMoveHandler handler, BaseEventData eventData)
		{
			handler.OnMove(ValidateEventData<AxisEventData>(eventData));
		}

		private static void Execute(ISubmitHandler handler, BaseEventData eventData)
		{
			handler.OnSubmit(eventData);
		}

		private static void Execute(ICancelHandler handler, BaseEventData eventData)
		{
			handler.OnCancel(eventData);
		}

		private static void GetEventChain(GameObject root, IList<Transform> eventChain)
		{
			eventChain.Clear();
			if (!(root == null))
			{
				Transform transform = root.transform;
				while (transform != null)
				{
					eventChain.Add(transform);
					transform = transform.parent;
				}
			}
		}

		public static bool Execute<T>(GameObject target, BaseEventData eventData, EventFunction<T> functor) where T : IEventSystemHandler
		{
			List<IEventSystemHandler> list = s_HandlerListPool.Get();
			GetEventList<T>(target, list);
			for (int i = 0; i < list.Count; i++)
			{
				T handler;
				try
				{
					handler = (T)list[i];
				}
				catch (Exception innerException)
				{
					IEventSystemHandler eventSystemHandler = list[i];
					Debug.LogException(new Exception($"Type {typeof(T).Name} expected {eventSystemHandler.GetType().Name} received.", innerException));
					continue;
				}
				try
				{
					functor(handler, eventData);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
			int count = list.Count;
			s_HandlerListPool.Release(list);
			return count > 0;
		}

		public static GameObject ExecuteHierarchy<T>(GameObject root, BaseEventData eventData, EventFunction<T> callbackFunction) where T : IEventSystemHandler
		{
			GetEventChain(root, s_InternalTransformList);
			for (int i = 0; i < s_InternalTransformList.Count; i++)
			{
				Transform transform = s_InternalTransformList[i];
				if (Execute(transform.gameObject, eventData, callbackFunction))
				{
					return transform.gameObject;
				}
			}
			return null;
		}

		private static bool ShouldSendToComponent<T>(Component component) where T : IEventSystemHandler
		{
			if (!(component is T))
			{
				return false;
			}
			Behaviour behaviour = component as Behaviour;
			if (behaviour != null)
			{
				return behaviour.isActiveAndEnabled;
			}
			return true;
		}

		private static void GetEventList<T>(GameObject go, IList<IEventSystemHandler> results) where T : IEventSystemHandler
		{
			if (results == null)
			{
				throw new ArgumentException("Results array is null", "results");
			}
			if (go == null || !go.activeInHierarchy)
			{
				return;
			}
			List<Component> list = ListPool<Component>.Get();
			go.GetComponents(list);
			for (int i = 0; i < list.Count; i++)
			{
				if (ShouldSendToComponent<T>(list[i]))
				{
					results.Add(list[i] as IEventSystemHandler);
				}
			}
			ListPool<Component>.Release(list);
		}

		public static bool CanHandleEvent<T>(GameObject go) where T : IEventSystemHandler
		{
			List<IEventSystemHandler> list = s_HandlerListPool.Get();
			GetEventList<T>(go, list);
			int count = list.Count;
			s_HandlerListPool.Release(list);
			return count != 0;
		}

		public static GameObject GetEventHandler<T>(GameObject root) where T : IEventSystemHandler
		{
			if (root == null)
			{
				return null;
			}
			Transform transform = root.transform;
			while (transform != null)
			{
				if (CanHandleEvent<T>(transform.gameObject))
				{
					return transform.gameObject;
				}
				transform = transform.parent;
			}
			return null;
		}
	}
}
