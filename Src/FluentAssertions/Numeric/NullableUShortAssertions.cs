using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="ushort"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class NullableUShortAssertions : NullableNumericAssertions<ushort>
    {
        public NullableUShortAssertions(ushort? value)
            : base(value)
        {
        }

        private protected override ushort? CalculateDifferenceForFailureMessage(ushort expected) => (ushort?)(Subject - expected);

        private protected override ushort GetMinimalDifferenceThresholdForFailureMessage() => 0;

        private protected override ushort GetMaximalDifferenceThresholdForFailureMessage() => 10;
    }
}
