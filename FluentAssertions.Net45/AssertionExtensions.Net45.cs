using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Formatting;

namespace FluentAssertions
{
    public static partial class AssertionExtensions
    {
        static AssertionExtensions()
        {
#if !__IOS__ && !ANDROID
            Services.Configuration = new Configuration(new AppSettingsConfigurationStore());
#endif
            Services.TestFramework = TestFrameworkProvider.TestFramework;
            Services.Reflector = new DefaultReflector();

            Formatter.AddFormatter(new AggregateExceptionValueFormatter());
        }
    }
}