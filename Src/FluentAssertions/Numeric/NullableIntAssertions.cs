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

        private protected override int? CalculateDifferenceForFailureMessage(int expected)
        {
            if (Subject!.Value > 0 && Subject!.Value < 10 && expected > 0 && expected < 10)
            {
                return null;
            }

            var difference = Subject - expected;
            return difference != 0 ? difference : null;
        }
    }
}
