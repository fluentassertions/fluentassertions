using System.Diagnostics;
using System.Globalization;

namespace FluentAssertionsAsync.Numeric;

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

    private protected override string CalculateDifferenceForFailureMessage(double subject, double expected)
    {
        double difference = subject - expected;
        return difference != 0 ? difference.ToString("R", CultureInfo.InvariantCulture) : null;
    }
}
