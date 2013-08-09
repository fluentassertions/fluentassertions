using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions.Formatting;
using FluentAssertions.Specialized;

namespace FluentAssertions
{
    public static class ExceptionExtensionsNet40
    {
        private static readonly AggregateExceptionExtractor extractor = new AggregateExceptionExtractor();

        static ExceptionExtensionsNet40()
        {
            Formatter.AddFormatter(new AggregateExceptionFormatter());
        }

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
    }

    internal class AggregateExceptionExtractor : IExtractExceptions
    {
        public IEnumerable<T> OfType<T>(Exception actualException) where T : Exception
        {
            var exceptions = new List<T>();

            var aggregateException = actualException as AggregateException;
            if (aggregateException != null)
            {
                exceptions.AddRange(aggregateException.InnerExceptions.OfType<T>());
            }
            else if (actualException is T)
            {
                exceptions.Add((T)actualException);
            }

            return exceptions;
        }
    }

    internal class AggregateExceptionFormatter : IValueFormatter
    {
        /// <summary>
        /// Indicates whether the current <see cref="IValueFormatter"/> can handle the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value for which to create a <see cref="System.String"/>.</param>
        /// <returns>
        /// <c>true</c> if the current <see cref="IValueFormatter"/> can handle the specified value; otherwise, <c>false</c>.
        /// </returns>
        public bool CanHandle(object value)
        {
            return value is AggregateException;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="value">The value for which to create a <see cref="System.String"/>.</param>
        /// <param name="useLineBreaks"> </param>
        /// <param name="processedObjects">
        /// A collection of objects that 
        /// </param>
        /// <param name="nestedPropertyLevel">
        /// The level of nesting for the supplied value. This is used for indenting the format string for objects that have
        /// no <see cref="object.ToString()"/> override.
        /// </param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(object value, bool useLineBreaks, IList<object> processedObjects = null, int nestedPropertyLevel = 0)
        {
            var exception = (AggregateException)value;
            if (exception.InnerExceptions.Count == 1)
            {
                return "(aggregated) " + exception.InnerException.ToString();
            }
            else
            {
                var builder = new StringBuilder("(aggregated) exceptions ");

                foreach (Exception innerException in exception.InnerExceptions)
                {
                    builder.AppendLine();
                    builder.AppendLine(innerException.ToString());
                }

                return builder.ToString();
            }
        }
    }
}