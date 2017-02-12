using System;
using FluentAssertions.Execution;
using FluentAssertions.Formatting;

namespace FluentAssertions.Common
{
    public class ProvidePlatformServices : IProvidePlatformServices
    {
        public Action<string> Throw
        {
            get { return message => { throw new AssertionFailedException(message); }; }
        }

        public IValueFormatter[] Formatters => new IValueFormatter[]
        {
            new AggregateExceptionValueFormatter(),
            new XDocumentValueFormatter(),
            new XElementValueFormatter(),
            new XAttributeValueFormatter(),
        };

        public IConfigurationStore ConfigurationStore => null;

        public IReflector Reflector => null;
    }
}