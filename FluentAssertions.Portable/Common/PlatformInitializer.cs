using FluentAssertions.Execution;
using FluentAssertions.Formatting;

namespace FluentAssertions.Common
{
    public class PlatformInitializer : IPlatformInitializer
    {
        public void Initialize()
        {
            if (Services.ThrowException == null)
            {
                Services.ThrowException = message => { throw new AssertionFailedException(message); };
            }

            Formatter.AddFormatter(new AggregateExceptionValueFormatter());
        }
    }
}