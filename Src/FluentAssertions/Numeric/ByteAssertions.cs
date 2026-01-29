using System.Diagnostics;
using System.Globalization;
using FluentAssertions.Execution;

namespace FluentAssertions.Numeric;

/// <summary>
/// Contains a number of methods to assert that a <see cref="byte"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
internal class ByteAssertions : NumericAssertions<byte>
{
    internal ByteAssertions(byte value, AssertionChain assertionChain)
        : base(value, assertionChain)
    {
    }

    private protected override string CalculateDifferenceForFailureMessage(byte subject, byte expected)
    {
        int difference = subject - expected;
        return difference != 0 ? difference.ToString(CultureInfo.InvariantCulture) : null;
    }
}
