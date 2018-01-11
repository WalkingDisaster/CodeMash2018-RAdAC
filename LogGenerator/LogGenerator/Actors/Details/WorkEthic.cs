using System;

namespace LogGenerator.Actors.Details
{
    public class WorkEthic
    {
        public WorkEthic(
            double workWeekendPercent,
            double worksThroughLunchPercent,
            ScheduleAffinity arrive,
            ScheduleAffinity leave) =>
            (WorkWeekendPercent, WorksThroughLunch, Arrive, Leave) =
            (workWeekendPercent, worksThroughLunchPercent, arrive, leave);

        public double WorkWeekendPercent { get; set; }
        public double WorksThroughLunch { get; set; }
        public ScheduleAffinity Arrive { get; set; }
        public ScheduleAffinity Leave { get; set; }

        public TimeSpan CalcualteStart(TimeSpan target)
        {
            return CalculateTime(Arrive, target);
        }

        public TimeSpan CalcualteEnd(TimeSpan target)
        {
            return CalculateTime(Leave, target);
        }

        public (TimeSpan start, TimeSpan end) CalculateLunch(TimeSpan targetStart, TimeSpan targetEnd)
        {
            var random = new Random();
            if (random.NextDouble() < WorksThroughLunch)
            {
                return (TimeSpan.Zero, TimeSpan.Zero);
            }

            return (targetStart, targetEnd);
        }

        public TimeSpan CalculateTime(ScheduleAffinity affinity, TimeSpan target)
        {
            var random = new Random();

            var earlyLate = 0d;
            while (random.NextDouble() < affinity.EarlyPercent)
            {
                earlyLate -= 60;
            }
            while (random.NextDouble() < affinity.LatePercent)
            {
                earlyLate += 60;
            }

            earlyLate -= (random.NextDouble() * 5);
            while (random.NextDouble() < affinity.EarlyAffinity)
            {
                earlyLate -= random.NextDouble() * 5;
            }
            while (random.NextDouble() < affinity.LateAffinity)
            {
                earlyLate += random.NextDouble() * 5;
            }
            var result = target.Add(TimeSpan.FromMinutes(earlyLate));

            Console.WriteLine($"Scheduled Time: {target}. Actual Time: {result}");
            return result;
        }
    }
}