using System;
using System.Reflection;
using UnityEngine.Scripting;
using UnityEngine.Serialization;

namespace UnityEngine.Events
{
	[Serializable]
	[UsedByNativeCode]
	public abstract class UnityEventBase : ISerializationCallbackReceiver
	{
		private InvokableCallList m_Calls;

		[FormerlySerializedAs("m_PersistentListeners")]
		[SerializeField]
		private PersistentCallGroup m_PersistentCalls;

		[SerializeField]
		private string m_TypeName;

		private bool m_CallsDirty = true;

		protected UnityEventBase()
		{
			m_Calls = new InvokableCallList();
			m_PersistentCalls = new PersistentCallGroup();
			m_TypeName = GetType().AssemblyQualifiedName;
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			DirtyPersistentCalls();
			m_TypeName = GetType().AssemblyQualifiedName;
		}

		protected abstract MethodInfo FindMethod_Impl(string name, object targetObj);

		internal abstract BaseInvokableCall GetDelegate(object target, MethodInfo theFunction);

		internal MethodInfo FindMethod(PersistentCall call)
		{
			Type argumentType = typeof(Object);
			if (!string.IsNullOrEmpty(call.arguments.unityObjectArgumentAssemblyTypeName))
			{
				argumentType = (Type.GetType(call.arguments.unityObjectArgumentAssemblyTypeName, throwOnError: false) ?? typeof(Object));
			}
			return FindMethod(call.methodName, call.target, call.mode, argumentType);
		}

		internal MethodInfo FindMethod(string name, object listener, PersistentListenerMode mode, Type argumentType)
		{
			switch (mode)
			{
			case PersistentListenerMode.EventDefined:
				return FindMethod_Impl(name, listener);
			case PersistentListenerMode.Void:
				return GetValidMethodInfo(listener, name, new Type[0]);
			case PersistentListenerMode.Float:
				return GetValidMethodInfo(listener, name, new Type[1]
				{
					typeof(float)
				});
			case PersistentListenerMode.Int:
				return GetValidMethodInfo(listener, name, new Type[1]
				{
					typeof(int)
				});
			case PersistentListenerMode.Bool:
				return GetValidMethodInfo(listener, name, new Type[1]
				{
					typeof(bool)
				});
			case PersistentListenerMode.String:
				return GetValidMethodInfo(listener, name, new Type[1]
				{
					typeof(string)
				});
			case PersistentListenerMode.Object:
				return GetValidMethodInfo(listener, name, new Type[1]
				{
					argumentType ?? typeof(Object)
				});
			default:
				return null;
			}
		}

		public int GetPersistentEventCount()
		{
			return m_PersistentCalls.Count;
		}

		public Object GetPersistentTarget(int index)
		{
			return m_PersistentCalls.GetListener(index)?.target;
		}

		public string GetPersistentMethodName(int index)
		{
			PersistentCall listener = m_PersistentCalls.GetListener(index);
			return (listener == null) ? string.Empty : listener.methodName;
		}

		private void DirtyPersistentCalls()
		{
			m_Calls.ClearPersistent();
			m_CallsDirty = true;
		}

		private void RebuildPersistentCallsIfNeeded()
		{
			if (m_CallsDirty)
			{
				m_PersistentCalls.Initialize(m_Calls, this);
				m_CallsDirty = false;
			}
		}

		public void SetPersistentListenerState(int index, UnityEventCallState state)
		{
			PersistentCall listener = m_PersistentCalls.GetListener(index);
			if (listener != null)
			{
				listener.callState = state;
			}
			DirtyPersistentCalls();
		}

		protected void AddListener(object targetObj, MethodInfo method)
		{
			m_Calls.AddListener(GetDelegate(targetObj, method));
		}

		internal void AddCall(BaseInvokableCall call)
		{
			m_Calls.AddListener(call);
		}

		protected void RemoveListener(object targetObj, MethodInfo method)
		{
			m_Calls.RemoveListener(targetObj, method);
		}

		public void RemoveAllListeners()
		{
			m_Calls.Clear();
		}

		protected void Invoke(object[] parameters)
		{
			RebuildPersistentCallsIfNeeded();
			m_Calls.Invoke(parameters);
		}

		public override string ToString()
		{
			return base.ToString() + " " + GetType().FullName;
		}

		public static MethodInfo GetValidMethodInfo(object obj, string functionName, Type[] argumentTypes)
		{
			Type type = obj.GetType();
			while (type != typeof(object) && type != null)
			{
				MethodInfo method = type.GetMethod(functionName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, argumentTypes, null);
				if (method != null)
				{
					ParameterInfo[] parameters = method.GetParameters();
					bool flag = true;
					int num = 0;
					ParameterInfo[] array = parameters;
					foreach (ParameterInfo parameterInfo in array)
					{
						Type type2 = argumentTypes[num];
						Type parameterType = parameterInfo.ParameterType;
						flag = (type2.IsPrimitive == parameterType.IsPrimitive);
						if (!flag)
						{
							break;
						}
						num++;
					}
					if (flag)
					{
						return method;
					}
				}
				type = type.BaseType;
			}
			return null;
		}
	}
}
