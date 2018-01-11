using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LogGenerator.Actors;
using LogGenerator.Actors.Archetypes;
using LogGenerator.Actors.Details;
using LogGenerator.DateUtil;
using LogGenerator.Logging;

namespace LogGenerator.Console
{
    public class Program
    {
        private static ICalendar Calendar = new CalendarWrapper(2017, new DateTime[]
        {
            new DateTime(2017, 1, 2),
            new DateTime(2017, 1, 16),
            new DateTime(2017, 2, 10),
            new DateTime(2017, 5, 29),
            new DateTime(2017, 7, 4),
            new DateTime(2017, 9, 4),
            new DateTime(2017, 10, 9),
            new DateTime(2017, 11, 10),
            new DateTime(2017, 11, 23),
            new DateTime(2017, 12, 25),
        });

        private const double MinutesPerSecond = 2880;

        public static async Task Main(string[] args)
        {
//            await MakeyLoggey();
            await ReduceyLoggey(Calendar);
//            await Testadilly.InvokeRequestResponseService();
        }

        private static async Task ReduceyLoggey(ICalendar calendar)
        {
            const string filePath = @"C:\source\Logs\access-log.csv";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (var reader = new StreamReader(@"C:\source\Logs\access-log.txt"))
            using (var writer = new StreamWriter(filePath))
            {
                await writer.WriteLineAsync(AggregatedLogRecord.GetHeader());
                DateTime? dateOfFirstTrackdEvent = null;
                var eventQueue = new Queue<LogRecord>();

                // ReSharper disable once RedundantAssignment
                var line = await reader.ReadLineAsync(); // header

                line = await reader.ReadLineAsync();
                while (line != null)
                {
                    var fields = line.Split(',');
                    if (!DateTime.TryParse(fields[0], out var eventDateTime))
                    {
                        throw new ArgumentException($"Bad date format {fields[0]}", nameof(eventDateTime));
                    }

                    var eventDate = eventDateTime.Date;

                    if (!dateOfFirstTrackdEvent.HasValue)
                    {
                        dateOfFirstTrackdEvent = eventDateTime.AddDays(1);
                    }

                    var logRecord = new LogRecord(eventDateTime, fields[1], fields[2], fields[3], fields[4], fields[5]);

                    if (eventDateTime >= dateOfFirstTrackdEvent)
                    {
                        var oneDayAgo = eventDateTime.AddDays(-1);
                        while (eventQueue.Count > 0 && eventQueue.Peek().EventTime < oneDayAgo)
                        {
                            eventQueue.Dequeue();
                        }

                        var record = new AggregatedLogRecord
                        {
                            User = logRecord.User,
                            Role = logRecord.Role,
                            MandatoryAccess = logRecord.Request,
                            DiscretionaryAccess = logRecord.Workflow,
                            IpAddress = logRecord.IpAddress,
                            DayOfMonth = eventDate.Day,
                            DayOfWeek = (int) eventDate.DayOfWeek,
                            HourOfDay = eventDateTime.Hour,
                            IsWeekend = calendar.IsWeekend(eventDate),
                            IsHoliday = calendar.IsHoliday(eventDate)
                        };
                        var userSearch = eventQueue.Where(x => x.User == record.User).OrderBy(x => x.EventTime).ToList();
                        var allSearch = eventQueue.OrderBy(x => x.EventTime).ToList();
                        if (userSearch.Any())
                        {
                            var lastEvent = userSearch.Last();
                            var timeSinceLastEvent = eventDateTime.Subtract(lastEvent.EventTime);
                            record.UserSecondsSinceLastRequest = timeSinceLastEvent.TotalSeconds;
                        }
                        else
                        {
                            record.UserSecondsSinceLastRequest = 0;
                        }

                        var oneMinutePrior = eventDateTime.AddMinutes(-1);
                        var tenMinutesPrior = eventDateTime.AddMinutes(-10);
                        var oneHourPrior = eventDateTime.AddHours(-1);

                        record.UserRequestsInLastMinute = userSearch.Count(x => x.EventTime > oneMinutePrior);
                        record.UserRequestsInLastTenMinutes = userSearch.Count(x => x.EventTime > tenMinutesPrior);

                        record.AllRequestsInLastMinute = allSearch.Count(x => x.EventTime > oneMinutePrior);
                        record.AllRequestsInLastHour = allSearch.Count(x => x.EventTime > oneHourPrior);
                        record.AllRequestsInLastDay = allSearch.Count;

                        await writer.WriteLineAsync(record.ToString());
                    }

                    eventQueue.Enqueue(logRecord);
                    line = await reader.ReadLineAsync();
                }
            }
        }

        private static async Task MakeyLoggey()
        {
            var logger = new Logger();
            var start = new DateTimeOffset(2017, 1, 1, 0, 0, 0, 0, TimeSpan.FromHours(-5));
            var startUtc = start.UtcDateTime;
            var dateTime = new DateTimeWrapper(startUtc);
            var time = new TimeDilationClock(MinutesPerSecond, start, dateTime);
            var stopTime = new DateTimeOffset(2018, 1, 1, 0, 0, 0, 0, TimeSpan.FromHours(-5));
            var now = time.GetCurrentLocalDateTime();
            var date = now.Date;
            System.Console.WriteLine($"{date} ({now.Offset})");
            IEnumerable<IActor> actors = new IActor[]
            {
                new DrCurlyHoward(Calendar),
                new DrLarryFine(Calendar),
                new DrShempHoward(Calendar)
            };
            while (now < stopTime)
            {
                date = await Tick(actors, now, date, logger);
                now = time.GetCurrentLocalDateTime();
            }
        }

        private static async Task<DateTime> Tick(IEnumerable<IActor> actors, DateTimeOffset now, DateTime lastDate, Logger logger)
        {
            var newDate = now.Date;
            if (newDate > lastDate)
            {
                lastDate = newDate;
                await logger.Purge();
                System.Console.WriteLine($"{lastDate} ({now.Offset})");
            }

            foreach (var actor in actors)
            {
                actor.Do(now.LocalDateTime, logger);
            }
            await Task.Run(() => System.Threading.Thread.Sleep(20));
            return lastDate;
        }
    }
}
