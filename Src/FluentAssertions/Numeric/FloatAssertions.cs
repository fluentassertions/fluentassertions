namespace FluentAssertions.Numeric
{
    internal class FloatAssertions : NumericAssertions<float>
    {
        public FloatAssertions(float value)
            : base(value)
        {
        }

        private protected override bool IsNaN(float value) => float.IsNaN(value);
    }
}
