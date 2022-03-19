namespace FluentAssertions.Numeric
{
    internal class DoubleAssertions : NumericAssertions<double>
    {
        public DoubleAssertions(double value) 
            : base(value)
        {
        }

        private protected override bool IsNaN(double value) => double.IsNaN(value);
    }
}
