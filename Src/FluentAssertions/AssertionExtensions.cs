using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using FluentAssertions.Collections;
using FluentAssertions.Common;
using FluentAssertions.Data;
using FluentAssertions.Numeric;
using FluentAssertions.Primitives;
using FluentAssertions.Reflection;
using FluentAssertions.Specialized;
using FluentAssertions.Streams;
using FluentAssertions.Types;
using FluentAssertions.Xml;
using JetBrains.Annotations;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;
#if !NETSTANDARD2_0
using FluentAssertions.Events;
#endif

namespace FluentAssertions;

/// <summary>
/// Contains extension methods for custom assertions in unit tests.
/// </summary>
[DebuggerNonUserCode]
public static class AssertionExtensions
{
    private static readonly AggregateExceptionExtractor Extractor = new();

    /// <summary>
    /// Invokes the specified action on a subject so that you can chain it
    /// with any of the assertions from <see cref="ActionAssertions"/>
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="subject"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
    [Pure]
    public static Action Invoking<T>(this T subject, Action<T> action)
    {
        Guard.ThrowIfArgumentIsNull(subject);
        Guard.ThrowIfArgumentIsNull(action);

        return () => action(subject);
    }

    /// <summary>
    /// Invokes the specified action on a subject so that you can chain it
    /// with any of the assertions from <see cref="FunctionAssertions{T}"/>
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="subject"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
    [Pure]
    public static Func<TResult> Invoking<T, TResult>(this T subject, Func<T, TResult> action)
    {
        Guard.ThrowIfArgumentIsNull(subject);
        Guard.ThrowIfArgumentIsNull(action);

        return () => action(subject);
    }

    /// <summary>
    /// Invokes the specified action on a subject so that you can chain it
    /// with any of the assertions from <see cref="NonGenericAsyncFunctionAssertions"/>
    /// </summary>
    [Pure]
    public static Func<Task> Awaiting<T>(this T subject, Func<T, Task> action)
    {
        return () => action(subject);
    }

    /// <summary>
    /// Invokes the specified action on a subject so that you can chain it
    /// with any of the assertions from <see cref="GenericAsyncFunctionAssertions{TResult}"/>
    /// </summary>
    [Pure]
    public static Func<Task<TResult>> Awaiting<T, TResult>(this T subject, Func<T, Task<TResult>> action)
    {
        return () => action(subject);
    }

    /// <summary>
    /// Invokes the specified action on a subject so that you can chain it
    /// with any of the assertions from <see cref="NonGenericAsyncFunctionAssertions"/>
    /// </summary>
    [Pure]
    public static Func<Task> Awaiting<T>(this T subject, Func<T, ValueTask> action)
    {
        return () => action(subject).AsTask();
    }

    /// <summary>
    /// Invokes the specified action on a subject so that you can chain it
    /// with any of the assertions from <see cref="GenericAsyncFunctionAssertions{TResult}"/>
    /// </summary>
    [Pure]
    public static Func<Task<TResult>> Awaiting<T, TResult>(this T subject, Func<T, ValueTask<TResult>> action)
    {
        return () => action(subject).AsTask();
    }

    /// <summary>
    /// Provides methods for asserting the execution time of a method or property.
    /// </summary>
    /// <param name="subject">The object that exposes the method or property.</param>
    /// <param name="action">A reference to the method or property to measure the execution time of.</param>
    /// <returns>
    /// Returns an object for asserting that the execution time matches certain conditions.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="subject"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
    [MustUseReturnValue /* do not use Pure because this method executes the action before returning to the caller */]
    public static MemberExecutionTime<T> ExecutionTimeOf<T>(this T subject, Expression<Action<T>> action,
        StartTimer createTimer = null)
    {
        Guard.ThrowIfArgumentIsNull(subject);
        Guard.ThrowIfArgumentIsNull(action);

        createTimer ??= () => new StopwatchTimer();

        return new MemberExecutionTime<T>(subject, action, createTimer);
    }

    /// <summary>
    /// Provides methods for asserting the execution time of an action.
    /// </summary>
    /// <param name="action">An action to measure the execution time of.</param>
    /// <returns>
    /// Returns an object for asserting that the execution time matches certain conditions.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
    [MustUseReturnValue /* do not use Pure because this method executes the action before returning to the caller */]
    public static ExecutionTime ExecutionTime(this Action action, StartTimer createTimer = null)
    {
        createTimer ??= () => new StopwatchTimer();

        return new ExecutionTime(action, createTimer);
    }

