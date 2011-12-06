using FluentAssertions.Common;

namespace FluentAssertions.Formatting
{
    internal class NullValueFormatter : IValueFormatter
    {
        public bool CanHandle(object value)
        {
            return ReferenceEquals(value, null);
        }

        public string ToString(object value, UniqueObjectTracker uniqueObjectTracker, int nestedPropertyLevel = 0)
        {
            return "<null>";
        }
    }
}