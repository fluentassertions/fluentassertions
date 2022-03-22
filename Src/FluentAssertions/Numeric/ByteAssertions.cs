﻿using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="byte"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class ByteAssertions : NumericAssertions<byte>
    {
        public ByteAssertions(byte value)
            : base(value)
        {
        }

        private protected override byte? CalculateDifferenceForFailureMessage(byte expected)
        {
            var difference = (byte?)(Subject - expected);
            return difference != 0 ? difference : null;
        }
    }
}