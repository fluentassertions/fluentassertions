using System.Diagnostics;
using System.Globalization;

namespace FluentAssertionsAsync.Numeric;

/// <summary>
/// Contains a number of methods to assert that a nullable <see cref="ulong"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
internal class NullableUInt64Assertions : NullableNumericAssertions<ulong>
{
    internal NullableUInt64Assertions(ulong? value)
        : base(value)
    {
    }

    private protected override string CalculateDifferenceForFailureMessage(ulong subject, ulong expected)
    {
        if (subject < 10 && expected < 10)
        {
            return null;
        }

        decimal difference = (decimal)subject - expected;
        return difference != 0 ? difference.ToString(CultureInfo.InvariantCulture) : null;
    }
}
