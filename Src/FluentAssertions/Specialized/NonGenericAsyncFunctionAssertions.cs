using System;
using System.Threading.Tasks;
using FluentAssertions.Common;

namespace FluentAssertions.Specialized
{
    public class NonGenericAsyncFunctionAssertions : AsyncFunctionAssertions<Task, NonGenericAsyncFunctionAssertions>
    {
        public NonGenericAsyncFunctionAssertions(Func<Task> subject, IExtractExceptions extractor) : this(subject, extractor, new Clock())
        {
        }

        public NonGenericAsyncFunctionAssertions(Func<Task> subject, IExtractExceptions extractor, IClock clock) : base(subject, extractor, clock)
        {
        }
    }
}
