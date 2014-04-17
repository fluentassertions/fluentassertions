using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Formatting;

namespace FluentAssertions
{
    public static partial class AssertionExtensions
    {
        static AssertionExtensions()
        {
            Services.Configuration = new Configuration(new AppSettingsConfigurationStore());
            Services.ThrowException = TestFrameworkProvider.Throw;
            Services.Reflector = new DefaultReflector();

            Formatter.AddFormatter(new AggregateExceptionValueFormatter());
        }
    }
}