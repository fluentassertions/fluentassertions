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
        GenericCollectionAssertions<TCollection, string, TAssertions>
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
            return BeEquivalentTo(expectation, config => config);
        }

        /// <summary>
        /// Asserts that a collection of objects is equivalent to another collection of objects.
        /// </summary>
        /// <remarks>
        /// The two collections are equivalent when they both contain the same strings in any order.
        /// </remarks>
        /// <param name="expectation">An <see cref="IEnumerable{String}"/> with the expected elements.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public AndConstraint<TAssertions> BeEquivalentTo(IEnumerable<string> expectation, string because = "", params object[] becauseArgs)
        {
            return BeEquivalentTo(expectation, config => config, because, becauseArgs);
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
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public AndConstraint<TAssertions> BeEquivalentTo(IEnumerable<string> expectation,
            Func<EquivalencyAssertionOptions<string>, EquivalencyAssertionOptions<string>> config, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(config, nameof(config));

            EquivalencyAssertionOptions<IEnumerable<string>> options = config(AssertionOptions.CloneDefaults<string>()).AsCollection();

            var context = new EquivalencyValidationContext(Node.From<IEnumerable<string>>(() => AssertionScope.Current.CallerIdentity), options)
            {
                Reason = new Reason(because, becauseArgs),
                TraceWriter = options.TraceWriter
            };

            var comparands = new Comparands
            {
                Subject = Subject,
                Expectation = expectation,
                CompileTimeType = typeof(IEnumerable<string>),
            };

            new EquivalencyValidator().AssertEquality(comparands, context);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that all strings in a collection of strings are equal to the given string.
        /// </summary>
        /// <param name="expectation">An expected <see cref="string"/>.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public AndConstraint<TAssertions> AllBe(string expectation,
            string because = "", params object[] becauseArgs)
        {
            return AllBe(expectation, options => options, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that all strings in a collection of strings are equal to the given string.
        /// </summary>
        /// <param name="expectation">An expected <see cref="string"/>.</param>
        /// <param name="config">
        /// A reference to the <see cref="EquivalencyAssertionOptions{String}"/> configuration object that can be used
        /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
        /// <see cref="EquivalencyAssertionOptions{String}"/> class. The global defaults are determined by the
        /// <see cref="AssertionOptions"/> class.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public AndConstraint<TAssertions> AllBe(string expectation,
            Func<EquivalencyAssertionOptions<string>, EquivalencyAssertionOptions<string>> config,
            string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(config, nameof(config));

            string[] repeatedExpectation = RepeatAsManyAs(expectation, Subject).ToArray();

            // Because we have just manually created the collection based on single element
            // we are sure that we can force strict ordering, because ordering does not matter in terms
            // of correctness. On the other hand we do not want to change ordering rules for nested objects
            // in case user needs to use them. Strict ordering improves algorithmic complexity
            // from O(n^2) to O(n). For bigger tables it is necessary in order to achieve acceptable
            // execution times.
            Func<EquivalencyAssertionOptions<string>, EquivalencyAssertionOptions<string>> forceStringOrderingConfig =
                x => config(x).WithStrictOrderingFor(s => string.IsNullOrEmpty(s.Path));

            return BeEquivalentTo(repeatedExpectation, forceStringOrderingConfig, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the collection contains at least one string that matches the <paramref name="wildcardPattern"/>.
        /// </summary>
        /// <param name="wildcardPattern">
        /// The pattern to match against the subject. This parameter can contain a combination of literal text and wildcard
        /// (* and ?) characters, but it doesn't support regular expressions.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        /// <remarks>
        /// <paramref name="wildcardPattern"/> can be a combination of literal and wildcard characters,
        /// but it doesn't support regular expressions. The following wildcard specifiers are permitted in
        /// <paramref name="wildcardPattern"/>.
        /// <list type="table">
        /// <listheader>
        /// <term>Wildcard character</term>
        /// <description>Description</description>
        /// </listheader>
        /// <item>
        /// <term>* (asterisk)</term>
        /// <description>Zero or more characters in that position.</description>
        /// </item>
        /// <item>
        /// <term>? (question mark)</term>
        /// <description>Exactly one character in that position.</description>
        /// </item>
        /// </list>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="wildcardPattern"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="wildcardPattern"/> is empty.</exception>
        public AndWhichConstraint<TAssertions, string> ContainMatch(string wildcardPattern, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(wildcardPattern, nameof(wildcardPattern), "Cannot match strings in collection against <null>. Provide a wildcard pattern or use the Contain method.");

            if (wildcardPattern.Length == 0)
            {
                throw new ArgumentException("Cannot match strings in collection against an empty string. Provide a wildcard pattern or use the Contain method.", nameof(wildcardPattern));
            }

            bool success = Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject is not null)
                .FailWith("Expected {context:collection} to contain a match of {0}{reason}, but found <null>.", wildcardPattern);

            IEnumerable<string> matched = new List<string>(0);

            if (success)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(ContainsMatch(wildcardPattern))
                    .FailWith("Expected {context:collection} {0} to contain a match of {1}{reason}.", Subject, wildcardPattern);

                matched = AllThatMatch(wildcardPattern);
            }

            return new AndWhichConstraint<TAssertions, string>((TAssertions)this, matched);
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
        /// Asserts that the collection does not contain any string that matches the <paramref name="wildcardPattern"/>.
        /// </summary>
        /// <param name="wildcardPattern">
        /// The pattern to match against the subject. This parameter can contain a combination of literal text and wildcard
        /// (* and ?) characters, but it doesn't support regular expressions.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        /// <remarks>
        /// <paramref name="wildcardPattern"/> can be a combination of literal and wildcard characters,
        /// but it doesn't support regular expressions. The following wildcard specifiers are permitted in
        /// <paramref name="wildcardPattern"/>.
        /// <list type="table">
        /// <listheader>
        /// <term>Wildcard character</term>
        /// <description>Description</description>
        /// </listheader>
        /// <item>
        /// <term>* (asterisk)</term>
        /// <description>Zero or more characters in that position.</description>
        /// </item>
        /// <item>
        /// <term>? (question mark)</term>
        /// <description>Exactly one character in that position.</description>
        /// </item>
        /// </list>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="wildcardPattern"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="wildcardPattern"/> is empty.</exception>
        public AndConstraint<TAssertions> NotContainMatch(string wildcardPattern, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(wildcardPattern, nameof(wildcardPattern), "Cannot match strings in collection against <null>. Provide a wildcard pattern or use the NotContain method.");

            if (wildcardPattern.Length == 0)
            {
                throw new ArgumentException("Cannot match strings in collection against an empty string. Provide a wildcard pattern or use the NotContain method.", nameof(wildcardPattern));
            }

            bool success = Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject is not null)
                .FailWith("Did not expect {context:collection} to contain a match of {0}{reason}, but found <null>.", wildcardPattern);

            if (success)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(NotContainsMatch(wildcardPattern))
                    .FailWith("Did not expect {context:collection} {0} to contain a match of {1}{reason}.", Subject, wildcardPattern);
            }

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
