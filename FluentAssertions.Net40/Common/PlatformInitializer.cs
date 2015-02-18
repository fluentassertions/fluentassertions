using FluentAssertions.Execution;
using FluentAssertions.Formatting;

namespace FluentAssertions.Common
{
    public class PlatformInitializer : IPlatformInitializer
    {
        public void Initialize()
        {
            Services.ConfigurationStore = new AppSettingsConfigurationStore();
            Services.ThrowException = TestFrameworkProvider.Throw;
            Services.Reflector = new DefaultReflector();

            Formatter.AddFormatter(new AggregateExceptionValueFormatter());
            Formatter.AddFormatter(new XDocumentValueFormatter());
            Formatter.AddFormatter(new XElementValueFormatter());
            Formatter.AddFormatter(new XAttributeValueFormatter());
        }
    }
}