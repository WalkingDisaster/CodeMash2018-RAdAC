using System;

namespace LogGenerator.Actors.Details
{
    public class Schedule
    {
        public Schedule(TimeSpan dayStart, TimeSpan lunchStart, TimeSpan lunchEnd, TimeSpan dayEnd) =>
            (DayStart, LunchStart, LunchEnd, DayEnd) = (dayStart, lunchStart, lunchEnd, dayEnd);
        public TimeSpan DayStart { get; set; }
        public TimeSpan LunchStart { get; set; }
        public TimeSpan LunchEnd { get; set; }
        public TimeSpan DayEnd { get; set; }
    }
}