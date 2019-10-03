using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace UnityEngine.Networking
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public abstract class SyncList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
	{
		public delegate void SyncListChanged(Operation op, int itemIndex);

		public enum Operation
		{
			OP_ADD,
			OP_CLEAR,
			OP_INSERT,
			OP_REMOVE,
			OP_REMOVEAT,
			OP_SET,
			OP_DIRTY
		}

		private List<T> m_Objects = new List<T>();

		private NetworkBehaviour m_Behaviour;

		private int m_CmdHash;

		private SyncListChanged m_Callback;

		public int Count => m_Objects.Count;

		public bool IsReadOnly => false;

		public SyncListChanged Callback
		{
			get
			{
				return m_Callback;
			}
			set
			{
				m_Callback = value;
			}
		}

		public T this[int i]
		{
			get
			{
				return m_Objects[i];
			}
			set
			{
				bool flag = false;
				if (m_Objects[i] == null)
				{
					if (value == null)
					{
						return;
					}
					flag = true;
				}
				else
				{
					flag = !m_Objects[i].Equals(value);
				}
				m_Objects[i] = value;
				if (flag)
				{
					SendMsg(Operation.OP_SET, i, value);
				}
			}
		}

		protected abstract void SerializeItem(NetworkWriter writer, T item);

		protected abstract T DeserializeItem(NetworkReader reader);

		public void InitializeBehaviour(NetworkBehaviour beh, int cmdHash)
		{
			m_Behaviour = beh;
			m_CmdHash = cmdHash;
		}

		private void SendMsg(Operation op, int itemIndex, T item)
		{
			if (m_Behaviour == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("SyncList not initialized");
				}
				return;
			}
			NetworkIdentity component = m_Behaviour.GetComponent<NetworkIdentity>();
			if (component == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("SyncList no NetworkIdentity");
				}
			}
			else if (component.isServer)
			{
				NetworkWriter networkWriter = new NetworkWriter();
				networkWriter.StartMessage(9);
				networkWriter.Write(component.netId);
				networkWriter.WritePackedUInt32((uint)m_CmdHash);
				networkWriter.Write((byte)op);
				networkWriter.WritePackedUInt32((uint)itemIndex);
				SerializeItem(networkWriter, item);
				networkWriter.FinishMessage();
				NetworkServer.SendWriterToReady(component.gameObject, networkWriter, m_Behaviour.GetNetworkChannel());
				if (m_Behaviour.isServer && m_Behaviour.isClient && m_Callback != null)
				{
					m_Callback(op, itemIndex);
				}
			}
		}

		private void SendMsg(Operation op, int itemIndex)
		{
			SendMsg(op, itemIndex, default(T));
		}

		public void HandleMsg(NetworkReader reader)
		{
			byte b = reader.ReadByte();
			int num = (int)reader.ReadPackedUInt32();
			T val = DeserializeItem(reader);
			switch (b)
			{
			case 0:
				m_Objects.Add(val);
				break;
			case 1:
				m_Objects.Clear();
				break;
			case 2:
				m_Objects.Insert(num, val);
				break;
			case 3:
				m_Objects.Remove(val);
				break;
			case 4:
				m_Objects.RemoveAt(num);
				break;
			case 5:
			case 6:
				m_Objects[num] = val;
				break;
			}
			if (m_Callback != null)
			{
				m_Callback((Operation)b, num);
			}
		}

		internal void AddInternal(T item)
		{
			m_Objects.Add(item);
		}

		public void Add(T item)
		{
			m_Objects.Add(item);
			SendMsg(Operation.OP_ADD, m_Objects.Count - 1, item);
		}

		public void Clear()
		{
			m_Objects.Clear();
			SendMsg(Operation.OP_CLEAR, 0);
		}

		public bool Contains(T item)
		{
			return m_Objects.Contains(item);
		}

		public void CopyTo(T[] array, int index)
		{
			m_Objects.CopyTo(array, index);
		}

		public int IndexOf(T item)
		{
			return m_Objects.IndexOf(item);
		}

		public void Insert(int index, T item)
		{
			m_Objects.Insert(index, item);
			SendMsg(Operation.OP_INSERT, index, item);
		}

		public bool Remove(T item)
		{
			bool flag = m_Objects.Remove(item);
			if (flag)
			{
				SendMsg(Operation.OP_REMOVE, 0, item);
			}
			return flag;
		}

		public void RemoveAt(int index)
		{
			m_Objects.RemoveAt(index);
			SendMsg(Operation.OP_REMOVEAT, index);
		}

		public void Dirty(int index)
		{
			SendMsg(Operation.OP_DIRTY, index, m_Objects[index]);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return m_Objects.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
