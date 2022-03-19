using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="decimal"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class DecimalAssertions : NumericAssertions<decimal>
    {
        public DecimalAssertions(decimal value)
            : base(value)
        {
        }

        private protected override decimal? CalculateDifference(decimal? actual, decimal expected) => actual - expected;
    }
}
