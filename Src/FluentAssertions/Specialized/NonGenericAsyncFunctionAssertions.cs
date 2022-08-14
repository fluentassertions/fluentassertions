using System;
using System.Threading.Tasks;
using FluentAssertions.Common;

namespace FluentAssertions.Specialized;

public class NonGenericAsyncFunctionAssertions : AsyncFunctionAssertions<Task, NonGenericAsyncFunctionAssertions>
{
    public NonGenericAsyncFunctionAssertions(Func<Task> subject, IExtractExceptions extractor)
        : this(subject, extractor, new Clock())
    {
    }

    public NonGenericAsyncFunctionAssertions(Func<Task> subject, IExtractExceptions extractor, IClock clock)
#pragma warning disable CS0618 // is currently obsolete to make it protected in Version 7
        : base(subject, extractor, clock)
#pragma warning restore CS0618
    {
    }
}
