namespace FluentAssertions.Formatting
{
    internal class StringValueFormatter : IValueFormatter
    {
        public bool CanHandle(object value)
        {
            return value is string;
        }

        public string ToString(object value)
        {
            return "\"" + value.ToString().Replace("\"", "\\\"") + "\"";
        }
    }
}