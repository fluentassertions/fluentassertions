namespace FluentAssertions.Common
{
    internal static class ObjectExtensions
    {
        public static bool IsSameOrEqualTo(this object actual, object expected)
        {
            if (ReferenceEquals(actual, null) && ReferenceEquals(expected, null))
            {
                return true;
            }

            if (ReferenceEquals(actual, null))
            {
                return false;
            }

            return actual.Equals(expected);
        }
    }
}