using System;

namespace LogGenerator.DateUtil
{
    public class DateTimeWrapper : IDateTime
    {
        private DateTime StartTimeUtc { get; }
        private TimeSpan Difference { get; }

        public DateTimeWrapper(DateTime startTimeUtc)
        {
            StartTimeUtc = startTimeUtc;
            Difference = startTimeUtc.Subtract(DateTime.UtcNow);
        }

        public DateTime UtcNow => DateTime.UtcNow.Add(Difference);
        public DateTimeOffset GetLocalTime(DateTime utcDateTime)
        {
            var currentTimeZone = TimeZoneInfo.Local;
            var offsetTime = new DateTimeOffset(utcDateTime, TimeSpan.Zero);
            return TimeZoneInfo.ConvertTime(offsetTime, currentTimeZone);
        }
    }
}