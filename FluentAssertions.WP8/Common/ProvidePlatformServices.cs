using System;

using FluentAssertions.Execution;
using FluentAssertions.Formatting;

namespace FluentAssertions.Common
{
    public class ProvidePlatformServices : IProvidePlatformServices
    {
        public Action<string> Throw
        {
            get { return TestFrameworkProvider.Throw; }
        }

        public IValueFormatter[] Formatters
        {
            get
            {
                return new IValueFormatter[]
                {
                    new AggregateExceptionValueFormatter(),
                    new XDocumentValueFormatter(),
                    new XElementValueFormatter(),
                    new XAttributeValueFormatter()
                };
            }
        }

        public Configuration Configuration
        {
            get { return null; }
        }

        public IReflector Reflector
        {
            get { return new WpReflector(); }
        }
    }
}