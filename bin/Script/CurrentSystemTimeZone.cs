// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System
{
    class CurrentSystemTimeZone : TimeZone
    {
      // Fields:
  m_standardName : String
  m_daylightName : String
  m_CachedDaylightChanges : Hashtable
  m_ticksOffset : Int64
  utcOffsetWithOutDLS : TimeSpan
  utcOffsetWithDLS : TimeSpan
  this_year : Int32
  this_year_dlt : DaylightTime
      // Properties:
  DaylightName : String
  StandardName : String
      // Events:
      // Methods:
      VoidCurrentSystemTimeZone::.ctor()
      VoidCurrentSystemTimeZone::.ctorInt64)
      VoidCurrentSystemTimeZone::System.Runtime.Serialization.IDeserializationCallback.OnDeserializationObject)
      BooleanCurrentSystemTimeZone::GetTimeZoneDataInt32Int64[]&String[]&)
      public StringCurrentSystemTimeZone::get_DaylightName()
      public StringCurrentSystemTimeZone::get_StandardName()
      public Globalization.DaylightTimeCurrentSystemTimeZone::GetDaylightChangesInt32)
      public TimeSpanCurrentSystemTimeZone::GetUtcOffsetDateTime)
      VoidCurrentSystemTimeZone::OnDeserializationGlobalization.DaylightTime)
      Globalization.DaylightTimeCurrentSystemTimeZone::GetDaylightTimeFromDataInt64[])
    }
}
