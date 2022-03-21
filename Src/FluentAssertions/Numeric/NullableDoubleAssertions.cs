using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="double"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class NullableDoubleAssertions : NullableNumericAssertions<double>
    {
        public NullableDoubleAssertions(double? value)
            : base(value)
        {
        }

        private protected override double? CalculateDifferenceForFailureMessage(double expected) => Subject - expected;

        private protected override double GetMinimalDifferenceThresholdForFailureMessage() => 0;

        private protected override double GetMaximalDifferenceThresholdForFailureMessage() => 0;
    }
}
