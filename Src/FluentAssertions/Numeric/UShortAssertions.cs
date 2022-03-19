using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="ushort"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class UShortAssertions : NumericAssertions<ushort>
    {
        public UShortAssertions(ushort value)
            : base(value)
        {
        }

        private protected override ushort? CalculateDifference(ushort? actual, ushort expected) => (ushort?)(actual - expected);
    }
}
