using System;
using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="int"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    internal class Int32Assertions : NumericAssertions<int>
    {
        internal Int32Assertions(int value)
            : base(value)
        {
        }

        private protected override int? CalculateDifferenceForFailureMessage(int expected)
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
