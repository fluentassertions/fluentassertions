using System.Diagnostics;
using System.Globalization;
using FluentAssertions.Execution;

namespace FluentAssertions.Numeric;

/// <summary>
/// Contains a number of methods to assert that a <see cref="double"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
internal class DoubleAssertions : NumericAssertions<double>
{
    internal DoubleAssertions(double value, AssertionChain assertionChain)
        : base(value, assertionChain)
    {
    }

    private protected override bool IsNaN(double value) => double.IsNaN(value);

    private protected override string CalculateDifferenceForFailureMessage(double subject, double expected)
    {
        var difference = subject - expected;
        return difference != 0 ? difference.ToString("R", CultureInfo.InvariantCulture) : null;
    }
}
