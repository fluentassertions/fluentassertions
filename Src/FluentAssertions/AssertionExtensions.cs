using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using FluentAssertions.Collections;
using FluentAssertions.Equivalency;
using FluentAssertions.Events;
using FluentAssertions.Numeric;
using FluentAssertions.Primitives;
using FluentAssertions.Reflection;
using FluentAssertions.Types;
using FluentAssertions.Xml;
using JetBrains.Annotations;

using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions.Specialized;

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
        [MustUseReturnValue /* do not use Pure because this method executes the action before returning to the caller */]
        public static ExecutionTimeAssertions ExecutionTime(this Action action)
        {
            return new ExecutionTimeAssertions(action);
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
        /// Returns a <see cref="MethodBaseAssertions"/> object that can be used to assert the current <see cref="MethodInfo"/>.
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
        /// Asserts that all elements in a collection of objects are equivalent to a given object. 
        /// </summary>
        /// <remarks>
        /// Objects within the collection are equivalent to given object when both object graphs have equally named properties with the same 
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
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public static void ShouldAllBeEquivalentTo<T>(this IEnumerable<T> subject, object expectation,
            string because = "", params object[] becauseArgs)
        {
            subject.ShouldAllBeEquivalentTo(expectation, options => options, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that all elements in a collection of objects are equivalent to a given object. 
        /// </summary>
        /// <remarks>
        /// Objects within the collection are equivalent to given object when both object graphs have equally named properties with the same 
        /// value, irrespective of the type of those objects. Two properties are also equal if one type can be converted to another 
        /// and the result is equal. 
        /// The type of a collection property is ignored as long as the collection implements <see cref="IEnumerable"/> and all
        /// items in the collection are structurally equal. 
        /// Notice that actual behavior is determined by the global defaults managed by <see cref="AssertionOptions"/>.
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
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public static void ShouldAllBeEquivalentTo<T>(this IEnumerable<T> subject, object expectation,
            Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> config, string because = "",
            params object[] becauseArgs)
        {
            IEnumerable repeatedExpectation = RepeatAsManyAs(expectation, subject);

            subject.ShouldAllBeEquivalentTo(repeatedExpectation, config, because, becauseArgs);
        }

        private static IEnumerable RepeatAsManyAs<T>(object value, IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return Enumerable.Empty<object>();
            }

            return RepeatAsManyAsIterator(value, enumerable);
        }

        private static IEnumerable RepeatAsManyAsIterator<T>(object value, IEnumerable<T> enumerable)
        {
            using (IEnumerator<T> enumerator = enumerable.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return value;
                }
            }
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
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public static void ShouldAllBeEquivalentTo<T>(this IEnumerable<T> subject, IEnumerable expectation,
            string because = "", params object[] becauseArgs)
        {
            subject.ShouldAllBeEquivalentTo(expectation, options => options, because, becauseArgs);
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
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public static void ShouldAllBeEquivalentTo<T>(this IEnumerable<T> subject, IEnumerable expectation,
            Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> config, string because = "",
            params object[] becauseArgs)
        {
            EquivalencyAssertionOptions<IEnumerable<T>> options = config(AssertionOptions.CloneDefaults<T>()).AsCollection();

            var context = new EquivalencyValidationContext
            {
                Subject = subject,
                Expectation = expectation,
                RootIsCollection = true,
                CompileTimeType = typeof (IEnumerable<T>),
                Because = because,
                BecauseArgs = becauseArgs,
                Tracer = options.TraceWriter
            };

            new EquivalencyValidator(options).AssertEquality(context);
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
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public static void ShouldBeEquivalentTo<T>(this T subject, object expectation, string because = "",
            params object[] becauseArgs)
        {
            ShouldBeEquivalentTo(subject, expectation, config => config, because, becauseArgs);
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
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public static void ShouldBeEquivalentTo<T>(this T subject, object expectation,
            Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> config, string because = "",
            params object[] becauseArgs)
        {
            IEquivalencyAssertionOptions options = config(AssertionOptions.CloneDefaults<T>());

            var context = new EquivalencyValidationContext
            {
                Subject = subject,
                Expectation = expectation,
                CompileTimeType = typeof (T),
                Because = because,
                BecauseArgs = becauseArgs,
                Tracer = options.TraceWriter
            };

            new EquivalencyValidator(options).AssertEquality(context);
        }

#if NET45
        /// <summary>
        ///   Starts monitoring <paramref name="eventSource"/> for its events.
        /// </summary>
        /// <param name="eventSource">The object for which to monitor the events.</param>
        /// <exception cref = "ArgumentNullException">Thrown if <paramref name="eventSource"/> is Null.</exception>
        public static IEventMonitor MonitorEvents(this object eventSource)
        {
            if (eventSource == null)
            {
                throw new NullReferenceException("Cannot monitor the events of a <null> object.");
            }

            return EventMonitor.Attach(eventSource, eventSource.GetType());
        }

        /// <summary>
        ///   Starts monitoring <paramref name="eventSource"/> for events defined in the type parameter <typeparamref name="T"/>.
        /// </summary>
        /// <param name="eventSource">The object for which to monitor the events.</param>
        /// <typeparam name="T">The type defining the events it should monitor.</typeparam>
        /// <exception cref = "ArgumentNullException">Thrown if <paramref name="eventSource"/> is Null.</exception>
        public static IEventMonitor MonitorEvents<T>(this object eventSource)
        {
            if (eventSource == null)
            {
                throw new NullReferenceException("Cannot monitor the events of a <null> object.");
            }

            return EventMonitor.Attach( eventSource, typeof(T));
        }
#endif

        /// <summary>
        ///   Starts monitoring an object for its <see cref="INotifyPropertyChanged.PropertyChanged"/> events.
        /// </summary>
        /// <exception cref = "ArgumentNullException">Thrown if eventSource is Null.</exception>
        public static IEventMonitor MonitorEvents(this INotifyPropertyChanged eventSource)
        {
            if (eventSource == null)
            {
                throw new NullReferenceException("Cannot monitor the events of a <null> object.");
            }

            return EventMonitor.Attach( eventSource, typeof(INotifyPropertyChanged) );
        }

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
            return subject is TTo ? (TTo) subject : default(TTo);
        }
    }
}