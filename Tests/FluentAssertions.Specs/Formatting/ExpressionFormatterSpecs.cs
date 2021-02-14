using System;
using System.Linq.Expressions;
using FluentAssertions.Formatting;
using Xunit;

namespace FluentAssertions.Specs.Formatting
{
    public class ExpressionFormatterSpecs
    {
        [Fact]
        public void Test1()
        {
            var a = "123";

            // Act
            string result = GetExpressionDescription<TestClass>(_ => _.PropertyA == "Hello" && _.PropertyB == a && _.PropertyB == "blah" && _.PropertyB == "meh");

            // Assert
            //result.Should().Be("<1973-09-20>");
        }

        private string GetExpressionDescription<T>(Expression<Func<T, bool>> predicate)
        {
            return AdvancedExpressionFormatter.FormatExpression(predicate);
        }

        private class TestClass
        {
            public string PropertyA { get; set; }

            public string PropertyB { get; set; }
        }
    }
}
