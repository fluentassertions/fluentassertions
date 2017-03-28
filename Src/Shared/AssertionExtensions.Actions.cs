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
        /// Asserts that the <paramref name="action"/> throws the exact exception (and not a derived exception type).
        /// </summary>
        /// <param name="action">A reference to the method or property.</param>
        /// <typeparam name="TException">
        /// The type of the exception it should throw.
        /// </typeparam>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        /// <returns>
        /// Returns an object that allows asserting additional members of the thrown exception.
        /// </returns>
        public static ExceptionAssertions<TException> ShouldThrowExactly<TException>(this Action action, string because = "",
            params object[] becauseArgs)
            where TException : Exception
        {
            var exceptionAssertions = action.ShouldThrow<TException>(because, becauseArgs);
            exceptionAssertions.Which.GetType().Should().Be<TException>(because, becauseArgs);
            return exceptionAssertions;
        }

        /// <summary>
        /// Asserts that the <paramref name="asyncAction"/> throws the exact exception (and not a derived exception type).
        /// </summary>
        /// <param name="asyncAction">A reference to the method or property.</param>
        /// <typeparam name="TException">
        /// The type of the exception it should throw.
        /// </typeparam>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        /// <returns>
        /// Returns an object that allows asserting additional members of the thrown exception.
        /// </returns>
        public static ExceptionAssertions<TException> ShouldThrowExactly<TException>(this Func<Task> asyncAction, string because = "",
            params object[] becauseArgs)
            where TException : Exception
        {
            var exceptionAssertions = asyncAction.ShouldThrow<TException>(because, becauseArgs);
            exceptionAssertions.Which.GetType().Should().Be<TException>(because, becauseArgs);
            return exceptionAssertions;
        }

        /// <summary>
        /// Asserts that the <paramref name="action"/> throws an exception.
        /// </summary>
        /// <param name="action">A reference to the method or property.</param>
        /// <typeparam name="TException">
        /// The type of the exception it should throw.
        /// </typeparam>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        /// <returns>
        /// Returns an object that allows asserting additional members of the thrown exception.
        /// </returns>
        public static ExceptionAssertions<TException> ShouldThrow<TException>(this Action action, string because = "",
            params object[] becauseArgs)
            where TException : Exception
        {
            return new ActionAssertions(action, extractor).ShouldThrow<TException>(because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the <paramref name="action"/> does not throw a particular exception.
        /// </summary>
        /// <param name="action">The current method or property.</param>
        /// <typeparam name="TException">
        /// The type of the exception it should not throw. Any other exceptions are ignored and will satisfy the assertion.
        /// </typeparam>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public static void ShouldNotThrow<TException>(this Action action, string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            new ActionAssertions(action, extractor).ShouldNotThrow<TException>(because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the <paramref name="action"/> does not throw any exception at all.
        /// </summary>
        /// <param name="action">The current method or property.</param>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public static void ShouldNotThrow(this Action action, string because = "", params object[] becauseArgs)
        {
            new ActionAssertions(action, extractor).ShouldNotThrow(because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the <paramref name="asyncAction"/> throws an exception.
        /// </summary>
        /// <param name="asyncAction">A reference to the method or property.</param>
        /// <typeparam name="TException">
        /// The type of the exception it should throw.
        /// </typeparam>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        /// <returns>
        /// Returns an object that allows asserting additional members of the thrown exception.
        /// </returns>
        public static ExceptionAssertions<TException> ShouldThrow<TException>(this Func<Task> asyncAction, string because = "",
            params object[] becauseArgs)
            where TException : Exception
        {
            return new AsyncFunctionAssertions(asyncAction, extractor).ShouldThrow<TException>(because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the <paramref name="asyncAction"/> does not throw a particular exception.
        /// </summary>
        /// <param name="asyncAction">The current method or property.</param>
        /// <typeparam name="TException">
        /// The type of the exception it should not throw. Any other exceptions are ignored and will satisfy the assertion.
        /// </typeparam>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public static void ShouldNotThrow<TException>(this Func<Task> asyncAction, string because = "", params object[] becauseArgs)
        {
            new AsyncFunctionAssertions(asyncAction, extractor).ShouldNotThrow<TException>(because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the <paramref name="asyncAction"/> does not throw any exception at all.
        /// </summary>
        /// <param name="asyncAction">The current method or property.</param>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public static void ShouldNotThrow(this Func<Task> asyncAction, string because = "", params object[] becauseArgs)
        {
            new AsyncFunctionAssertions(asyncAction, extractor).ShouldNotThrow(because, becauseArgs);
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