    /// <summary>
    /// Provides methods for asserting the execution time of an async action.
    /// </summary>
    /// <param name="action">An async action to measure the execution time of.</param>
    /// <returns>
    /// Returns an object for asserting that the execution time matches certain conditions.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
    [MustUseReturnValue /* do not use Pure because this method executes the action before returning to the caller */]
    public static ExecutionTime ExecutionTime(this Func<Task> action)
    {
        return new ExecutionTime(action, () => new StopwatchTimer());
    }

    /// <summary>
    /// Returns an <see cref="ExecutionTimeAssertions"/> object that can be used to assert the
    /// current <see cref="FluentAssertions.Specialized.ExecutionTime"/>.
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
    public static AssemblyAssertions Should([NotNull] this Assembly assembly)
    {
        return new AssemblyAssertions(assembly);
    }

    /// <summary>
    /// Returns an <see cref="XDocumentAssertions"/> object that can be used to assert the
    /// current <see cref="XElement"/>.
    /// </summary>
    [Pure]
    public static XDocumentAssertions Should([NotNull] this XDocument actualValue)
    {
        return new XDocumentAssertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="XElementAssertions"/> object that can be used to assert the
    /// current <see cref="XElement"/>.
    /// </summary>
    [Pure]
    public static XElementAssertions Should([NotNull] this XElement actualValue)
    {
        return new XElementAssertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="XAttributeAssertions"/> object that can be used to assert the
    /// current <see cref="XAttribute"/>.
    /// </summary>
    [Pure]
    public static XAttributeAssertions Should([NotNull] this XAttribute actualValue)
    {
        return new XAttributeAssertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="StreamAssertions"/> object that can be used to assert the
    /// current <see cref="Stream"/>.
    /// </summary>
    [Pure]
    public static StreamAssertions Should([NotNull] this Stream actualValue)
    {
        return new StreamAssertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="BufferedStreamAssertions"/> object that can be used to assert the
    /// current <see cref="BufferedStream"/>.
    /// </summary>
    [Pure]
    public static BufferedStreamAssertions Should([NotNull] this BufferedStream actualValue)
    {
        return new BufferedStreamAssertions(actualValue);
    }

    /// <summary>
    /// Forces enumerating a collection. Should be used to assert that a method that uses the
    /// <see langword="yield"/> keyword throws a particular exception.
    /// </summary>
    [Pure]
    public static Action Enumerating(this Func<IEnumerable> enumerable)
    {
        return () => ForceEnumeration(enumerable);
    }

    /// <summary>
    /// Forces enumerating a collection. Should be used to assert that a method that uses the
    /// <see langword="yield"/> keyword throws a particular exception.
    /// </summary>
    [Pure]
    public static Action Enumerating<T>(this Func<IEnumerable<T>> enumerable)
    {
        return () => ForceEnumeration(enumerable);
    }

    /// <summary>
    /// Forces enumerating a collection of the provided <paramref name="subject"/>.
    /// Should be used to assert that a method that uses the <see langword="yield"/> keyword throws a particular exception.
    /// </summary>
    /// <param name="subject">The object that exposes the method or property.</param>
    /// <param name="enumerable">A reference to the method or property to force enumeration of.</param>
    public static Action Enumerating<T, TResult>(this T subject, Func<T, IEnumerable<TResult>> enumerable)
    {
        return () => ForceEnumeration(subject, enumerable);
    }

    private static void ForceEnumeration(Func<IEnumerable> enumerable)
    {
        foreach (object _ in enumerable())
        {
            // Do nothing
        }
    }

    private static void ForceEnumeration<T>(T subject, Func<T, IEnumerable> enumerable)
    {
        foreach (object _ in enumerable(subject))
        {
            // Do nothing
        }
    }

    /// <summary>
    /// Returns an <see cref="ObjectAssertions"/> object that can be used to assert the
    /// current <see cref="object"/>.
    /// </summary>
    [Pure]
    public static ObjectAssertions Should([NotNull] this object actualValue)
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
    public static NullableBooleanAssertions Should([NotNull] this bool? actualValue)
    {
        return new NullableBooleanAssertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="HttpResponseMessageAssertions"/> object that can be used to assert the
    /// current <see cref="HttpResponseMessage"/>.
    /// </summary>
    [Pure]
    public static HttpResponseMessageAssertions Should([NotNull] this HttpResponseMessage actualValue)
    {
        return new HttpResponseMessageAssertions(actualValue);
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
    public static NullableGuidAssertions Should([NotNull] this Guid? actualValue)
    {
        return new NullableGuidAssertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="GenericCollectionAssertions{T}"/> object that can be used to assert the
    /// current <see cref="IEnumerable{T}"/>.
    /// </summary>
    [Pure]
    public static GenericCollectionAssertions<T> Should<T>([NotNull] this IEnumerable<T> actualValue)
    {
        return new GenericCollectionAssertions<T>(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="StringCollectionAssertions"/> object that can be used to assert the
    /// current <see cref="IEnumerable{T}"/>.
    /// </summary>
    [Pure]
    public static StringCollectionAssertions Should([NotNull] this IEnumerable<string> @this)
    {
        return new StringCollectionAssertions(@this);
    }

    /// <summary>
    /// Returns an <see cref="GenericDictionaryAssertions{TCollection, TKey, TValue}"/> object that can be used to assert the
    /// current <see cref="IDictionary{TKey, TValue}"/>.
    /// </summary>
    [Pure]
    public static GenericDictionaryAssertions<IDictionary<TKey, TValue>, TKey, TValue> Should<TKey, TValue>(
        [NotNull] this IDictionary<TKey, TValue> actualValue)
    {
        return new GenericDictionaryAssertions<IDictionary<TKey, TValue>, TKey, TValue>(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="GenericDictionaryAssertions{TCollection, TKey, TValue}"/> object that can be used to assert the
    /// current <see cref="IEnumerable{T}"/> of <see cref="KeyValuePair{TKey, TValue}"/>.
    /// </summary>
    [Pure]
    public static GenericDictionaryAssertions<IEnumerable<KeyValuePair<TKey, TValue>>, TKey, TValue> Should<TKey, TValue>(
        [NotNull] this IEnumerable<KeyValuePair<TKey, TValue>> actualValue)
    {
        return new GenericDictionaryAssertions<IEnumerable<KeyValuePair<TKey, TValue>>, TKey, TValue>(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="GenericDictionaryAssertions{TCollection, TKey, TValue}"/> object that can be used to assert the
    /// current <typeparamref name="TCollection"/>.
    /// </summary>
    [Pure]
    public static GenericDictionaryAssertions<TCollection, TKey, TValue> Should<TCollection, TKey, TValue>(
        [NotNull] this TCollection actualValue)
        where TCollection : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        return new GenericDictionaryAssertions<TCollection, TKey, TValue>(actualValue);
    }

    /// <summary>
    /// Returns an assertions object that provides methods for asserting the state of a <see cref="DataTableCollection"/>.
    /// </summary>
    [Pure]
    public static GenericCollectionAssertions<DataTable> Should([NotNull] this DataTableCollection actualValue)
    {
        return new GenericCollectionAssertions<DataTable>(
            ReadOnlyNonGenericCollectionWrapper.Create(actualValue));
    }

    /// <summary>
    /// Returns an assertions object that provides methods for asserting the state of a <see cref="DataColumnCollection"/>.
    /// </summary>
    [Pure]
    public static GenericCollectionAssertions<DataColumn> Should([NotNull] this DataColumnCollection actualValue)
    {
        return new GenericCollectionAssertions<DataColumn>(
            ReadOnlyNonGenericCollectionWrapper.Create(actualValue));
    }

    /// <summary>
    /// Returns an assertions object that provides methods for asserting the state of a <see cref="DataRowCollection"/>.
    /// </summary>
    [Pure]
    public static GenericCollectionAssertions<DataRow> Should([NotNull] this DataRowCollection actualValue)
    {
        return new GenericCollectionAssertions<DataRow>(
            ReadOnlyNonGenericCollectionWrapper.Create(actualValue));
    }

    /// <summary>
    /// Returns a <see cref="DataColumnAssertions"/> object that can be used to assert the
    /// current <see cref="DataColumn"/>.
    /// </summary>
    [Pure]
    public static DataColumnAssertions Should([NotNull] this DataColumn actualValue)
    {
        return new DataColumnAssertions(actualValue);
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
    public static NullableDateTimeAssertions Should([NotNull] this DateTime? actualValue)
    {
        return new NullableDateTimeAssertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="NullableDateTimeOffsetAssertions"/> object that can be used to assert the
    /// current nullable <see cref="DateTimeOffset"/>.
    /// </summary>
    [Pure]
    public static NullableDateTimeOffsetAssertions Should([NotNull] this DateTimeOffset? actualValue)
    {
        return new NullableDateTimeOffsetAssertions(actualValue);
    }

#if NET6_0_OR_GREATER
    /// <summary>
    /// Returns an <see cref="DateOnlyAssertions"/> object that can be used to assert the
    /// current <see cref="DateOnly"/>.
    /// </summary>
    [Pure]
    public static DateOnlyAssertions Should(this DateOnly actualValue)
    {
        return new DateOnlyAssertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="NullableDateOnlyAssertions"/> object that can be used to assert the
    /// current nullable <see cref="DateOnly"/>.
    /// </summary>
    [Pure]
    public static NullableDateOnlyAssertions Should([NotNull] this DateOnly? actualValue)
    {
        return new NullableDateOnlyAssertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="TimeOnlyAssertions"/> object that can be used to assert the
    /// current <see cref="TimeOnly"/>.
    /// </summary>
    [Pure]
    public static TimeOnlyAssertions Should(this TimeOnly actualValue)
    {
        return new TimeOnlyAssertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="NullableTimeOnlyAssertions"/> object that can be used to assert the
    /// current nullable <see cref="TimeOnly"/>.
    /// </summary>
    [Pure]
    public static NullableTimeOnlyAssertions Should([NotNull] this TimeOnly? actualValue)
    {
        return new NullableTimeOnlyAssertions(actualValue);
    }

#endif

    /// <summary>
    /// Returns an <see cref="ComparableTypeAssertions{T}"/> object that can be used to assert the
    /// current <see cref="IComparable{T}"/>.
    /// </summary>
    [Pure]
    public static ComparableTypeAssertions<T> Should<T>([NotNull] this IComparable<T> comparableValue)
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
        return new Int32Assertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
    /// current nullable <see cref="int"/>.
    /// </summary>
    [Pure]
    public static NullableNumericAssertions<int> Should([NotNull] this int? actualValue)
    {
        return new NullableInt32Assertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
    /// current <see cref="uint"/>.
    /// </summary>
    [Pure]
    public static NumericAssertions<uint> Should(this uint actualValue)
    {
        return new UInt32Assertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
    /// current nullable <see cref="uint"/>.
    /// </summary>
    [Pure]
    public static NullableNumericAssertions<uint> Should([NotNull] this uint? actualValue)
    {
        return new NullableUInt32Assertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
    /// current <see cref="decimal"/>.
    /// </summary>
    [Pure]
    public static NumericAssertions<decimal> Should(this decimal actualValue)
    {
        return new DecimalAssertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
    /// current nullable <see cref="decimal"/>.
    /// </summary>
    [Pure]
    public static NullableNumericAssertions<decimal> Should([NotNull] this decimal? actualValue)
    {
        return new NullableDecimalAssertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
    /// current <see cref="byte"/>.
    /// </summary>
    [Pure]
    public static NumericAssertions<byte> Should(this byte actualValue)
    {
        return new ByteAssertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
    /// current nullable <see cref="byte"/>.
    /// </summary>
    [Pure]
    public static NullableNumericAssertions<byte> Should([NotNull] this byte? actualValue)
    {
        return new NullableByteAssertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
    /// current <see cref="sbyte"/>.
    /// </summary>
    [Pure]
    public static NumericAssertions<sbyte> Should(this sbyte actualValue)
    {
        return new SByteAssertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
    /// current nullable <see cref="sbyte"/>.
    /// </summary>
    [Pure]
    public static NullableNumericAssertions<sbyte> Should([NotNull] this sbyte? actualValue)
    {
        return new NullableSByteAssertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
    /// current <see cref="short"/>.
    /// </summary>
    [Pure]
    public static NumericAssertions<short> Should(this short actualValue)
    {
        return new Int16Assertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
    /// current nullable <see cref="short"/>.
    /// </summary>
    [Pure]
    public static NullableNumericAssertions<short> Should([NotNull] this short? actualValue)
    {
        return new NullableInt16Assertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
    /// current <see cref="ushort"/>.
    /// </summary>
    [Pure]
    public static NumericAssertions<ushort> Should(this ushort actualValue)
    {
        return new UInt16Assertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
    /// current nullable <see cref="ushort"/>.
    /// </summary>
    [Pure]
    public static NullableNumericAssertions<ushort> Should([NotNull] this ushort? actualValue)
    {
        return new NullableUInt16Assertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
    /// current <see cref="long"/>.
    /// </summary>
    [Pure]
    public static NumericAssertions<long> Should(this long actualValue)
    {
        return new Int64Assertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
    /// current nullable <see cref="long"/>.
    /// </summary>
    [Pure]
    public static NullableNumericAssertions<long> Should([NotNull] this long? actualValue)
    {
        return new NullableInt64Assertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
    /// current <see cref="ulong"/>.
    /// </summary>
    [Pure]
    public static NumericAssertions<ulong> Should(this ulong actualValue)
    {
        return new UInt64Assertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
    /// current nullable <see cref="ulong"/>.
    /// </summary>
    [Pure]
    public static NullableNumericAssertions<ulong> Should([NotNull] this ulong? actualValue)
    {
        return new NullableUInt64Assertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
    /// current <see cref="float"/>.
    /// </summary>
    [Pure]
    public static NumericAssertions<float> Should(this float actualValue)
    {
        return new SingleAssertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
    /// current nullable <see cref="float"/>.
    /// </summary>
    [Pure]
    public static NullableNumericAssertions<float> Should([NotNull] this float? actualValue)
    {
        return new NullableSingleAssertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="NumericAssertions{T}"/> object that can be used to assert the
    /// current <see cref="double"/>.
    /// </summary>
    [Pure]
    public static NumericAssertions<double> Should(this double actualValue)
    {
        return new DoubleAssertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
    /// current nullable <see cref="double"/>.
    /// </summary>
    [Pure]
    public static NullableNumericAssertions<double> Should([NotNull] this double? actualValue)
    {
        return new NullableDoubleAssertions(actualValue);
    }

    /// <summary>
    /// Returns an <see cref="StringAssertions"/> object that can be used to assert the
    /// current <see cref="string"/>.
    /// </summary>
    [Pure]
    public static StringAssertions Should([NotNull] this string actualValue)
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
    public static NullableSimpleTimeSpanAssertions Should([NotNull] this TimeSpan? actualValue)
    {
        return new NullableSimpleTimeSpanAssertions(actualValue);
    }

    /// <summary>
    /// Returns a <see cref="TypeAssertions"/> object that can be used to assert the
    /// current <see cref="System.Type"/>.
    /// </summary>
    [Pure]
    public static TypeAssertions Should([NotNull] this Type subject)
    {
        return new TypeAssertions(subject);
    }

    /// <summary>
    /// Returns a <see cref="TypeAssertions"/> object that can be used to assert the
    /// current <see cref="Type"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="typeSelector"/> is <see langword="null"/>.</exception>
    [Pure]
    public static TypeSelectorAssertions Should(this TypeSelector typeSelector)
    {
        Guard.ThrowIfArgumentIsNull(typeSelector);

        return new TypeSelectorAssertions(typeSelector.ToArray());
    }

    /// <summary>
    /// Returns a <see cref="FluentAssertions.Types.MethodBaseAssertions{TSubject, TAssertions}"/> object
    /// that can be used to assert the current <see cref="MethodInfo"/>.
    /// </summary>
    /// <seealso cref="TypeAssertions"/>
    [Pure]
    public static ConstructorInfoAssertions Should([NotNull] this ConstructorInfo constructorInfo)
    {
        return new ConstructorInfoAssertions(constructorInfo);
    }

    /// <summary>
    /// Returns a <see cref="MethodInfoAssertions"/> object that can be used to assert the current <see cref="MethodInfo"/>.
    /// </summary>
    /// <seealso cref="TypeAssertions"/>
    [Pure]
    public static MethodInfoAssertions Should([NotNull] this MethodInfo methodInfo)
    {
        return new MethodInfoAssertions(methodInfo);
    }

    /// <summary>
    /// Returns a <see cref="MethodInfoSelectorAssertions"/> object that can be used to assert the methods returned by the
    /// current <see cref="MethodInfoSelector"/>.
    /// </summary>
    /// <seealso cref="TypeAssertions"/>
    /// <exception cref="ArgumentNullException"><paramref name="methodSelector"/> is <see langword="null"/>.</exception>
    [Pure]
    public static MethodInfoSelectorAssertions Should(this MethodInfoSelector methodSelector)
    {
        Guard.ThrowIfArgumentIsNull(methodSelector);

        return new MethodInfoSelectorAssertions(methodSelector.ToArray());
    }

    /// <summary>
    /// Returns a <see cref="PropertyInfoAssertions"/> object that can be used to assert the
    /// current <see cref="PropertyInfoSelector"/>.
    /// </summary>
    /// <seealso cref="TypeAssertions"/>
    [Pure]
    public static PropertyInfoAssertions Should([NotNull] this PropertyInfo propertyInfo)
    {
        return new PropertyInfoAssertions(propertyInfo);
    }

    /// <summary>
    /// Returns a <see cref="PropertyInfoSelectorAssertions"/> object that can be used to assert the properties returned by the
    /// current <see cref="PropertyInfoSelector"/>.
    /// </summary>
    /// <seealso cref="TypeAssertions"/>
    /// <exception cref="ArgumentNullException"><paramref name="propertyInfoSelector"/> is <see langword="null"/>.</exception>
    [Pure]
    public static PropertyInfoSelectorAssertions Should(this PropertyInfoSelector propertyInfoSelector)
    {
        Guard.ThrowIfArgumentIsNull(propertyInfoSelector);

        return new PropertyInfoSelectorAssertions(propertyInfoSelector.ToArray());
    }

    /// <summary>
    /// Returns a <see cref="ActionAssertions"/> object that can be used to assert the
    /// current <see cref="System.Action"/>.
    /// </summary>
    [Pure]
    public static ActionAssertions Should([NotNull] this Action action)
    {
        return new ActionAssertions(action, Extractor);
    }

    /// <summary>
    /// Returns a <see cref="NonGenericAsyncFunctionAssertions"/> object that can be used to assert the
    /// current <see cref="System.Func{Task}"/>.
    /// </summary>
    [Pure]
    public static NonGenericAsyncFunctionAssertions Should([NotNull] this Func<Task> action)
    {
        return new NonGenericAsyncFunctionAssertions(action, Extractor);
    }

    /// <summary>
    /// Returns a <see cref="GenericAsyncFunctionAssertions{T}"/> object that can be used to assert the
    /// current <see><cref>System.Func{Task{T}}</cref></see>.
    /// </summary>
    [Pure]
    public static GenericAsyncFunctionAssertions<T> Should<T>([NotNull] this Func<Task<T>> action)
    {
        return new GenericAsyncFunctionAssertions<T>(action, Extractor);
    }

    /// <summary>
    /// Returns a <see cref="FunctionAssertions{T}"/> object that can be used to assert the
    /// current <see cref="System.Func{T}"/>.
    /// </summary>
    [Pure]
    public static FunctionAssertions<T> Should<T>([NotNull] this Func<T> func)
    {
        return new FunctionAssertions<T>(func, Extractor);
    }

    /// <summary>
    /// Returns a <see cref="TaskCompletionSourceAssertions{T}"/> object that can be used to assert the
    /// current <see cref="TaskCompletionSource{T}"/>.
    /// </summary>
    [Pure]
    public static TaskCompletionSourceAssertions<T> Should<T>(this TaskCompletionSource<T> tcs)
    {
        return new TaskCompletionSourceAssertions<T>(tcs);
    }

#if !NETSTANDARD2_0

    /// <summary>
    /// Starts monitoring <paramref name="eventSource"/> for its events.
    /// </summary>
    /// <param name="eventSource">The object for which to monitor the events.</param>
    /// <param name="utcNow">
    /// An optional delegate that returns the current date and time in UTC format.
    /// Will revert to <see cref="DateTime.UtcNow"/> if no delegate was provided.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="eventSource"/> is <see langword="null"/>.</exception>
    public static IMonitor<T> Monitor<T>(this T eventSource, Func<DateTime> utcNow = null)
    {
        return new EventMonitor<T>(eventSource, utcNow ?? (() => DateTime.UtcNow));
    }

#endif

#if NET6_0_OR_GREATER
    /// <summary>
    /// Returns a <see cref="TaskCompletionSourceAssertions"/> object that can be used to assert the
    /// current <see cref="TaskCompletionSource"/>.
    /// </summary>
    [Pure]
    public static TaskCompletionSourceAssertions Should(this TaskCompletionSource tcs)
    {
        return new TaskCompletionSourceAssertions(tcs);
    }

#endif

    /// <summary>
    /// Safely casts the specified object to the type specified through <typeparamref name="TTo"/>.
    /// </summary>
    /// <remarks>
    /// Has been introduced to allow casting objects without breaking the fluent API.
    /// </remarks>
    /// <typeparam name="TTo">The <see cref="Type"/> to cast <paramref name="subject"/> to</typeparam>
    [Pure]
    public static TTo As<TTo>(this object subject)
    {
        return subject is TTo to ? to : default;
    }

    #region Prevent chaining on AndConstraint

    /// <inheritdoc cref="Should(ExecutionTimeAssertions)" />
    [Obsolete("You are asserting the 'AndConstraint' itself. Remove the 'Should()' method directly following 'And'", error: true)]
    public static void Should<TSubject, TAssertions>(this ReferenceTypeAssertions<TSubject, TAssertions> _)
        where TAssertions : ReferenceTypeAssertions<TSubject, TAssertions>
    {
        InvalidShouldCall();
    }

    /// <inheritdoc cref="Should(ExecutionTimeAssertions)" />
    [Obsolete("You are asserting the 'AndConstraint' itself. Remove the 'Should()' method directly following 'And'", error: true)]
    public static void Should<TAssertions>(this BooleanAssertions<TAssertions> _)
        where TAssertions : BooleanAssertions<TAssertions>
    {
        InvalidShouldCall();
    }

    /// <inheritdoc cref="Should(ExecutionTimeAssertions)" />
    [Obsolete("You are asserting the 'AndConstraint' itself. Remove the 'Should()' method directly following 'And'", error: true)]
    public static void Should<TAssertions>(this DateTimeAssertions<TAssertions> _)
        where TAssertions : DateTimeAssertions<TAssertions>
    {
        InvalidShouldCall();
    }

    /// <inheritdoc cref="Should(ExecutionTimeAssertions)" />
    [Obsolete("You are asserting the 'AndConstraint' itself. Remove the 'Should()' method directly following 'And'", error: true)]
    public static void Should<TAssertions>(this DateTimeOffsetAssertions<TAssertions> _)
        where TAssertions : DateTimeOffsetAssertions<TAssertions>
    {
        InvalidShouldCall();
    }

#if NET6_0_OR_GREATER
    /// <inheritdoc cref="Should(ExecutionTimeAssertions)" />
    [Obsolete("You are asserting the 'AndConstraint' itself. Remove the 'Should()' method directly following 'And'", error: true)]
    public static void Should<TAssertions>(this DateOnlyAssertions<TAssertions> _)
        where TAssertions : DateOnlyAssertions<TAssertions>
    {
        InvalidShouldCall();
    }

    /// <inheritdoc cref="Should(ExecutionTimeAssertions)" />
    [Obsolete("You are asserting the 'AndConstraint' itself. Remove the 'Should()' method directly following 'And'", error: true)]
    public static void Should<TAssertions>(this TimeOnlyAssertions<TAssertions> _)
        where TAssertions : TimeOnlyAssertions<TAssertions>
    {
        InvalidShouldCall();
    }

#endif

    /// <summary>
    /// You are asserting the <see cref="AndConstraint{T}"/> itself. Remove the <c>Should()</c> method directly following <c>And</c>.
    /// </summary>
    [Obsolete("You are asserting the 'AndConstraint' itself. Remove the 'Should()' method directly following 'And'", error: true)]
    public static void Should(this ExecutionTimeAssertions _)
    {
        InvalidShouldCall();
    }

    /// <inheritdoc cref="Should(ExecutionTimeAssertions)" />
    [Obsolete("You are asserting the 'AndConstraint' itself. Remove the 'Should()' method directly following 'And'", error: true)]
    public static void Should<TAssertions>(this GuidAssertions<TAssertions> _)
        where TAssertions : GuidAssertions<TAssertions>
    {
        InvalidShouldCall();
    }

    /// <inheritdoc cref="Should(ExecutionTimeAssertions)" />
    [Obsolete("You are asserting the 'AndConstraint' itself. Remove the 'Should()' method directly following 'And'", error: true)]
    public static void Should(this MethodInfoSelectorAssertions _)
    {
        InvalidShouldCall();
    }

    /// <inheritdoc cref="Should(ExecutionTimeAssertions)" />
    [Obsolete("You are asserting the 'AndConstraint' itself. Remove the 'Should()' method directly following 'And'", error: true)]
    public static void Should<TSubject, TAssertions>(this NumericAssertions<TSubject, TAssertions> _)
        where TSubject : struct, IComparable<TSubject>
        where TAssertions : NumericAssertions<TSubject, TAssertions>
    {
        InvalidShouldCall();
    }

    /// <inheritdoc cref="Should(ExecutionTimeAssertions)" />
    [Obsolete("You are asserting the 'AndConstraint' itself. Remove the 'Should()' method directly following 'And'", error: true)]
    public static void Should(this PropertyInfoSelectorAssertions _)
    {
        InvalidShouldCall();
    }

    /// <inheritdoc cref="Should(ExecutionTimeAssertions)" />
    [Obsolete("You are asserting the 'AndConstraint' itself. Remove the 'Should()' method directly following 'And'", error: true)]
    public static void Should<TAssertions>(this SimpleTimeSpanAssertions<TAssertions> _)
        where TAssertions : SimpleTimeSpanAssertions<TAssertions>
    {
        InvalidShouldCall();
    }

    /// <inheritdoc cref="Should(ExecutionTimeAssertions)" />
    [Obsolete("You are asserting the 'AndConstraint' itself. Remove the 'Should()' method directly following 'And'", error: true)]
    public static void Should(this TaskCompletionSourceAssertionsBase _)
    {
        InvalidShouldCall();
    }

    /// <inheritdoc cref="Should(ExecutionTimeAssertions)" />
    [Obsolete("You are asserting the 'AndConstraint' itself. Remove the 'Should()' method directly following 'And'", error: true)]
    public static void Should(this TypeSelectorAssertions _)
    {
        InvalidShouldCall();
    }

    /// <inheritdoc cref="Should(ExecutionTimeAssertions)" />
    [Obsolete("You are asserting the 'AndConstraint' itself. Remove the 'Should()' method directly following 'And'", error: true)]
    public static void Should<TEnum, TAssertions>(this EnumAssertions<TEnum, TAssertions> _)
        where TEnum : struct, Enum
        where TAssertions : EnumAssertions<TEnum, TAssertions>
    {
        InvalidShouldCall();
    }

    /// <inheritdoc cref="Should(ExecutionTimeAssertions)" />
    [Obsolete("You are asserting the 'AndConstraint' itself. Remove the 'Should()' method directly following 'And'", error: true)]
    public static void Should<TAssertions>(this DateTimeRangeAssertions<TAssertions> _)
        where TAssertions : DateTimeAssertions<TAssertions>
    {
        InvalidShouldCall();
    }

    /// <inheritdoc cref="Should(ExecutionTimeAssertions)" />
    [Obsolete("You are asserting the 'AndConstraint' itself. Remove the 'Should()' method directly following 'And'", error: true)]
    public static void Should<TAssertions>(this DateTimeOffsetRangeAssertions<TAssertions> _)
        where TAssertions : DateTimeOffsetAssertions<TAssertions>
    {
        InvalidShouldCall();
    }

    [DoesNotReturn]
    private static void InvalidShouldCall()
    {
        throw new InvalidOperationException(
            "You are asserting the 'AndConstraint' itself. Remove the 'Should()' method directly following 'And'.");
    }

    #endregion
}
