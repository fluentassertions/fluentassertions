using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Formatting;

namespace FluentAssertions
{
    public static partial class AssertionExtensions
    {
        static AssertionExtensions()
        {
            Services.ReflectionProvider = new WPReflectionProvider();
            Services.TestFramework = TestFrameworkProvider.TestFramework;

            Formatter.AddFormatter(new AggregateExceptionValueFormatter());
        }
    }
}