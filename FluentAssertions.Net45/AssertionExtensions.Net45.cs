using FluentAssertions.Common;
using FluentAssertions.Formatting;

namespace FluentAssertions
{
    public static partial class AssertionExtensions
    {
        static AssertionExtensions()
        {
            Formatter.AddFormatter(new AggregateExceptionValueFormatter());
            Services.Configuration = new Configuration(new AppSettingsConfigurationStore());
        }
    }
}