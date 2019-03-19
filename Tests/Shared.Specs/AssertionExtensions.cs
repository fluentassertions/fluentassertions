using System;
using System.Threading.Tasks;
using FluentAssertions.Common;
using FluentAssertions.Specialized;

namespace FluentAssertions.Specs
{
    internal static class AssertionExtensions
    {
        private static readonly AggregateExceptionExtractor extractor = new AggregateExceptionExtractor();

        public static NonGenericAsyncFunctionAssertions Should(this Func<Task> action, ITimer timer)
        {
            return new NonGenericAsyncFunctionAssertions(action, extractor, timer);
        }

        /// <summary>
        /// Returns a <see cref="AsyncFunctionAssertions"/> object that can be used to assert the
        /// current <see><cref>System.Func{Task{T}}</cref></see>.
        /// </summary>
        public static GenericAsyncFunctionAssertions<T> Should<T>(this Func<Task<T>> action, ITimer timer)
        {
            return new GenericAsyncFunctionAssertions<T>(action, extractor, timer);
        }
    }
}
