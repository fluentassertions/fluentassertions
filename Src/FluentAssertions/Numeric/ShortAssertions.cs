using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="short"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class ShortAssertions : NumericAssertions<short>
    {
        public ShortAssertions(short value)
            : base(value)
        {
        }

        private protected override short? CalculateDifferenceForFailureMessage(short expected)
        {
            if (Subject!.Value < 10 && expected < 10)
            {
                return null;
            }

            var difference = (short?)(Subject - expected);
            return difference != 0 ? difference : null;
        }
    }
}
