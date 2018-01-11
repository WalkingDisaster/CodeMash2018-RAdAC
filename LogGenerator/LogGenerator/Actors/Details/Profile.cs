using System;
using System.Collections.Generic;

namespace LogGenerator.Actors.Details
{
    public class Profile
    {
        public Profile(Schedule schedule, WorkEthic workEthic, IEnumerable<DateTime> vacationDays) =>
            (Schedule, WorkEthic, VacationDays) = (schedule, workEthic, vacationDays);
        public Schedule Schedule { get; set; }
        public WorkEthic WorkEthic { get; set; }
        public IEnumerable<DateTime> VacationDays { get; set; }

        public Workday CreateWorkDay(DateTime today)
        {
            var startTime = WorkEthic.CalcualteStart(Schedule.DayStart);
            var endTime = WorkEthic.CalcualteEnd(Schedule.DayEnd);
            var (lunchStart, lunchEnd) = WorkEthic.CalculateLunch(Schedule.LunchStart, Schedule.LunchEnd);
            return new Workday(today, new Schedule(startTime, lunchStart, lunchEnd, endTime));
        }
    }
}