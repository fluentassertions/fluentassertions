using System.Diagnostics;
using FluentAssertions.Numeric;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="byte"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class NullableByteAssertions
        : NullableNumericAssertions<byte>
    {
        public NullableByteAssertions(byte? value)
            : base(value)
        {
        }

        private protected override byte? CalculateDifference(byte? actual, byte expected) => (byte?)(actual - expected);

        private protected override byte? CalculateDifference(byte? actual, byte? expected) => (byte?)(actual - expected);
    }
}
