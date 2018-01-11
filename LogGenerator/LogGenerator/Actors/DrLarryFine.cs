using System;
using LogGenerator.Actors.Archetypes;
using LogGenerator.Actors.Details;
using LogGenerator.DateUtil;

namespace LogGenerator.Actors
{
    public class DrLarryFine : Doctor
    {
        private static readonly Schedule Schedule = new Schedule(
            TimeSpan.FromHours(7.5),
            TimeSpan.FromHours(11),
            TimeSpan.FromHours(12),
            TimeSpan.FromHours(16));

        private static readonly ScheduleAffinity ArriveAffinity = new ScheduleAffinity(.1, .15, .25, .30);
        private static readonly ScheduleAffinity LeaveAffinity = new ScheduleAffinity(.08, .30, .20, .25);
        private static readonly WorkEthic WorkEthic = new WorkEthic(.025, .28, ArriveAffinity, LeaveAffinity);
        private static readonly DateTime[] VacationDays = {
            new DateTime(2017, 7, 10),
            new DateTime(2017, 7, 11),
            new DateTime(2017, 7, 12),
            new DateTime(2017, 7, 13),
            new DateTime(2017, 7, 14),
            new DateTime(2017, 11, 24),
            new DateTime(2017, 12, 26),
            new DateTime(2017, 12, 29),
        };
        private static readonly Profile MyProfile = new Profile(Schedule, WorkEthic, VacationDays);


        public DrLarryFine(ICalendar calendar) : base(.05, calendar, MyProfile, "34.199.18.202", "10.10.5.2")
        {
        }

        protected override string UserName => "Dr. Larry Fine";
    }
}