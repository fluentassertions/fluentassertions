using System.Diagnostics;
using System.Globalization;
using FluentAssertions.Execution;

namespace FluentAssertions.Numeric;

/// <summary>
/// Contains a number of methods to assert that a <see cref="uint"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
internal class UInt32Assertions : NumericAssertions<uint>
{
    internal UInt32Assertions(uint value, AssertionChain assertionChain)
        : base(value, assertionChain)
    {
    }

    private protected override string CalculateDifferenceForFailureMessage(uint subject, uint expected)
    {
        if (subject < 10 && expected < 10)
        {
            return null;
        }

        long difference = (long)subject - expected;
        return difference != 0 ? difference.ToString(CultureInfo.InvariantCulture) : null;
    }
}
