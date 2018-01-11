using System;
using LogGenerator;
using LogGenerator.DateUtil;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LogGeneratorTests
{
    [TestClass]
    public class UnitTest1
    {
        private const string EasternTime = "Eastern Standard Time";
        private TimeZoneInfo CurrentTimeZone { get; set; }
 
        private Mock<IDateTime> DateTimeMock { get; set; }

        private void SetLocalTimeZone(string timeZone)
        {
            CurrentTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
        }

        private DateTimeOffset GetLocalDateTime(DateTime utcDateTime)
        {
            return TimeZoneInfo.ConvertTime(utcDateTime, CurrentTimeZone);
        }

        private DateTime ConvertLocalToUtc(int year,
            int month,
            int day,
            int hours = 0,
            int minutes = 0,
            int seconds = 0,
            int milliseconds = 0
        )
        {
            var localDate = new DateTime(year, month, day, hours, minutes, seconds, milliseconds, DateTimeKind.Unspecified);
            var offset = CurrentTimeZone.GetUtcOffset(localDate);
            var withOffset = new DateTimeOffset(localDate, offset);
            return withOffset.UtcDateTime;
        }

        private TimeSpan GetOffset(DateTime startTime, int minutes)
        {
            var newTime = startTime.AddMinutes(minutes);
            return GetLocalDateTime(newTime).Offset;
        }


        [TestInitialize]
        public void Setup()
        {
            DateTimeMock = new Mock<IDateTime>();
        }

        [TestMethod]
        public void OneMinute()
        {
            SetLocalTimeZone(EasternTime);
            var startTimeUtc = ConvertLocalToUtc(2001, 1, 1);

            DateTimeMock.Setup(x => x.UtcNow).Returns(startTimeUtc.AddSeconds(1));
            DateTimeMock.Setup(x => x.GetLocalTime(startTimeUtc.AddMinutes(1))).Returns(GetLocalDateTime(startTimeUtc.AddMinutes(1)));
            var dateTime = DateTimeMock.Object;

            var expected = GetLocalDateTime(startTimeUtc.AddMinutes(1));
            var sut = new TimeDilationClock(1, startTimeUtc, dateTime);
            var result = sut.GetCurrentLocalDateTime();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DaylightSavingsSwitch()
        {
            SetLocalTimeZone(EasternTime);
            var startTimeUtc = ConvertLocalToUtc(2018, 3, 11, 1, 58, 30);

            DateTimeMock.SetupSequence(x => x.UtcNow)
                .Returns(startTimeUtc.AddSeconds(1))
                .Returns(startTimeUtc.AddSeconds(2));
            DateTimeMock.Setup(x => x.GetLocalTime(startTimeUtc.AddMinutes(1)))
                .Returns(GetLocalDateTime(startTimeUtc.AddMinutes(1)));
            DateTimeMock.Setup(x => x.GetLocalTime(startTimeUtc.AddMinutes(2)))
                .Returns(GetLocalDateTime(startTimeUtc.AddMinutes(2)));
            var dateTime = DateTimeMock.Object;

            var sut = new TimeDilationClock(1, startTimeUtc, dateTime);

            var expected = GetLocalDateTime(startTimeUtc.AddMinutes(1));
            var result = sut.GetCurrentLocalDateTime();
            Assert.AreEqual(expected, result);

            expected = GetLocalDateTime(startTimeUtc.AddMinutes(2));
            result = sut.GetCurrentLocalDateTime();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void StandardTimeSwitch()
        {
            SetLocalTimeZone(EasternTime);
            var startTimeUtc = ConvertLocalToUtc(2018, 11, 4, 0, 58, 30);

            DateTimeMock.SetupSequence(x => x.UtcNow)
                .Returns(startTimeUtc.AddSeconds(1))
                .Returns(startTimeUtc.AddSeconds(2))
                .Returns(startTimeUtc.AddSeconds(62));
            DateTimeMock.Setup(x => x.GetLocalTime(startTimeUtc.AddMinutes(1)))
                .Returns(GetLocalDateTime(startTimeUtc.AddMinutes(1)));
            DateTimeMock.Setup(x => x.GetLocalTime(startTimeUtc.AddMinutes(2)))
                .Returns(GetLocalDateTime(startTimeUtc.AddMinutes(2)));
            DateTimeMock.Setup(x => x.GetLocalTime(startTimeUtc.AddMinutes(62)))
                .Returns(GetLocalDateTime(startTimeUtc.AddMinutes(62)));
            var dateTime = DateTimeMock.Object;
            
            var sut = new TimeDilationClock(1, startTimeUtc, dateTime);

            var expected = GetLocalDateTime(startTimeUtc.AddMinutes(1));
            var result = sut.GetCurrentLocalDateTime();
            Assert.AreEqual(expected, result);

            expected = GetLocalDateTime(startTimeUtc.AddMinutes(2));
            result = sut.GetCurrentLocalDateTime();
            Assert.AreEqual(expected, result);

            expected = GetLocalDateTime(startTimeUtc.AddMinutes(62));
            result = sut.GetCurrentLocalDateTime();
            Assert.AreEqual(expected, result);
        }
    }
}