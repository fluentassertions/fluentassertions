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

        public string ToString(object value, int nestedPropertyLevel = 0)
        {
            var enumerable = ((IEnumerable)value).Cast<object>();
            if (enumerable.Any())
            {
                return "{" + string.Join(", ", enumerable.Select(Formatter.ToString).ToArray()) + "}";
            }
            else
            {
                return "{empty}";
            }
        }
    }
}