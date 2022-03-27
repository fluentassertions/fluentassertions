using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="ushort"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class UShortAssertions : NumericAssertions<ushort>
    {
        public UShortAssertions(ushort value)
            : base(value)
        {
        }

        private protected override ushort? CalculateDifferenceForFailureMessage(ushort expected)
        {
            if (Subject < 10 && expected < 10)
            {
                return null;
            }

            var difference = (ushort?)(Subject - expected);
            return difference != 0 ? difference : null;
        }
    }
}
