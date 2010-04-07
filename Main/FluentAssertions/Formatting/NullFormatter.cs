namespace FluentAssertions.Formatting
{
    internal class NullFormatter : IFormatter
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