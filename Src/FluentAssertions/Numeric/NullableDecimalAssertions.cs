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

        private protected override decimal? CalculateDifferenceForFailureMessage(decimal expected) => Subject - expected;

        private protected override decimal GetMinimalDifferenceThresholdForFailureMessage() => 0;

        private protected override decimal GetMaximalDifferenceThresholdForFailureMessage() => 0;
    }
}
