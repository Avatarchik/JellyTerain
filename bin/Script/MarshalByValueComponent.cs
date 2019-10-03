using System.ComponentModel.Design;
using System.Runtime.InteropServices;

namespace System.ComponentModel
{
	/// <summary>Implements <see cref="T:System.ComponentModel.IComponent" /> and provides the base implementation for remotable components that are marshaled by value (a copy of the serialized object is passed).</summary>
	[DesignerCategory("Component")]
	[ComVisible(true)]
	[Designer("System.Windows.Forms.Design.ComponentDocumentDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(IRootDesigner))]
	[TypeConverter(typeof(ComponentConverter))]
	public class MarshalByValueComponent : IDisposable, IServiceProvider, IComponent
	{
		private EventHandlerList eventList;

		private ISite mySite;

		private object disposedEvent = new object();

		/// <summary>Gets the container for the component.</summary>
		/// <returns>An object implementing the <see cref="T:System.ComponentModel.IContainer" /> interface that represents the component's container, or null if the component does not have a site.</returns>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual IContainer Container
		{
			get
			{
				if (mySite == null)
				{
					return null;
				}
				return mySite.Container;
			}
		}

		/// <summary>Gets a value indicating whether the component is currently in design mode.</summary>
		/// <returns>true if the component is in design mode; otherwise, false.</returns>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public virtual bool DesignMode
		{
			get
			{
				if (mySite == null)
				{
					return false;
				}
				return mySite.DesignMode;
			}
		}

		/// <summary>Gets or sets the site of the component.</summary>
		/// <returns>An object implementing the <see cref="T:System.ComponentModel.ISite" /> interface that represents the site of the component.</returns>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public virtual ISite Site
		{
			get
			{
				return mySite;
			}
			set
			{
				mySite = value;
			}
		}

		/// <summary>Gets the list of event handlers that are attached to this component.</summary>
		/// <returns>An <see cref="T:System.ComponentModel.EventHandlerList" /> that provides the delegates for this component.</returns>
		protected EventHandlerList Events
		{
			get
			{
				if (eventList == null)
				{
					eventList = new EventHandlerList();
				}
				return eventList;
			}
		}

		/// <summary>Adds an event handler to listen to the <see cref="E:System.ComponentModel.MarshalByValueComponent.Disposed" /> event on the component.</summary>
		public event EventHandler Disposed
		{
			add
			{
				Events.AddHandler(disposedEvent, value);
			}
			remove
			{
				Events.RemoveHandler(disposedEvent, value);
			}
		}

		/// <summary>Releases all resources used by the <see cref="T:System.ComponentModel.MarshalByValueComponent" />.</summary>
		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.ComponentModel.MarshalByValueComponent" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
		[MonoTODO]
		protected virtual void Dispose(bool disposing)
		{
			if (!disposing)
			{
			}
		}

		~MarshalByValueComponent()
		{
			Dispose(disposing: false);
		}

		/// <summary>Gets the implementer of the <see cref="T:System.IServiceProvider" />.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the implementer of the <see cref="T:System.IServiceProvider" />.</returns>
		/// <param name="service">A <see cref="T:System.Type" /> that represents the type of service you want. </param>
		public virtual object GetService(Type service)
		{
			if (mySite != null)
			{
				return mySite.GetService(service);
			}
			return null;
		}

		/// <summary>Returns a <see cref="T:System.String" /> containing the name of the <see cref="T:System.ComponentModel.Component" />, if any. This method should not be overridden.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the name of the <see cref="T:System.ComponentModel.Component" />, if any.null if the <see cref="T:System.ComponentModel.Component" /> is unnamed.</returns>
		public override string ToString()
		{
			if (mySite == null)
			{
				return GetType().ToString();
			}
			return $"{mySite.Name} [{GetType().ToString()}]";
		}
	}
}
