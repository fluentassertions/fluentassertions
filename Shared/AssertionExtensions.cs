using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;

using FluentAssertions.Collections;
using FluentAssertions.Common;
using FluentAssertions.Equivalency;
using FluentAssertions.Events;
using FluentAssertions.Numeric;
using FluentAssertions.Primitives;
using FluentAssertions.Reflection;
using FluentAssertions.Specialized;
using FluentAssertions.Types;
using FluentAssertions.Xml;

#if NET45 || WINRT
using System.Threading.Tasks;
#endif

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
        public static Action Invoking<T>(this T subject, Action<T> action)
        {
            return () => action(subject);
        }

#if NET45 || WINRT
        public static Func<Task> Awaiting<T>(this T subject, Func<T, Task> action)
        {
            return () => action(subject);
        }
#endif

#if !SILVERLIGHT && !PORTABLE

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

#if !WINRT

        /// <summary>
        /// Returns an <see cref="AssemblyAssertions"/> object that can be used to assert the
        /// current <see cref="Assembly"/>.
        /// </summary>
        public static AssemblyAssertions Should(this Assembly assembly)
        {
            return new AssemblyAssertions(assembly);
        }
#endif

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
            foreach (object item in enumerable())
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
        /// Returns an <see cref="StringCollectionAssertions"/> object that can be used to assert the
        /// current <see cref="IEnumerable{T}"/>.
        /// </summary>
        public static StringCollectionAssertions Should(this IEnumerable<string> @this)
        {
            return new StringCollectionAssertions(@this);
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
        /// Returns an <see cref="DateTimeOffsetAssertions"/> object that can be used to assert the
        /// current <see cref="DateTime"/>.
        /// </summary>
        public static DateTimeOffsetAssertions Should(this DateTime actualValue)
        {
            return new DateTimeOffsetAssertions(actualValue.ToDateTimeOffset());
        }

        /// <summary>
        /// Returns an <see cref="DateTimeOffsetAssertions"/> object that can be used to assert the
        /// current <see cref="DateTimeOffset"/>.
        /// </summary>
        public static DateTimeOffsetAssertions Should(this DateTimeOffset actualValue)
        {
            return new DateTimeOffsetAssertions(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableDateTimeOffsetAssertions"/> object that can be used to assert the
        /// current nullable <see cref="DateTime"/>.
        /// </summary>
        public static NullableDateTimeOffsetAssertions Should(this DateTime? actualValue)
        {
            return
                new NullableDateTimeOffsetAssertions(actualValue.HasValue
                    ? actualValue.Value.ToDateTimeOffset()
                    : (DateTimeOffset?)null);
        }

        /// <summary>
        /// Returns an <see cref="NullableDateTimeOffsetAssertions"/> object that can be used to assert the
        /// current nullable <see cref="DateTimeOffset"/>.
        /// </summary>
        public static NullableDateTimeOffsetAssertions Should(this DateTimeOffset? actualValue)
        {
            return new NullableDateTimeOffsetAssertions(actualValue);
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
        /// current <see cref="decimal"/>.
        /// </summary>
        public static NumericAssertions<decimal> Should(this decimal actualValue)
        {
            return new NumericAssertions<decimal>(actualValue);
        }

        /// <summary>
        /// Returns an <see cref="NullableNumericAssertions{T}"/> object that can be used to assert the
        /// current nullable <see cref="decimal"/>.
        /// </summary>
        public static NullableNumericAssertions<decimal> Should(this decimal? actualValue)
        {
            return new NullableNumericAssertions<decimal>(actualValue);
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
        /// Returns a <see cref="TypeAssertions"/> object that can be used to assert the
        /// current <see cref="Type"/>.
        /// </summary>
        public static TypeAssertions Should(this Type subject)
        {
            return new TypeAssertions(subject);
        }

        /// <summary>
        /// Returns a <see cref="TypeAssertions"/> object that can be used to assert the
        /// current <see cref="Type"/>.
        /// </summary>
        public static TypeSelectorAssertions Should(this TypeSelector typeSelector)
        {
            return new TypeSelectorAssertions(typeSelector.ToArray());
        }

        /// <summary>
        /// Returns a <see cref="MethodInfoAssertions"/> object that can be used to assert the current <see cref="MethodInfo"/>.
        /// </summary>
        /// <seealso cref="TypeAssertions"/>
        public static MethodInfoAssertions Should(this MethodInfo methodInfo)
        {
            return new MethodInfoAssertions(methodInfo);
        }

        /// <summary>
        /// Returns a <see cref="MethodInfoSelectorAssertions"/> object that can be used to assert the methods returned by the
        /// current <see cref="MethodInfoSelector"/>.
        /// </summary>
        /// <seealso cref="TypeAssertions"/>
        public static MethodInfoSelectorAssertions Should(this MethodInfoSelector methodSelector)
        {
            return new MethodInfoSelectorAssertions(methodSelector.ToArray());
        }

        /// <summary>
        /// Returns a <see cref="PropertyInfoAssertions"/> object that can be used to assert the
        /// current <see cref="PropertyInfoSelector"/>.
        /// </summary>
        /// <seealso cref="TypeAssertions"/>
        public static PropertyInfoAssertions Should(this PropertyInfo propertyInfo)
        {
            return new PropertyInfoAssertions(propertyInfo);
        }

        /// <summary>
        /// Returns a <see cref="PropertyInfoSelectorAssertions"/> object that can be used to assert the properties returned by the
        /// current <see cref="PropertyInfoSelector"/>.
        /// </summary>
        /// <seealso cref="TypeAssertions"/>
        public static PropertyInfoSelectorAssertions Should(this PropertyInfoSelector propertyInfoSelector)
        {
            return new PropertyInfoSelectorAssertions(propertyInfoSelector.ToArray());
        }

        /// <summary>
        /// Asserts that an object is equivalent to another object. 
        /// </summary>
        /// <remarks>
        /// Objects are equivalent when both object graphs have equally named properties with the same value, 
        /// irrespective of the type of those objects. Two properties are also equal if one type can be converted to another and the result is equal.
        /// The type of a collection property is ignored as long as the collection implements <see cref="IEnumerable"/> and all
        /// items in the collection are structurally equal. 
        /// Notice that actual behavior is determined by the global defaults managed by <see cref="AssertionOptions"/>.
        /// </remarks>
        /// <param name="because">
        /// An optional formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the 
        /// assertion is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public static void ShouldBeEquivalentTo<T>(this T subject, object expectation, string because = "",
            params object[] reasonArgs)
        {
            ShouldBeEquivalentTo(subject, expectation, config => config, because, reasonArgs);
        }

        /// <summary>
        /// Asserts that an object is equivalent to another object. 
        /// </summary>
        /// <remarks>
        /// Objects are equivalent when both object graphs have equally named properties with the same value, 
        /// irrespective of the type of those objects. Two properties are also equal if one type can be converted to another and the result is equal.
        /// The type of a collection property is ignored as long as the collection implements <see cref="IEnumerable"/> and all
        /// items in the collection are structurally equal. 
        /// </remarks>
        /// <param name="config">
        /// A reference to the <see cref="EquivalencyAssertionOptions{TSubject}"/> configuration object that can be used 
        /// to influence the way the object graphs are compared. You can also provide an alternative instance of the 
        /// <see cref="EquivalencyAssertionOptions{TSubject}"/> class. The global defaults are determined by the 
        /// <see cref="AssertionOptions"/> class.
        /// </param>
        /// <param name="because">
        /// An optional formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the 
        /// assertion is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public static void ShouldBeEquivalentTo<T>(this T subject, object expectation,
            Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> config, string because = "",
            params object[] reasonArgs)
        {
            var context = new EquivalencyValidationContext
            {
                Subject = subject,
                Expectation = expectation,
                CompileTimeType = typeof(T),
                Reason = because,
                ReasonArgs = reasonArgs
            };

#pragma warning disable 618
            new EquivalencyValidator(config(EquivalencyAssertionOptions<T>.Default())).AssertEquality(context);
#pragma warning restore 618
        }

        /// <summary>
        /// Asserts that a collection of objects is equivalent to another collection of objects. 
        /// </summary>
        /// <remarks>
        /// Objects within the collections are equivalent when both object graphs have equally named properties with the same 
        /// value, irrespective of the type of those objects. Two properties are also equal if one type can be converted to another 
        /// and the result is equal. 
        /// The type of a collection property is ignored as long as the collection implements <see cref="IEnumerable"/> and all
        /// items in the collection are structurally equal. 
        /// Notice that actual behavior is determined by the global defaults managed by <see cref="AssertionOptions"/>.
        /// </remarks>
        /// <param name="because">
        /// An optional formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the 
        /// assertion is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public static void ShouldAllBeEquivalentTo<T>(this IEnumerable<T> subject, IEnumerable expectation,
            string because = "", params object[] reasonArgs)
        {
            subject.ShouldBeEquivalentTo(expectation, because, reasonArgs);
        }

        /// <summary>
        /// Asserts that a collection of objects is equivalent to another collection of objects. 
        /// </summary>
        /// <remarks>
        /// Objects within the collections are equivalent when both object graphs have equally named properties with the same 
        /// value,  irrespective of the type of those objects. Two properties are also equal if one type can be converted to another 
        /// and the result is equal. 
        /// The type of a collection property is ignored as long as the collection implements <see cref="IEnumerable"/> and all
        /// items in the collection are structurally equal. 
        /// </remarks>
        /// <param name="config">
        /// A reference to the <see cref="EquivalencyAssertionOptions{TSubject}"/> configuration object that can be used 
        /// to influence the way the object graphs are compared. You can also provide an alternative instance of the 
        /// <see cref="EquivalencyAssertionOptions{TSubject}"/> class. The global defaults are determined by the 
        /// <see cref="AssertionOptions"/> class.
        /// </param>
        /// <param name="because">
        /// An optional formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the 
        /// assertion is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public static void ShouldAllBeEquivalentTo<T>(this IEnumerable<T> subject, IEnumerable expectation,
            Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> config, string because = "",
            params object[] reasonArgs)
        {
            subject.ShouldBeEquivalentTo<IEnumerable<T>>(
                expectation,
                c => ShouldAllBeEquivalentToHelper.ForCollectionMemberType(c, config),
                because,
                reasonArgs);
        }

#if !SILVERLIGHT && !WINRT && !PORTABLE
        /// <summary>
        ///   Starts monitoring an object for its events.
        /// </summary>
        /// <exception cref = "ArgumentNullException">Thrown if eventSource is Null.</exception>
        public static void MonitorEvents(this object eventSource)
        {
            EventMonitor.AddRecordersFor(eventSource, BuildRecorders);
        }

        private static EventRecorder[] BuildRecorders(object eventSource)
        {
            var recorders =
                eventSource.GetType()
                    .GetEvents()
                    .Select(@event => CreateEventHandler(eventSource, @event)).ToArray();

            if (!recorders.Any())
            {
                throw new InvalidOperationException(
                    string.Format("Type {0} does not expose any events.", eventSource.GetType().Name));
            }

            return recorders;
        }

        private static EventRecorder CreateEventHandler(object eventSource, EventInfo eventInfo)
        {
            var eventRecorder = new EventRecorder(eventSource, eventInfo.Name);

            Delegate handler = EventHandlerFactory.GenerateHandler(eventInfo.EventHandlerType, eventRecorder);
            eventInfo.AddEventHandler(eventSource, handler);

            return eventRecorder;
        }
#else
    /// <summary>
    ///   Starts monitoring an object for its <see cref="INotifyPropertyChanged.PropertyChanged"/> events.
    /// </summary>
    /// <exception cref = "ArgumentNullException">Thrown if eventSource is Null.</exception>
        public static void MonitorEvents(this INotifyPropertyChanged eventSource)
        {
            EventMonitor.AddRecordersFor(eventSource, source => BuildRecorders((INotifyPropertyChanged)source));
        }

        private static EventRecorder[] BuildRecorders(INotifyPropertyChanged eventSource)
        {
            var eventRecorder = new EventRecorder(eventSource, "PropertyChanged");

            eventSource.PropertyChanged += (sender, args) => eventRecorder.RecordEvent(sender, args);
            return new[] { eventRecorder };
        }

#endif

        /// <summary>
        /// Safely casts the specified object to the type specified through <typeparamref name="TTo"/>.
        /// </summary>
        /// <remarks>
        /// Has been introduced to allow casting objects without breaking the fluent API.
        /// </remarks>
        /// <typeparam name="TTo"></typeparam>
        public static TTo As<TTo>(this object subject)
        {
            return subject is TTo ? (TTo)subject : default(TTo);
        }
    }
}
