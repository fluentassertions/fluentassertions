using FluentAssertions.Execution;
using FluentAssertions.Formatting;

namespace FluentAssertions.Common
{
    public class PlatformInitializer : IPlatformInitializer
    {
        public void Initialize()
        {
            Services.ThrowException = TestFrameworkProvider.Throw;

            Formatter.AddFormatter(new AggregateExceptionValueFormatter());
        }
    }
}