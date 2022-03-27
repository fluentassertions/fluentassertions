using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="uint"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class NullableUInt32Assertions : NullableNumericAssertions<uint>
    {
        public NullableUInt32Assertions(uint? value)
            : base(value)
        {
        }

        private protected override uint? CalculateDifferenceForFailureMessage(uint expected)
        {
            if (Subject < 10 && expected < 10)
            {
                return null;
            }

            var difference = Subject - expected;
            return difference != 0 ? difference : null;
        }
    }
}
