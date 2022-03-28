using System;
using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="decimal"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    internal class DecimalAssertions : NumericAssertions<decimal>
    {
        internal DecimalAssertions(decimal value)
            : base(value)
        {
        }

        private protected override decimal? CalculateDifferenceForFailureMessage(decimal expected)
        {
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
