using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="int"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class NullableInt32Assertions : NullableNumericAssertions<int>
    {
        public NullableInt32Assertions(int? value)
            : base(value)
        {
        }

        private protected override int? CalculateDifferenceForFailureMessage(int expected)
        {
            if (Subject is > 0 and < 10 && expected is > 0 and < 10)
            {
                return null;
            }

            var difference = Subject - expected;
            return difference != 0 ? difference : null;
        }
    }
}
