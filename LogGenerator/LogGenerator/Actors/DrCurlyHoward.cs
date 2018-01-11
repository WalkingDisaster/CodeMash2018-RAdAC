using System;
using LogGenerator.Actors.Archetypes;
using LogGenerator.Actors.Details;
using LogGenerator.DateUtil;

namespace LogGenerator.Actors
{
    public class DrCurlyHoward : Doctor
    {
        private static readonly Schedule Schedule = new Schedule(
            TimeSpan.FromHours(7.5),
            TimeSpan.FromHours(11),
            TimeSpan.FromHours(12),
            TimeSpan.FromHours(16));

        private static readonly ScheduleAffinity ArriveAffinity = new ScheduleAffinity(.15, .02, .65, .15);
        private static readonly ScheduleAffinity LeaveAffinity = new ScheduleAffinity(.02, .20, .10, .25);
        private static readonly WorkEthic WorkEthic = new WorkEthic(.1, .18, ArriveAffinity, LeaveAffinity);
        private static readonly DateTime[] VacationDays = {
            new DateTime(2017, 6, 26),
            new DateTime(2017, 6, 27),
            new DateTime(2017, 6, 28),
            new DateTime(2017, 6, 29),
            new DateTime(2017, 6, 30),
            new DateTime(2017, 11, 24),
            new DateTime(2017, 12, 26),
            new DateTime(2017, 12, 27),
            new DateTime(2017, 12, 28),
            new DateTime(2017, 12, 29),
        };
        private static readonly Profile MyProfile = new Profile(Schedule, WorkEthic, VacationDays);


        public DrCurlyHoward(ICalendar calendar) : base(.15, calendar, MyProfile, "162.10.59.12", "10.10.5.1")
        {
        }

        protected override string UserName => "Dr. Curly Howard";
    }
}