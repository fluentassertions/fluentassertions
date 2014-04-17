using FluentAssertions.Common;
using FluentAssertions.Formatting;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions
{
    public static partial class AssertionExtensions
    {
        static AssertionExtensions()
        {
            Services.ThrowException = message => { throw new AssertFailedException(message); };
            Services.Reflector = new SilverlightReflector();

            Formatter.AddFormatter(new AggregateExceptionValueFormatter());
        }
    }
}