namespace LogGenerator.Logging
{
    public class AggregatedLogRecord
    {
        public string User {get;set;}
        public string Role {get;set;}
        public string MandatoryAccess {get;set;}
        public string DiscretionaryAccess {get;set;}
        public string IpAddress {get;set;}
        public int DayOfWeek {get;set;}
        public int DayOfMonth {get;set;}
        public int HourOfDay {get;set;}
        public bool IsWeekend {get;set;}
        public bool IsHoliday {get;set;}
        public double UserSecondsSinceLastRequest {get;set;}
        public int UserRequestsInLastMinute {get;set;}
        public int UserRequestsInLastTenMinutes {get;set;}
        public int AllRequestsInLastMinute {get;set;}
        public int AllRequestsInLastHour {get;set;}
        public int AllRequestsInLastDay {get;set;}

        public override string ToString()
        {
            return $"1,{User},{Role},{MandatoryAccess},{DiscretionaryAccess},{IpAddress},{DayOfWeek},{DayOfMonth},{HourOfDay},{IsWeekend},{IsHoliday},{UserSecondsSinceLastRequest},{UserRequestsInLastMinute},{UserRequestsInLastTenMinutes},{AllRequestsInLastMinute},{AllRequestsInLastHour},{AllRequestsInLastDay}";
        }

        public static string GetHeader()
        {
            return $"Label,User,Role,MandatoryAccess,DiscretionaryAccess,IpAddress,DayOfWeek,DayOfMonth,HourOfDay,IsWeekend,IsHoliday,UserSecondsSinceLastRequest,UserRequestsInLastMinute,UserRequestsInLastTenMinutes,AllRequestsInLastMinute,AllRequestsInLastHour,AllRequestsInLastDay";
        }
    }
}