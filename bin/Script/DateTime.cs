// Class info from mscorlib.dll
// 
using UnityEngine;

namespace System
{
    public class DateTime : ValueType
    {
      // Fields:
  dp400 : Int32
  dp100 : Int32
  dp4 : Int32
  w32file_epoch : Int64
  MAX_VALUE_TICKS : Int64
  UnixEpoch : Int64
  ticks18991230 : Int64
  OAMinValue : Double
  OAMaxValue : Double
  formatExceptionMessage : String
  ticks : TimeSpan
  kind : DateTimeKind
  public MaxValue : DateTime
  public MinValue : DateTime
  ParseTimeFormats : String[]
  ParseYearDayMonthFormats : String[]
  ParseYearMonthDayFormats : String[]
  ParseDayMonthYearFormats : String[]
  ParseMonthDayYearFormats : String[]
  MonthDayShortFormats : String[]
  DayMonthShortFormats : String[]
  daysmonth : Int32[]
  daysmonthleap : Int32[]
  to_local_time_span_object : Object
  last_now : Int64
      // Properties:
  Date : DateTime
  Month : Int32
  Day : Int32
  DayOfWeek : DayOfWeek
  DayOfYear : Int32
  TimeOfDay : TimeSpan
  Hour : Int32
  Minute : Int32
  Second : Int32
  Millisecond : Int32
  Now : DateTime
  Ticks : Int64
  Today : DateTime
  UtcNow : DateTime
  Year : Int32
  Kind : DateTimeKind
      // Events:
      // Methods:
      public VoidDateTime::.ctorInt64)
      public VoidDateTime::.ctorInt32Int32Int32)
      public VoidDateTime::.ctorInt32Int32Int32Int32Int32Int32)
      public VoidDateTime::.ctorInt32Int32Int32Int32Int32Int32Int32)
      public VoidDateTime::.ctorInt32Int32Int32Globalization.Calendar)
      public VoidDateTime::.ctorInt32Int32Int32Int32Int32Int32Globalization.Calendar)
      public VoidDateTime::.ctorInt32Int32Int32Int32Int32Int32Int32Globalization.Calendar)
      VoidDateTime::.ctorBooleanTimeSpan)
      public VoidDateTime::.ctorInt64DateTimeKind)
      public VoidDateTime::.ctorInt32Int32Int32Int32Int32Int32DateTimeKind)
      public VoidDateTime::.ctorInt32Int32Int32Int32Int32Int32Int32DateTimeKind)
      public VoidDateTime::.ctorInt32Int32Int32Int32Int32Int32Int32Globalization.CalendarDateTimeKind)
      VoidDateTime::.cctor()
      BooleanDateTime::System.IConvertible.ToBooleanIFormatProvider)
      ByteDateTime::System.IConvertible.ToByteIFormatProvider)
      CharDateTime::System.IConvertible.ToCharIFormatProvider)
      DateTimeDateTime::System.IConvertible.ToDateTimeIFormatProvider)
      DecimalDateTime::System.IConvertible.ToDecimalIFormatProvider)
      DoubleDateTime::System.IConvertible.ToDoubleIFormatProvider)
      Int16DateTime::System.IConvertible.ToInt16IFormatProvider)
      Int32DateTime::System.IConvertible.ToInt32IFormatProvider)
      Int64DateTime::System.IConvertible.ToInt64IFormatProvider)
      SByteDateTime::System.IConvertible.ToSByteIFormatProvider)
      SingleDateTime::System.IConvertible.ToSingleIFormatProvider)
      ObjectDateTime::System.IConvertible.ToTypeTypeIFormatProvider)
      UInt16DateTime::System.IConvertible.ToUInt16IFormatProvider)
      UInt32DateTime::System.IConvertible.ToUInt32IFormatProvider)
      UInt64DateTime::System.IConvertible.ToUInt64IFormatProvider)
      Int32DateTime::AbsoluteDaysInt32Int32Int32)
      Int32DateTime::FromTicksDateTime/Which)
      public DateTimeDateTime::get_Date()
      public Int32DateTime::get_Month()
      public Int32DateTime::get_Day()
      public DayOfWeekDateTime::get_DayOfWeek()
      public Int32DateTime::get_DayOfYear()
      public TimeSpanDateTime::get_TimeOfDay()
      public Int32DateTime::get_Hour()
      public Int32DateTime::get_Minute()
      public Int32DateTime::get_Second()
      public Int32DateTime::get_Millisecond()
      Int64DateTime::GetTimeMonotonic()
      Int64DateTime::GetNow()
      public DateTimeDateTime::get_Now()
      public Int64DateTime::get_Ticks()
      public DateTimeDateTime::get_Today()
      public DateTimeDateTime::get_UtcNow()
      public Int32DateTime::get_Year()
      public DateTimeKindDateTime::get_Kind()
      public DateTimeDateTime::AddTimeSpan)
      public DateTimeDateTime::AddDaysDouble)
      public DateTimeDateTime::AddTicksInt64)
      public DateTimeDateTime::AddHoursDouble)
      public DateTimeDateTime::AddMillisecondsDouble)
      DateTimeDateTime::AddRoundedMillisecondsDouble)
      public DateTimeDateTime::AddMinutesDouble)
      public DateTimeDateTime::AddMonthsInt32)
      public DateTimeDateTime::AddSecondsDouble)
      public DateTimeDateTime::AddYearsInt32)
      public Int32DateTime::CompareDateTimeDateTime)
      public Int32DateTime::CompareToObject)
      public BooleanDateTime::IsDaylightSavingTime()
      public Int32DateTime::CompareToDateTime)
      public BooleanDateTime::EqualsDateTime)
      public Int64DateTime::ToBinary()
      public DateTimeDateTime::FromBinaryInt64)
      public DateTimeDateTime::SpecifyKindDateTimeDateTimeKind)
      public Int32DateTime::DaysInMonthInt32Int32)
      public BooleanDateTime::EqualsObject)
      public BooleanDateTime::EqualsDateTimeDateTime)
      public DateTimeDateTime::FromFileTimeInt64)
      public DateTimeDateTime::FromFileTimeUtcInt64)
      public DateTimeDateTime::FromOADateDouble)
      public String[]DateTime::GetDateTimeFormats()
      public String[]DateTime::GetDateTimeFormatsChar)
      public String[]DateTime::GetDateTimeFormatsIFormatProvider)
      public String[]DateTime::GetDateTimeFormatsCharIFormatProvider)
      String[]DateTime::GetDateTimeFormatsBooleanString[]Globalization.DateTimeFormatInfo)
      VoidDateTime::CheckDateTimeKindDateTimeKind)
      public Int32DateTime::GetHashCode()
      public TypeCodeDateTime::GetTypeCode()
      public BooleanDateTime::IsLeapYearInt32)
      public DateTimeDateTime::ParseString)
      public DateTimeDateTime::ParseStringIFormatProvider)
      public DateTimeDateTime::ParseStringIFormatProviderGlobalization.DateTimeStyles)
      BooleanDateTime::CoreParseStringIFormatProviderGlobalization.DateTimeStylesDateTime&DateTimeOffset&BooleanException&)
      public DateTimeDateTime::ParseExactStringStringIFormatProvider)
      String[]DateTime::YearMonthDayFormatsGlobalization.DateTimeFormatInfoBooleanException&)
      Int32DateTime::_ParseNumberStringInt32Int32Int32BooleanBooleanInt32&)
      Int32DateTime::_ParseEnumStringInt32String[]String[]BooleanInt32&)
      BooleanDateTime::_ParseStringStringInt32Int32StringInt32&)
      BooleanDateTime::_ParseAmPmStringInt32Int32Globalization.DateTimeFormatInfoBooleanInt32&Int32&)
      BooleanDateTime::_ParseTimeSeparatorStringInt32Globalization.DateTimeFormatInfoBooleanInt32&)
      BooleanDateTime::_ParseDateSeparatorStringInt32Globalization.DateTimeFormatInfoBooleanInt32&)
      BooleanDateTime::IsLetterStringInt32)
      BooleanDateTime::_DoParseStringStringStringBooleanDateTime&DateTimeOffset&Globalization.DateTimeFormatInfoGlobalization.DateTimeStylesBooleanBoolean&Boolean&)
      public DateTimeDateTime::ParseExactStringStringIFormatProviderGlobalization.DateTimeStyles)
      public DateTimeDateTime::ParseExactStringString[]IFormatProviderGlobalization.DateTimeStyles)
      VoidDateTime::CheckStyleGlobalization.DateTimeStyles)
      public BooleanDateTime::TryParseStringDateTime&)
      public BooleanDateTime::TryParseStringIFormatProviderGlobalization.DateTimeStylesDateTime&)
      public BooleanDateTime::TryParseExactStringStringIFormatProviderGlobalization.DateTimeStylesDateTime&)
      public BooleanDateTime::TryParseExactStringString[]IFormatProviderGlobalization.DateTimeStylesDateTime&)
      BooleanDateTime::ParseExactStringString[]Globalization.DateTimeFormatInfoGlobalization.DateTimeStylesDateTime&BooleanBoolean&BooleanException&)
      public TimeSpanDateTime::SubtractDateTime)
      public DateTimeDateTime::SubtractTimeSpan)
      public Int64DateTime::ToFileTime()
      public Int64DateTime::ToFileTimeUtc()
      public StringDateTime::ToLongDateString()
      public StringDateTime::ToLongTimeString()
      public DoubleDateTime::ToOADate()
      public StringDateTime::ToShortDateString()
      public StringDateTime::ToShortTimeString()
      public StringDateTime::ToString()
      public StringDateTime::ToStringIFormatProvider)
      public StringDateTime::ToStringString)
      public StringDateTime::ToStringStringIFormatProvider)
      public DateTimeDateTime::ToLocalTime()
      public DateTimeDateTime::ToUniversalTime()
      public DateTimeDateTime::op_AdditionDateTimeTimeSpan)
      public BooleanDateTime::op_EqualityDateTimeDateTime)
      public BooleanDateTime::op_GreaterThanDateTimeDateTime)
      public BooleanDateTime::op_GreaterThanOrEqualDateTimeDateTime)
      public BooleanDateTime::op_InequalityDateTimeDateTime)
      public BooleanDateTime::op_LessThanDateTimeDateTime)
      public BooleanDateTime::op_LessThanOrEqualDateTimeDateTime)
      public TimeSpanDateTime::op_SubtractionDateTimeDateTime)
      public DateTimeDateTime::op_SubtractionDateTimeTimeSpan)
    }
}
