using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="int"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class NullableIntAssertions : NullableNumericAssertions<int>
    {
        public NullableIntAssertions(int? value)
            : base(value)
        {
        }

        private protected override int? CalculateDifference(int? actual, int expected) => actual - expected;
    }
}
