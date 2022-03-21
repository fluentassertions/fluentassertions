using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="byte"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class NullableByteAssertions : NullableNumericAssertions<byte>
    {
        public NullableByteAssertions(byte? value)
            : base(value)
        {
        }

        private protected override byte? CalculateDifferenceForFailureMessage(byte expected) => (byte?)(Subject - expected);

        private protected override byte GetMinimalDifferenceThresholdForFailureMessage() => 0;

        private protected override byte GetMaximalDifferenceThresholdForFailureMessage() => 0;
    }
}
