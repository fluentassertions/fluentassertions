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
        /// Initializes a new instance of the <see cref="System.Object" /> class.
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
                .FailWith("Expected all types to be decorated with {0}{reason}," +
                    " but the attribute was not found on the following types:{1}{2}.",
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
            Guard.ThrowIfArgumentIsNull(isMatchingAttributePredicate, nameof(isMatchingAttributePredicate));

            Type[] typesWithoutMatchingAttribute = Subject
                .Where(type => !type.IsDecoratedWith(isMatchingAttributePredicate))
                .ToArray();

            Execute.Assertion
                .ForCondition(!typesWithoutMatchingAttribute.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected all types to be decorated with {0} that matches {1}{reason}," +
                    " but no matching attribute was found on the following types:{2}{3}.",
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
                .Where(type => !type.IsDecoratedWithOrInherit<TAttribute>())
                .ToArray();

            Execute.Assertion
                .ForCondition(!typesWithoutAttribute.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected all types to be decorated with or inherit {0}{reason}," +
                    " but the attribute was not found on the following types:{1}{2}.",
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
            Guard.ThrowIfArgumentIsNull(isMatchingAttributePredicate, nameof(isMatchingAttributePredicate));

            Type[] typesWithoutMatchingAttribute = Subject
                .Where(type => !type.IsDecoratedWithOrInherit(isMatchingAttributePredicate))
                .ToArray();

            Execute.Assertion
                .ForCondition(!typesWithoutMatchingAttribute.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected all types to be decorated with or inherit {0} that matches {1}{reason}," +
                    " but no matching attribute was found on the following types:{2}{3}.",
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
                .FailWith("Expected all types to not be decorated with {0}{reason}," +
                    " but the attribute was found on the following types:{1}{2}.",
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
            Guard.ThrowIfArgumentIsNull(isMatchingAttributePredicate, nameof(isMatchingAttributePredicate));

            Type[] typesWithMatchingAttribute = Subject
                .Where(type => type.IsDecoratedWith(isMatchingAttributePredicate))
                .ToArray();

            Execute.Assertion
                .ForCondition(!typesWithMatchingAttribute.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected all types to not be decorated with {0} that matches {1}{reason}," +
                    " but a matching attribute was found on the following types:{2}{3}.",
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
                .Where(type => type.IsDecoratedWithOrInherit<TAttribute>())
                .ToArray();

            Execute.Assertion
                .ForCondition(!typesWithAttribute.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected all types to not be decorated with or inherit {0}{reason}," +
                    " but the attribute was found on the following types:{1}{2}.",
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
            Guard.ThrowIfArgumentIsNull(isMatchingAttributePredicate, nameof(isMatchingAttributePredicate));

            Type[] typesWithMatchingAttribute = Subject
                .Where(type => type.IsDecoratedWithOrInherit(isMatchingAttributePredicate))
                .ToArray();

            Execute.Assertion
                .ForCondition(!typesWithMatchingAttribute.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected all types to not be decorated with or inherit {0} that matches {1}{reason}," +
                    " but a matching attribute was found on the following types:{2}{3}.",
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
