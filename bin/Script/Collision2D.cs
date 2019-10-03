using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[StructLayout(LayoutKind.Sequential)]
	[RequiredByNativeCode]
	public class Collision2D
	{
		internal int m_Collider;

		internal int m_OtherCollider;

		internal int m_Rigidbody;

		internal int m_OtherRigidbody;

		internal ContactPoint2D[] m_Contacts;

		internal Vector2 m_RelativeVelocity;

		internal int m_Enabled;

		public Collider2D collider => Physics2D.GetColliderFromInstanceID(m_Collider);

		public Collider2D otherCollider => Physics2D.GetColliderFromInstanceID(m_OtherCollider);

		public Rigidbody2D rigidbody => Physics2D.GetRigidbodyFromInstanceID(m_Rigidbody);

		public Rigidbody2D otherRigidbody => Physics2D.GetRigidbodyFromInstanceID(m_OtherRigidbody);

		public Transform transform => (!(rigidbody != null)) ? collider.transform : rigidbody.transform;

		public GameObject gameObject => (!(rigidbody != null)) ? collider.gameObject : rigidbody.gameObject;

		public ContactPoint2D[] contacts => m_Contacts;

		public Vector2 relativeVelocity => m_RelativeVelocity;

		public bool enabled => m_Enabled == 1;
	}
}
