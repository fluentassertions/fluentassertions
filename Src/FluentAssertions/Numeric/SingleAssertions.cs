using System.Diagnostics;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="float"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    internal class SingleAssertions : NumericAssertions<float>
    {
        internal SingleAssertions(float value)
            : base(value)
        {
        }

        private protected override bool IsNaN(float value) => float.IsNaN(value);

        private protected override float? CalculateDifferenceForFailureMessage(float expected)
        {
            var difference = Subject - expected;
            return difference != 0 ? difference : null;
        }
    }
}
