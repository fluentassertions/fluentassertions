using System.Diagnostics;
using System.Globalization;
using FluentAssertions.Execution;

namespace FluentAssertions.Numeric;

/// <summary>
/// Contains a number of methods to assert that a <see cref="int"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
internal class Int32Assertions : NumericAssertions<int>
{
    internal Int32Assertions(int value, AssertionChain assertionChain)
        : base(value, assertionChain)
    {
    }

    private protected override string CalculateDifferenceForFailureMessage(int subject, int expected)
    {
        if (subject is > 0 and < 10 && expected is > 0 and < 10)
        {
            return null;
        }

        long difference = (long)subject - expected;
        return difference != 0 ? difference.ToString(CultureInfo.InvariantCulture) : null;
    }
}
