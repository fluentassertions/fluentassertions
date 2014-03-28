using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using FluentAssertions.Common;
using FluentAssertions.Specialized;

namespace FluentAssertions
{
    public static partial class AssertionExtensions
    {
        private static readonly AggregateExceptionExtractor extractor = new AggregateExceptionExtractor();

        /// <summary>
        /// Asserts that the <paramref name="action"/> throws an exception.
        /// </summary>
        /// <param name="action">A reference to the method or property.</param>
        /// <typeparam name="TException">
        /// The type of the exception it should throw.
        /// </typeparam>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        /// <returns>
        /// Returns an object that allows asserting additional members of the thrown exception.
        /// </returns>
        public static ExceptionAssertions<TException> ShouldThrow<TException>(this Action action, string reason = "",
            params object[] reasonArgs)
            where TException : Exception
        {
            return new ActionAssertions(action, extractor).ShouldThrow<TException>(reason, reasonArgs);
        }

        /// <summary>
        /// Asserts that the <paramref name="action"/> does not throw a particular exception.
        /// </summary>
        /// <param name="action">The current method or property.</param>
        /// <typeparam name="TException">
        /// The type of the exception it should not throw. Any other exceptions are ignored and will satisfy the assertion.
        /// </typeparam>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public static void ShouldNotThrow<TException>(this Action action, string reason = "", params object[] reasonArgs)
            where TException : Exception
        {
            new ActionAssertions(action, extractor).ShouldNotThrow<TException>(reason, reasonArgs);
        }

        /// <summary>
        /// Asserts that the <paramref name="action"/> does not throw any exception at all.
        /// </summary>
        /// <param name="action">The current method or property.</param>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public static void ShouldNotThrow(this Action action, string reason = "", params object[] reasonArgs)
        {
            new ActionAssertions(action, extractor).ShouldNotThrow(reason, reasonArgs);
        }

        /// <summary>
        /// Asserts that the <paramref name="asyncAction"/> throws an exception.
        /// </summary>
        /// <param name="asyncAction">A reference to the method or property.</param>
        /// <typeparam name="TException">
        /// The type of the exception it should throw.
        /// </typeparam>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        /// <returns>
        /// Returns an object that allows asserting additional members of the thrown exception.
        /// </returns>
        public static ExceptionAssertions<TException> ShouldThrow<TException>(this Func<Task> asyncAction, string reason = "",
            params object[] reasonArgs)
            where TException : Exception
        {
            return new AsyncFunctionAssertions(asyncAction, extractor).ShouldThrow<TException>(reason, reasonArgs);
        }

        /// <summary>
        /// Asserts that the <paramref name="asyncAction"/> does not throw a particular exception.
        /// </summary>
        /// <param name="asyncAction">The current method or property.</param>
        /// <typeparam name="TException">
        /// The type of the exception it should not throw. Any other exceptions are ignored and will satisfy the assertion.
        /// </typeparam>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public static void ShouldNotThrow<TException>(this Func<Task> asyncAction, string reason = "", params object[] reasonArgs)
        {
            new AsyncFunctionAssertions(asyncAction, extractor).ShouldNotThrow<TException>(reason, reasonArgs);
        }

        /// <summary>
        /// Asserts that the <paramref name="asyncAction"/> does not throw any exception at all.
        /// </summary>
        /// <param name="asyncAction">The current method or property.</param>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public static void ShouldNotThrow(this Func<Task> asyncAction, string reason = "", params object[] reasonArgs)
        {
            new AsyncFunctionAssertions(asyncAction, extractor).ShouldNotThrow(reason, reasonArgs);
        }

        private class AggregateExceptionExtractor : IExtractExceptions
        {
            public IEnumerable<T> OfType<T>(Exception actualException) where T : Exception
            {
                if (typeof(T).IsSameOrInherits(typeof(AggregateException)))
                {
                    var exception = actualException as T;

                    return (exception == null) ? Enumerable.Empty<T>() : new[] { exception };
                }

                return GetExtractedExceptions<T>(actualException);
            }

            private static List<T> GetExtractedExceptions<T>(Exception actualException)
                where T : Exception
            {
                var exceptions = new List<T>();

                var aggregateException = actualException as AggregateException;
                if (aggregateException != null)
                {
                    var flattenedExceptions = aggregateException.Flatten();

                    exceptions.AddRange(flattenedExceptions.InnerExceptions.OfType<T>());
                }
                else if (actualException is T)
                {
                    exceptions.Add((T)actualException);
                }

                return exceptions;
            }
        }
    }
}