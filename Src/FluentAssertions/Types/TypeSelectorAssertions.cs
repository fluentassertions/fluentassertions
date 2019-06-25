using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Types
{
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
        public TypeSelectorAssertions(params Type[] types)
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
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TypeSelectorAssertions> BeDecoratedWith<TAttribute>(string because = "", params object[] becauseArgs)
            where TAttribute : Attribute
        {
            Type[] typesWithoutAttribute = Subject
                .Where(type => !type.IsDecoratedWith<TAttribute>())
                .ToArray();

            Execute.Assertion
                .ForCondition(!typesWithoutAttribute.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Type_ExpectedAllTypesToBeDecoratedWithXButTheAttributeWasNotFoundOnTypesYZFormat,
                    typeof(TAttribute),
                    Environment.NewLine,
                    GetDescriptionsFor(typesWithoutAttribute));

            return new AndConstraint<TypeSelectorAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="Type"/> is decorated with an attribute of type <typeparamref name="TAttribute"/>
        /// that matches the specified <paramref name="isMatchingAttributePredicate"/>.
        /// </summary>
        /// <param name="isMatchingAttributePredicate">
        /// The predicate that the attribute must match.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TypeSelectorAssertions> BeDecoratedWith<TAttribute>(
            Expression<Func<TAttribute, bool>> isMatchingAttributePredicate, string because = "", params object[] becauseArgs)
            where TAttribute : Attribute
        {
            Type[] typesWithoutMatchingAttribute = Subject
                .Where(type => !type.HasMatchingAttribute(isMatchingAttributePredicate))
                .ToArray();

            Execute.Assertion
                .ForCondition(!typesWithoutMatchingAttribute.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    Resources.Type_ExpectedAllTypesToBeDecoratedWithXThatMatchesYButNoMatchingAttributeWasFoundOnTypesZWFormat,
                    typeof(TAttribute),
                    isMatchingAttributePredicate.Body,
                    Environment.NewLine,
                    GetDescriptionsFor(typesWithoutMatchingAttribute));

            return new AndConstraint<TypeSelectorAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="Type"/> is decorated with, or inherits from a parent class, the specified <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TypeSelectorAssertions> BeDecoratedWithOrInherit<TAttribute>(string because = "", params object[] becauseArgs)
            where TAttribute : Attribute
        {
            Type[] typesWithoutAttribute = Subject
                .Where(type => !type.IsDecoratedWith<TAttribute>(true))
                .ToArray();

            Execute.Assertion
                .ForCondition(!typesWithoutAttribute.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    Resources.Type_ExpectedAllTypesToBeDecoratedWithOrInheritXButTheAttributeWasNotFoundOnTypesYZFormat,
                    typeof(TAttribute),
                    Environment.NewLine,
                    GetDescriptionsFor(typesWithoutAttribute));

            return new AndConstraint<TypeSelectorAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="Type"/> is decorated with, or inherits from a parent class, an attribute of type <typeparamref name="TAttribute"/>
        /// that matches the specified <paramref name="isMatchingAttributePredicate"/>.
        /// </summary>
        /// <param name="isMatchingAttributePredicate">
        /// The predicate that the attribute must match.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TypeSelectorAssertions> BeDecoratedWithOrInherit<TAttribute>(
            Expression<Func<TAttribute, bool>> isMatchingAttributePredicate, string because = "", params object[] becauseArgs)
            where TAttribute : Attribute
        {
            Type[] typesWithoutMatchingAttribute = Subject
                .Where(type => !type.HasMatchingAttribute(isMatchingAttributePredicate, true))
                .ToArray();

            Execute.Assertion
                .ForCondition(!typesWithoutMatchingAttribute.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    Resources.Type_ExpectedAllTypesToBeDecoratedWithOrInheritXThatMatchesZButTheAttributeWasNotFoundOnTypesZWFormat,
                    typeof(TAttribute),
                    isMatchingAttributePredicate.Body,
                    Environment.NewLine,
                    GetDescriptionsFor(typesWithoutMatchingAttribute));

            return new AndConstraint<TypeSelectorAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="Type"/> is not decorated with the specified <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TypeSelectorAssertions> NotBeDecoratedWith<TAttribute>(string because = "", params object[] becauseArgs)
            where TAttribute : Attribute
        {
            Type[] typesWithAttribute = Subject
                .Where(type => type.IsDecoratedWith<TAttribute>())
                .ToArray();

            Execute.Assertion
                .ForCondition(!typesWithAttribute.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    Resources.Type_ExpectedAllTypesToNotBeDecoratedWithXButTheAttributeWasFoundOnTypesYZFormat,
                    typeof(TAttribute),
                    Environment.NewLine,
                    GetDescriptionsFor(typesWithAttribute));

            return new AndConstraint<TypeSelectorAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="Type"/> is not decorated with an attribute of type <typeparamref name="TAttribute"/>
        /// that matches the specified <paramref name="isMatchingAttributePredicate"/>.
        /// </summary>
        /// <param name="isMatchingAttributePredicate">
        /// The predicate that the attribute must match.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TypeSelectorAssertions> NotBeDecoratedWith<TAttribute>(
            Expression<Func<TAttribute, bool>> isMatchingAttributePredicate, string because = "", params object[] becauseArgs)
            where TAttribute : Attribute
        {
            Type[] typesWithMatchingAttribute = Subject
                .Where(type => type.HasMatchingAttribute(isMatchingAttributePredicate))
                .ToArray();

            Execute.Assertion
                .ForCondition(!typesWithMatchingAttribute.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    Resources.Type_ExpectedAllTypesToNotBeDecoratedWithXThatMatchesYButAMatchingAttributeWasFoundOnTypesZWFormat,
                    typeof(TAttribute),
                    isMatchingAttributePredicate.Body,
                    Environment.NewLine,
                    GetDescriptionsFor(typesWithMatchingAttribute));

            return new AndConstraint<TypeSelectorAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="Type"/> is not decorated with and does not inherit from a parent class, the specified <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TypeSelectorAssertions> NotBeDecoratedWithOrInherit<TAttribute>(string because = "", params object[] becauseArgs)
            where TAttribute : Attribute
        {
            Type[] typesWithAttribute = Subject
                .Where(type => type.IsDecoratedWith<TAttribute>(true))
                .ToArray();

            Execute.Assertion
                .ForCondition(!typesWithAttribute.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    Resources.Type_ExpectedAllTypesToNotBeDecoratedWithOrInheritXButAttributeWasFoundOnTypesYZFormat,
                    typeof(TAttribute),
                    Environment.NewLine,
                    GetDescriptionsFor(typesWithAttribute));

            return new AndConstraint<TypeSelectorAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="Type"/> is not decorated with and does not inherit from a parent class,  an attribute of type <typeparamref name="TAttribute"/>
        /// that matches the specified <paramref name="isMatchingAttributePredicate"/>.
        /// </summary>
        /// <param name="isMatchingAttributePredicate">
        /// The predicate that the attribute must match.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TypeSelectorAssertions> NotBeDecoratedWithOrInherit<TAttribute>(
            Expression<Func<TAttribute, bool>> isMatchingAttributePredicate, string because = "", params object[] becauseArgs)
            where TAttribute : Attribute
        {
            Type[] typesWithMatchingAttribute = Subject
                .Where(type => type.HasMatchingAttribute(isMatchingAttributePredicate, true))
                .ToArray();

            Execute.Assertion
                .ForCondition(!typesWithMatchingAttribute.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    Resources.Type_ExpectedAllTypesToNotBeDecoratedWithOrInheritXThatMatchesYButAttributeWasFoundOnTypesZWFormat,
                    typeof(TAttribute),
                    isMatchingAttributePredicate.Body,
                    Environment.NewLine,
                    GetDescriptionsFor(typesWithMatchingAttribute));

            return new AndConstraint<TypeSelectorAssertions>(this);
        }

        private static string GetDescriptionsFor(IEnumerable<Type> types)
        {
            string[] descriptions = types.Select(GetDescriptionFor).ToArray();
            return string.Join(Environment.NewLine, descriptions);
        }

        private static string GetDescriptionFor(Type type)
        {
            return type.ToString();
        }
    }
}
