using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

using System.Xml.Linq;

using FluentAssertions.Assertions;
using FluentAssertions.Assertions.Structure;
using FluentAssertions.Common;

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
        /// Returns an <see cref="XDocumentAssertions"/> object that can be used to assert the
        /// current <see cref="XElement"/>.
        /// </summary>
        public static XDocumentAssertions Should(this XDocument actualValue)
        {
            return new XDocumentAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="XElementAssertions"/> object that can be used to assert the
        /// current <see cref="XElement"/>.
        /// </summary>
        public static XElementAssertions Should(this XElement actualValue)
        {
            return new XElementAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="XAttributeAssertions"/> object that can be used to assert the
        /// current <see cref="XAttribute"/>.
        /// </summary>
        public static XAttributeAssertions Should(this XAttribute actualValue)
        {
            return new XAttributeAssertions(actualValue);
        }

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
            return ShouldThrow<TException>(action, String.Empty);
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
        public static ExceptionAssertions<TException> ShouldThrow<TException>(this Action action, string reason, params object[] reasonArgs) 
            where TException : Exception
        {
            return new ActionAssertions(action).ShouldThrow<TException>(reason, reasonArgs);
        }

        /// <summary>
        /// Asserts that the <paramref name="action"/> does not throw a particular exception.
        /// </summary>
        /// <typeparam name="TException">
        /// The type of the exception it should not throw. Any other exceptions are ignored and will satisfy the assertion.
        /// </typeparam>
        public static void ShouldNotThrow<TException>(this Action action)
        {
            ShouldNotThrow<TException>(action, String.Empty);
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
        public static void ShouldNotThrow<TException>(this Action action, string reason, params object[] reasonArgs)
        {
            new ActionAssertions(action).ShouldNotThrow<TException>(reason, reasonArgs);
        }

        /// <summary>
        /// Asserts that the <paramref name="action"/> does not throw any exception at all.
        /// </summary>
        public static void ShouldNotThrow(this Action action)
        {
            ShouldNotThrow(action, String.Empty);
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
        public static void ShouldNotThrow(this Action action, string reason, params object[] reasonArgs)
        {
            new ActionAssertions(action).ShouldNotThrow(reason, reasonArgs);
        }

        /// <summary>
        /// Forces enumerating a collection. Should be used to assert that a method that uses the 
        /// <c>yield</c> keyword throws a particular exception.
        /// </summary>
        public static Action Enumerating(this Func<IEnumerable> enumerable)
        {
            return () => ForceEnumeration(enumerable);
        }        
        
        /// <summary>
        /// Forces enumerating a collection. Should be used to assert that a method that uses the 
        /// <c>yield</c> keyword throws a particular exception.
        /// </summary>
        public static Action Enumerating<T>(this Func<IEnumerable<T>> enumerable)
        {
            return () => ForceEnumeration(() => (IEnumerable)enumerable());
        }

        private static void ForceEnumeration(Func<IEnumerable> enumerable)
        {
            foreach (var item in enumerable())
            {
                // Do nothing
            }
        }

        /// <summary>
        /// Returns an <see cref="ObjectAssertions"/> object that can be used to assert the
        /// current <see cref="object"/>.
        /// </summary>
        public static ObjectAssertions Should(this object actualValue)
        {
            return new ObjectAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="BooleanAssertions"/> object that can be used to assert the
        /// current <see cref="bool"/>.
        /// </summary>
        public static BooleanAssertions Should(this bool actualValue)
        {
            return new BooleanAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableBooleanAssertions"/> object that can be used to assert the
        /// current nullable <see cref="bool"/>.
        /// </summary>
        public static NullableBooleanAssertions Should(this bool? actualValue)
        {
            return new NullableBooleanAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="GuidAssertions"/> object that can be used to assert the
        /// current <see cref="Guid"/>.
        /// </summary>
        public static GuidAssertions Should(this Guid actualValue)
        {
            return new GuidAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableGuidAssertions"/> object that can be used to assert the
        /// current nullable <see cref="Guid"/>.
        /// </summary>
        public static NullableGuidAssertions Should(this Guid? actualValue)
        {
            return new NullableGuidAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NonGenericCollectionAssertions"/> object that can be used to assert the
        /// current <see cref="IEnumerable"/>.
        /// </summary>
        public static NonGenericCollectionAssertions Should(this IEnumerable actualValue)
        {
            return new NonGenericCollectionAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="GenericCollectionAssertions{T}"/> object that can be used to assert the
        /// current <see cref="IEnumerable{T}"/>.
        /// </summary>
        public static GenericCollectionAssertions<T> Should<T>(this IEnumerable<T> actualValue)
        {
            return new GenericCollectionAssertions<T>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="GenericDictionaryAssertions{TKey, TValue}"/> object that can be used to assert the
        /// current <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        public static GenericDictionaryAssertions<TKey, TValue> Should<TKey, TValue>(this IDictionary<TKey, TValue> actualValue)
        {
            return new GenericDictionaryAssertions<TKey, TValue>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="DateTimeAssertions"/> object that can be used to assert the
        /// current <see cref="DateTime"/>.
        /// </summary>
        public static DateTimeAssertions Should(this DateTime actualValue)
        {
            return new DateTimeAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableDateTimeAssertions"/> object that can be used to assert the
        /// current nullable <see cref="DateTime"/>.
        /// </summary>
        public static NullableDateTimeAssertions Should(this DateTime? actualValue)
        {
            return new NullableDateTimeAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="ComparableTypeAssertions{T}"/> object that can be used to assert the
        /// current <see cref="IComparable{T}"/>.
        /// </summary>
        public static ComparableTypeAssertions<T> Should<T>(this IComparable<T> comparableValue)
        {
            return new ComparableTypeAssertions<T>(comparableValue);
        }

        /// <summary>
        /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
        /// current <see cref="int"/>.
        /// </summary>
        public static NumericAssertions<int> Should(this int actualValue)
        {
            return new NumericAssertions<int>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
        /// current nullable <see cref="int"/>.
        /// </summary>
        public static NullableNumericAssertions<int> Should(this int? actualValue)
        {
            return new NullableNumericAssertions<int>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
        /// current <see cref="byte"/>.
        /// </summary>
        public static NumericAssertions<byte> Should(this byte actualValue)
        {
            return new NumericAssertions<byte>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
        /// current nullable <see cref="byte"/>.
        /// </summary>
        public static NullableNumericAssertions<byte> Should(this byte? actualValue)
        {
            return new NullableNumericAssertions<byte>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
        /// current <see cref="short"/>.
        /// </summary>
        public static NumericAssertions<short> Should(this short actualValue)
        {
            return new NumericAssertions<short>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
        /// current nullable <see cref="short"/>.
        /// </summary>
        public static NullableNumericAssertions<short> Should(this short? actualValue)
        {
            return new NullableNumericAssertions<short>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
        /// current <see cref="long"/>.
        /// </summary>
        public static NumericAssertions<long> Should(this long actualValue)
        {
            return new NumericAssertions<long>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
        /// current nullable <see cref="long"/>.
        /// </summary>
        public static NullableNumericAssertions<long> Should(this long? actualValue)
        {
            return new NullableNumericAssertions<long>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
        /// current <see cref="float"/>.
        /// </summary>
        public static NumericAssertions<float> Should(this float actualValue)
        {
            return new NumericAssertions<float>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
        /// current nullable <see cref="float"/>.
        /// </summary>
        public static NullableNumericAssertions<float> Should(this float? actualValue)
        {
            return new NullableNumericAssertions<float>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
        /// current <see cref="double"/>.
        /// </summary>
        public static NumericAssertions<double> Should(this double actualValue)
        {
            return new NumericAssertions<double>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
        /// current nullable <see cref="double"/>.
        /// </summary>
        public static NullableNumericAssertions<double> Should(this double? actualValue)
        {
            return new NullableNumericAssertions<double>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="StringAssertions"/> object that can be used to assert the
        /// current <see cref="string"/>.
        /// </summary>
        public static StringAssertions Should(this string actualValue)
        {
            return new StringAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="SimpleTimeSpanAssertions"/> object that can be used to assert the
        /// current <see cref="TimeSpan"/>.
        /// </summary>
        public static SimpleTimeSpanAssertions Should(this TimeSpan actualValue)
        {
            return new SimpleTimeSpanAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableSimpleTimeSpanAssertions"/> object that can be used to assert the
        /// current nullable <see cref="TimeSpan"/>.
        /// </summary>
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
        /// Returns a <see cref="TypeAssertions"/> object that can be used to assert the
        /// current <see cref="Type"/>.
        /// </summary>
        public static TypeAssertions Should(this Type subject)
        {
            return new TypeAssertions(subject);
        }

        /// <summary>
        /// Returns a <see cref="MethodInfoAssertions"/> object that can be used to assert the methods returned by the
        /// current <see cref="MethodInfoSelector"/>.
        /// </summary>
        /// <seealso cref="TypeAssertions"/>
        public static MethodInfoAssertions Should(this MethodInfoSelector methodSelector)
        {
            return new MethodInfoAssertions(methodSelector.ToArray());
        }

        /// <summary>
        /// Returns a <see cref="PropertyInfoAssertions"/> object that can be used to assert the properties returned by the
        /// current <see cref="PropertyInfoSelector"/>.
        /// </summary>
        /// <seealso cref="TypeAssertions"/>
        public static PropertyInfoAssertions Should(this PropertyInfoSelector propertyInfoSelector)
        {
            return new PropertyInfoAssertions(propertyInfoSelector.ToArray());
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
