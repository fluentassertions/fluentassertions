using System.Diagnostics;
using System.Globalization;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="long"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    internal class Int64Assertions : NumericAssertions<long>
    {
        internal Int64Assertions(long value)
            : base(value)
        {
        }

        private protected override string CalculateDifferenceForFailureMessage(long subject, long expected)
        {
            if (subject is > 0 and < 10 && expected is > 0 and < 10)
            {
                return null;
            }

            decimal difference = (decimal)subject - expected;
            return difference != 0 ? difference.ToString(CultureInfo.InvariantCulture) : null;
        }
    }
}
