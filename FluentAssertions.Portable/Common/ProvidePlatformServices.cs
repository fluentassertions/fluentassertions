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

        public IValueFormatter[] Formatters
        {
            get { return new IValueFormatter[] { new AggregateExceptionValueFormatter() }; }
        }

        public Configuration Configuration
        {
            get { return null; }
        }

        public IReflector Reflector
        {
            get { return null; }
        }
    }
}