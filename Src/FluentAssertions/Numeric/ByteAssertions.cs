using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="byte"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class ByteAssertions : NumericAssertions<byte>
    {
        public ByteAssertions(byte value)
            : base(value)
        {
        }

        private protected override byte? CalculateDifferenceForFailureMessage(byte expected) => (byte?)(Subject - expected);

        private protected override byte GetMinimalDifferenceThresholdForFailureMessage() => 0;

        private protected override byte GetMaximalDifferenceThresholdForFailureMessage() => 0;
    }
}
