using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="sbyte"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class SByteAssertions : NumericAssertions<sbyte>
    {
        public SByteAssertions(sbyte value)
            : base(value)
        {
        }

        private protected override sbyte? CalculateDifferenceForFailureMessage(sbyte expected)
        {
            var difference = (sbyte?)(Subject - expected);
            return difference != 0 ? difference : null;
        }
    }
}
