using System.Diagnostics;
using System.Globalization;
using FluentAssertions.Execution;

namespace FluentAssertions.Numeric;

/// <summary>
/// Contains a number of methods to assert that a <see cref="ulong"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
internal class UInt64Assertions : NumericAssertions<ulong>
{
    internal UInt64Assertions(ulong value, AssertionChain assertionChain)
        : base(value, assertionChain)
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
