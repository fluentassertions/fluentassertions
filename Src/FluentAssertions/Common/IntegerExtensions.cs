namespace FluentAssertions.Common
{
    internal static class IntegerExtensions
    {
        public static string Times(this int count)
        {
            return count == 1 ? "1 time" : $"{count} times";
        }
    }
}
