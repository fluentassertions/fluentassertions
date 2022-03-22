﻿using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="ulong"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class NullableULongAssertions : NullableNumericAssertions<ulong>
    {
        public NullableULongAssertions(ulong? value)
            : base(value)
        {
        }

        private protected override ulong? CalculateDifferenceForFailureMessage(ulong expected)
        {
            if (Subject!.Value < 10 && expected < 10)
            {
                return null;
            }

            var difference = Subject - expected;
            return difference != 0 ? difference : null;
        }
    }
}