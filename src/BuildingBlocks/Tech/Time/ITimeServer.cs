using System;

namespace Gherkin.BuildingBlocks.Tech.Time
{
    public interface ITimeServer
    {
        DateTime UtcNow();
        DateTime LocalTimeNow();
        TimeZoneInfo TimeZone();
    }
}
