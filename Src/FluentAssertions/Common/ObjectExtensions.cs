using System;

namespace FluentAssertions.Common
{
    public static class ObjectExtensions
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

            if (ReferenceEquals(expected, null))
            {
                return false;
            }

            if (actual.Equals(expected))
            {
                return true;
            }

            try
            {
                if (expected.GetType() != typeof(string) && actual.GetType() != typeof(string))
                {
                    var convertedActual = Convert.ChangeType(actual, expected.GetType());

                    return convertedActual.Equals(expected);
                }
            }
            catch
            {
                // ignored
            }

            return false;
        }
    }
}
