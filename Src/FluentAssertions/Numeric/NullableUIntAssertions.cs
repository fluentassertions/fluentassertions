using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="uint"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class NullableUIntAssertions : NullableNumericAssertions<uint>
    {
        public NullableUIntAssertions(uint? value)
            : base(value)
        {
        }

        private protected override uint? CalculateDifferenceForFailureMessage(uint expected) => Subject - expected;

        private protected override uint GetMinimalDifferenceThresholdForFailureMessage() => 0;

        private protected override uint GetMaximalDifferenceThresholdForFailureMessage() => 10;
    }
}
