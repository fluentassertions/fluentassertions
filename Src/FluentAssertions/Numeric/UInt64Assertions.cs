using System;
using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="ulong"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    internal class UInt64Assertions : NumericAssertions<ulong>
    {
        internal UInt64Assertions(ulong value)
            : base(value)
        {
        }

        private protected override ulong? CalculateDifferenceForFailureMessage(ulong expected)
        {
            if (Subject < 10 && expected < 10)
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
