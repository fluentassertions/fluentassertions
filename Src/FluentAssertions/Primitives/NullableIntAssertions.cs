using System.Diagnostics;
using FluentAssertions.Numeric;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="int"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class NullableIntAssertions
        : NullableNumericAssertions<int>
    {
        public NullableIntAssertions(int? value)
            : base(value)
        {
        }

        private protected override int? CalculateDifference(int? actual, int expected) => actual - expected;

        private protected override int? CalculateDifference(int? actual, int? expected) => actual - expected;
    }
}
