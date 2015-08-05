using System;

using FluentAssertions.Formatting;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Common
{
    public class ProvidePlatformServices : IProvidePlatformServices
    {
        public Action<string> Throw
        {
            get { return message => { throw new AssertFailedException(message); }; }
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

        public IConfigurationStore ConfigurationStore
        {
            get { return null; }
        }

        public Configuration Configuration
        {
            get { return null; }
        }

        public IReflector Reflector
        {
            get { return new SilverlightReflector(); }
        }
    }
}