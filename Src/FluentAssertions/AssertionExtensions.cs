using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using FluentAssertions.Collections;
using FluentAssertions.Common;
using FluentAssertions.Events;
using FluentAssertions.Numeric;
using FluentAssertions.Primitives;
using FluentAssertions.Reflection;
using FluentAssertions.Specialized;
using FluentAssertions.Types;
using FluentAssertions.Xml;
using JetBrains.Annotations;

namespace FluentAssertions
{
    /// <summary>
    /// Contains extension methods for custom assertions in unit tests.
    /// </summary>
    [DebuggerNonUserCode]
    public static partial class AssertionExtensions
    {
        /// <summary>
        /// Invokes the specified action on an subject so that you can chain it with any of the ShouldThrow or ShouldNotThrow
        /// overloads.
        /// </summary>
        [Pure]
        public static Action Invoking<T>(this T subject, Action<T> action)
        {
            return () => action(subject);
        }

        [Pure]
        public static Func<Task> Awaiting<T>(this T subject, Func<T, Task> action)
        {
            return () => action(subject);
        }

        /// <summary>
        /// Provides methods for asserting the execution time of a method or property.
        /// </summary>
        /// <param name="subject">The object that exposes the method or property.</param>
        /// <param name="action">A reference to the method or property to measure the execution time of.</param>
        /// <returns>
        /// Returns an object for asserting that the execution time matches certain conditions.
        /// </returns>
        [MustUseReturnValue /* do not use Pure because this method executes the action before returning to the caller */]
        public static MemberExecutionTime<T> ExecutionTimeOf<T>(this T subject, Expression<Action<T>> action)
        {
            return new MemberExecutionTime<T>(subject, action);
        }

        /// <summary>
        /// Provides methods for asserting the execution time of an action.
        /// </summary>
        /// <param name="action">An action to measure the execution time of.</param>
        /// <returns>
        /// Returns an object for asserting that the execution time matches certain conditions.
        /// </returns>
        [MustUseReturnValue /* do not use Pure because this method executes the action before returning to the caller */]
        public static ExecutionTime ExecutionTime(this Action action)
        {
            return new ExecutionTime(action);
        }

        /// <summary>
        /// Provides methods for asserting the execution time of an async action.
        /// </summary>
        /// <param name="action">An async action to measure the execution time of.</param>
        /// <returns>
        /// Returns an object for asserting that the execution time matches certain conditions.
        /// </returns>
        [MustUseReturnValue /* do not use Pure because this method executes the action before returning to the caller */]
        public static ExecutionTime ExecutionTime(this Func<Task> action)
        {
            return new ExecutionTime(action.ExecuteInDefaultSynchronizationContext);
        }

        /// <summary>
        /// Returns an <see cref="ExecutionTimeAssertions"/> object that can be used to assert the
        /// current <see cref="ExecutionTime"/>.
        /// </summary>
        [Pure]
        public static ExecutionTimeAssertions Should(this ExecutionTime executionTime)
        {
            return new ExecutionTimeAssertions(executionTime);
        }

        /// <summary>
        /// Returns an <see cref="AssemblyAssertions"/> object that can be used to assert the
        /// current <see cref="Assembly"/>.
        /// </summary>
        [Pure]
        public static AssemblyAssertions Should(this Assembly assembly)
        {
            return new AssemblyAssertions(assembly);
        }

