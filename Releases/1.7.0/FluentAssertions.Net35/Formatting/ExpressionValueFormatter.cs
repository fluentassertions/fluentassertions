using System.Linq.Expressions;

using FluentAssertions.Common;

namespace FluentAssertions.Formatting
{
    internal class ExpressionValueFormatter : IValueFormatter
    {
        public bool CanHandle(object value)
        {
            return value is Expression;
        }

        public string ToString(object value, UniqueObjectTracker uniqueObjectTracker, int nestedPropertyLevel = 0)
        {
            return value.ToString();
        }
    }
}