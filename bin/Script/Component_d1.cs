using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;
using UnityEngine.Internal;
using UnityEngine.Scripting;
using UnityEngineInternal;

namespace UnityEngine
{
	[RequiredByNativeCode]
	public class Component : Object
	{
		public Transform transform
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public GameObject gameObject
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[GeneratedByOldBindingsGenerator]
			get;
		}

		public string tag
		{
			get
			{
				return gameObject.tag;
			}
			set
			{
				gameObject.tag = value;
			}
		}

		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public Component GetComponent(Type type)
		{
			return gameObject.GetComponent(type);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		internal extern void GetComponentFastPath(Type type, IntPtr oneFurtherThanResultValue);

		[SecuritySafeCritical]
		public unsafe T GetComponent<T>()
		{
			CastHelper<T> castHelper = default(CastHelper<T>);
			GetComponentFastPath(typeof(T), new IntPtr(&castHelper.onePointerFurtherThanT));
			return castHelper.t;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern Component GetComponent(string type);

		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public Component GetComponentInChildren(Type t, bool includeInactive)
		{
			return gameObject.GetComponentInChildren(t, includeInactive);
		}

		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public Component GetComponentInChildren(Type t)
		{
			return GetComponentInChildren(t, includeInactive: false);
		}

		[ExcludeFromDocs]
		public T GetComponentInChildren<T>()
		{
			bool includeInactive = false;
			return GetComponentInChildren<T>(includeInactive);
		}

		public T GetComponentInChildren<T>([DefaultValue("false")] bool includeInactive)
		{
			return (T)(object)GetComponentInChildren(typeof(T), includeInactive);
		}

		[ExcludeFromDocs]
		public Component[] GetComponentsInChildren(Type t)
		{
			bool includeInactive = false;
			return GetComponentsInChildren(t, includeInactive);
		}

		public Component[] GetComponentsInChildren(Type t, [DefaultValue("false")] bool includeInactive)
		{
			return gameObject.GetComponentsInChildren(t, includeInactive);
		}

		public T[] GetComponentsInChildren<T>(bool includeInactive)
		{
			return gameObject.GetComponentsInChildren<T>(includeInactive);
		}

		public void GetComponentsInChildren<T>(bool includeInactive, List<T> result)
		{
			gameObject.GetComponentsInChildren(includeInactive, result);
		}

		public T[] GetComponentsInChildren<T>()
		{
			return GetComponentsInChildren<T>(includeInactive: false);
		}

		public void GetComponentsInChildren<T>(List<T> results)
		{
			GetComponentsInChildren(includeInactive: false, results);
		}

		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public Component GetComponentInParent(Type t)
		{
			return gameObject.GetComponentInParent(t);
		}

		public T GetComponentInParent<T>()
		{
			return (T)(object)GetComponentInParent(typeof(T));
		}

		[ExcludeFromDocs]
		public Component[] GetComponentsInParent(Type t)
		{
			bool includeInactive = false;
			return GetComponentsInParent(t, includeInactive);
		}

		public Component[] GetComponentsInParent(Type t, [DefaultValue("false")] bool includeInactive)
		{
			return gameObject.GetComponentsInParent(t, includeInactive);
		}

		public T[] GetComponentsInParent<T>(bool includeInactive)
		{
			return gameObject.GetComponentsInParent<T>(includeInactive);
		}

		public void GetComponentsInParent<T>(bool includeInactive, List<T> results)
		{
			gameObject.GetComponentsInParent(includeInactive, results);
		}

		public T[] GetComponentsInParent<T>()
		{
			return GetComponentsInParent<T>(includeInactive: false);
		}

		public Component[] GetComponents(Type type)
		{
			return gameObject.GetComponents(type);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern void GetComponentsForListInternal(Type searchType, object resultList);

		public void GetComponents(Type type, List<Component> results)
		{
			GetComponentsForListInternal(type, results);
		}

		public void GetComponents<T>(List<T> results)
		{
			GetComponentsForListInternal(typeof(T), results);
		}

		public T[] GetComponents<T>()
		{
			return gameObject.GetComponents<T>();
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern bool CompareTag(string tag);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SendMessageUpwards(string methodName, [DefaultValue("null")] object value, [DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options);

		[ExcludeFromDocs]
		public void SendMessageUpwards(string methodName, object value)
		{
			SendMessageOptions options = SendMessageOptions.RequireReceiver;
			SendMessageUpwards(methodName, value, options);
		}

		[ExcludeFromDocs]
		public void SendMessageUpwards(string methodName)
		{
			SendMessageOptions options = SendMessageOptions.RequireReceiver;
			object value = null;
			SendMessageUpwards(methodName, value, options);
		}

		public void SendMessageUpwards(string methodName, SendMessageOptions options)
		{
			SendMessageUpwards(methodName, null, options);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void SendMessage(string methodName, [DefaultValue("null")] object value, [DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options);

		[ExcludeFromDocs]
		public void SendMessage(string methodName, object value)
		{
			SendMessageOptions options = SendMessageOptions.RequireReceiver;
			SendMessage(methodName, value, options);
		}

		[ExcludeFromDocs]
		public void SendMessage(string methodName)
		{
			SendMessageOptions options = SendMessageOptions.RequireReceiver;
			object value = null;
			SendMessage(methodName, value, options);
		}

		public void SendMessage(string methodName, SendMessageOptions options)
		{
			SendMessage(methodName, null, options);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void BroadcastMessage(string methodName, [DefaultValue("null")] object parameter, [DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options);

		[ExcludeFromDocs]
		public void BroadcastMessage(string methodName, object parameter)
		{
			SendMessageOptions options = SendMessageOptions.RequireReceiver;
			BroadcastMessage(methodName, parameter, options);
		}

		[ExcludeFromDocs]
		public void BroadcastMessage(string methodName)
		{
			SendMessageOptions options = SendMessageOptions.RequireReceiver;
			object parameter = null;
			BroadcastMessage(methodName, parameter, options);
		}

		public void BroadcastMessage(string methodName, SendMessageOptions options)
		{
			BroadcastMessage(methodName, null, options);
		}
	}
}
