using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="ulong"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class ULongAssertions : NumericAssertions<ulong>
    {
        public ULongAssertions(ulong value)
            : base(value)
        {
        }

        private protected override ulong? CalculateDifferenceForFailureMessage(ulong expected) => Subject - expected;

        private protected override ulong GetMinimalDifferenceThresholdForFailureMessage() => 0;

        private protected override ulong GetMaximalDifferenceThresholdForFailureMessage() => 10;
    }
}
