using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

namespace FluentAssertions
{
    /// <summary>
    /// Contains extension methods for custom assertions in unit tests.
    /// </summary>
    [DebuggerNonUserCode]
    public static class AssertionExtensions
    {
        /// <summary>
        /// Invokes the specified action on an subject so that you can chain it with any of the ShouldThrow or ShouldNotThrow 
        /// overloads.
        /// </summary>
        public static Action Invoking<T>(this T subject, Action<T> action)
        {
            return () => action(subject);
        }

#if !SILVERLIGHT
        /// <summary>
        /// Provides methods for asserting the execution time of a method or property.
        /// </summary>
        /// <param name="subject">The object that exposes the method or property.</param>
        /// <param name="action">A reference to the method or property to measure the execution time of.</param>
        /// <returns>
        /// Returns an object for asserting that the execution time matches certain conditions.
        /// </returns>
        public static MemberExecutionTimeAssertions<T> ExecutionTimeOf<T>(this T subject, Expression<Action<T>> action)
        {
            return new MemberExecutionTimeAssertions<T>(subject, action);
        }

        /// <summary>
        /// Provides methods for asserting the execution time of a method or property.
        /// </summary>
        /// <param name="subject">The object that exposes the method or property.</param>
        /// <param name="action">A reference to the method or property to measure the execution time of.</param>
        /// <returns>
        /// Returns an object for asserting that the execution time matches certain conditions.
        /// </returns>
        public static ExecutionTimeAssertions ExecutionTime(this Action action)
        {
            return new ExecutionTimeAssertions(action);
        }
#endif

        /// <summary>
        /// Asserts that the <paramref name="action"/> throws an exception.
        /// </summary>
        /// <typeparam name="TException">
        /// The type of the exception it should throw.
        /// </typeparam>
        /// <returns>
        /// Returns an object that allows asserting additional members of the thrown exception.
        /// </returns>
        public static ExceptionAssertions<TException> ShouldThrow<TException>(this Action action) 
            where TException : Exception
        {
            return ShouldThrow<TException>(action, string.Empty);
        }

        /// <summary>
        /// Asserts that the <paramref name="action"/> throws an exception.
        /// </summary>
        /// <typeparam name="TException">
        /// The type of the exception it should throw.
        /// </typeparam>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonParameters">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        /// <returns>
        /// Returns an object that allows asserting additional members of the thrown exception.
        /// </returns>
        public static ExceptionAssertions<TException> ShouldThrow<TException>(this Action action, string reason, params object[] reasonParameters) 
            where TException : Exception
        {
            return new ActionAssertions(action).ShouldThrow<TException>(reason, reasonParameters);
        }

        /// <summary>
        /// Asserts that the <paramref name="action"/> does not throw a particular exception.
        /// </summary>
        /// <typeparam name="TException">
        /// The type of the exception it should not throw. Any other exceptions are ignored and will satisfy the assertion.
        /// </typeparam>
        public static void ShouldNotThrow<TException>(this Action action)
        {
            ShouldNotThrow<TException>(action, string.Empty);
        }

        /// <summary>
        /// Asserts that the <paramref name="action"/> does not throw a particular exception.
        /// </summary>
        /// <typeparam name="TException">
        /// The type of the exception it should not throw. Any other exceptions are ignored and will satisfy the assertion.
        /// </typeparam>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonParameters">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public static void ShouldNotThrow<TException>(this Action action, string reason, params object[] reasonParameters)
        {
            new ActionAssertions(action).ShouldNotThrow<TException>(reason, reasonParameters);
        }

        /// <summary>
        /// Asserts that the <paramref name="action"/> does not throw any exception at all.
        /// </summary>
        public static void ShouldNotThrow(this Action action)
        {
            ShouldNotThrow(action, string.Empty);
        }

        /// <summary>
        /// Asserts that the <paramref name="action"/> does not throw any exception at all.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonParameters">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public static void ShouldNotThrow(this Action action, string reason, params object[] reasonParameters)
        {
            new ActionAssertions(action).ShouldNotThrow(reason, reasonParameters);
        }

