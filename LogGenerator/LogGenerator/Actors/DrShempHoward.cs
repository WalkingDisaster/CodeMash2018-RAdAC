using System;
using LogGenerator.Actors.Archetypes;
using LogGenerator.Actors.Details;
using LogGenerator.DateUtil;

namespace LogGenerator.Actors
{
    public class DrShempHoward : Doctor
    {
        private static readonly Schedule Schedule = new Schedule(
            TimeSpan.FromHours(10),
            TimeSpan.FromHours(11),
            TimeSpan.FromHours(12),
            TimeSpan.FromHours(18.5));

        private static readonly ScheduleAffinity ArriveAffinity = new ScheduleAffinity(.025, .15, .015, .25);
        private static readonly ScheduleAffinity LeaveAffinity = new ScheduleAffinity(.05, .05, .05, .05);
        private static readonly WorkEthic WorkEthic = new WorkEthic(.05, .05, ArriveAffinity, LeaveAffinity);
        private static readonly DateTime[] VacationDays = {
        };
        private static readonly Profile MyProfile = new Profile(Schedule, WorkEthic, VacationDays);


        public DrShempHoward(ICalendar calendar) : base(.05, calendar, MyProfile, "12.33.188.88", "10.10.5.3")
        {
        }

        protected override string UserName => "Dr. Shemp Howard";
    }
}