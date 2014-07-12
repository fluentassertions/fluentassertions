using FluentAssertions.Formatting;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Common
{
    public class PlatformInitializer : IPlatformInitializer
    {
        public void Initialize()
        {
            Services.ThrowException = message => { throw new AssertFailedException(message); };
            Services.Reflector = new SilverlightReflector();

            Formatter.AddFormatter(new AggregateExceptionValueFormatter());
            Formatter.AddFormatter(new XDocumentValueFormatter());
            Formatter.AddFormatter(new XElementValueFormatter());
            Formatter.AddFormatter(new XAttributeValueFormatter());
        }
    }
}