        /// <summary>
        /// Returns an <see cref="XDocumentAssertions"/> object that can be used to assert the
        /// current <see cref="XElement"/>.
        /// </summary>
        [Pure]
        public static XDocumentAssertions Should(this XDocument actualValue)
        {
            return new XDocumentAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="XElementAssertions"/> object that can be used to assert the
        /// current <see cref="XElement"/>.
        /// </summary>
        [Pure]
        public static XElementAssertions Should(this XElement actualValue)
        {
            return new XElementAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="XAttributeAssertions"/> object that can be used to assert the
        /// current <see cref="XAttribute"/>.
        /// </summary>
        [Pure]
        public static XAttributeAssertions Should(this XAttribute actualValue)
        {
            return new XAttributeAssertions(actualValue);
        }

        /// <summary>
        /// Forces enumerating a collection. Should be used to assert that a method that uses the
        /// <c>yield</c> keyword throws a particular exception.
        /// </summary>
        [Pure]
        public static Action Enumerating(this Func<IEnumerable> enumerable)
        {
            return () => ForceEnumeration(enumerable);
        }

        /// <summary>
        /// Forces enumerating a collection. Should be used to assert that a method that uses the
        /// <c>yield</c> keyword throws a particular exception.
        /// </summary>
        [Pure]
        public static Action Enumerating<T>(this Func<IEnumerable<T>> enumerable)
        {
            return () => ForceEnumeration(enumerable);
        }

        private static void ForceEnumeration(Func<IEnumerable> enumerable)
        {
            foreach (object _ in enumerable())
            {
                // Do nothing
            }
        }

        /// <summary>
        /// Returns an <see cref="ObjectAssertions"/> object that can be used to assert the
        /// current <see cref="object"/>.
        /// </summary>
        [Pure]
        public static ObjectAssertions Should(this object actualValue)
        {
            return new ObjectAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="BooleanAssertions"/> object that can be used to assert the
        /// current <see cref="bool"/>.
        /// </summary>
        [Pure]
        public static BooleanAssertions Should(this bool actualValue)
        {
            return new BooleanAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableBooleanAssertions"/> object that can be used to assert the
        /// current nullable <see cref="bool"/>.
        /// </summary>
        [Pure]
        public static NullableBooleanAssertions Should(this bool? actualValue)
        {
            return new NullableBooleanAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="GuidAssertions"/> object that can be used to assert the
        /// current <see cref="Guid"/>.
        /// </summary>
        [Pure]
        public static GuidAssertions Should(this Guid actualValue)
        {
            return new GuidAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableGuidAssertions"/> object that can be used to assert the
        /// current nullable <see cref="Guid"/>.
        /// </summary>
        [Pure]
        public static NullableGuidAssertions Should(this Guid? actualValue)
        {
            return new NullableGuidAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NonGenericCollectionAssertions"/> object that can be used to assert the
        /// current <see cref="IEnumerable"/>.
        /// </summary>
        [Pure]
        public static NonGenericCollectionAssertions Should(this IEnumerable actualValue)
        {
            return new NonGenericCollectionAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="GenericCollectionAssertions{T}"/> object that can be used to assert the
        /// current <see cref="IEnumerable{T}"/>.
        /// </summary>
        [Pure]
        public static GenericCollectionAssertions<T> Should<T>(this IEnumerable<T> actualValue)
        {
            return new GenericCollectionAssertions<T>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="StringCollectionAssertions"/> object that can be used to assert the
        /// current <see cref="IEnumerable{T}"/>.
        /// </summary>
        [Pure]
        public static StringCollectionAssertions Should(this IEnumerable<string> @this)
        {
            return new StringCollectionAssertions(@this);
        }

        /// <summary>
        /// Returns an <see cref="GenericDictionaryAssertions{TKey, TValue}"/> object that can be used to assert the
        /// current <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        [Pure]
        public static GenericDictionaryAssertions<TKey, TValue> Should<TKey, TValue>(this IDictionary<TKey, TValue> actualValue)
        {
            return new GenericDictionaryAssertions<TKey, TValue>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="DateTimeAssertions"/> object that can be used to assert the
        /// current <see cref="DateTime"/>.
        /// </summary>
        [Pure]
        public static DateTimeAssertions Should(this DateTime actualValue)
        {
            return new DateTimeAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="DateTimeOffsetAssertions"/> object that can be used to assert the
        /// current <see cref="DateTimeOffset"/>.
        /// </summary>
        [Pure]
        public static DateTimeOffsetAssertions Should(this DateTimeOffset actualValue)
        {
            return new DateTimeOffsetAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableDateTimeAssertions"/> object that can be used to assert the
        /// current nullable <see cref="DateTime"/>.
        /// </summary>
        [Pure]
        public static NullableDateTimeAssertions Should(this DateTime? actualValue)
        {
            return new NullableDateTimeAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableDateTimeOffsetAssertions"/> object that can be used to assert the
        /// current nullable <see cref="DateTimeOffset"/>.
        /// </summary>
        [Pure]
        public static NullableDateTimeOffsetAssertions Should(this DateTimeOffset? actualValue)
        {
            return new NullableDateTimeOffsetAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="ComparableTypeAssertions{T}"/> object that can be used to assert the
        /// current <see cref="IComparable{T}"/>.
        /// </summary>
        [Pure]
        public static ComparableTypeAssertions<T> Should<T>(this IComparable<T> comparableValue)
        {
            return new ComparableTypeAssertions<T>(comparableValue);
        }

        /// <summary>
        /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
        /// current <see cref="int"/>.
        /// </summary>
        [Pure]
        public static NumericAssertions<int> Should(this int actualValue)
        {
            return new NumericAssertions<int>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
        /// current nullable <see cref="int"/>.
        /// </summary>
        [Pure]
        public static NullableNumericAssertions<int> Should(this int? actualValue)
        {
            return new NullableNumericAssertions<int>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
        /// current <see cref="uint"/>.
        /// </summary>
        [Pure]
        public static NumericAssertions<uint> Should(this uint actualValue)
        {
            return new NumericAssertions<uint>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
        /// current nullable <see cref="uint"/>.
        /// </summary>
        [Pure]
        public static NullableNumericAssertions<uint> Should(this uint? actualValue)
        {
            return new NullableNumericAssertions<uint>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
        /// current <see cref="decimal"/>.
        /// </summary>
        [Pure]
        public static NumericAssertions<decimal> Should(this decimal actualValue)
        {
            return new NumericAssertions<decimal>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
        /// current nullable <see cref="decimal"/>.
        /// </summary>
        [Pure]
        public static NullableNumericAssertions<decimal> Should(this decimal? actualValue)
        {
            return new NullableNumericAssertions<decimal>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
        /// current <see cref="byte"/>.
        /// </summary>
        [Pure]
        public static NumericAssertions<byte> Should(this byte actualValue)
        {
            return new NumericAssertions<byte>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
        /// current nullable <see cref="byte"/>.
        /// </summary>
        [Pure]
        public static NullableNumericAssertions<byte> Should(this byte? actualValue)
        {
            return new NullableNumericAssertions<byte>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
        /// current <see cref="sbyte"/>.
        /// </summary>
        [Pure]
        public static NumericAssertions<sbyte> Should(this sbyte actualValue)
        {
            return new NumericAssertions<sbyte>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
        /// current nullable <see cref="sbyte"/>.
        /// </summary>
        [Pure]
        public static NullableNumericAssertions<sbyte> Should(this sbyte? actualValue)
        {
            return new NullableNumericAssertions<sbyte>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
        /// current <see cref="short"/>.
        /// </summary>
        [Pure]
        public static NumericAssertions<short> Should(this short actualValue)
        {
            return new NumericAssertions<short>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
        /// current nullable <see cref="short"/>.
        /// </summary>
        [Pure]
        public static NullableNumericAssertions<short> Should(this short? actualValue)
        {
            return new NullableNumericAssertions<short>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
        /// current <see cref="ushort"/>.
        /// </summary>
        [Pure]
        public static NumericAssertions<ushort> Should(this ushort actualValue)
        {
            return new NumericAssertions<ushort>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
        /// current nullable <see cref="ushort"/>.
        /// </summary>
        [Pure]
        public static NullableNumericAssertions<ushort> Should(this ushort? actualValue)
        {
            return new NullableNumericAssertions<ushort>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
        /// current <see cref="long"/>.
        /// </summary>
        [Pure]
        public static NumericAssertions<long> Should(this long actualValue)
        {
            return new NumericAssertions<long>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
        /// current nullable <see cref="long"/>.
        /// </summary>
        [Pure]
        public static NullableNumericAssertions<long> Should(this long? actualValue)
        {
            return new NullableNumericAssertions<long>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
        /// current <see cref="ulong"/>.
        /// </summary>
        [Pure]
        public static NumericAssertions<ulong> Should(this ulong actualValue)
        {
            return new NumericAssertions<ulong>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
        /// current nullable <see cref="ulong"/>.
        /// </summary>
        [Pure]
        public static NullableNumericAssertions<ulong> Should(this ulong? actualValue)
        {
            return new NullableNumericAssertions<ulong>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
        /// current <see cref="float"/>.
        /// </summary>
        [Pure]
        public static NumericAssertions<float> Should(this float actualValue)
        {
            return new NumericAssertions<float>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
        /// current nullable <see cref="float"/>.
        /// </summary>
        [Pure]
        public static NullableNumericAssertions<float> Should(this float? actualValue)
        {
            return new NullableNumericAssertions<float>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
        /// current <see cref="double"/>.
        /// </summary>
        [Pure]
        public static NumericAssertions<double> Should(this double actualValue)
        {
            return new NumericAssertions<double>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
        /// current nullable <see cref="double"/>.
        /// </summary>
        [Pure]
        public static NullableNumericAssertions<double> Should(this double? actualValue)
        {
            return new NullableNumericAssertions<double>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="StringAssertions"/> object that can be used to assert the
        /// current <see cref="string"/>.
        /// </summary>
        [Pure]
        public static StringAssertions Should(this string actualValue)
        {
            return new StringAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="SimpleTimeSpanAssertions"/> object that can be used to assert the
        /// current <see cref="TimeSpan"/>.
        /// </summary>
        [Pure]
        public static SimpleTimeSpanAssertions Should(this TimeSpan actualValue)
        {
            return new SimpleTimeSpanAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableSimpleTimeSpanAssertions"/> object that can be used to assert the
        /// current nullable <see cref="TimeSpan"/>.
        /// </summary>
        [Pure]
        public static NullableSimpleTimeSpanAssertions Should(this TimeSpan? actualValue)
        {
            return new NullableSimpleTimeSpanAssertions(actualValue);
        }

        /// <summary>
        /// Returns a <see cref="TypeAssertions"/> object that can be used to assert the
        /// current <see cref="System.Type"/>.
        /// </summary>
        [Pure]
        public static TypeAssertions Should(this Type subject)
        {
            return new TypeAssertions(subject);
        }

        /// <summary>
        /// Returns a <see cref="TypeAssertions"/> object that can be used to assert the
        /// current <see cref="System.Type"/>.
        /// </summary>
        [Pure]
        public static TypeSelectorAssertions Should(this TypeSelector typeSelector)
        {
            return new TypeSelectorAssertions(typeSelector.ToArray());
        }

        /// <summary>
        /// Returns a <see cref="FluentAssertions.Types.MethodBaseAssertions{TSubject, TAssertions}"/> object
        /// that can be used to assert the current <see cref="MethodInfo"/>.
        /// </summary>
        /// <seealso cref="TypeAssertions"/>
        [Pure]
        public static ConstructorInfoAssertions Should(this ConstructorInfo constructorInfo)
        {
            return new ConstructorInfoAssertions(constructorInfo);
        }

        /// <summary>
        /// Returns a <see cref="MethodInfoAssertions"/> object that can be used to assert the current <see cref="MethodInfo"/>.
        /// </summary>
        /// <seealso cref="TypeAssertions"/>
        [Pure]
        public static MethodInfoAssertions Should(this MethodInfo methodInfo)
        {
            return new MethodInfoAssertions(methodInfo);
        }

        /// <summary>
        /// Returns a <see cref="MethodInfoSelectorAssertions"/> object that can be used to assert the methods returned by the
        /// current <see cref="MethodInfoSelector"/>.
        /// </summary>
        /// <seealso cref="TypeAssertions"/>
        [Pure]
        public static MethodInfoSelectorAssertions Should(this MethodInfoSelector methodSelector)
        {
            return new MethodInfoSelectorAssertions(methodSelector.ToArray());
        }

        /// <summary>
        /// Returns a <see cref="PropertyInfoAssertions"/> object that can be used to assert the
        /// current <see cref="PropertyInfoSelector"/>.
        /// </summary>
        /// <seealso cref="TypeAssertions"/>
        [Pure]
        public static PropertyInfoAssertions Should(this PropertyInfo propertyInfo)
        {
            return new PropertyInfoAssertions(propertyInfo);
        }

        /// <summary>
        /// Returns a <see cref="PropertyInfoSelectorAssertions"/> object that can be used to assert the properties returned by the
        /// current <see cref="PropertyInfoSelector"/>.
        /// </summary>
        /// <seealso cref="TypeAssertions"/>
        [Pure]
        public static PropertyInfoSelectorAssertions Should(this PropertyInfoSelector propertyInfoSelector)
        {
            return new PropertyInfoSelectorAssertions(propertyInfoSelector.ToArray());
        }

        /// <summary>
        /// Returns a <see cref="ActionAssertions"/> object that can be used to assert the
        /// current <see cref="System.Action"/> .
        /// </summary>
        [Pure]
        public static ActionAssertions Should(this Action action)
        {
            return new ActionAssertions(action, extractor);
        }

        /// <summary>
        /// Returns a <see cref="AsyncFunctionAssertions"/> object that can be used to assert the
        /// current <see cref="System.Func{Task}"/> .
        /// </summary>
        [Pure]
        public static AsyncFunctionAssertions Should(this Func<Task> action)
        {
            return new AsyncFunctionAssertions(action.ExecuteInDefaultSynchronizationContext, extractor);
        }

        /// <summary>
        /// Returns a <see cref="AsyncFunctionAssertions"/> object that can be used to assert the
        /// current <see><cref>System.Func{Task{T}}</cref></see>.
        /// </summary>
        [Pure]
        public static AsyncFunctionAssertions Should<T>(this Func<Task<T>> action)
        {
            return new AsyncFunctionAssertions(action.ExecuteInDefaultSynchronizationContext, extractor);
        }

        /// <summary>
        /// Returns a <see cref="FunctionAssertions{T}"/> object that can be used to assert the
        /// current <see cref="System.Func{T}"/> .
        /// </summary>
        [Pure]
        public static FunctionAssertions<T> Should<T>(this Func<T> func)
        {
            return new FunctionAssertions<T>(func, extractor);
        }
        

#if NET45 || NET47 || NETCOREAPP2_0

        /// <summary>
        ///   Starts monitoring <paramref name="eventSource"/> for its events.
        /// </summary>
        /// <param name="eventSource">The object for which to monitor the events.</param>
        /// <param name="utcNow">
        /// An optional delegate that returns the current date and time in UTC format.
        /// Will revert to <see cref="DateTime.UtcNow"/> if no delegate was provided.
        /// </param>
        /// <exception cref = "ArgumentNullException">Thrown if <paramref name="eventSource"/> is Null.</exception>
        public static IMonitor<T> Monitor<T>(this T eventSource, Func<DateTime> utcNow = null)
        {
            return new EventMonitor<T>(eventSource, utcNow ?? (() => DateTime.UtcNow));
        }

#endif

        /// <summary>
        /// Safely casts the specified object to the type specified through <typeparamref name="TTo"/>.
        /// </summary>
        /// <remarks>
        /// Has been introduced to allow casting objects without breaking the fluent API.
        /// </remarks>
        /// <typeparam name="TTo"></typeparam>
        [Pure]
        public static TTo As<TTo>(this object subject)
        {
            return subject is TTo ? (TTo)subject : default(TTo);
        }
    }
}
