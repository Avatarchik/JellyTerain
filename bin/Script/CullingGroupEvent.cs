// Class info from UnityEngine.dll
// 
using UnityEngine;

namespace UnityEngine
{
    public class CullingGroupEvent : ValueType
    {
      // Fields:
  m_Index : Int32
  m_PrevState : Byte
  m_ThisState : Byte
  kIsVisibleMask : Byte
  kDistanceMask : Byte
      // Properties:
  index : Int32
  isVisible : Boolean
  wasVisible : Boolean
  hasBecomeVisible : Boolean
  hasBecomeInvisible : Boolean
  currentDistance : Int32
  previousDistance : Int32
      // Events:
      // Methods:
      public Int32 UnityEngine.CullingGroupEvent::get_index()
      public Boolean UnityEngine.CullingGroupEvent::get_isVisible()
      public Boolean UnityEngine.CullingGroupEvent::get_wasVisible()
      public Boolean UnityEngine.CullingGroupEvent::get_hasBecomeVisible()
      public Boolean UnityEngine.CullingGroupEvent::get_hasBecomeInvisible()
      public Int32 UnityEngine.CullingGroupEvent::get_currentDistance()
      public Int32 UnityEngine.CullingGroupEvent::get_previousDistance()
    }
}
