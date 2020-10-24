using System.Diagnostics;
using FluentAssertions.Numeric;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="uint"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class NullableUIntAssertions
        : NullableNumericAssertions<uint>
    {
        public NullableUIntAssertions(uint? value)
            : base(value)
        {
        }

        private protected override uint? CalculateDifference(uint? actual, uint? expected) => actual - expected;

        private protected override uint? CalculateDifference(uint? actual, uint expected) => actual - expected;
    }
}
