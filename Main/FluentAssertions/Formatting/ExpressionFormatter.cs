using System;
using System.Linq.Expressions;

namespace FluentAssertions.Formatting
{
    internal class ExpressionFormatter : IFormatter
    {
        public bool CanHandle(object value)
        {
            return value is Expression;
        }

        public string ToString(object value)
        {
            return value.ToString();
        }
    }
}