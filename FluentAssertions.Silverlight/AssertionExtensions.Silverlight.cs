using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Formatting;

namespace FluentAssertions
{
    public static partial class AssertionExtensions
    {
        static AssertionExtensions()
        {
            Services.TestFramework = new SilverlightTestFramework();
            Services.ReflectionProvider = new SilverlightReflectionProvider();

            Formatter.AddFormatter(new AggregateExceptionValueFormatter());
        }
    }
}