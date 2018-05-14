using System;

namespace FluentAssertions.Common
{
    public static class ObjectExtensions
    {
        public static bool IsSameOrEqualTo(this object actual, object expected)
        {
            if (actual is null && expected is null)
            {
                return true;
            }

            if (actual is null)
            {
                return false;
            }

            if (expected is null)
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
