using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions.Collections
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="IEnumerable"/> is in the expected state.
    /// </summary>
    public abstract class CollectionAssertions<TSubject, TAssertions> : ReferenceTypeAssertions<TSubject, TAssertions>
        where TSubject : IEnumerable
        where TAssertions : CollectionAssertions<TSubject, TAssertions>
    {
        protected CollectionAssertions() : this(default)
        {
        }

        protected CollectionAssertions(TSubject subject) : base(subject)
        {
        }

        /// <summary>
        /// Asserts that the collection does not contain any items.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeEmpty(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:collection} to be empty{reason}, ")
                .ForCondition(!ReferenceEquals(Subject, null))
                .FailWith("but found {0}.", Subject)
                .Then
                .Given(() => Subject.Cast<object>())
                .ForCondition(collection => !collection.Any())
                .FailWith("but found {0}.", collection => collection)
                .Then
                .ClearExpectation();

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the collection contains at least 1 item.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeEmpty(string because = "", params object[] becauseArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} not to be empty{reason}, but found {0}.", Subject);
            }

            IEnumerable<object> enumerable = Subject.Cast<object>();

            Execute.Assertion
                .ForCondition(enumerable.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} not to be empty{reason}.");

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the collection is null or does not contain any items.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeNullOrEmpty(string because = "", params object[] becauseArgs)
        {
            var nullOrEmpty = ReferenceEquals(Subject, null) || !Subject.Cast<object>().Any();

            Execute.Assertion.ForCondition(nullOrEmpty)
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected {context:collection} to be null or empty{reason}, but found {0}.",
                    Subject);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the collection is not null and contains at least 1 item.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeNullOrEmpty(string because = "", params object[] becauseArgs)
        {
            return NotBeNull(because, becauseArgs)
                .And.NotBeEmpty(because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the collection does not contain any duplicate items.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> OnlyHaveUniqueItems(string because = "", params object[] becauseArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to only have unique items{reason}, but found {0}.", Subject);
            }

            IEnumerable<object> groupWithMultipleItems = Subject.Cast<object>()
                .GroupBy(o => o)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key);

            if (groupWithMultipleItems.Any())
            {
                if (groupWithMultipleItems.Count() > 1)
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:collection} to only have unique items{reason}, but items {0} are not unique.",
                            groupWithMultipleItems);
                }
                else
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:collection} to only have unique items{reason}, but item {0} is not unique.",
                            groupWithMultipleItems.First());
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the collection does not contain any <c>null</c> items.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotContainNulls(string because = "", params object[] becauseArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} not to contain <null>s{reason}, but collection is <null>.");
            }

            int[] indices = Subject
                .Cast<object>()
                .Select((item, index) => new { Item = item, Index = index })
                .Where(e => e.Item is null)
                .Select(e => e.Index)
                .ToArray();

            if (indices.Length > 0)
            {
                if (indices.Length > 1)
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:collection} not to contain <null>s{reason}, but found several at indices {0}.", indices);
                }
                else
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:collection} not to contain <null>s{reason}, but found one at index {0}.", indices[0]);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Expects the current collection to contain all the same elements in the same order as the collection identified by
        /// <paramref name="elements" />. Elements are compared using their <see cref="object.Equals(object)" />.
        /// </summary>
        /// <param name="elements">A params array with the expected elements.</param>
        public AndConstraint<TAssertions> Equal(params object[] elements)
        {
            return Equal(elements, string.Empty);
        }

        /// <summary>
        /// Expects the current collection to contain all the same elements in the same order as the collection identified by
        /// <paramref name="expected" />. Elements are compared using their <see cref="object.Equals(object)" />.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable"/> with the expected elements.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> Equal(IEnumerable expected, string because = "", params object[] becauseArgs)
        {
            AssertSubjectEquality<object, object>(expected, (s, e) => s.IsSameOrEqualTo(e), because, becauseArgs);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        protected void AssertSubjectEquality<TActual, TExpected>(IEnumerable expectation, Func<TActual, TExpected, bool> equalityComparison,
            string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(equalityComparison, nameof(equalityComparison));

            bool subjectIsNull = ReferenceEquals(Subject, null);
            bool expectationIsNull = expectation is null;
            if (subjectIsNull && expectationIsNull)
            {
                return;
            }

            Guard.ThrowIfArgumentIsNull(expectation, nameof(expectation), "Cannot compare collection with <null>.");

            ICollection<TExpected> expectedItems = expectation.ConvertOrCastToCollection<TExpected>();

            AssertionScope assertion = Execute.Assertion.BecauseOf(because, becauseArgs);
            if (subjectIsNull)
            {
                assertion.FailWith("Expected {context:collection} to be equal to {0}{reason}, but found <null>.", expectedItems);
            }

            assertion
                .WithExpectation("Expected {context:collection} to be equal to {0}{reason}, ", expectedItems)
                .Given(() => Subject.ConvertOrCastToCollection<TActual>())
                .AssertCollectionsHaveSameCount(expectedItems.Count)
                .Then
                .AssertCollectionsHaveSameItems(expectedItems, (a, e) => a.IndexOfFirstDifferenceWith(e, equalityComparison))
                .Then
                .ClearExpectation();
        }

        /// <summary>
        /// Expects the current collection not to contain all the same elements in the same order as the collection identified by
        /// <paramref name="unexpected" />. Elements are compared using their <see cref="object.Equals(object)" />.
        /// </summary>
        /// <param name="unexpected">An <see cref="IEnumerable"/> with the elements that are not expected.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotEqual(IEnumerable unexpected, string because = "", params object[] becauseArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected collections not to be equal{reason}, but found <null>.");
            }

            Guard.ThrowIfArgumentIsNull(unexpected, nameof(unexpected), "Cannot compare collection with <null>.");

            if (ReferenceEquals(Subject, unexpected))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected collections not to be equal{reason}, but they both reference the same object.");
            }

            ICollection<object> actualItems = Subject.ConvertOrCastToCollection<object>();

            if (actualItems.SequenceEqual(unexpected.Cast<object>()))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Did not expect collections {0} and {1} to be equal{reason}.", unexpected, actualItems);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
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
        public AndConstraint<TAssertions> BeEquivalentTo<TExpectation>(IEnumerable<TExpectation> expectation,
            string because = "", params object[] becauseArgs)
        {
            BeEquivalentTo(expectation, config => config, because, becauseArgs);

            return new AndConstraint<TAssertions>((TAssertions)this);
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
        public AndConstraint<TAssertions> BeEquivalentTo(params object[] expectations)
        {
            BeEquivalentTo(expectations, config => config, string.Empty);

            return new AndConstraint<TAssertions>((TAssertions)this);
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
        public AndConstraint<TAssertions> BeEquivalentTo(IEnumerable expectation, string because = "", params object[] becauseArgs)
        {
            BeEquivalentTo(expectation, config => config, because, becauseArgs);

            return new AndConstraint<TAssertions>((TAssertions)this);
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
        public AndConstraint<TAssertions> BeEquivalentTo(IEnumerable expectation,
            Func<EquivalencyAssertionOptions<IEnumerable>, EquivalencyAssertionOptions<IEnumerable>> config, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(config, nameof(config));

            EquivalencyAssertionOptions<IEnumerable> options = config(AssertionOptions.CloneDefaults<IEnumerable>());

            var context = new EquivalencyValidationContext
            {
                Subject = Subject,
                Expectation = expectation,
                RootIsCollection = true,
                CompileTimeType = typeof(IEnumerable),
                Because = because,
                BecauseArgs = becauseArgs,
                Tracer = options.TraceWriter
            };

            var equivalencyValidator = new EquivalencyValidator(options);
            equivalencyValidator.AssertEquality(context);

            return new AndConstraint<TAssertions>((TAssertions)this);
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
        public AndConstraint<TAssertions> BeEquivalentTo<TExpectation>(IEnumerable<TExpectation> expectation,
            Func<EquivalencyAssertionOptions<TExpectation>, EquivalencyAssertionOptions<TExpectation>> config, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(config, nameof(config));

            EquivalencyAssertionOptions<IEnumerable<TExpectation>> options = config(AssertionOptions.CloneDefaults<TExpectation>()).AsCollection();

            var context = new EquivalencyValidationContext
            {
                Subject = Subject,
                Expectation = expectation,
                RootIsCollection = true,
                CompileTimeType = typeof(IEnumerable<TExpectation>),
                Because = because,
                BecauseArgs = becauseArgs,
                Tracer = options.TraceWriter
            };

            var equivalencyValidator = new EquivalencyValidator(options);
            equivalencyValidator.AssertEquality(context);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Expects the current collection not to contain all elements of the collection identified by <paramref name="unexpected" />,
        /// regardless of the order. Elements are compared using their <see cref="object.Equals(object)" />.
        /// </summary>
        /// <param name="unexpected">An <see cref="IEnumerable"/> with the unexpected elements.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeEquivalentTo(IEnumerable unexpected, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(unexpected, nameof(unexpected), "Cannot verify inequivalence against a <null> collection.");

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} not to be equivalent{reason}, but found <null>.");
            }

            if (ReferenceEquals(Subject, unexpected))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} {0} not to be equivalent with collection {1}{reason}, but they both reference the same object.",
                        Subject,
                        unexpected);
            }

            IEnumerable<object> actualItems = Subject.Cast<object>();
            IEnumerable<object> unexpectedItems = unexpected.Cast<object>();

            if (actualItems.Count() == unexpectedItems.Count())
            {
                List<object> missingItems = GetMissingItems(unexpectedItems, actualItems);

                Execute.Assertion
                    .ForCondition(missingItems.Count > 0)
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} {0} not be equivalent with collection {1}{reason}.", Subject,
                        unexpected);
            }

            string[] failures;

            using (var scope = new AssertionScope())
            {
                Subject.Should().BeEquivalentTo(unexpected);

                failures = scope.Discard();
            }

            Execute.Assertion
                .ForCondition(failures.Length > 0)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} {0} not to be equivalent to collection {1}{reason}.", Subject,
                    unexpected);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that a collection of objects contains at least one object equivalent to another object.
        /// </summary>
        /// <remarks>
        /// Objects within the collection are equivalent to the expected object when both object graphs have equally named properties with the same
        /// value, irrespective of the type of those objects. Two properties are also equal if one type can be converted to another
        /// and the result is equal.
        /// Notice that actual behavior is determined by the global defaults managed by <see cref="AssertionOptions"/>.
        /// </remarks>
        /// <param name="because">
        /// An optional formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the
        /// assertion is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> ContainEquivalentOf<TExpectation>(TExpectation expectation, string because = "",
            params object[] becauseArgs)
        {
            return ContainEquivalentOf(expectation, config => config, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that a collection of objects contains at least one object equivalent to another object.
        /// </summary>
        /// <remarks>
        /// Objects within the collection are equivalent to the expected object when both object graphs have equally named properties with the same
        /// value, irrespective of the type of those objects. Two properties are also equal if one type can be converted to another
        /// and the result is equal.
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
        public AndConstraint<TAssertions> ContainEquivalentOf<TExpectation>(TExpectation expectation, Func<EquivalencyAssertionOptions<TExpectation>,
                EquivalencyAssertionOptions<TExpectation>> config, string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(config, nameof(config));

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to contain equivalent of {0}{reason}, but found <null>.", expectation);
            }

            EquivalencyAssertionOptions<TExpectation> options = config(AssertionOptions.CloneDefaults<TExpectation>());
            IEnumerable<object> actualItems = Subject.Cast<object>();

            using (var scope = new AssertionScope())
            {
                scope.AddReportable("configuration", options.ToString());

                foreach (object actualItem in actualItems)
                {
                    var context = new EquivalencyValidationContext
                    {
                        Subject = actualItem,
                        Expectation = expectation,
                        CompileTimeType = typeof(TExpectation),
                        Because = because,
                        BecauseArgs = becauseArgs,
                        Tracer = options.TraceWriter,
                    };

                    var equivalencyValidator = new EquivalencyValidator(options);
                    equivalencyValidator.AssertEquality(context);

                    string[] failures = scope.Discard();

                    if (!failures.Any())
                    {
                        return new AndConstraint<TAssertions>((TAssertions)this);
                    }
                }

                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} {0} to contain equivalent of {1}{reason}.", Subject, expectation);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current collection only contains items that are assignable to the type <typeparamref name="T" />.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> ContainItemsAssignableTo<T>(string because = "", params object[] becauseArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to contain element assignable to type {0}{reason}, but found <null>.",
                        typeof(T));
            }

            int index = 0;
            foreach (object item in Subject)
            {
                if (!(item is T))
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith(
                            "Expected {context:collection} to contain only items of type {0}{reason}, but item {1} at index {2} is of type {3}.",
                            typeof(T), item, index, item.GetType());
                }

                ++index;
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        private static List<T> GetMissingItems<T>(IEnumerable<T> expectedItems, IEnumerable<T> actualItems)
        {
            List<T> missingItems = new List<T>();
            List<T> subject = actualItems.ToList();

            foreach (T expectation in expectedItems)
            {
                if (subject.Contains(expectation))
                {
                    subject.Remove(expectation);
                }
                else
                {
                    missingItems.Add(expectation);
                }
            }

            return missingItems;
        }

        /// <summary>
        /// Expects the current collection to contain the specified elements in any order. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable"/> with the expected elements.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> Contain(IEnumerable expected, string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(expected, nameof(expected), "Cannot verify containment against a <null> collection");

            ICollection<object> expectedObjects = expected.ConvertOrCastToCollection<object>();
            if (!expectedObjects.Any())
            {
                throw new ArgumentException("Cannot verify containment against an empty collection",
                    nameof(expected));
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to contain {0}{reason}, but found <null>.", expected);
            }

            if (expected is string)
            {
                if (!Subject.Cast<object>().Contains(expected))
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:collection} {0} to contain {1}{reason}.", Subject, expected);
                }
            }
            else
            {
                IEnumerable<object> missingItems = expectedObjects.Except(Subject.Cast<object>());
                if (missingItems.Any())
                {
                    if (expectedObjects.Count > 1)
                    {
                        Execute.Assertion
                            .BecauseOf(because, becauseArgs)
                            .FailWith("Expected {context:collection} {0} to contain {1}{reason}, but could not find {2}.", Subject,
                                expected, missingItems);
                    }
                    else
                    {
                        Execute.Assertion
                            .BecauseOf(because, becauseArgs)
                            .FailWith("Expected {context:collection} {0} to contain {1}{reason}.", Subject,
                                expected.Cast<object>().First());
                    }
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Expects the current collection to contain the specified elements in the exact same order, not necessarily consecutive.
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable"/> with the expected elements.</param>
        public AndConstraint<TAssertions> ContainInOrder(params object[] expected)
        {
            return ContainInOrder(expected, "");
        }

        /// <summary>
        /// Expects the current collection to contain the specified elements in the exact same order, not necessarily consecutive.
        /// </summary>
        /// <remarks>
        /// Elements are compared using their <see cref="object.Equals(object)" /> implementation.
        /// </remarks>
        /// <param name="expected">An <see cref="IEnumerable"/> with the expected elements.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> ContainInOrder(IEnumerable expected, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(expected, nameof(expected), "Cannot verify ordered containment against a <null> collection.");

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to contain {0} in order{reason}, but found <null>.", expected);
            }

            IList<object> expectedItems = expected.ConvertOrCastToList<object>();
            IList<object> actualItems = Subject.ConvertOrCastToList<object>();

            for (int index = 0; index < expectedItems.Count; index++)
            {
                object expectedItem = expectedItems[index];
                actualItems = actualItems.SkipWhile(actualItem => !actualItem.IsSameOrEqualTo(expectedItem)).ToArray();
                if (actualItems.Any())
                {
                    actualItems = actualItems.Skip(1).ToArray();
                }
                else
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith(
                            "Expected {context:collection} {0} to contain items {1} in order{reason}, but {2} (index {3}) did not appear (in the right order).",
                            Subject, expected, expectedItem, index);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Expects the current collection to have all elements in ascending order. Elements are compared
        /// using their <see cref="IComparable.CompareTo(object)" /> implementation.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        /// <remarks>
        /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
        /// </remarks>
        public AndConstraint<TAssertions> BeInAscendingOrder(string because = "", params object[] becauseArgs)
        {
            return BeInAscendingOrder(Comparer<object>.Default, because, becauseArgs);
        }

        /// <summary>
        /// Expects the current collection to have all elements in ascending order. Elements are compared
        /// using the given <see cref="IComparer{T}" /> implementation.
        /// </summary>
        /// <param name="comparer">
        /// The object that should be used to determine the expected ordering.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        /// <remarks>
        /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
        /// </remarks>
        public AndConstraint<TAssertions> BeInAscendingOrder(IComparer<object> comparer, string because = "", params object[] becauseArgs)
        {
            return BeInOrder(comparer, SortOrder.Ascending, because, becauseArgs);
        }

        /// <summary>
        /// Expects the current collection to have all elements in descending order. Elements are compared
        /// using their <see cref="IComparable.CompareTo(object)" /> implementation.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        /// <remarks>
        /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
        /// </remarks>
        public AndConstraint<TAssertions> BeInDescendingOrder(string because = "", params object[] becauseArgs)
        {
            return BeInDescendingOrder(Comparer<object>.Default, because, becauseArgs);
        }

        /// <summary>
        /// Expects the current collection to have all elements in descending order. Elements are compared
        /// using the given <see cref="IComparer{T}" /> implementation.
        /// </summary>
        /// <param name="comparer">
        /// The object that should be used to determine the expected ordering.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        /// <remarks>
        /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
        /// </remarks>
        public AndConstraint<TAssertions> BeInDescendingOrder(IComparer<object> comparer, string because = "", params object[] becauseArgs)
        {
            return BeInOrder(comparer, SortOrder.Descending, because, becauseArgs);
        }

        /// <summary>
        /// Expects the current collection to have all elements in the specified <paramref name="expectedOrder"/>.
        /// Elements are compared using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        private AndConstraint<TAssertions> BeInOrder(
            IComparer<object> comparer, SortOrder expectedOrder, string because = "", params object[] becauseArgs)
        {
            string sortOrder = (expectedOrder == SortOrder.Ascending) ? "ascending" : "descending";

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to contain items in " + sortOrder + " order{reason}, but found {1}.",
                        Subject);
            }

            IList<object> actualItems = Subject.ConvertOrCastToList<object>();

            object[] orderedItems = (expectedOrder == SortOrder.Ascending)
                ? actualItems.OrderBy(item => item, comparer).ToArray()
                : actualItems.OrderByDescending(item => item, comparer).ToArray();

            for (int index = 0; index < orderedItems.Length; index++)
            {
                Execute.Assertion
                    .ForCondition(actualItems[index].IsSameOrEqualTo(orderedItems[index]))
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to contain items in " + sortOrder +
                              " order{reason}, but found {0} where item at index {1} is in wrong order.",
                        Subject, index);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts the current collection does not have all elements in ascending order. Elements are compared
        /// using their <see cref="IComparable.CompareTo(object)" /> implementation.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        /// <remarks>
        /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
        /// </remarks>
        [Obsolete("Use NotBeInAscendingOrder instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public AndConstraint<TAssertions> NotBeAscendingInOrder(string because = "", params object[] becauseArgs)
        {
            return NotBeInAscendingOrder(Comparer<object>.Default, because, becauseArgs);
        }

        /// <summary>
        /// Asserts the current collection does not have all elements in ascending order. Elements are compared
        /// using their <see cref="IComparable.CompareTo(object)" /> implementation.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        /// <remarks>
        /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
        /// </remarks>
        public AndConstraint<TAssertions> NotBeInAscendingOrder(string because = "", params object[] becauseArgs)
        {
            return NotBeInAscendingOrder(Comparer<object>.Default, because, becauseArgs);
        }

        /// <summary>
        /// Asserts the current collection does not have all elements in ascending order. Elements are compared
        /// using their <see cref="IComparable.CompareTo(object)" /> implementation.
        /// </summary>
        /// <param name="comparer">
        /// The object that should be used to determine the expected ordering.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        /// <remarks>
        /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
        /// </remarks>
        [Obsolete("Use NotBeInAscendingOrder instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public AndConstraint<TAssertions> NotBeAscendingInOrder(IComparer<object> comparer, string because = "", params object[] becauseArgs)
        {
            return NotBeInOrder(comparer, SortOrder.Ascending, because, becauseArgs);
        }

        /// <summary>
        /// Asserts the current collection does not have all elements in ascending order. Elements are compared
        /// using their <see cref="IComparable.CompareTo(object)" /> implementation.
        /// </summary>
        /// <param name="comparer">
        /// The object that should be used to determine the expected ordering.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        /// <remarks>
        /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
        /// </remarks>
        public AndConstraint<TAssertions> NotBeInAscendingOrder(IComparer<object> comparer, string because = "", params object[] becauseArgs)
        {
            return NotBeInOrder(comparer, SortOrder.Ascending, because, becauseArgs);
        }

        /// <summary>
        /// Asserts the current collection does not have all elements in descending order. Elements are compared
        /// using their <see cref="IComparable.CompareTo(object)" /> implementation.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        /// <remarks>
        /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
        /// </remarks>
        [Obsolete("Use NotBeInDescendingOrder instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public AndConstraint<TAssertions> NotBeDescendingInOrder(string because = "", params object[] becauseArgs)
        {
            return NotBeInDescendingOrder(Comparer<object>.Default, because, becauseArgs);
        }

        /// <summary>
        /// Asserts the current collection does not have all elements in descending order. Elements are compared
        /// using their <see cref="IComparable.CompareTo(object)" /> implementation.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        /// <remarks>
        /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
        /// </remarks>
        public AndConstraint<TAssertions> NotBeInDescendingOrder(string because = "", params object[] becauseArgs)
        {
            return NotBeInDescendingOrder(Comparer<object>.Default, because, becauseArgs);
        }

        /// <summary>
        /// Asserts the current collection does not have all elements in descending order. Elements are compared
        /// using their <see cref="IComparable.CompareTo(object)" /> implementation.
        /// </summary>
        /// <param name="comparer">
        /// The object that should be used to determine the expected ordering.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        /// <remarks>
        /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
        /// </remarks>
        [Obsolete("Use NotBeInDescendingOrder instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public AndConstraint<TAssertions> NotBeDescendingInOrder(IComparer<object> comparer, string because = "", params object[] becauseArgs)
        {
            return NotBeInOrder(comparer, SortOrder.Descending, because, becauseArgs);
        }

        /// <summary>
        /// Asserts the current collection does not have all elements in descending order. Elements are compared
        /// using their <see cref="IComparable.CompareTo(object)" /> implementation.
        /// </summary>
        /// <param name="comparer">
        /// The object that should be used to determine the expected ordering.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        /// <remarks>
        /// Empty and single element collections are considered to be ordered both in ascending and descending order at the same time.
        /// </remarks>
        public AndConstraint<TAssertions> NotBeInDescendingOrder(IComparer<object> comparer, string because = "", params object[] becauseArgs)
        {
            return NotBeInOrder(comparer, SortOrder.Descending, because, becauseArgs);
        }

        /// <summary>
        /// Asserts the current collection does not have all elements in ascending order. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        private AndConstraint<TAssertions> NotBeInOrder(IComparer<object> comparer, SortOrder order, string because = "", params object[] becauseArgs)
        {
            string sortOrder = (order == SortOrder.Ascending) ? "ascending" : "descending";

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Did not expect {context:collection} to contain items in " + sortOrder + " order{reason}, but found {1}.",
                        Subject);
            }

            object[] orderedItems = (order == SortOrder.Ascending)
                ? Subject.Cast<object>().OrderBy(item => item, comparer).ToArray()
                : Subject.Cast<object>().OrderByDescending(item => item, comparer).ToArray();

            bool itemsAreUnordered = Subject
                .Cast<object>()
                .Where((actualItem, index) => !actualItem.IsSameOrEqualTo(orderedItems[index]))
                .Any();

            if (!itemsAreUnordered)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Did not expect {context:collection} to contain items in " + sortOrder + " order{reason}, but found {0}.",
                        Subject);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the collection is a subset of the <paramref name="expectedSuperset" />.
        /// </summary>
        /// <param name="expectedSuperset">An <see cref="IEnumerable"/> with the expected superset.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeSubsetOf(IEnumerable expectedSuperset, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(expectedSuperset, nameof(expectedSuperset), "Cannot verify a subset against a <null> collection.");

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to be a subset of {0}{reason}, but found {1}.", expectedSuperset,
                        Subject);
            }

            IEnumerable<object> expectedItems = expectedSuperset.Cast<object>();
            IEnumerable<object> actualItems = Subject.Cast<object>();

            IEnumerable<object> excessItems = actualItems.Except(expectedItems);

            if (excessItems.Any())
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Expected {context:collection} to be a subset of {0}{reason}, but items {1} are not part of the superset.",
                        expectedSuperset, excessItems);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the collection is not a subset of the <paramref name="unexpectedSuperset" />.
        /// </summary>
        /// <param name="unexpectedSuperset">An <see cref="IEnumerable"/> with the unexpected superset.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeSubsetOf(IEnumerable unexpectedSuperset, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!ReferenceEquals(Subject, null))
                .BecauseOf(because, becauseArgs)
                .FailWith("Cannot assert a <null> collection against a subset.");

            if (ReferenceEquals(Subject, unexpectedSuperset))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Did not expect {context:collection} {0} to be a subset of {1}{reason}, but they both reference the same object.",
                        Subject,
                        unexpectedSuperset);
            }

            IEnumerable<object> expectedItems = unexpectedSuperset.Cast<object>();
            ICollection<object> actualItems = Subject.ConvertOrCastToCollection<object>();

            if (actualItems.Intersect(expectedItems).Count() == actualItems.Count)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Did not expect {context:collection} {0} to be a subset of {1}{reason}.", actualItems, expectedItems);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Assert that the current collection has the same number of elements as <paramref name="otherCollection" />.
        /// </summary>
        /// <param name="otherCollection">The other collection with the same expected number of elements</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> HaveSameCount(IEnumerable otherCollection, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(otherCollection, nameof(otherCollection), "Cannot verify count against a <null> collection.");

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to have the same count as {0}{reason}, but found {1}.",
                        otherCollection,
                        Subject);
            }

            IEnumerable<object> enumerable = Subject.Cast<object>();

            int actualCount = enumerable.Count();
            int expectedCount = otherCollection.Cast<object>().Count();

            Execute.Assertion
                .ForCondition(actualCount == expectedCount)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} to have {0} item(s){reason}, but found {1}.", expectedCount, actualCount);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Assert that the current collection does not have the same number of elements as <paramref name="otherCollection" />.
        /// </summary>
        /// <param name="otherCollection">The other collection with the unexpected number of elements</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotHaveSameCount(IEnumerable otherCollection, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(otherCollection, nameof(otherCollection), "Cannot verify count against a <null> collection.");

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to not have the same count as {0}{reason}, but found {1}.",
                        otherCollection,
                        Subject);
            }

            if (ReferenceEquals(Subject, otherCollection))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} {0} to not have the same count as {1}{reason}, but they both reference the same object.",
                        Subject,
                        otherCollection);
            }

            IEnumerable<object> enumerable = Subject.Cast<object>();

            int actualCount = enumerable.Count();
            int expectedCount = otherCollection.Cast<object>().Count();

            Execute.Assertion
                .ForCondition(actualCount != expectedCount)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} to not have {0} item(s){reason}, but found {1}.", expectedCount, actualCount);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current collection has the supplied <paramref name="element" /> at the
        /// supplied <paramref name="index" />.
        /// </summary>
        /// <param name="index">The index where the element is expected</param>
        /// <param name="element">The expected element</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndWhichConstraint<TAssertions, object> HaveElementAt(int index, object element, string because = "",
            params object[] becauseArgs)
        {
            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to have element at index {0}{reason}, but found {1}.", index, Subject);
            }

            IEnumerable<object> enumerable = Subject.Cast<object>();

            object actual = null;
            if (index < enumerable.Count())
            {
                actual = Subject.Cast<object>().ElementAt(index);

                Execute.Assertion
                    .ForCondition(actual.IsSameOrEqualTo(element))
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {0} at index {1}{reason}, but found {2}.", element, index, actual);
            }
            else
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {0} at index {1}{reason}, but found no element.", element, index);
            }

            return new AndWhichConstraint<TAssertions, object>((TAssertions)this, actual);
        }

        /// <summary>
        /// Asserts that the current collection does not contain the supplied items. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="unexpected">An <see cref="IEnumerable"/> with the unexpected elements.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotContain(IEnumerable unexpected, string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(unexpected, nameof(unexpected), "Cannot verify non-containment against a <null> collection");

            ICollection<object> unexpectedObjects = unexpected.ConvertOrCastToCollection<object>();
            if (!unexpectedObjects.Any())
            {
                throw new ArgumentException("Cannot verify non-containment against an empty collection",
                    nameof(unexpected));
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to not contain {0}{reason}, but found <null>.", unexpected);
            }

            if (unexpected is string)
            {
                if (Subject.Cast<object>().Contains(unexpected))
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected {context:collection} {0} to not contain {1}{reason}.", Subject, unexpected);
                }
            }
            else
            {
                IEnumerable<object> foundItems = unexpectedObjects.Intersect(Subject.Cast<object>());
                if (foundItems.Any())
                {
                    if (unexpectedObjects.Count > 1)
                    {
                        Execute.Assertion
                            .BecauseOf(because, becauseArgs)
                            .FailWith("Expected {context:collection} {0} to not contain {1}{reason}, but found {2}.", Subject,
                                unexpected, foundItems);
                    }
                    else
                    {
                        Execute.Assertion
                            .BecauseOf(because, becauseArgs)
                            .FailWith("Expected {context:collection} {0} to not contain element {1}{reason}.", Subject,
                                unexpectedObjects.First());
                    }
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the collection shares one or more items with the specified <paramref name="otherCollection"/>.
        /// </summary>
        /// <param name="otherCollection">The <see cref="IEnumerable"/> with the expected shared items.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> IntersectWith(IEnumerable otherCollection, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(otherCollection, nameof(otherCollection), "Cannot verify intersection against a <null> collection.");

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to intersect with {0}{reason}, but found {1}.", otherCollection,
                        Subject);
            }

            IEnumerable<object> otherItems = otherCollection.Cast<object>();
            IEnumerable<object> sharedItems = Subject.Cast<object>().Intersect(otherItems);

            if (!sharedItems.Any())
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Expected {context:collection} to intersect with {0}{reason}, but {1} does not contain any shared items.",
                        otherCollection, Subject);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the collection does not share any items with the specified <paramref name="otherCollection"/>.
        /// </summary>
        /// <param name="otherCollection">The <see cref="IEnumerable"/> to compare to.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotIntersectWith(IEnumerable otherCollection, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(otherCollection, nameof(otherCollection), "Cannot verify intersection against a <null> collection.");

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Did not expect {context:collection} to intersect with {0}{reason}, but found {1}.", otherCollection,
                        Subject);
            }

            if (ReferenceEquals(Subject, otherCollection))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Did not expect {context:collection} {0} to intersect with {1}{reason}, but they both reference the same object.",
                        Subject,
                        otherCollection);
            }

            IEnumerable<object> otherItems = otherCollection.Cast<object>();
            IEnumerable<object> sharedItems = Subject.Cast<object>().Intersect(otherItems);

            if (sharedItems.Any())
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Did not expect {context:collection} to intersect with {0}{reason}, but found the following shared items {1}.",
                        otherCollection, sharedItems);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the collection starts with the specified <paramref name="element"/>.
        /// </summary>
        /// <param name="element">
        /// The element that is expected to appear at the start of the collection. The object's <see cref="object.Equals(object)"/>
        /// is used to compare the element.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> StartWith(object element, string because = "", params object[] becauseArgs)
        {
            AssertCollectionStartsWith(Subject?.Cast<object>(), new[] { element }, ObjectExtensions.IsSameOrEqualTo, because, becauseArgs);
            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        protected void AssertCollectionStartsWith<TActual, TExpected>(IEnumerable<TActual> actualItems, TExpected[] expected, Func<TActual, TExpected, bool> equalityComparison, string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(equalityComparison, nameof(equalityComparison));

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:collection} to start with {0}{reason}, ", expected)
                .Given(() => actualItems)
                .AssertCollectionIsNotNull()
                .Then
                .AssertCollectionHasEnoughItems(expected.Length)
                .Then
                .AssertCollectionsHaveSameItems(expected, (a, e) => a.Take(e.Count).IndexOfFirstDifferenceWith(e, equalityComparison))
                .Then
                .ClearExpectation();
        }

        protected void AssertCollectionStartsWith<TActual, TExpected>(IEnumerable<TActual> actualItems, ICollection<TExpected> expected, Func<TActual, TExpected, bool> equalityComparison, string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(equalityComparison, nameof(equalityComparison));

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:collection} to start with {0}{reason}, ", expected)
                .Given(() => actualItems)
                .AssertCollectionIsNotNull()
                .Then
                .AssertCollectionHasEnoughItems(expected.Count)
                .Then
                .AssertCollectionsHaveSameItems(expected, (a, e) => a.Take(e.Count).IndexOfFirstDifferenceWith(e, equalityComparison))
                .Then
                .ClearExpectation();
        }

        /// <summary>
        /// Asserts that the collection ends with the specified <paramref name="element"/>.
        /// </summary>
        /// <param name="element">
        /// The element that is expected to appear at the end of the collection. The object's <see cref="object.Equals(object)"/>
        /// is used to compare the element.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> EndWith(object element, string because = "", params object[] becauseArgs)
        {
            AssertCollectionEndsWith(Subject?.Cast<object>(), new[] { element }, ObjectExtensions.IsSameOrEqualTo, because, becauseArgs);
            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        protected void AssertCollectionEndsWith<TActual, TExpected>(IEnumerable<TActual> actual, TExpected[] expected, Func<TActual, TExpected, bool> equalityComparison, string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(equalityComparison, nameof(equalityComparison));

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:collection} to end with {0}{reason}, ", expected)
                .Given(() => actual)
                .AssertCollectionIsNotNull()
                .Then
                .AssertCollectionHasEnoughItems(expected.Length)
                .Then
                .AssertCollectionsHaveSameItems(expected, (a, e) =>
                {
                    int firstIndexToCompare = a.Count - e.Count;
                    int index = a.Skip(firstIndexToCompare).IndexOfFirstDifferenceWith(e, equalityComparison);
                    return index >= 0 ? index + firstIndexToCompare : index;
                })
                .Then
                .ClearExpectation();
        }

        protected void AssertCollectionEndsWith<TActual, TExpected>(IEnumerable<TActual> actual, ICollection<TExpected> expected, Func<TActual, TExpected, bool> equalityComparison, string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(equalityComparison, nameof(equalityComparison));

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:collection} to end with {0}{reason}, ", expected)
                .Given(() => actual)
                .AssertCollectionIsNotNull()
                .Then
                .AssertCollectionHasEnoughItems(expected.Count)
                .Then
                .AssertCollectionsHaveSameItems(expected, (a, e) =>
                {
                    int firstIndexToCompare = a.Count - e.Count;
                    int index = a.Skip(firstIndexToCompare).IndexOfFirstDifferenceWith(e, equalityComparison);
                    return index >= 0 ? index + firstIndexToCompare : index;
                })
                .Then
                .ClearExpectation();
        }

        /// <summary>
        /// Asserts that the <paramref name="expectation"/> element directly precedes the <paramref name="successor"/>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> HaveElementPreceding(object successor, object expectation, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:collection} to have {0} precede {1}{reason}, ", expectation, successor)
                .Given(() => Subject.Cast<object>())
                .ForCondition(subject => subject.Any())
                .FailWith("but the collection is empty.")
                .Then
                .ForCondition(subject => HasPredecessor(successor, subject))
                .FailWith("but found nothing.")
                .Then
                .Given(subject => PredecessorOf(successor, subject))
                .ForCondition(predecessor => predecessor.IsSameOrEqualTo(expectation))
                .FailWith("but found {0}.", predecessor => predecessor)
                .Then
                .ClearExpectation();

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        private bool HasPredecessor(object successor, IEnumerable<object> subject)
        {
            return !ReferenceEquals(subject.First(), successor);
        }

        private object PredecessorOf(object successor, IEnumerable<object> subject)
        {
            IList<object> collection = subject.ConvertOrCastToList();
            int index = collection.IndexOf(successor);
            return (index > 0) ? collection[index - 1] : null;
        }

        /// <summary>
        /// Asserts that the <paramref name="expectation"/> element directly succeeds the <paramref name="predecessor"/>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> HaveElementSucceeding(object predecessor, object expectation, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:collection} to have {0} succeed {1}{reason}, ", expectation, predecessor)
                .Given(() => Subject.Cast<object>())
                .ForCondition(subject => subject.Any())
                .FailWith("but the collection is empty.")
                .Then
                .ForCondition(subject => HasSuccessor(predecessor, subject))
                .FailWith("but found nothing.")
                .Then
                .Given(subject => SuccessorOf(predecessor, subject))
                .ForCondition(successor => successor.IsSameOrEqualTo(expectation))
                .FailWith("but found {0}.", successor => successor)
                .Then
                .ClearExpectation();

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        private bool HasSuccessor(object predecessor, IEnumerable<object> subject)
        {
            return !ReferenceEquals(subject.Last(), predecessor);
        }

        private object SuccessorOf(object predecessor, IEnumerable<object> subject)
        {
            IList<object> collection = subject.ConvertOrCastToList();
            int index = collection.IndexOf(predecessor);
            return (index < (collection.Count - 1)) ? collection[index + 1] : null;
        }

        /// <summary>
        /// Asserts that all items in the collection are of the specified type <typeparamref name="T" />
        /// </summary>
        /// <typeparam name="T">The expected type of the objects</typeparam>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> AllBeAssignableTo<T>(string because = "", params object[] becauseArgs)
        {
            return AllBeAssignableTo(typeof(T), because, becauseArgs);
        }

        /// <summary>
        /// Asserts that all items in the collection are of the specified type <paramref name="expectedType"/>
        /// </summary>
        /// <param name="expectedType">The expected type of the objects</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> AllBeAssignableTo(Type expectedType, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected type to be {0}{reason}, ", expectedType.FullName)
                .Given(() => Subject.Cast<object>())
                .ForCondition(subject => subject.All(x => x != null))
                .FailWith("but found a null element.")
                .Then
                .ForCondition(subject => subject.All(x => expectedType.GetTypeInfo().IsAssignableFrom(GetType(x).GetTypeInfo())))
                .FailWith("but found {0}.", subject => $"[{string.Join(", ", subject.Select(x => GetType(x).FullName))}]")
                .Then
                .ClearExpectation();

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that all items in the collection are of the exact specified type <typeparamref name="T" />
        /// </summary>
        /// <typeparam name="T">The expected type of the objects</typeparam>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> AllBeOfType<T>(string because = "", params object[] becauseArgs)
        {
            return AllBeOfType(typeof(T), because, becauseArgs);
        }

        /// <summary>
        /// Asserts that all items in the collection are of the exact specified type <paramref name="expectedType"/>
        /// </summary>
        /// <param name="expectedType">The expected type of the objects</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> AllBeOfType(Type expectedType, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected type to be {0}{reason}, ", expectedType.FullName)
                .Given(() => Subject.Cast<object>())
                .ForCondition(subject => subject.All(x => x != null))
                .FailWith("but found a null element.")
                .Then
                .ForCondition(subject => subject.All(x => expectedType == GetType(x)))
                .FailWith("but found {0}.", subject => $"[{string.Join(", ", subject.Select(x => GetType(x).FullName))}]")
                .Then
                .ClearExpectation();

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        private static Type GetType(object o)
        {
            return o is Type t ? t : o.GetType();
        }

        /// <summary>
        /// Returns the type of the subject the assertion applies on.
        /// </summary>
        protected override string Identifier => "collection";
    }
}