        /// <summary>
        /// Forces enumerating a collection. Should be used to assert that a method that uses the 
        /// <c>yield</c> keyword throws a particular exception.
        /// </summary>
        public static Action Enumerating(this Func<IEnumerable> enumerable)
        {
            return () => ForceEnumeration(enumerable);
        }

        private static void ForceEnumeration(Func<IEnumerable> enumerable)
        {
            foreach (var item in enumerable())
            {
                // Do nothing
            }
        }

        public static ObjectAssertions Should(this object actualValue)
        {
            return new ObjectAssertions(actualValue);
        }

        public static BooleanAssertions Should(this bool actualValue)
        {
            return new BooleanAssertions(actualValue);
        }

        public static NullableBooleanAssertions Should(this bool? actualValue)
        {
            return new NullableBooleanAssertions(actualValue);
        }

        public static NonGenericCollectionAssertions Should(this IEnumerable actualValue)
        {
            return new NonGenericCollectionAssertions(actualValue);
        }

        public static GenericCollectionAssertions<T> Should<T>(this IEnumerable<T> actualValue)
        {
            return new GenericCollectionAssertions<T>(actualValue);
        }

        public static DateTimeAssertions Should(this DateTime actualValue)
        {
            return new DateTimeAssertions(actualValue);
        }

        public static NullableDateTimeAssertions Should(this DateTime? actualValue)
        {
            return new NullableDateTimeAssertions(actualValue);
        }

        public static NumericAssertions<int> Should(this int actualValue)
        {
            return new NumericAssertions<int>(actualValue);
        }

        public static NullableNumericAssertions<int> Should(this int? actualValue)
        {
            return new NullableNumericAssertions<int>(actualValue);
        }
        
        public static NumericAssertions<byte> Should(this byte actualValue)
        {
            return new NumericAssertions<byte>(actualValue);
        }

        public static NullableNumericAssertions<byte> Should(this byte? actualValue)
        {
            return new NullableNumericAssertions<byte>(actualValue);
        }
        
        public static NumericAssertions<short> Should(this short actualValue)
        {
            return new NumericAssertions<short>(actualValue);
        }

        public static NullableNumericAssertions<short> Should(this short? actualValue)
        {
            return new NullableNumericAssertions<short>(actualValue);
        }

        public static NumericAssertions<long> Should(this long actualValue)
        {
            return new NumericAssertions<long>(actualValue);
        }

        public static NullableNumericAssertions<long> Should(this long? actualValue)
        {
            return new NullableNumericAssertions<long>(actualValue);
        }

        public static NumericAssertions<float> Should(this float actualValue)
        {
            return new NumericAssertions<float>(actualValue);
        }

        public static NullableNumericAssertions<float> Should(this float? actualValue)
        {
            return new NullableNumericAssertions<float>(actualValue);
        }

        public static NumericAssertions<double> Should(this double actualValue)
        {
            return new NumericAssertions<double>(actualValue);
        }

        public static NullableNumericAssertions<double> Should(this double? actualValue)
        {
            return new NullableNumericAssertions<double>(actualValue);
        }

        public static StringAssertions Should(this string actualValue)
        {
            return new StringAssertions(actualValue);
        }

        public static SimpleTimeSpanAssertions Should(this TimeSpan actualValue)
        {
            return new SimpleTimeSpanAssertions(actualValue);
        }

        public static NullableSimpleTimeSpanAssertions Should(this TimeSpan? actualValue)
        {
            return new NullableSimpleTimeSpanAssertions(actualValue);
        }
        
        /// <summary>
        /// Asserts that the properties of an object matches those of another object.
        /// </summary>
        public static PropertyAssertions<T> ShouldHave<T>(this T subject)
        {
            return new PropertyAssertions<T>(subject);
        }

        /// <summary>
        /// Safely casts the specified object to the type specified through <typeparamref name="TTo"/>.
        /// </summary>
        /// <remarks>
        /// Has been introduced to allow casting objects without breaking the fluent API.
        /// </remarks>
        /// <typeparam name="TTo"></typeparam>
        public static TTo As<TTo>(this object subject)
        {
            return subject is TTo ? (TTo) subject : default(TTo);
        }
    }
}