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
            Services.Reflector = new SilverlightReflector();

            Formatter.AddFormatter(new AggregateExceptionValueFormatter());
        }
    }
}