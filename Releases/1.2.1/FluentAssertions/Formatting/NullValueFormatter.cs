namespace FluentAssertions.Formatting
{
    internal class NullValueFormatter : IValueFormatter
    {
        public bool CanHandle(object value)
        {
            return ReferenceEquals(value, null);
        }

        public string ToString(object value)
        {
            return "<null>";
        }
    }
}