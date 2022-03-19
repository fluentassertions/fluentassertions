using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="ulong"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class NullableULongAssertions : NullableNumericAssertions<ulong>
    {
        public NullableULongAssertions(ulong? value)
            : base(value)
        {
        }

        private protected override ulong? CalculateDifference(ulong? actual, ulong expected) => actual - expected;
    }
}
