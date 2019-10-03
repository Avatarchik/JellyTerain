// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System.Globalization
{
    public class DateTimeFormatInfo : Object
    {
      // Fields:
  _RoundtripPattern : String
  MSG_READONLY : String
  MSG_ARRAYSIZE_MONTH : String
  MSG_ARRAYSIZE_DAY : String
  INVARIANT_ABBREVIATED_DAY_NAMES : String[]
  INVARIANT_DAY_NAMES : String[]
  INVARIANT_ABBREVIATED_MONTH_NAMES : String[]
  INVARIANT_MONTH_NAMES : String[]
  INVARIANT_SHORT_DAY_NAMES : String[]
  theInvariantDateTimeFormatInfo : DateTimeFormatInfo
  m_isReadOnly : Boolean
  amDesignator : String
  pmDesignator : String
  dateSeparator : String
  timeSeparator : String
  shortDatePattern : String
  longDatePattern : String
  shortTimePattern : String
  longTimePattern : String
  monthDayPattern : String
  yearMonthPattern : String
  fullDateTimePattern : String
  _RFC1123Pattern : String
  _SortableDateTimePattern : String
  _UniversalSortableDateTimePattern : String
  firstDayOfWeek : Int32
  calendar : Calendar
  calendarWeekRule : Int32
  abbreviatedDayNames : String[]
  dayNames : String[]
  monthNames : String[]
  abbreviatedMonthNames : String[]
  allShortDatePatterns : String[]
  allLongDatePatterns : String[]
  allShortTimePatterns : String[]
  allLongTimePatterns : String[]
  monthDayPatterns : String[]
  yearMonthPatterns : String[]
  shortDayNames : String[]
  nDataItem : Int32
  m_useUserOverride : Boolean
  m_isDefaultCalendar : Boolean
  CultureID : Int32
  bUseCalendarInfo : Boolean
  generalShortTimePattern : String
  generalLongTimePattern : String
  m_eraNames : String[]
  m_abbrevEraNames : String[]
  m_abbrevEnglishEraNames : String[]
  m_dateWords : String[]
  optionalCalendars : Int32[]
  m_superShortDayNames : String[]
  genitiveMonthNames : String[]
  m_genitiveAbbreviatedMonthNames : String[]
  leapYearMonthNames : String[]
  formatFlags : DateTimeFormatFlags
  m_name : String
  all_date_time_patterns : String[] modreq(System.Runtime.CompilerServices.IsVolatile)
      // Properties:
  IsReadOnly : Boolean
  AbbreviatedDayNames : String[]
  RawAbbreviatedDayNames : String[]
  AbbreviatedMonthNames : String[]
  RawAbbreviatedMonthNames : String[]
  DayNames : String[]
  RawDayNames : String[]
  MonthNames : String[]
  RawMonthNames : String[]
  AMDesignator : String
  PMDesignator : String
  DateSeparator : String
  TimeSeparator : String
  LongDatePattern : String
  ShortDatePattern : String
  ShortTimePattern : String
  LongTimePattern : String
  MonthDayPattern : String
  YearMonthPattern : String
  FullDateTimePattern : String
  CurrentInfo : DateTimeFormatInfo
  InvariantInfo : DateTimeFormatInfo
  FirstDayOfWeek : DayOfWeek
  Calendar : Calendar
  CalendarWeekRule : CalendarWeekRule
  RFC1123Pattern : String
  RoundtripPattern : String
  SortableDateTimePattern : String
  UniversalSortableDateTimePattern : String
  AbbreviatedMonthGenitiveNames : String[]
  MonthGenitiveNames : String[]
  NativeCalendarName : String
  ShortestDayNames : String[]
      // Events:
      // Methods:
      VoidGlobalization.DateTimeFormatInfo::.ctorBoolean)
      public VoidGlobalization.DateTimeFormatInfo::.ctor()
      VoidGlobalization.DateTimeFormatInfo::.cctor()
      public Globalization.DateTimeFormatInfoGlobalization.DateTimeFormatInfo::GetInstanceIFormatProvider)
      public BooleanGlobalization.DateTimeFormatInfo::get_IsReadOnly()
      public Globalization.DateTimeFormatInfoGlobalization.DateTimeFormatInfo::ReadOnlyGlobalization.DateTimeFormatInfo)
      public ObjectGlobalization.DateTimeFormatInfo::Clone()
      public ObjectGlobalization.DateTimeFormatInfo::GetFormatType)
      public StringGlobalization.DateTimeFormatInfo::GetAbbreviatedEraNameInt32)
      public StringGlobalization.DateTimeFormatInfo::GetAbbreviatedMonthNameInt32)
      public Int32Globalization.DateTimeFormatInfo::GetEraString)
      public StringGlobalization.DateTimeFormatInfo::GetEraNameInt32)
      public StringGlobalization.DateTimeFormatInfo::GetMonthNameInt32)
      public String[]Globalization.DateTimeFormatInfo::get_AbbreviatedDayNames()
      public VoidGlobalization.DateTimeFormatInfo::set_AbbreviatedDayNamesString[])
      String[]Globalization.DateTimeFormatInfo::get_RawAbbreviatedDayNames()
      VoidGlobalization.DateTimeFormatInfo::set_RawAbbreviatedDayNamesString[])
      public String[]Globalization.DateTimeFormatInfo::get_AbbreviatedMonthNames()
      public VoidGlobalization.DateTimeFormatInfo::set_AbbreviatedMonthNamesString[])
      String[]Globalization.DateTimeFormatInfo::get_RawAbbreviatedMonthNames()
      VoidGlobalization.DateTimeFormatInfo::set_RawAbbreviatedMonthNamesString[])
      public String[]Globalization.DateTimeFormatInfo::get_DayNames()
      public VoidGlobalization.DateTimeFormatInfo::set_DayNamesString[])
      String[]Globalization.DateTimeFormatInfo::get_RawDayNames()
      VoidGlobalization.DateTimeFormatInfo::set_RawDayNamesString[])
      public String[]Globalization.DateTimeFormatInfo::get_MonthNames()
      public VoidGlobalization.DateTimeFormatInfo::set_MonthNamesString[])
      String[]Globalization.DateTimeFormatInfo::get_RawMonthNames()
      VoidGlobalization.DateTimeFormatInfo::set_RawMonthNamesString[])
      public StringGlobalization.DateTimeFormatInfo::get_AMDesignator()
      public VoidGlobalization.DateTimeFormatInfo::set_AMDesignatorString)
      public StringGlobalization.DateTimeFormatInfo::get_PMDesignator()
      public VoidGlobalization.DateTimeFormatInfo::set_PMDesignatorString)
      public StringGlobalization.DateTimeFormatInfo::get_DateSeparator()
      public VoidGlobalization.DateTimeFormatInfo::set_DateSeparatorString)
      public StringGlobalization.DateTimeFormatInfo::get_TimeSeparator()
      public VoidGlobalization.DateTimeFormatInfo::set_TimeSeparatorString)
      public StringGlobalization.DateTimeFormatInfo::get_LongDatePattern()
      public VoidGlobalization.DateTimeFormatInfo::set_LongDatePatternString)
      public StringGlobalization.DateTimeFormatInfo::get_ShortDatePattern()
      public VoidGlobalization.DateTimeFormatInfo::set_ShortDatePatternString)
      public StringGlobalization.DateTimeFormatInfo::get_ShortTimePattern()
      public VoidGlobalization.DateTimeFormatInfo::set_ShortTimePatternString)
      public StringGlobalization.DateTimeFormatInfo::get_LongTimePattern()
      public VoidGlobalization.DateTimeFormatInfo::set_LongTimePatternString)
      public StringGlobalization.DateTimeFormatInfo::get_MonthDayPattern()
      public VoidGlobalization.DateTimeFormatInfo::set_MonthDayPatternString)
      public StringGlobalization.DateTimeFormatInfo::get_YearMonthPattern()
      public VoidGlobalization.DateTimeFormatInfo::set_YearMonthPatternString)
      public StringGlobalization.DateTimeFormatInfo::get_FullDateTimePattern()
      public VoidGlobalization.DateTimeFormatInfo::set_FullDateTimePatternString)
      public Globalization.DateTimeFormatInfoGlobalization.DateTimeFormatInfo::get_CurrentInfo()
      public Globalization.DateTimeFormatInfoGlobalization.DateTimeFormatInfo::get_InvariantInfo()
      public DayOfWeekGlobalization.DateTimeFormatInfo::get_FirstDayOfWeek()
      public VoidGlobalization.DateTimeFormatInfo::set_FirstDayOfWeekDayOfWeek)
      public Globalization.CalendarGlobalization.DateTimeFormatInfo::get_Calendar()
      public VoidGlobalization.DateTimeFormatInfo::set_CalendarGlobalization.Calendar)
      public Globalization.CalendarWeekRuleGlobalization.DateTimeFormatInfo::get_CalendarWeekRule()
      public VoidGlobalization.DateTimeFormatInfo::set_CalendarWeekRuleGlobalization.CalendarWeekRule)
      public StringGlobalization.DateTimeFormatInfo::get_RFC1123Pattern()
      StringGlobalization.DateTimeFormatInfo::get_RoundtripPattern()
      public StringGlobalization.DateTimeFormatInfo::get_SortableDateTimePattern()
      public StringGlobalization.DateTimeFormatInfo::get_UniversalSortableDateTimePattern()
      public String[]Globalization.DateTimeFormatInfo::GetAllDateTimePatterns()
      String[]Globalization.DateTimeFormatInfo::GetAllDateTimePatternsInternal()
      VoidGlobalization.DateTimeFormatInfo::FillAllDateTimePatterns()
      public String[]Globalization.DateTimeFormatInfo::GetAllDateTimePatternsChar)
      String[]Globalization.DateTimeFormatInfo::GetAllRawDateTimePatternsChar)
      public StringGlobalization.DateTimeFormatInfo::GetDayNameDayOfWeek)
      public StringGlobalization.DateTimeFormatInfo::GetAbbreviatedDayNameDayOfWeek)
      VoidGlobalization.DateTimeFormatInfo::FillInvariantPatterns()
      String[]Globalization.DateTimeFormatInfo::PopulateCombinedListString[]String[])
      public String[]Globalization.DateTimeFormatInfo::get_AbbreviatedMonthGenitiveNames()
      public VoidGlobalization.DateTimeFormatInfo::set_AbbreviatedMonthGenitiveNamesString[])
      public String[]Globalization.DateTimeFormatInfo::get_MonthGenitiveNames()
      public VoidGlobalization.DateTimeFormatInfo::set_MonthGenitiveNamesString[])
      public StringGlobalization.DateTimeFormatInfo::get_NativeCalendarName()
      public String[]Globalization.DateTimeFormatInfo::get_ShortestDayNames()
      public VoidGlobalization.DateTimeFormatInfo::set_ShortestDayNamesString[])
      public StringGlobalization.DateTimeFormatInfo::GetShortestDayNameDayOfWeek)
      public VoidGlobalization.DateTimeFormatInfo::SetAllDateTimePatternsString[]Char)
    }
}
