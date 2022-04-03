using System.Diagnostics;
using System.Globalization;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="byte"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    internal class NullableByteAssertions : NullableNumericAssertions<byte>
    {
        internal NullableByteAssertions(byte? value)
            : base(value)
        {
        }

        private protected override string CalculateDifferenceForFailureMessage(byte subject, byte expected)
        {
            int difference = subject - expected;
            return difference != 0 ? difference.ToString(CultureInfo.InvariantCulture) : null;
        }
    }
}
