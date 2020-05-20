using System;

namespace Gherkin.BuildingBlocks.Neutral.Time
{
    public interface ITimeServer
    {
        DateTime UtcNow();
        DateTime LocalTimeNow();
        TimeZoneInfo TimeZone();
    }
}
