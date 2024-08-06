using System;
using System.Threading.Tasks;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Specialized;

namespace FluentAssertions.Specs;

internal static class AssertionExtensions
{
    private static readonly AggregateExceptionExtractor Extractor = new();

    public static NonGenericAsyncFunctionAssertions Should(this Func<Task> action, IClock clock)
    {
        return new NonGenericAsyncFunctionAssertions(action, Extractor, AssertionChain.GetOrCreate(), clock);
    }

    public static GenericAsyncFunctionAssertions<T> Should<T>(this Func<Task<T>> action, IClock clock)
    {
        return new GenericAsyncFunctionAssertions<T>(action, Extractor, AssertionChain.GetOrCreate(), clock);
    }

    public static ActionAssertions Should(this Action action, IClock clock)
    {
        return new ActionAssertions(action, Extractor, AssertionChain.GetOrCreate(),  clock);
    }

    public static FunctionAssertions<T> Should<T>(this Func<T> func, IClock clock)
    {
        return new FunctionAssertions<T>(func, Extractor, AssertionChain.GetOrCreate(), clock);
    }

    public static TaskCompletionSourceAssertions<T> Should<T>(this TaskCompletionSource<T> tcs, IClock clock)
    {
        return new TaskCompletionSourceAssertions<T>(tcs, AssertionChain.GetOrCreate(), clock);
    }

#if NET6_0_OR_GREATER
    public static TaskCompletionSourceAssertions Should(this TaskCompletionSource tcs, IClock clock)
    {
        return new TaskCompletionSourceAssertions(tcs, AssertionChain.GetOrCreate(), clock);
    }

#endif
}
