using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="decimal"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class NullableDecimalAssertions : NullableNumericAssertions<decimal>
    {
        public NullableDecimalAssertions(decimal? value)
            : base(value)
        {
        }

        private protected override decimal? CalculateDifference(decimal? actual, decimal expected) => actual - expected;
    }
}
