using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="long"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class LongAssertions : NumericAssertions<long>
    {
        public LongAssertions(long value)
            : base(value)
        {
        }

        private protected override long? CalculateDifferenceForFailureMessage(long expected) => Subject - expected;

        private protected override long GetMinimalDifferenceThresholdForFailureMessage() => -10;

        private protected override long GetMaximalDifferenceThresholdForFailureMessage() => 10;
    }
}
