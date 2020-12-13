using System.Diagnostics;
using FluentAssertions.Numeric;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="byte"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class ByteAssertions
        : NumericAssertions<byte>
    {
        public ByteAssertions(byte value)
            : base(value)
        {
        }

        private protected override byte? CalculateDifference(byte? actual, byte expected) => (byte?)(actual - expected);

        private protected override byte? CalculateDifference(byte? actual, byte? expected) => (byte?)(actual - expected);
    }
}
