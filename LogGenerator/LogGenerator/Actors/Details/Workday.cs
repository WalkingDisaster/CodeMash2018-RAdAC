using System;

namespace LogGenerator.Actors.Details
{
    public abstract class Day
    {
        protected Day(DateTime day)
        {
            Date = day;
        }
        public DateTime Date { get; }

        public abstract bool IsWorkDay { get; }
    }

    public class DayOff : Day
    {
        public DayOff(DateTime day) : base(day)
        {
        }

        public override bool IsWorkDay => false;
    }

    public class Workday : Day
    {
        public Workday(DateTime day, Schedule schedule) : base(day)
        {
            Day = day;
            Schedule = schedule;
        }

        public DateTime Day { get; }
        public Schedule Schedule { get; }
        public override bool IsWorkDay => true;
    }
}