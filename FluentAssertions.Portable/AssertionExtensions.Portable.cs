using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Formatting;

namespace FluentAssertions
{
    public static partial class AssertionExtensions
    {
        static AssertionExtensions()
        {
            if (Services.ThrowException == null)
            {
                Services.ThrowException = message => { throw new AssertionFailedException(message); };
            }

            Formatter.AddFormatter(new AggregateExceptionValueFormatter());
        }
    }
}