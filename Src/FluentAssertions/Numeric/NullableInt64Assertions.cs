using System;
using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="long"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    internal class NullableInt64Assertions : NullableNumericAssertions<long>
    {
        internal NullableInt64Assertions(long? value)
            : base(value)
        {
        }

        private protected override long? CalculateDifferenceForFailureMessage(long expected)
        {
            if (Subject is > 0 and < 10 && expected is > 0 and < 10)
            {
                return null;
            }

            try
            {
                var difference = checked(Subject - expected);
                return difference != 0 ? difference : null;
            }
            catch (OverflowException)
            {
                return null;
            }
        }
    }
}
