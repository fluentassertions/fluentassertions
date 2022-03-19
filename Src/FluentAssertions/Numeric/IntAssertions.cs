using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="int"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class IntAssertions : NumericAssertions<int>
    {
        public IntAssertions(int value)
            : base(value)
        {
        }

        private protected override int? CalculateDifference(int? actual, int expected) => actual - expected;
    }
}
