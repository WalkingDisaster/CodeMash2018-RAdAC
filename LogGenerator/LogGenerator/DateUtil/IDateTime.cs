using System;

namespace LogGenerator.DateUtil
{
    public interface IDateTime
    {
        DateTime UtcNow { get; }
        DateTimeOffset GetLocalTime(DateTime utcDateTime);
    }
}