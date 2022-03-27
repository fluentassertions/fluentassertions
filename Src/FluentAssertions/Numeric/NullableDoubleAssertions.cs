namespace FluentAssertions.Numeric
{
    internal class NullableDoubleAssertions : NullableNumericAssertions<double>
    {
        public NullableDoubleAssertions(double? value)
            : base(value)
        {
        }

        private protected override bool IsNaN(double value) => double.IsNaN(value);
    }
}
