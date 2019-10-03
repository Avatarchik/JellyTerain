using System;
using System.Reflection;
using UnityEngine.Serialization;

namespace UnityEngine.Events
{
	[Serializable]
	internal class PersistentCall
	{
		[FormerlySerializedAs("instance")]
		[SerializeField]
		private Object m_Target;

		[FormerlySerializedAs("methodName")]
		[SerializeField]
		private string m_MethodName;

		[FormerlySerializedAs("mode")]
		[SerializeField]
		private PersistentListenerMode m_Mode = PersistentListenerMode.EventDefined;

		[FormerlySerializedAs("arguments")]
		[SerializeField]
		private ArgumentCache m_Arguments = new ArgumentCache();

		[FormerlySerializedAs("enabled")]
		[FormerlySerializedAs("m_Enabled")]
		[SerializeField]
		private UnityEventCallState m_CallState = UnityEventCallState.RuntimeOnly;

		public Object target => m_Target;

		public string methodName => m_MethodName;

		public PersistentListenerMode mode
		{
			get
			{
				return m_Mode;
			}
			set
			{
				m_Mode = value;
			}
		}

		public ArgumentCache arguments => m_Arguments;

		public UnityEventCallState callState
		{
			get
			{
				return m_CallState;
			}
			set
			{
				m_CallState = value;
			}
		}

		public bool IsValid()
		{
			return target != null && !string.IsNullOrEmpty(methodName);
		}

		public BaseInvokableCall GetRuntimeCall(UnityEventBase theEvent)
		{
			if (m_CallState == UnityEventCallState.Off || theEvent == null)
			{
				return null;
			}
			MethodInfo methodInfo = theEvent.FindMethod(this);
			if (methodInfo == null)
			{
				return null;
			}
			switch (m_Mode)
			{
			case PersistentListenerMode.EventDefined:
				return theEvent.GetDelegate(target, methodInfo);
			case PersistentListenerMode.Object:
				return GetObjectCall(target, methodInfo, m_Arguments);
			case PersistentListenerMode.Float:
				return new CachedInvokableCall<float>(target, methodInfo, m_Arguments.floatArgument);
			case PersistentListenerMode.Int:
				return new CachedInvokableCall<int>(target, methodInfo, m_Arguments.intArgument);
			case PersistentListenerMode.String:
				return new CachedInvokableCall<string>(target, methodInfo, m_Arguments.stringArgument);
			case PersistentListenerMode.Bool:
				return new CachedInvokableCall<bool>(target, methodInfo, m_Arguments.boolArgument);
			case PersistentListenerMode.Void:
				return new InvokableCall(target, methodInfo);
			default:
				return null;
			}
		}

		private static BaseInvokableCall GetObjectCall(Object target, MethodInfo method, ArgumentCache arguments)
		{
			Type type = typeof(Object);
			if (!string.IsNullOrEmpty(arguments.unityObjectArgumentAssemblyTypeName))
			{
				type = (Type.GetType(arguments.unityObjectArgumentAssemblyTypeName, throwOnError: false) ?? typeof(Object));
			}
			Type typeFromHandle = typeof(CachedInvokableCall<>);
			Type type2 = typeFromHandle.MakeGenericType(type);
			ConstructorInfo constructor = type2.GetConstructor(new Type[3]
			{
				typeof(Object),
				typeof(MethodInfo),
				type
			});
			Object @object = arguments.unityObjectArgument;
			if (@object != null && !type.IsAssignableFrom(@object.GetType()))
			{
				@object = null;
			}
			return constructor.Invoke(new object[3]
			{
				target,
				method,
				@object
			}) as BaseInvokableCall;
		}

		public void RegisterPersistentListener(Object ttarget, string mmethodName)
		{
			m_Target = ttarget;
			m_MethodName = mmethodName;
		}

		public void UnregisterPersistentListener()
		{
			m_MethodName = string.Empty;
			m_Target = null;
		}
	}
}
