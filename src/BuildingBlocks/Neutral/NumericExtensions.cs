namespace Gherkin.BuildingBlocks.Neutral
{
    public static class NumericExtensions
    {
        public static bool InRange(this int value, int min, int max)
        {
            return (min <= value) && (value <= max);
        }

        public static bool InRange(this byte value, byte min, byte max)
        {
            return (min <= value) && (value <= max);
        }

        public static bool InRange(this long value, long min, long max)
        {
            return (min <= value) && (value <= max);
        }
    }
}
