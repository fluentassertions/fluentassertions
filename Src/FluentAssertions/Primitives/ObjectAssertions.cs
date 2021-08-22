using System;
using System.Collections.Generic;
using FluentAssertions.Common;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="object"/> is in the expected state.
    /// </summary>
    public class ObjectAssertions : ObjectAssertions<object, ObjectAssertions>
    {
        public ObjectAssertions(object value)
            : base(value)
        {
        }
    }

    /// <summary>
    /// Contains a number of methods to assert that a <typeparamref name="TSubject"/> is in the expected state.
    /// </summary>
    public class ObjectAssertions<TSubject, TAssertions> : ReferenceTypeAssertions<TSubject, TAssertions>
        where TAssertions : ObjectAssertions<TSubject, TAssertions>
    {
        public ObjectAssertions(TSubject value)
            : base(value)
        {
        }

        /// <summary>
        /// Asserts that a <typeparamref name="TSubject"/> equals another <typeparamref name="TSubject"/> using its <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> Be(TSubject expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(ObjectExtensions.GetComparer<TSubject>()(Subject, expected))
                .WithDefaultIdentifier(Identifier)
                .FailWith("Expected {context} to be {0}{reason}, but found {1}.", expected,
                    Subject);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that a <typeparamref name="TSubject"/> does not equal another <typeparamref name="TSubject"/> using its <see cref="object.Equals(object)" /> method.
        /// </summary>
        /// <param name="unexpected">The unexpected value</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public AndConstraint<TAssertions> NotBe(TSubject unexpected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!ObjectExtensions.GetComparer<TSubject>()(Subject, unexpected))
                .BecauseOf(because, becauseArgs)
                .WithDefaultIdentifier(Identifier)
                .FailWith("Did not expect {context} to be equal to {0}{reason}.", unexpected);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that an object is equivalent to another object.
        /// </summary>
        /// <remarks>
        /// Objects are equivalent when both object graphs have equally named properties with the same value,
        /// irrespective of the type of those objects. Two properties are also equal if one type can be converted to another and the result is equal.
        /// The type of a collection property is ignored as long as the collection implements <see cref="IEnumerable{T}"/> and all
        /// items in the collection are structurally equal.
        /// Notice that actual behavior is determined by the global defaults managed by <see cref="AssertionOptions"/>.
        /// </remarks>
        /// <param name="expectation">The expected element.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public AndConstraint<TAssertions> BeEquivalentTo<TExpectation>(TExpectation expectation, string because = "",
            params object[] becauseArgs)
        {
            return BeEquivalentTo(expectation, config => config, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that an object is equivalent to another object.
        /// </summary>
        /// <remarks>
        /// Objects are equivalent when both object graphs have equally named properties with the same value,
        /// irrespective of the type of those objects. Two properties are also equal if one type can be converted to another and the result is equal.
        /// The type of a collection property is ignored as long as the collection implements <see cref="IEnumerable{T}"/> and all
        /// items in the collection are structurally equal.
        /// </remarks>
        /// <param name="expectation">The expected element.</param>
        /// <param name="config">
        /// A reference to the <see cref="EquivalencyAssertionOptions{TSubject}"/> configuration object that can be used
        /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
        /// <see cref="EquivalencyAssertionOptions{TSubject}"/> class. The global defaults are determined by the
        /// <see cref="AssertionOptions"/> class.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public AndConstraint<TAssertions> BeEquivalentTo<TExpectation>(TExpectation expectation,
            Func<EquivalencyAssertionOptions<TExpectation>, EquivalencyAssertionOptions<TExpectation>> config, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(config, nameof(config));

            EquivalencyAssertionOptions<TExpectation> options = config(AssertionOptions.CloneDefaults<TExpectation>());

            var context = new EquivalencyValidationContext(Node.From<TExpectation>(() =>
                AssertionScope.Current.CallerIdentity), options)
            {
                Reason = new Reason(because, becauseArgs),
                TraceWriter = options.TraceWriter
            };

            var comparands = new Comparands
            {
                Subject = Subject,
                Expectation = expectation,
                CompileTimeType = typeof(TExpectation),
            };

            new EquivalencyValidator().AssertEquality(comparands, context);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that an object is not equivalent to another object.
        /// </summary>
        /// <remarks>
        /// Objects are equivalent when both object graphs have equally named properties with the same value,
        /// irrespective of the type of those objects. Two properties are also equal if one type can be converted to another and the result is equal.
        /// The type of a collection property is ignored as long as the collection implements <see cref="IEnumerable{T}"/> and all
        /// items in the collection are structurally equal.
        /// Notice that actual behavior is determined by the global defaults managed by <see cref="AssertionOptions"/>.
        /// </remarks>
        /// <param name="unexpected">The unexpected element.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public AndConstraint<TAssertions> NotBeEquivalentTo<TExpectation>(
            TExpectation unexpected,
            string because = "",
            params object[] becauseArgs)
        {
            return NotBeEquivalentTo(unexpected, config => config, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that an object is not equivalent to another object.
        /// </summary>
        /// <remarks>
        /// Objects are equivalent when both object graphs have equally named properties with the same value,
        /// irrespective of the type of those objects. Two properties are also equal if one type can be converted to another and the result is equal.
        /// The type of a collection property is ignored as long as the collection implements <see cref="IEnumerable{T}"/> and all
        /// items in the collection are structurally equal.
        /// </remarks>
        /// <param name="unexpected">The unexpected element.</param>
        /// <param name="config">
        /// A reference to the <see cref="EquivalencyAssertionOptions{TSubject}"/> configuration object that can be used
        /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
        /// <see cref="EquivalencyAssertionOptions{TSubject}"/> class. The global defaults are determined by the
        /// <see cref="AssertionOptions"/> class.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public AndConstraint<TAssertions> NotBeEquivalentTo<TExpectation>(
            TExpectation unexpected,
            Func<EquivalencyAssertionOptions<TExpectation>, EquivalencyAssertionOptions<TExpectation>> config,
            string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(config, nameof(config));

            bool hasMismatches;
            using (var scope = new AssertionScope())
            {
                Subject.Should().BeEquivalentTo(unexpected, config);
                hasMismatches = scope.Discard().Length > 0;
            }

            Execute.Assertion
                .ForCondition(hasMismatches)
                .BecauseOf(because, becauseArgs)
                .WithDefaultIdentifier(Identifier)
                .FailWith("Expected {context} not to be equivalent to {0}{reason}, but they are.", unexpected);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Returns the type of the subject the assertion applies on.
        /// </summary>
        protected override string Identifier => "object";
    }
}
