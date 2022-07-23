using System;
using System.Threading.Tasks;
using FluentAssertions.Common;
using FluentAssertions.Specialized;

namespace FluentAssertions.Specs;

internal static class AssertionExtensions
{
    private static readonly AggregateExceptionExtractor Extractor = new();

    public static NonGenericAsyncFunctionAssertions Should(this Func<Task> action, IClock clock)
    {
        return new NonGenericAsyncFunctionAssertions(action, Extractor, clock);
    }

    public static GenericAsyncFunctionAssertions<T> Should<T>(this Func<Task<T>> action, IClock clock)
    {
        return new GenericAsyncFunctionAssertions<T>(action, Extractor, clock);
    }

    public static ActionAssertions Should(this Action action, IClock clock)
    {
        return new ActionAssertions(action, Extractor, clock);
    }

    public static FunctionAssertions<T> Should<T>(this Func<T> func, IClock clock)
    {
        return new FunctionAssertions<T>(func, Extractor, clock);
    }

    public static TaskCompletionSourceAssertions<T> Should<T>(this TaskCompletionSource<T> tcs, IClock clock)
    {
        return new TaskCompletionSourceAssertions<T>(tcs, clock);
    }

#if NET6_0_OR_GREATER

    public static TaskCompletionSourceAssertions Should(this TaskCompletionSource tcs, IClock clock)
    {
        return new TaskCompletionSourceAssertions(tcs, clock);
    }

#endif
}
