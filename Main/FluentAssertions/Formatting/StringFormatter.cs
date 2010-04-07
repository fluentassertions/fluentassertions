namespace FluentAssertions.Formatting
{
    internal class StringFormatter : IFormatter
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