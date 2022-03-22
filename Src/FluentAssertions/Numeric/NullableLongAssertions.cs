using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="long"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class NullableLongAssertions : NullableNumericAssertions<long>
    {
        public NullableLongAssertions(long? value)
            : base(value)
        {
        }

        private protected override long? CalculateDifferenceForFailureMessage(long expected)
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
