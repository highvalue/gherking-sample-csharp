namespace Gherkin.BuildingBlocks.Utils
{
   public static class StringExtensions
    {
        /// <summary>
        /// Shortcut for string.IsNullOrWhiteSpace(value)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Inverted IsEmpty()
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool NotEmpty(this string value)
        {
            return ! value.IsEmpty();
        }
    }
}
