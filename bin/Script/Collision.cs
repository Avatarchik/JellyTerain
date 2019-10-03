using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[StructLayout(LayoutKind.Sequential)]
	[RequiredByNativeCode]
	public class Collision
	{
		internal Vector3 m_Impulse;

		internal Vector3 m_RelativeVelocity;

		internal Rigidbody m_Rigidbody;

		internal Collider m_Collider;

		internal ContactPoint[] m_Contacts;

		public Vector3 relativeVelocity => m_RelativeVelocity;

		public Rigidbody rigidbody => m_Rigidbody;

		public Collider collider => m_Collider;

		public Transform transform => (!(rigidbody != null)) ? collider.transform : rigidbody.transform;

		public GameObject gameObject => (!(m_Rigidbody != null)) ? m_Collider.gameObject : m_Rigidbody.gameObject;

		public ContactPoint[] contacts => m_Contacts;

		public Vector3 impulse => m_Impulse;

		[Obsolete("Use Collision.relativeVelocity instead.", false)]
		public Vector3 impactForceSum => relativeVelocity;

		[Obsolete("Will always return zero.", false)]
		public Vector3 frictionForceSum => Vector3.zero;

		[Obsolete("Please use Collision.rigidbody, Collision.transform or Collision.collider instead", false)]
		public Component other => (!(m_Rigidbody != null)) ? ((Component)m_Collider) : ((Component)m_Rigidbody);

		public virtual IEnumerator GetEnumerator()
		{
			return contacts.GetEnumerator();
		}
	}
}
