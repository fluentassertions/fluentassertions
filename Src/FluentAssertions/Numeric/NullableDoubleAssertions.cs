using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="double"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class NullableDoubleAssertions : NullableNumericAssertions<double>
    {
        public NullableDoubleAssertions(double? value)
            : base(value)
        {
        }

        private protected override double? CalculateDifferenceForFailureMessage(double expected)
        {
            var difference = Subject - expected;
            return difference != 0 ? difference : null;
        }
    }
}
