using System;

namespace Gherkin.BuildingBlocks.Utils.Time
{
    public interface ITimeServer
    {
        DateTime UtcNow();
        DateTime LocalTimeNow();
        TimeZoneInfo TimeZone();
    }
}
