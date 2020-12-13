using System.Diagnostics;
using FluentAssertions.Numeric;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="short"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class ShortAssertions
        : NumericAssertions<short>
    {
        public ShortAssertions(short value)
            : base(value)
        {
        }

        private protected override short? CalculateDifference(short? actual, short expected) => (short?)(actual - expected);

        private protected override short? CalculateDifference(short? actual, short? expected) => (short?)(actual - expected);
    }
}
