using System;
using System.Collections.Generic;
using System.Globalization;
using LogGenerator.Actors.Details;
using LogGenerator.DateUtil;
using LogGenerator.Logging;

namespace LogGenerator.Actors.Archetypes
{
    public abstract class Doctor : Worker
    {
        private readonly double _frontLoadPercent;
        private readonly string _homeIpAddress;
        private readonly  string _officeIpAddress;
        protected abstract string UserName { get; }

        protected Doctor(
            double frontLoadPercent,
            ICalendar calendar,
            Profile profile,
            string homeIpAddress,
            string officeIpAddress) : base(calendar, profile)
        {
            _frontLoadPercent = frontLoadPercent;
            _homeIpAddress = homeIpAddress;
            _officeIpAddress = officeIpAddress;
        }

        protected override void OnInitializeNewDay(Day day, IList<(DateTime time, Action<Logger> activity)> activities)
        {
            PlanDay(day, activities);

            base.OnInitializeNewDay(day, activities);
        }

        private void PlanDay(Day day, IList<(DateTime time, Action<Logger> activity)> activities)
        {
            if (day is DayOff dayOff)
            {
                PlanOccasionalActivity(dayOff, activities);
            }
            else if (day is Workday workday)
            {
                PlanWorkActivity(workday, activities);
            }
        }

        private void PlanWorkActivity(Workday workday, IList<(DateTime time, Action<Logger> activity)> activities)
        {
            var frontLoaded = 0;
            var random = new Random();
            var currentTime = workday.Schedule.DayStart;
            while (currentTime < workday.Schedule.DayEnd)
            {
                var minutesUntilNextAppointment = 5 + (random.NextDouble() * 5);
                var nextAppointmentTime = currentTime.Add(TimeSpan.FromMinutes(minutesUntilNextAppointment));
                if (nextAppointmentTime > workday.Schedule.LunchStart &&
                    nextAppointmentTime < workday.Schedule.LunchEnd)
                {
                    nextAppointmentTime =
                        workday.Schedule.LunchEnd.Add(TimeSpan.FromMinutes(minutesUntilNextAppointment));
                }

                var minutesToPrep = 2 + (random.NextDouble() * 5);
                var prepTime = currentTime.Add(TimeSpan.FromMinutes(minutesToPrep));
                while (prepTime < nextAppointmentTime
                    && prepTime < workday.Schedule.LunchStart
                    && prepTime < workday.Schedule.LunchEnd
                    && random.NextDouble() < _frontLoadPercent
                    && random.Next(0, 20) > ++frontLoaded)
                {
                    var time = workday.Day.Add(prepTime);
                    activities.Add((time, logger => logger.Log(time, "Review Patient Records", "View::ePHI", "Doctor", UserName, _officeIpAddress)));
                    minutesToPrep = 2 + (random.NextDouble() * 5);
                    prepTime = currentTime.Add(TimeSpan.FromMinutes(minutesToPrep));
                }

                var appointmentDuration = 15 + (random.NextDouble() * 10);
                var appointmentEnd = nextAppointmentTime.Add(TimeSpan.FromMinutes(appointmentDuration));
                while (currentTime < appointmentEnd)
                {
                    var time = workday.Day.Add(currentTime);
                    switch (random.Next(0,3))
                    {
                        case 1:
                            activities.Add((time, logger => logger.Log(time, "Patient Appointment", "View::ePHI", "Doctor", UserName, _officeIpAddress)));
                            break;
                        default:
                            activities.Add((time, logger => logger.Log(time, "Patient Appointment", "Update::ePHI", "Doctor", UserName, _officeIpAddress)));
                            break;
                    }

                    currentTime = currentTime.Add(TimeSpan.FromSeconds(random.NextDouble() * 600));
                }

                currentTime = appointmentEnd;
            }
        }

        private void PlanOccasionalActivity(Day dayOff, ICollection<(DateTime time, Action<Logger> activity)> activities)
        {
            var random = new Random();
            var offHoursActivity = (Profile.WorkEthic.WorkWeekendPercent + Profile.WorkEthic.WorksThroughLunch) / 2;
            for (var hour = 6; hour < 20; hour ++)
            {
                var seconds = random.Next(0, 3599);
                while (random.NextDouble() < offHoursActivity)
                {
                    var time = new TimeSpan(hour, 0, 0);
                    time = time.Add(TimeSpan.FromSeconds(seconds));
                    var activityTime = dayOff.Date.Add(time);
                    activities.Add((activityTime, logger => logger.Log(activityTime, "Check Patient Status", "View::ePHI", "Doctor", UserName, _homeIpAddress)));
                    seconds = random.Next(3599 - seconds, 3599);
                }
            }
        }
    }
}