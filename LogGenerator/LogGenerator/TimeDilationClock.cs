using System;
using LogGenerator.DateUtil;

namespace LogGenerator
{
    public class TimeDilationClock
    {
        private readonly double _minutesPerSecond;
        private readonly IDateTime _dateTime;
        private readonly DateTime _startTime;

        public TimeDilationClock(double minutesPerSecond, DateTimeOffset startTime, IDateTime dateTime)
        {
            _minutesPerSecond = minutesPerSecond;
            _dateTime = dateTime;
            _startTime = startTime.UtcDateTime;
        }

        public DateTimeOffset GetCurrentLocalDateTime()
        {
            var now = _dateTime.UtcNow;
            var difference = now.Subtract(_startTime).TotalSeconds;
            var totalMinutes = difference * _minutesPerSecond;
            var newUtcTime = _startTime.AddMinutes(totalMinutes);
            return _dateTime.GetLocalTime(newUtcTime);
        }
    }
}