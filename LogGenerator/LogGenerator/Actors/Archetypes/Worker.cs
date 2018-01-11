using System;
using System.Collections.Generic;
using System.Linq;
using LogGenerator.Actors.Details;
using LogGenerator.DateUtil;
using LogGenerator.Logging;

namespace LogGenerator.Actors.Archetypes
{
    public class Worker : IActor
    {
        protected ICalendar Calendar { get; }
        protected Profile Profile { get; }
        protected Day CurrentDay { get; private set; }
        private Queue<(DateTime time, Action<Logger> activity)> Activities { get; set; }

        public Worker(ICalendar calendar, Profile profile)
        {
            Activities = new Queue<(DateTime time, Action<Logger> activity)>();
            Calendar = calendar;
            Profile = profile;
        }

        public void Do(DateTime localNow, Logger logger)
        {
            var today = localNow.Date;
            if (CurrentDay?.Date != today)
            {
                ItIsABrandNewDay(today);
            }

            if (Profile.VacationDays.Any(d => d == today)
                || Calendar.IsHoliday(today)
            )
            {
                return;
            }
            OnDo(localNow, logger);
        }

        protected virtual void OnDo(DateTime localNow, Logger logger)
        {
            while (Activities.Count > 0 && Activities.Peek().time < localNow)
            {
                var current = Activities.Dequeue();
                current.activity(logger);
            }
        }

        protected virtual void OnInitializeNewDay(Day day, IList<(DateTime time, Action<Logger> activity)> activities)
        {
            CurrentDay = day;
            var newActivities = new Queue<(DateTime time, Action<Logger> activity)>();
            foreach (var activity in activities.OrderBy(x => x.time))
            {
                newActivities.Enqueue(activity);
            }
            Activities = newActivities;
        }

        private void ItIsABrandNewDay(DateTime today)
        {
            Day newDay;
            if (Profile.VacationDays.Any(d => d == today)
                || Calendar.IsHoliday(today)
                || Calendar.IsWeekend(today) && IsNotWorkingWeekend()
            )
            {
                newDay = new DayOff(today);
            }
            else
            {
                newDay = new Workday(today, Profile.Schedule);
            }
            OnInitializeNewDay(newDay, new List<(DateTime time, Action<Logger> activity)>());
        }

        private bool IsNotWorkingWeekend()
        {
            var roll = new Random().NextDouble();
            return roll >= Profile.WorkEthic.WorkWeekendPercent;
        }
    }
}