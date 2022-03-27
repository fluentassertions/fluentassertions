using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="long"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class Int64Assertions : NumericAssertions<long>
    {
        public Int64Assertions(long value)
            : base(value)
        {
        }

        private protected override long? CalculateDifferenceForFailureMessage(long expected)
        {
            if (Subject is > 0 and < 10 && expected is > 0 and < 10)
            {
                return null;
            }

            var difference = Subject - expected;
            return difference != 0 ? difference : null;
        }
    }
}
