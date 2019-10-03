// Class info from UnityEngine.UI.dll
// 
using UnityEngine;

namespace UnityEngine.UI
{
    class ObjectPool`1 : Object
    {
      // Fields:
  m_Stack : Stack`1
  m_ActionOnGet : UnityAction`1
  m_ActionOnRelease : UnityAction`1
  <countAll>k__BackingField : Int32
      // Properties:
  countAll : Int32
  countActive : Int32
  countInactive : Int32
      // Events:
      // Methods:
      public Void UnityEngine.UI.ObjectPool`1::.ctor(UnityEngine.Events.UnityAction`1<T>,UnityEngine.Events.UnityAction`1<T>)
      public Int32 UnityEngine.UI.ObjectPool`1::get_countAll()
      Void UnityEngine.UI.ObjectPool`1::set_countAllInt32)
      public Int32 UnityEngine.UI.ObjectPool`1::get_countActive()
      public Int32 UnityEngine.UI.ObjectPool`1::get_countInactive()
      public T UnityEngine.UI.ObjectPool`1::Get()
      public Void UnityEngine.UI.ObjectPool`1::Release(T)
    }
}
