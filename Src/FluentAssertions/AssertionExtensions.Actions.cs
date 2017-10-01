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
        /// Asserts that the <paramref name="actionAssertions"/> throws the exact exception (and not a derived exception type).
        /// </summary>
        /// <param name="actionAssertions">A reference to the method or property.</param>
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
        public static ExceptionAssertions<TException> ThrowExactly<TException>(this ActionAssertions actionAssertions, string because = "",
            params object[] becauseArgs)
            where TException : Exception
        {
            var exceptionAssertions = Throw<TException>(actionAssertions, because, becauseArgs);
            exceptionAssertions.Which.GetType().Should().Be<TException>(because, becauseArgs);
            return exceptionAssertions;
        }

        /// <summary>
        /// Asserts that the <paramref name="asyncActionAssertions"/> throws the exact exception (and not a derived exception type).
        /// </summary>
        /// <param name="asyncActionAssertions">A reference to the method or property.</param>
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
        public static ExceptionAssertions<TException> ThrowExactly<TException>(this AsyncFunctionAssertions asyncActionAssertions, string because = "",
            params object[] becauseArgs)
            where TException : Exception
        {
            var exceptionAssertions = Throw<TException>(asyncActionAssertions ,because, becauseArgs);
            exceptionAssertions.Which.GetType().Should().Be<TException>(because, becauseArgs);
            return exceptionAssertions;
        }

        /// <summary>
        /// Asserts that the <paramref name="actionAssertions"/> throws an exception.
        /// </summary>
        /// <param name="actionAssertions">A reference to the method or property.</param>
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
        public static ExceptionAssertions<TException> Throw<TException>(this ActionAssertions actionAssertions, string because = "",
            params object[] becauseArgs)
            where TException : Exception
        {
            return actionAssertions.Throw<TException>(because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the <paramref name="actionAssertions"/> does not throw a particular exception.
        /// </summary>
        /// <param name="actionAssertions">The current method or property.</param>
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
        public static void NotThrow<TException>(this ActionAssertions actionAssertions, string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            actionAssertions.NotThrow<TException>(because, becauseArgs);
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
        public static void NotThrow(this ActionAssertions actionAssertions, string because = "", params object[] becauseArgs)
        {
            actionAssertions.NotThrow(because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the <paramref name="asyncActionAssertions"/> throws an exception.
        /// </summary>
        /// <param name="asyncActionAssertions">A reference to the method or property.</param>
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
        public static ExceptionAssertions<TException> Throw<TException>(this AsyncFunctionAssertions asyncActionAssertions, string because = "",
            params object[] becauseArgs)
            where TException : Exception
        {
            return asyncActionAssertions.Throw<TException>(because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the <paramref name="asyncActionAssertions"/> does not throw a particular exception.
        /// </summary>
        /// <param name="asyncActionAssertions">The current method or property.</param>
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
        public static void NotThrow<TException>(this AsyncFunctionAssertions asyncActionAssertions, string because = "", params object[] becauseArgs)
        {
            asyncActionAssertions.NotThrow<TException>(because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the <paramref name="asyncActionAssertions"/> does not throw any exception at all.
        /// </summary>
        /// <param name="asyncActionAssertions">The current method or property.</param>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public static void NotThrow(this AsyncFunctionAssertions asyncActionAssertions, string because = "", params object[] becauseArgs)
        {
            asyncActionAssertions.NotThrow(because, becauseArgs);
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
