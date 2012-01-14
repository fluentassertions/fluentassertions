using System.Collections;
using System.Linq;

namespace FluentAssertions.Formatting
{
    internal class EnumerableValueFormatter : IValueFormatter
    {
        public bool CanHandle(object value)
        {
            return value is IEnumerable;
        }

        public string ToString(object value)
        {
            var enumerable = ((IEnumerable)value).Cast<object>();
            if (enumerable.Count() > 0)
            {
                return "<" + string.Join(", ", enumerable.Select(o => o.ToString()).ToArray()) + ">";
            }
            else
            {
                return "<empty collection>";
            }
        }
    }
}