using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="float"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class NullableFloatAssertions : NullableNumericAssertions<float>
    {
        public NullableFloatAssertions(float? value)
            : base(value)
        {
        }

        private protected override float? CalculateDifferenceForFailureMessage(float expected) => Subject - expected;

        private protected override float GetMinimalDifferenceThresholdForFailureMessage() => 0;

        private protected override float GetMaximalDifferenceThresholdForFailureMessage() => 0;
    }
}
