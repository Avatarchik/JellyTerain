using System.Reflection;

namespace System.ComponentModel
{
	internal class ReflectionEventDescriptor : EventDescriptor
	{
		private Type _eventType;

		private Type _componentType;

		private EventInfo _eventInfo;

		private MethodInfo add_method;

		private MethodInfo remove_method;

		public override Type ComponentType => _componentType;

		public override Type EventType => _eventType;

		public override bool IsMulticast => GetEventInfo().IsMulticast;

		public ReflectionEventDescriptor(EventInfo eventInfo)
			: base(eventInfo.Name, (Attribute[])eventInfo.GetCustomAttributes(inherit: true))
		{
			_eventInfo = eventInfo;
			_componentType = eventInfo.DeclaringType;
			_eventType = eventInfo.EventHandlerType;
			add_method = eventInfo.GetAddMethod();
			remove_method = eventInfo.GetRemoveMethod();
		}

		public ReflectionEventDescriptor(Type componentType, EventDescriptor oldEventDescriptor, Attribute[] attrs)
			: base(oldEventDescriptor, attrs)
		{
			_componentType = componentType;
			_eventType = oldEventDescriptor.EventType;
			EventInfo @event = componentType.GetEvent(oldEventDescriptor.Name);
			add_method = @event.GetAddMethod();
			remove_method = @event.GetRemoveMethod();
		}

		public ReflectionEventDescriptor(Type componentType, string name, Type type, Attribute[] attrs)
			: base(name, attrs)
		{
			_componentType = componentType;
			_eventType = type;
			EventInfo @event = componentType.GetEvent(name);
			add_method = @event.GetAddMethod();
			remove_method = @event.GetRemoveMethod();
		}

		private EventInfo GetEventInfo()
		{
			if (_eventInfo == null)
			{
				_eventInfo = _componentType.GetEvent(Name);
				if (_eventInfo == null)
				{
					throw new ArgumentException("Accessor methods for the " + Name + " event are missing");
				}
			}
			return _eventInfo;
		}

		public override void AddEventHandler(object component, Delegate value)
		{
			add_method.Invoke(component, new object[1]
			{
				value
			});
		}

		public override void RemoveEventHandler(object component, Delegate value)
		{
			remove_method.Invoke(component, new object[1]
			{
				value
			});
		}
	}
}
