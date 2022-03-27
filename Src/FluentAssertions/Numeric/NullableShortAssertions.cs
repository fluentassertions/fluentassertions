using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <see cref="short"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class NullableShortAssertions : NullableNumericAssertions<short>
    {
        public NullableShortAssertions(short? value)
            : base(value)
        {
        }

        private protected override short? CalculateDifferenceForFailureMessage(short expected)
        {
            if (Subject < 10 && expected < 10)
            {
                return null;
            }

            var difference = (short?)(Subject - expected);
            return difference != 0 ? difference : null;
        }
    }
}
