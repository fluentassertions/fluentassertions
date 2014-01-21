using FluentAssertions.Common;

namespace FluentAssertions
{
    public static partial class AssertionExtensions
    {
        static AssertionExtensions()
        {
            Services.Configuration = new Configuration(new AppSettingsConfigurationStore());
            Services.Reflector = new DefaultReflector();
        }
    }
}