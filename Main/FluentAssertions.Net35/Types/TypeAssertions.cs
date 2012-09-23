using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions.Execution;

#if WINRT
using System.Reflection;
#endif

namespace FluentAssertions.Types
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="Type"/> meets certain expectations.
    /// </summary>
    [DebuggerNonUserCode]
    public class TypeAssertions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        protected internal TypeAssertions(Type type)
        {
            Subject = type;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public Type Subject { get; private set; }

        /// <summary>
        /// Asserts that the current type is equal to the specified <typeparamref name="TExpected"/> type.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TypeAssertions> Be<TExpected>(string reason = "", params object[] reasonArgs)
        {
            return Be(typeof(TExpected), reason, reasonArgs);
        }

        /// <summary>
        /// Asserts that the current type is equal to the specified <paramref name="expected"/> type.
        /// </summary>
        /// <param name="expected">The expected type</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TypeAssertions> Be(Type expected, string reason = "", params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject == expected)
                .BecauseOf(reason, reasonArgs)
                .FailWith(GetFailureMessageIfTypesAreDifferent(Subject, expected));

            return new AndConstraint<TypeAssertions>(this);
        }

        /// <summary>
        /// Creates an error message in case the specifed <paramref name="actual"/> type differs from the 
        /// <paramref name="expected"/> type.
        /// </summary>
        /// <returns>
        /// An empty <see cref="string"/> if the two specified types are the same, or an error message that describes that
        /// the two specified types are not the same.
        /// </returns>
        private static string GetFailureMessageIfTypesAreDifferent(Type actual, Type expected)
        {
            if (actual == expected)
            {
                return "";
            }

            string expectedType = expected.FullName;
            string actualType = actual.FullName;

            if (expectedType == actualType)
            {
                expectedType = "[" + expected.AssemblyQualifiedName + "]";
                actualType = "[" + actual.AssemblyQualifiedName + "]";
            }

            return string.Format("Expected type to be {0}{{reason}}, but found {1}.", expectedType, actualType);
        }

        /// <summary>
        /// Asserts that the current type is not equal to the specified <typeparamref name="TUnexpected"/> type.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TypeAssertions> NotBe<TUnexpected>(string reason = "", params object[] reasonArgs)
        {
            return NotBe(typeof(TUnexpected), reason, reasonArgs);
        }

        /// <summary>
        /// Asserts that the current type is not equal to the specified <paramref name="unexpected"/> type.
        /// </summary>
        /// <param name="unexpected">The unexpected type</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TypeAssertions> NotBe(Type unexpected, string reason = "", params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject != unexpected)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected type not to be [" + unexpected.AssemblyQualifiedName + "]{reason}.");

            return new AndConstraint<TypeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="Type"/> is decorated with the specified <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TypeAssertions> BeDecoratedWith<TAttribute>(string reason = "", params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.IsDecoratedWith<TAttribute>())
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected type {0} to be decorated with {1}{reason}, but the attribute was not found.",
                    Subject, typeof (TAttribute));

            return new AndConstraint<TypeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="Type"/> is decorated with an attribute of type <typeparamref name="TAttribute"/>
        /// that matches the specified <paramref name="isMatchingAttributePredicate"/>.
        /// </summary>
        /// <param name="isMatchingAttributePredicate">
        /// The predicate that the attribute must match.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TypeAssertions> BeDecoratedWith<TAttribute>(
            Expression<Func<TAttribute, bool>> isMatchingAttributePredicate, string reason = "", params object[] reasonArgs)
        {
            BeDecoratedWith<TAttribute>(reason, reasonArgs);

            Execute.Verification
                .ForCondition(Subject.HasMatchingAttribute(isMatchingAttributePredicate))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected type {0} to be decorated with {1} that matches {2}{reason}, but no matching attribute was found.",
                    Subject, typeof(TAttribute), isMatchingAttributePredicate.Body);

            return new AndConstraint<TypeAssertions>(this);
        }
    }

    /// <summary>
    /// Contains a number of methods to assert that all <see cref="Type"/>s in a <see cref="TypeSelector"/>
    /// meet certain expectations.
    /// </summary>
    [DebuggerNonUserCode]
    public class TypeSelectorAssertions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        protected internal TypeSelectorAssertions(IEnumerable<Type> types)
        {
            Subject = types;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public IEnumerable<Type> Subject { get; private set; }

        /// <summary>
        /// Asserts that the current <see cref="Type"/> is decorated with the specified <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TypeSelectorAssertions> BeDecoratedWith<TAttribute>(string reason = "", params object[] reasonArgs)
        {
            IEnumerable<Type> typesWithoutAttribute = Subject
                .Where(type => !type.IsDecoratedWith<TAttribute>())
                .ToArray();

            Execute.Verification
                .ForCondition(!typesWithoutAttribute.Any())
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected all types to be decorated with {0}{reason}," +
                    " but the attribute was not found on the following types:\r\n" + GetDescriptionsFor(typesWithoutAttribute),
                    typeof(TAttribute));

            return new AndConstraint<TypeSelectorAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="Type"/> is decorated with an attribute of type <typeparamref name="TAttribute"/>
        /// that matches the specified <paramref name="isMatchingAttributePredicate"/>.
        /// </summary>
        /// <param name="isMatchingAttributePredicate">
        /// The predicate that the attribute must match.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TypeSelectorAssertions> BeDecoratedWith<TAttribute>(
            Expression<Func<TAttribute, bool>> isMatchingAttributePredicate, string reason = "", params object[] reasonArgs)
        {
            IEnumerable<Type> typesWithoutMatchingAttribute = Subject
                .Where(type => !type.HasMatchingAttribute(isMatchingAttributePredicate))
                .ToArray();

            Execute.Verification
                .ForCondition(!typesWithoutMatchingAttribute.Any())
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected all types to be decorated with {0} that matches {1}{reason}," +
                    " but no matching attribute was found on the following types:\r\n" + GetDescriptionsFor(typesWithoutMatchingAttribute),
                    typeof(TAttribute), isMatchingAttributePredicate.Body);

            return new AndConstraint<TypeSelectorAssertions>(this);
        }

        private static string GetDescriptionsFor(IEnumerable<Type> types)
        {
            return string.Join(Environment.NewLine, types.Select(GetDescriptionFor).ToArray());
        }

        private static string GetDescriptionFor(Type type)
        {
#if !WINRT
            return type.ToString();
#else
            return type.GetTypeInfo().ToString();
#endif
        }
    }

    internal static class TypeExtensions
    {
        public static bool HasMatchingAttribute<TAttribute>(this Type type, Expression<Func<TAttribute, bool>> isMatchingAttributePredicate)
        {
            Func<TAttribute, bool> isMatchingAttribute = isMatchingAttributePredicate.Compile();

            return GetCustomAttributes<TAttribute>(type).Any(isMatchingAttribute);
        }

        public static bool IsDecoratedWith<TAttribute>(this Type type)
        {
            return GetCustomAttributes<TAttribute>(type).Any();
        }

        private static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>(Type type)
        {
#if !WINRT
            return type.GetCustomAttributes(false).OfType<TAttribute>();
#else
            return type.GetTypeInfo().GetCustomAttributes(false).OfType<TAttribute>();
#endif
        }
    }
}