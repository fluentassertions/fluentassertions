using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="double"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    internal class NullableDoubleAssertions : NullableNumericAssertions<double>
    {
        internal NullableDoubleAssertions(double? value)
            : base(value)
        {
        }

        private protected override bool IsNaN(double value) => double.IsNaN(value);

        private protected override double? CalculateDifferenceForFailureMessage(double expected)
        {
            var difference = Subject - expected;
            return difference != 0 ? difference : null;
        }
    }
}
