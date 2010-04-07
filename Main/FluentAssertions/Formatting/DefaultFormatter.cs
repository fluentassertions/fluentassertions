namespace FluentAssertions.Formatting
{
    internal class DefaultFormatter : IFormatter
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