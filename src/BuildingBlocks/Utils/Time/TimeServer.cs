using System;

namespace Gherkin.BuildingBlocks.Utils.Time
{
    public class TimeServer : ITimeServer
    {
        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }
        
        public TimeZoneInfo TimeZone()
        {
            return TimeZoneInfo.Local;
        }

        public DateTime LocalTimeNow()
        {
            return DateTime.Now;
        }
    }
}
