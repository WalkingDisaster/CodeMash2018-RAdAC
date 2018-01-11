using System;

namespace LogGenerator.DateUtil
{
    public interface ICalendar
    {
        bool IsWeekend(DateTimeOffset day);
        bool IsHoliday(DateTimeOffset day);
        int WeekendsInYear { get; }
    }
}