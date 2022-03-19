using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="uint"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class UIntAssertions : NumericAssertions<uint>
    {
        public UIntAssertions(uint value)
            : base(value)
        {
        }

        private protected override uint? CalculateDifference(uint? actual, uint expected) => actual - expected;
    }
}
