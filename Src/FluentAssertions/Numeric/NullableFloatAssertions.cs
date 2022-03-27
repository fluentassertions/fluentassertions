namespace FluentAssertions.Numeric
{
    internal class NullableFloatAssertions : NullableNumericAssertions<float>
    {
        public NullableFloatAssertions(float? value)
            : base(value)
        {
        }

        private protected override bool IsNaN(float value) => float.IsNaN(value);
    }
}
