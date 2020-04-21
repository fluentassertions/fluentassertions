using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Common;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;

namespace FluentAssertions.Collections
{
    public class StringCollectionAssertions :
        StringCollectionAssertions<IEnumerable<string>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringCollectionAssertions"/> class.
        /// </summary>
        public StringCollectionAssertions(IEnumerable<string> actualValue)
            : base(actualValue)
        {
        }
    }

    public class StringCollectionAssertions<TCollection> :
        StringCollectionAssertions<TCollection, StringCollectionAssertions<TCollection>>
        where TCollection : IEnumerable<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringCollectionAssertions{TCollection}"/> class.
        /// </summary>
        public StringCollectionAssertions(TCollection actualValue)
            : base(actualValue)
        {
        }
    }

    public class StringCollectionAssertions<TCollection, TAssertions> :
        SelfReferencingCollectionAssertions<TCollection, string, TAssertions>
        where TCollection : IEnumerable<string>
        where TAssertions : StringCollectionAssertions<TCollection, TAssertions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringCollectionAssertions{TCollection, TAssertions}"/> class.
        /// </summary>
        public StringCollectionAssertions(TCollection actualValue)
            : base(actualValue)
        {
        }

        /// <summary>
        /// Expects the current collection to contain all the same elements in the same order as the collection identified by
        /// <paramref name="expected" />. Elements are compared using their <see cref="object.Equals(object)" />.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable{T}"/> with the expected elements.</param>
        public new AndConstraint<TAssertions> Equal(params string[] expected)
        {
            return base.Equal(expected.AsEnumerable());
        }

        /// <summary>
        /// Expects the current collection to contain all the same elements in the same order as the collection identified by
        /// <paramref name="expected" />. Elements are compared using their <see cref="object.Equals(object)" />.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable{T}"/> with the expected elements.</param>
        public AndConstraint<TAssertions> Equal(IEnumerable<string> expected)
        {
            return base.Equal(expected);
        }

        /// <summary>
        /// Asserts that a collection of string is equivalent to another collection of strings.
        /// </summary>
        /// <remarks>
        /// The two collections are equivalent when they both contain the same strings in any order.
        /// </remarks>
        public AndConstraint<TAssertions> BeEquivalentTo(params string[] expectation)
        {
            BeEquivalentTo(expectation, config => config);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that a collection of objects is equivalent to another collection of objects.
        /// </summary>
        /// <remarks>
        /// The two collections are equivalent when they both contain the same strings in any order.
        /// </remarks>
        /// <param name="expectation">An <see cref="IEnumerable{String}"/> with the expected elements.</param>
        /// <param name="because">
        /// An optional formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the
        /// assertion is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeEquivalentTo(IEnumerable<string> expectation, string because = "", params object[] becauseArgs)
        {
            BeEquivalentTo(expectation, config => config, because, becauseArgs);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that a collection of objects is equivalent to another collection of objects.
        /// </summary>
        /// <remarks>
        /// The two collections are equivalent when they both contain the same strings in any order.
        /// </remarks>
        /// <param name="expectation">An <see cref="IEnumerable{String}"/> with the expected elements.</param>
        /// <param name="config">
        /// A reference to the <see cref="EquivalencyAssertionOptions{String}"/> configuration object that can be used
        /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
        /// <see cref="EquivalencyAssertionOptions{String}"/> class. The global defaults are determined by the
        /// <see cref="AssertionOptions"/> class.
        /// </param>
        /// <param name="because">
        /// An optional formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the
        /// assertion is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeEquivalentTo(IEnumerable<string> expectation,
            Func<EquivalencyAssertionOptions<string>, EquivalencyAssertionOptions<string>> config, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(config, nameof(config));

            EquivalencyAssertionOptions<IEnumerable<string>> options = config(AssertionOptions.CloneDefaults<string>()).AsCollection();

            var context = new EquivalencyValidationContext
            {
                Subject = Subject,
                Expectation = expectation,
                RootIsCollection = true,
                CompileTimeType = typeof(IEnumerable<string>),
                Because = because,
                BecauseArgs = becauseArgs,
                Tracer = options.TraceWriter
            };

            var equivalencyValidator = new EquivalencyValidator(options);
            equivalencyValidator.AssertEquality(context);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Expects the current collection to contain the specified elements in the exact same order. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable{T}"/> with the expected elements.</param>
        public AndConstraint<TAssertions> ContainInOrder(params string[] expected)
        {
            return base.ContainInOrder(expected.AsEnumerable());
        }

        /// <summary>
        /// Expects the current collection to contain the specified elements in the exact same order. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable{T}"/> with the expected elements.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> ContainInOrder(IEnumerable<string> expected, string because = "",
            params object[] becauseArgs)
        {
            return base.ContainInOrder(expected, because, becauseArgs);
        }

        /// <summary>
        /// Expects the current collection to contain the specified elements in any order. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable{T}"/> with the expected elements.</param>
        public AndConstraint<TAssertions> Contain(IEnumerable<string> expected)
        {
            return base.Contain(expected);
        }

        /// <summary>
        /// Expects the current collection to contain the specified elements in any order. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable{T}" /> with the expected elements.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArg">
        /// An object to format using the placeholders in <paramref name="because" />.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> Contain(IEnumerable<string> expected, string because = null,
            object becauseArg = null,
            params object[] becauseArgs)
        {
            var args = new List<object> { becauseArg };
            args.AddRange(becauseArgs);
            return base.Contain(expected, because, args.ToArray());
        }

        /// <summary>
        /// Asserts that the current collection does not contain the supplied items. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="unexpected">An <see cref="IEnumerable{T}"/> with the unexpected elements.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArg">
        /// An object to format using the placeholders in <paramref name="because" />.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotContain(IEnumerable<string> unexpected, string because = null,
            object becauseArg = null,
            params object[] becauseArgs)
        {
            var args = new List<object> { becauseArg };
            args.AddRange(becauseArgs);
            return base.NotContain(unexpected, because, args.ToArray());
        }

        /// <summary>
        /// Asserts that the collection contains at least one string that matches a wildcard pattern.
        /// </summary>
        /// <param name="wildcardPattern">
        /// The wildcard pattern with which the subject is matched, where * and ? have special meanings.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndWhichConstraint<TAssertions, string> ContainMatch(string wildcardPattern, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject is object)
                .FailWith("Expected {context:collection} to contain a match of {0}{reason}, but found <null>.", wildcardPattern)
                .Then
                .ForCondition(ContainsMatch(wildcardPattern))
                .FailWith("Expected {context:collection} {0} to contain a match of {1}{reason}.", Subject, wildcardPattern);

            return new AndWhichConstraint<TAssertions, string>((TAssertions)this, AllThatMatch(wildcardPattern));
        }

        private bool ContainsMatch(string wildcardPattern)
        {
            using var scope = new AssertionScope();
            return Subject.Any(item =>
            {
                item.Should().Match(wildcardPattern);
                return !scope.Discard().Any();
            });
        }

        private IEnumerable<string> AllThatMatch(string wildcardPattern)
        {
            return Subject.Where(item =>
            {
                using var scope = new AssertionScope();
                item.Should().Match(wildcardPattern);
                return !scope.Discard().Any();
            });
        }

        /// <summary>
        /// Asserts that the collection does not contain any string that matches a wildcard pattern.
        /// </summary>
        /// <param name="wildcardPattern">
        /// The wildcard pattern with which the subject is matched, where * and ? have special meanings.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotContainMatch(string wildcardPattern, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject is object)
                .FailWith("Did not expect {context:collection} to contain a match of {0}{reason}, but found <null>.", wildcardPattern)
                .Then
                .ForCondition(NotContainsMatch(wildcardPattern))
                .FailWith("Did not expect {context:collection} {0} to contain a match of {1}{reason}.", Subject, wildcardPattern);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        private bool NotContainsMatch(string wildcardPattern)
        {
            using var scope = new AssertionScope();
            return Subject.All(item =>
            {
                item.Should().NotMatch(wildcardPattern);
                return !scope.Discard().Any();
            });
        }
    }
}
