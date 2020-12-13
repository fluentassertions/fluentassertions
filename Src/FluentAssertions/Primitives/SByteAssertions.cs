using System.Diagnostics;
using FluentAssertions.Numeric;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="sbyte"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class SByteAssertions
        : NumericAssertions<sbyte>
    {
        public SByteAssertions(sbyte value)
            : base(value)
        {
        }

        private protected override sbyte? CalculateDifference(sbyte? actual, sbyte expected) => (sbyte?)(actual - expected);

        private protected override sbyte? CalculateDifference(sbyte? actual, sbyte? expected) => (sbyte?)(actual - expected);
    }
}
