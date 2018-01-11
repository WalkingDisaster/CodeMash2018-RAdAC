namespace LogGenerator.Actors.Details
{
    public class ScheduleAffinity
    {
        public ScheduleAffinity(double earlyPercent, double latePercent, double earlyAffinity, double lateAffinity) =>
            (EarlyPercent, LatePercent, EarlyAffinity, LateAffinity) =
            (earlyPercent, latePercent, earlyAffinity, lateAffinity);
        public double EarlyPercent { get; set; }
        public double LatePercent { get; set; }
        public double EarlyAffinity { get; set; }
        public double LateAffinity { get; set; }
    }
}