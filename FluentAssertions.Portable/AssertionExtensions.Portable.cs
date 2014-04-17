using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Formatting;

namespace FluentAssertions
{
    public static partial class AssertionExtensions
    {
        static AssertionExtensions()
        {
            Services.TestFramework = Services.TestFramework ?? new FallbackTestFramework();

            Formatter.AddFormatter(new AggregateExceptionValueFormatter());
        }
    }
}