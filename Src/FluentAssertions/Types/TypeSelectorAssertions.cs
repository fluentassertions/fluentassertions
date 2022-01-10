using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Types
{
#pragma warning disable CS0659 // Ignore not overriding Object.GetHashCode()
#pragma warning disable CA1065 // Ignore throwing NotSupportedException from Equals
    /// <summary>
    /// Contains a number of methods to assert that all <see cref="Type"/>s in a <see cref="TypeSelector"/>
    /// meet certain expectations.
    /// </summary>
    [DebuggerNonUserCode]
    public class TypeSelectorAssertions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeSelectorAssertions"/> class.
        /// </summary>
        public TypeSelectorAssertions(params Type[] types)
        {
            Guard.ThrowIfArgumentIsNull(types, nameof(types));
            Guard.ThrowIfArgumentContainsNull(types, nameof(types));

            Subject = types;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public IEnumerable<Type> Subject { get; }

        /// <summary>
        /// Asserts that the current <see cref="Type"/> is decorated with the specified <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
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
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
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
                    isMatchingAttributePredicate,
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
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
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
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
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
                    isMatchingAttributePredicate,
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
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
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
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
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
                    isMatchingAttributePredicate,
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
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
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
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
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
                    isMatchingAttributePredicate,
                    Environment.NewLine,
                    GetDescriptionsFor(typesWithMatchingAttribute));

            return new AndConstraint<TypeSelectorAssertions>(this);
        }

        /// <summary>
        /// Asserts that the selected types are sealed
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TypeSelectorAssertions> BeSealed(string because = "", params object[] becauseArgs)
        {
            var notSealedTypes = Subject.Where(type => !type.IsCSharpSealed()).ToArray();

            Execute.Assertion.ForCondition(!notSealedTypes.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected all types to be sealed{reason}, but the following types are not:{0}{1}.", Environment.NewLine, GetDescriptionsFor(notSealedTypes));

            return new AndConstraint<TypeSelectorAssertions>(this);
        }

        /// <summary>
        /// Asserts that the all <see cref="Type"/> are not sealed classes
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TypeSelectorAssertions> NotBeSealed(string because = "", params object[] becauseArgs)
        {
            var sealedTypes = Subject.Where(type => type.IsCSharpSealed()).ToArray();

            Execute.Assertion.ForCondition(!sealedTypes.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected all types not to be sealed{reason}, but the following types are:{0}{1}.", Environment.NewLine, GetDescriptionsFor(sealedTypes));

            return new AndConstraint<TypeSelectorAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="Type"/> is in the specified <paramref name="namespace"/>.
        /// </summary>
        /// <param name="namespace">
        /// The namespace that the type must be in.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TypeSelectorAssertions> BeInNamespace(string @namespace, string because = "", params object[] becauseArgs)
        {
            Type[] typesNotInNamespace = Subject
                .Where(t => t.Namespace != @namespace)
                .ToArray();

            Execute.Assertion
                .ForCondition(!typesNotInNamespace.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected all types to be in namespace {0}{reason}," +
                          " but the following types are in a different namespace:{1}{2}.",
                    @namespace,
                    Environment.NewLine,
                    GetDescriptionsFor(typesNotInNamespace));

            return new AndConstraint<TypeSelectorAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="Type"/> is not in the specified <paramref name="namespace"/>.
        /// </summary>
        /// <param name="namespace">
        /// The namespace that the type must not be in.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TypeSelectorAssertions> NotBeInNamespace(string @namespace, string because = "", params object[] becauseArgs)
        {
            Type[] typesInNamespace = Subject
                .Where(t => t.Namespace == @namespace)
                .ToArray();

            Execute.Assertion
                .ForCondition(!typesInNamespace.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected no types to be in namespace {0}{reason}," +
                          " but the following types are in the namespace:{1}{2}.",
                    @namespace,
                    Environment.NewLine,
                    GetDescriptionsFor(typesInNamespace));

            return new AndConstraint<TypeSelectorAssertions>(this);
        }

        /// <summary>
        /// Asserts that the namespace of the current <see cref="Type"/> starts with the specified <paramref name="namespace"/>.
        /// </summary>
        /// <param name="namespace">
        /// The namespace that the namespace of the type must start with.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TypeSelectorAssertions> BeUnderNamespace(string @namespace, string because = "", params object[] becauseArgs)
        {
            Type[] typesNotUnderNamespace = Subject
                .Where(t => !t.IsUnderNamespace(@namespace))
                .ToArray();

            Execute.Assertion
                .ForCondition(!typesNotUnderNamespace.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected the namespaces of all types to start with {0}{reason}," +
                          " but the namespaces of the following types do not start with it:{1}{2}.",
                    @namespace,
                    Environment.NewLine,
                    GetDescriptionsFor(typesNotUnderNamespace));

            return new AndConstraint<TypeSelectorAssertions>(this);
        }

        /// <summary>
        /// Asserts that the namespace of the current <see cref="Type"/>
        /// does not starts with the specified <paramref name="namespace"/>.
        /// </summary>
        /// <param name="namespace">
        /// The namespace that the namespace of the type must not start with.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TypeSelectorAssertions> NotBeUnderNamespace(string @namespace, string because = "", params object[] becauseArgs)
        {
            Type[] typesUnderNamespace = Subject
                .Where(t => t.IsUnderNamespace(@namespace))
                .ToArray();

            Execute.Assertion
                .ForCondition(!typesUnderNamespace.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected the namespaces of all types to not start with {0}{reason}," +
                          " but the namespaces of the following types start with it:{1}{2}.",
                    @namespace,
                    Environment.NewLine,
                    GetDescriptionsFor(typesUnderNamespace));

            return new AndConstraint<TypeSelectorAssertions>(this);
        }

        private static string GetDescriptionsFor(IEnumerable<Type> types)
        {
            IEnumerable<string> descriptions = types.Select(type => GetDescriptionFor(type));
            return string.Join(Environment.NewLine, descriptions);
        }

        private static string GetDescriptionFor(Type type)
        {
            return type.ToString();
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            throw new NotSupportedException("Calling Equals on Assertion classes is not supported.");
    }
}
