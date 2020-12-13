using System.Diagnostics;
using FluentAssertions.Numeric;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="ushort"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class UShortAssertions
        : NumericAssertions<ushort>
    {
        public UShortAssertions(ushort value)
            : base(value)
        {
        }

        private protected override ushort? CalculateDifference(ushort? actual, ushort expected) => (ushort?)(actual - expected);

        private protected override ushort? CalculateDifference(ushort? actual, ushort? expected) => (ushort?)(actual - expected);
    }
}
