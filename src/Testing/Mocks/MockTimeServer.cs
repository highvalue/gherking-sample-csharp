using System;
using Gherkin.BuildingBlocks.Utils.Time;

namespace Gherkin.Testing.Mocks
{
    public class MockTimeServer : ITimeServer
    {

        private TimeData _timeData;

        public MockTimeServer(TimeData timeData)
        {
            _timeData = timeData;
        }

        public DateTime LocalTimeNow()
        {
            return _timeData.LocalTimeNow;
        }

        public TimeZoneInfo TimeZone()
        {
            return _timeData.TimeZone;
        }

        public DateTime UtcNow()
        {
            return _timeData.UtcNow;
        }
    }

    public class TimeData
    {
        public DateTime LocalTimeNow { get; set; }
        public TimeZoneInfo TimeZone { get; set; }
        public DateTime UtcNow { get; set; }
    }
}
