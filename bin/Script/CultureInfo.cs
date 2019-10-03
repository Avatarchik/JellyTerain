// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Globalization
{
    public class CultureInfo : Object
    {
      // Fields:
  NumOptionalCalendars : Int32
  GregorianTypeMask : Int32
  CalendarTypeBits : Int32
  InvariantCultureId : Int32
  invariant_culture_info : CultureInfo modreq(System.Runtime.CompilerServices.IsVolatile)
  shared_table_lock : Object
  BootstrapCultureID : Int32
  m_isReadOnly : Boolean
  cultureID : Int32
  parent_lcid : Int32
  specific_lcid : Int32
  datetime_index : Int32
  number_index : Int32
  m_useUserOverride : Boolean
  numInfo : NumberFormatInfo modreq(System.Runtime.CompilerServices.IsVolatile)
  dateTimeInfo : DateTimeFormatInfo modreq(System.Runtime.CompilerServices.IsVolatile)
  textInfo : TextInfo modreq(System.Runtime.CompilerServices.IsVolatile)
  m_name : String
  displayname : String
  englishname : String
  nativename : String
  iso3lang : String
  iso2lang : String
  icu_name : String
  win3lang : String
  territory : String
  compareInfo : CompareInfo modreq(System.Runtime.CompilerServices.IsVolatile)
  calendar_data : Int32*
  textinfo_data : Void*
  optional_calendars : Calendar[]
  parent_culture : CultureInfo
  m_dataItem : Int32
  calendar : Calendar
  constructed : Boolean
  cached_serialized_form : Byte[]
  MSG_READONLY : String
  shared_by_number : Hashtable
  shared_by_name : Hashtable
  <>f__switch$map19 : Dictionary`2
  <>f__switch$map1A : Dictionary`2
      // Properties:
  InvariantCulture : CultureInfo
  CurrentCulture : CultureInfo
  CurrentUICulture : CultureInfo
  Territory : String
  LCID : Int32
  Name : String
  NativeName : String
  Calendar : Calendar
  OptionalCalendars : Calendar[]
  Parent : CultureInfo
  TextInfo : TextInfo
  ThreeLetterISOLanguageName : String
  ThreeLetterWindowsLanguageName : String
  TwoLetterISOLanguageName : String
  UseUserOverride : Boolean
  IcuName : String
  CompareInfo : CompareInfo
  IsNeutralCulture : Boolean
  NumberFormat : NumberFormatInfo
  DateTimeFormat : DateTimeFormatInfo
  DisplayName : String
  EnglishName : String
  InstalledUICulture : CultureInfo
  IsReadOnly : Boolean
      // Events:
      // Methods:
      public VoidGlobalization.CultureInfo::.ctorInt32)
      public VoidGlobalization.CultureInfo::.ctorInt32Boolean)
      VoidGlobalization.CultureInfo::.ctorInt32BooleanBoolean)
      public VoidGlobalization.CultureInfo::.ctorString)
      public VoidGlobalization.CultureInfo::.ctorStringBoolean)
      VoidGlobalization.CultureInfo::.ctorStringBooleanBoolean)
      VoidGlobalization.CultureInfo::.ctor()
      VoidGlobalization.CultureInfo::.cctor()
      public Globalization.CultureInfoGlobalization.CultureInfo::get_InvariantCulture()
      public Globalization.CultureInfoGlobalization.CultureInfo::CreateSpecificCultureString)
      public Globalization.CultureInfoGlobalization.CultureInfo::get_CurrentCulture()
      public Globalization.CultureInfoGlobalization.CultureInfo::get_CurrentUICulture()
      Globalization.CultureInfoGlobalization.CultureInfo::ConstructCurrentCulture()
      Globalization.CultureInfoGlobalization.CultureInfo::ConstructCurrentUICulture()
      StringGlobalization.CultureInfo::get_Territory()
      public Int32Globalization.CultureInfo::get_LCID()
      public StringGlobalization.CultureInfo::get_Name()
      public StringGlobalization.CultureInfo::get_NativeName()
      public Globalization.CalendarGlobalization.CultureInfo::get_Calendar()
      public Globalization.Calendar[]Globalization.CultureInfo::get_OptionalCalendars()
      public Globalization.CultureInfoGlobalization.CultureInfo::get_Parent()
      public Globalization.TextInfoGlobalization.CultureInfo::get_TextInfo()
      public StringGlobalization.CultureInfo::get_ThreeLetterISOLanguageName()
      public StringGlobalization.CultureInfo::get_ThreeLetterWindowsLanguageName()
      public StringGlobalization.CultureInfo::get_TwoLetterISOLanguageName()
      public BooleanGlobalization.CultureInfo::get_UseUserOverride()
      StringGlobalization.CultureInfo::get_IcuName()
      public VoidGlobalization.CultureInfo::ClearCachedData()
      public ObjectGlobalization.CultureInfo::Clone()
      public BooleanGlobalization.CultureInfo::EqualsObject)
      public Globalization.CultureInfo[]Globalization.CultureInfo::GetCulturesGlobalization.CultureTypes)
      public Int32Globalization.CultureInfo::GetHashCode()
      public Globalization.CultureInfoGlobalization.CultureInfo::ReadOnlyGlobalization.CultureInfo)
      public StringGlobalization.CultureInfo::ToString()
      public Globalization.CompareInfoGlobalization.CultureInfo::get_CompareInfo()
      BooleanGlobalization.CultureInfo::IsIDNeutralCultureInt32)
      public BooleanGlobalization.CultureInfo::get_IsNeutralCulture()
      VoidGlobalization.CultureInfo::CheckNeutral()
      public Globalization.NumberFormatInfoGlobalization.CultureInfo::get_NumberFormat()
      public VoidGlobalization.CultureInfo::set_NumberFormatGlobalization.NumberFormatInfo)
      public Globalization.DateTimeFormatInfoGlobalization.CultureInfo::get_DateTimeFormat()
      public VoidGlobalization.CultureInfo::set_DateTimeFormatGlobalization.DateTimeFormatInfo)
      public StringGlobalization.CultureInfo::get_DisplayName()
      public StringGlobalization.CultureInfo::get_EnglishName()
      public Globalization.CultureInfoGlobalization.CultureInfo::get_InstalledUICulture()
      public BooleanGlobalization.CultureInfo::get_IsReadOnly()
      public ObjectGlobalization.CultureInfo::GetFormatType)
      VoidGlobalization.CultureInfo::Construct()
      BooleanGlobalization.CultureInfo::ConstructInternalLocaleFromNameString)
      BooleanGlobalization.CultureInfo::ConstructInternalLocaleFromLcidInt32)
      BooleanGlobalization.CultureInfo::ConstructInternalLocaleFromSpecificNameGlobalization.CultureInfoString)
      BooleanGlobalization.CultureInfo::ConstructInternalLocaleFromCurrentLocaleGlobalization.CultureInfo)
      BooleanGlobalization.CultureInfo::construct_internal_locale_from_lcidInt32)
      BooleanGlobalization.CultureInfo::construct_internal_locale_from_nameString)
      BooleanGlobalization.CultureInfo::construct_internal_locale_from_specific_nameGlobalization.CultureInfoString)
      BooleanGlobalization.CultureInfo::construct_internal_locale_from_current_localeGlobalization.CultureInfo)
      Globalization.CultureInfo[]Globalization.CultureInfo::internal_get_culturesBooleanBooleanBoolean)
      VoidGlobalization.CultureInfo::construct_datetime_format()
      VoidGlobalization.CultureInfo::construct_number_format()
      BooleanGlobalization.CultureInfo::internal_is_lcid_neutralInt32Boolean&)
      VoidGlobalization.CultureInfo::ConstructInvariantBoolean)
      Globalization.TextInfoGlobalization.CultureInfo::CreateTextInfoBoolean)
      VoidGlobalization.CultureInfo::insert_into_shared_tablesGlobalization.CultureInfo)
      public Globalization.CultureInfoGlobalization.CultureInfo::GetCultureInfoInt32)
      public Globalization.CultureInfoGlobalization.CultureInfo::GetCultureInfoString)
      public Globalization.CultureInfoGlobalization.CultureInfo::GetCultureInfoStringString)
      public Globalization.CultureInfoGlobalization.CultureInfo::GetCultureInfoByIetfLanguageTagString)
      Globalization.CultureInfoGlobalization.CultureInfo::CreateCultureStringBoolean)
      VoidGlobalization.CultureInfo::ConstructCalendars()
    }
}
