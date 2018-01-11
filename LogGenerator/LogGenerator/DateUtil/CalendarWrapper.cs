using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LogGenerator.DateUtil
{
    public class CalendarWrapper : ICalendar
    {
        private readonly IEnumerable<DateTime> _holidays;
        public int WeekendsInYear { get; }

        public CalendarWrapper(int year, IEnumerable<DateTime> holidays)
        {
            var weekendDays = 0;
            var dayOfYear = new DateTime(year, 1, 1);
            while (dayOfYear.Year == year)
            {
                if (dayOfYear.DayOfWeek == DayOfWeek.Saturday || dayOfYear.DayOfWeek == DayOfWeek.Sunday)
                {
                    weekendDays++;
                }
                dayOfYear = dayOfYear.AddDays(1);
            }
            WeekendsInYear = weekendDays;
            _holidays = holidays.Select(d => d.Date).AsEnumerable();
        }

        public bool IsWeekend(DateTimeOffset day)
        {
            return day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday;
        }

        public bool IsHoliday(DateTimeOffset day)
        {
            var theDay = day.Date;
            return _holidays.Any(d => d == theDay);
        }
    }
}