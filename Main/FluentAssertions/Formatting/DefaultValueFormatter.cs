namespace FluentAssertions.Formatting
{
    internal class DefaultValueFormatter : IValueFormatter
    {
        public bool CanHandle(object value)
        {
            return true;
        }

        public string ToString(object value)
        {
            return "<" + value + ">";
        }
    }
}