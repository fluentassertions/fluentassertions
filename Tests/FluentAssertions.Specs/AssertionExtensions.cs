using System;
using System.Threading.Tasks;
using FluentAssertions.Common;
using FluentAssertions.Specialized;

namespace FluentAssertions.Specs
{
    internal static class AssertionExtensions
    {
        private static readonly AggregateExceptionExtractor extractor = new AggregateExceptionExtractor();

        public static NonGenericAsyncFunctionAssertions Should(this Func<Task> action, IClock clock)
        {
            return new NonGenericAsyncFunctionAssertions(action, extractor, clock);
        }

        public static GenericAsyncFunctionAssertions<T> Should<T>(this Func<Task<T>> action, IClock clock)
        {
            return new GenericAsyncFunctionAssertions<T>(action, extractor, clock);
        }

        public static ActionAssertions Should(this Action action, IClock clock)
        {
            return new ActionAssertions(action, extractor, clock);
        }

        public static FunctionAssertions<T> Should<T>(this Func<T> func, IClock clock)
        {
            return new FunctionAssertions<T>(func, extractor, clock);
        }

        public static TaskCompletionSourceAssertions<T> Should<T>(this TaskCompletionSource<T> tcs, IClock clock)
        {
            return new TaskCompletionSourceAssertions<T>(tcs, clock);
        }
    }
}
