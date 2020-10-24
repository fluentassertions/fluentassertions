using System.Diagnostics;
using FluentAssertions.Numeric;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="double"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class DoubleAssertions
        : NumericAssertions<double>
    {
        public DoubleAssertions(double value)
            : base(value)
        {
        }

        private protected override double? CalculateDifference(double? actual, double expected) => actual - expected;

        private protected override double? CalculateDifference(double? actual, double? expected) => actual - expected;
    }
}
