using System;
using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="uint"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    internal class UInt32Assertions : NumericAssertions<uint>
    {
        internal UInt32Assertions(uint value)
            : base(value)
        {
        }

        private protected override uint? CalculateDifferenceForFailureMessage(uint expected)
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
