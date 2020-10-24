using System.Diagnostics;
using FluentAssertions.Numeric;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="ushort"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class NullableUShortAssertions
        : NullableNumericAssertions<ushort>
    {
        public NullableUShortAssertions(ushort? value)
            : base(value)
        {
        }

        private protected override ushort? CalculateDifference(ushort? actual, ushort expected) => (ushort?)(actual - expected);

        private protected override ushort? CalculateDifference(ushort? actual, ushort? expected) => (ushort?)(actual - expected);
    }
}
