using System;
using System.Collections;
using System.Collections.Generic;
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
                .WithExpectation(Resources.Collection_ExpectedCollectionNotToBeEmptyComma)
                .ForCondition(!ReferenceEquals(Subject, null))
                .FailWith(Resources.Common_ButFoundXFormat, Subject)
                .Then
                .Given(() => Subject.Cast<object>())
                .ForCondition(collection => !collection.Any())
                .FailWith(Resources.Common_ButFoundXFormat, collection => collection);

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
                    .FailWith(Resources.Collection_ExpectedCollectionNotToBeEmpty + Resources.Common_CommaButFoundXFormat, Subject);
            }

            IEnumerable<object> enumerable = Subject.Cast<object>();

            Execute.Assertion
                .ForCondition(enumerable.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Collection_ExpectedCollectionNotToBeEmpty);

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
                    Resources.Collection_ExpectedCollectionNotToBeNullOrEmpty + Resources.Common_CommaButFoundXFormat,
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
                    .FailWith(Resources.Collection_ExpectedCollectionToOnlyHaveUniqueItems + Resources.Common_CommaButFoundXFormat, Subject);
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
                        .FailWith(Resources.Collection_ExpectedCollectionToOnlyHaveUniqueItems + Resources.Collection_CommaButItemsXAreNotUniqueFormat,
                            groupWithMultipleItems);
                }
                else
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith(Resources.Collection_ExpectedCollectionToOnlyHaveUniqueItems + Resources.Collection_CommaButItemXIsNotUniqueFormat,
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
                    .FailWith(Resources.Collection_ExpectedCollectionNotToContainNull + Resources.Collection_CommaButCollectionIsNull);
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
                        .FailWith(Resources.Collection_ExpectedCollectionNotToContainNull + Resources.Collection_CommaButFoundSeveralAtXFormat, indices);
                }
                else
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith(Resources.Collection_ExpectedCollectionNotToContainNull + Resources.Collection_CommaButFoundOneAtIndexXFormat, indices[0]);
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
            bool subjectIsNull = ReferenceEquals(Subject, null);
            bool expectationIsNull = expectation is null;
            if (subjectIsNull && expectationIsNull)
            {
                return;
            }

            if (expectation is null)
            {
                throw new ArgumentNullException(nameof(expectation), Resources.Collection_CannotCompareCollectionWithNull);
            }

            ICollection<TExpected> expectedItems = expectation.ConvertOrCastToCollection<TExpected>();

            IAssertionScope assertion = Execute.Assertion.BecauseOf(because, becauseArgs);
            if (subjectIsNull)
            {
                assertion.FailWith(Resources.Collection_ExpectedCollectionToBeEqualToXFormat + Resources.Common_CommaButFoundNull, expectedItems);
            }

            assertion
                .WithExpectation(Resources.Collection_ExpectedCollectionToBeEqualToXCommaFormat, expectedItems)
                .Given(() => Subject.ConvertOrCastToCollection<TActual>())
                .AssertCollectionsHaveSameCount(expectedItems.Count)
                .Then
                .AssertCollectionsHaveSameItems(expectedItems, (a, e) => a.IndexOfFirstDifferenceWith(e, equalityComparison));
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
                    .FailWith(Resources.Collection_ExpectedCollectionsNotToBeEqual + Resources.Common_CommaButFoundNull);
            }

            if (unexpected is null)
            {
                throw new ArgumentNullException(nameof(unexpected), Resources.Collection_CannotCompareCollectionWithNull);
            }

            if (ReferenceEquals(Subject, unexpected))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_ExpectedCollectionsNotToBeEqual + Resources.Common_CommaButBothReferenceSameObject);
            }

            ICollection<object> actualItems = Subject.ConvertOrCastToCollection<object>();

            if (actualItems.SequenceEqual(unexpected.Cast<object>()))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_DidNotExpectCollectionsXAndYToBeEqualFormat, unexpected, actualitems);
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
            if (unexpected is null)
            {
                throw new ArgumentNullException(nameof(unexpected), Resources.Collection_CannotVerifyInequivalenceAgainstANullCollection);
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_ExpectedCollectionNotToBeEquivalent + Resources.Common_CommaButFoundNull);
            }

            if (ReferenceEquals(Subject, unexpected))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_ExpectedCollectionXNotToBeEquivalentWithYFormat + Resources.Common_CommaButBothReferenceSameObject,
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
                    .FailWith(Resources.Collection_ExpectedCollectionXNotToBeEquivalentWithYFormat, Subject,
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
                .FailWith(Resources.Collection_ExpectedCollectionXNotToBeEquivalentToYFormat, Subject,
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
            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_ExpectedCollectionToContainEquivalentOfXFormat + Resources.Common_CommaButFoundNull, expectation);
            }

            IEquivalencyAssertionOptions options = config(AssertionOptions.CloneDefaults<TExpectation>());
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
                    .FailWith(Resources.Collection_ExpectedCollectionXToContainEquivalentOfYFormat, Subject, expectation);
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
                    .FailWith(Resources.Collection_ExpectedCollectionToContainElementAssignableToTypeXFormat + Resources.Common_CommaButFoundNull,
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
                            Resources.Collection_ExpectedCollectionToContainOnlyItemsOfTypeXFormat,
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
            if (expected is null)
            {
                throw new ArgumentNullException(nameof(expected), Resources.Collection_CannotVerifyContainmentAgainstNullCollection);
            }

            ICollection<object> expectedObjects = expected.ConvertOrCastToCollection<object>();
            if (!expectedObjects.Any())
            {
                throw new ArgumentException(Resources.Collection_CannotVerifyContainmentAgainstEmptyCollection,
                    nameof(expected));
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_ExpectedCollectionToContainXFormat + Resources.Common_CommaButFoundNull, expected);
            }

            if (expected is string)
            {
                if (!Subject.Cast<object>().Contains(expected))
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith(Resources.Collection_ExpectedCollectionXToContainYFormat, Subject, expected);
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
                            .FailWith(Resources.Collection_ExpectedCollectionXToContainYButCouldNotFindZFormat, Subject,
                                expected, missingItems);
                    }
                    else
                    {
                        Execute.Assertion
                            .BecauseOf(because, becauseArgs)
                            .FailWith(Resources.Collection_ExpectedCollectionXToContainYFormat, Subject,
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
            if (expected is null)
            {
                throw new ArgumentNullException(nameof(expected), Resources.Collection_CannotVerifyOrderedContainmentAgainstNull);
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_ExpectedCollectionToContainXInOrderFormat + Resources.Common_CommaButFoundNull, expected);
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
                            Resources.Collection_ExpectedCollectionXToContainItemsYInOrderButZDidNotAppearFormat,
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
            if (ReferenceEquals(Subject, null))
            {
                string failWithFirstPart = (expectedOrder == SortOrder.Ascending)
                    ? Resources.Collection_ExpectedCollectionToContainItemsInAscendingOrder
                    : Resources.Collection_ExpectedCollectionToContainItemsInDescendingOrder;

                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(failWithFirstPart + Resources.Common_CommaButFoundXFormat, Subject);
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
                    .FailWith(Resources.Collection_ExpectedCollectionToContainItemsInXOrderButFoundYWhereZInWrongOrderFormat,
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
        public AndConstraint<TAssertions> NotBeAscendingInOrder(string because = "", params object[] becauseArgs)
        {
            return NotBeAscendingInOrder(Comparer<object>.Default, because, becauseArgs);
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
        public AndConstraint<TAssertions> NotBeAscendingInOrder(IComparer<object> comparer, string because = "", params object[] becauseArgs)
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
        public AndConstraint<TAssertions> NotBeDescendingInOrder(string because = "", params object[] becauseArgs)
        {
            return NotBeDescendingInOrder(Comparer<object>.Default, because, becauseArgs);
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
        public AndConstraint<TAssertions> NotBeDescendingInOrder(IComparer<object> comparer, string because = "", params object[] becauseArgs)
        {
            return NotBeInOrder(comparer, SortOrder.Descending, because, becauseArgs);
        }

        /// <summary>
        /// Asserts the current collection does not have all elements in ascending order. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        private AndConstraint<TAssertions> NotBeInOrder(IComparer<object> comparer, SortOrder order, string because = "", params object[] becauseArgs)
        {
            string failWithFirstPart = (order == SortOrder.Ascending)
                ? Resources.Collection_ExpectedCollectionToContainItemsInAscendingOrder
                : Resources.Collection_ExpectedCollectionToContainItemsInDescendingOrder;

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(failWithFirstPart + Resources.Common_CommaButFoundXFormat, Subject);
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
                    .FailWith(failWithFirstPart + Resources.Common_CommaButFoundXFormat, Subject);
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
            if (expectedSuperset is null)
            {
                throw new ArgumentNullException(nameof(expectedSuperset), Resources.Collection_CannotVerifySubsetAgainstNull);
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_ExpectedCollectionToBeSubsetOfXFormat + Resources.Common_CommaButFoundYFormat, expectedSuperset,
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
                        Resources.Collection_ExpectedCollectionToBeSubsetOfXFormat + Resources.Collection_CommaButItemsYAreNotPartOfSupersetFormat,
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
                .FailWith(Resources.Collection_CannotAssertNullAgainstSubset);

            if (ReferenceEquals(Subject, unexpectedSuperset))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_DidNotExpectCollectionXToBeSubsetOfYFormat + Resources.Common_CommaButBothReferenceSameObject,
                        Subject,
                        unexpectedSuperset);
            }

            IEnumerable<object> expectedItems = unexpectedSuperset.Cast<object>();
            ICollection<object> actualItems = Subject.ConvertOrCastToCollection<object>();

            if (actualItems.Intersect(expectedItems).Count() == actualItems.Count)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_DidNotExpectCollectionXToBeSubsetOfYFormat, actualItems, expectedItems);
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
            if (otherCollection is null)
            {
                throw new ArgumentNullException(nameof(otherCollection), Resources.Collection_CannotVerifyCountAgainstNull);
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_ExpectedCollectionToHaveSameCountAsXFormat + Resources.Common_CommaButFoundYFormat,
                        otherCollection,
                        Subject);
            }

            IEnumerable<object> enumerable = Subject.Cast<object>();

            int actualCount = enumerable.Count();
            int expectedCount = otherCollection.Cast<object>().Count();

            Execute.Assertion
                .ForCondition(actualCount == expectedCount)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Collection_ExpectedCollectionToHaveXItemsFormat + Resources.Common_CommaButFoundYFormat, expectedCount, actualCount);

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
            if (otherCollection is null)
            {
                throw new ArgumentNullException(nameof(otherCollection), Resources.Collection_CannotVerifyCountAgainstNull);
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_ExpectedCollectionToNotHaveSameCountAsXFormat,
                        otherCollection,
                        Subject);
            }

            if (ReferenceEquals(Subject, otherCollection))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_ExpectedCollectionXToNotHaveSameCountAsYFormat + Resources.Common_CommaButBothReferenceSameObject,
                        Subject,
                        otherCollection);
            }

            IEnumerable<object> enumerable = Subject.Cast<object>();

            int actualCount = enumerable.Count();
            int expectedCount = otherCollection.Cast<object>().Count();

            Execute.Assertion
                .ForCondition(actualCount != expectedCount)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Collection_ExpectedCollectionToNotHaveXItemsFormat + Resources.Common_CommaButFoundYFormat, expectedCount, actualCount);

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
                    .FailWith(Resources.Collection_ExpectedCollectionToHaveElementAtIndexXFormat + Resources.Common_CommaButFoundYFormat, index, Subject);
            }

            IEnumerable<object> enumerable = Subject.Cast<object>();

            object actual = null;
            if (index < enumerable.Count())
            {
                actual = Subject.Cast<object>().ElementAt(index);

                Execute.Assertion
                    .ForCondition(actual.IsSameOrEqualTo(element))
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_ExpectedXAtIndexYFormat + Resources.Common_CommaButFoundZFormat, element, index, actual);
            }
            else
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_ExpectedXAtIndexYButFoundNoElementFormat, element, index);
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
            if (unexpected is null)
            {
                throw new ArgumentNullException(nameof(unexpected), Resources.Collection_CannotVerifyNonContainmentAgainstNull);
            }

            ICollection<object> unexpectedObjects = unexpected.ConvertOrCastToCollection<object>();
            if (!unexpectedObjects.Any())
            {
                throw new ArgumentException(Resources.Collection_CannotVerifyNonContainmentAgainstEmptyCollection,
                    nameof(unexpected));
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_ExpectedCollectionToNotXContainFormat + Resources.Common_CommaButFoundNull, unexpected);
            }

            if (unexpected is string)
            {
                if (Subject.Cast<object>().Contains(unexpected))
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith(Resources.Collection_ExpectedCollectionXToNotContainYDotFormat, Subject, unexpected);
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
                            .FailWith(Resources.Collection_ExpectedCollectionXToNotContainYFormat + Resources.Common_CommaButFoundZFormat, Subject,
                                unexpected, foundItems);
                    }
                    else
                    {
                        Execute.Assertion
                            .BecauseOf(because, becauseArgs)
                            .FailWith(Resources.Collection_ExpectedCollectionXToNotContainElementYFormat, Subject,
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
            if (otherCollection is null)
            {
                throw new ArgumentNullException(nameof(otherCollection), Resources.Collection_CannotVerifyIntersectionAgainstNull);
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_ExpectedCollectionToIntersectWithXFormat + Resources.Common_CommaButFoundYFormat, otherCollection,
                        Subject);
            }

            IEnumerable<object> otherItems = otherCollection.Cast<object>();
            IEnumerable<object> sharedItems = Subject.Cast<object>().Intersect(otherItems);

            if (!sharedItems.Any())
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        Resources.Collection_ExpectedCollectionToIntersectWithXButYDoesNotContainSharedItemsFormat,
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
            if (otherCollection is null)
            {
                throw new ArgumentNullException(nameof(otherCollection), Resources.Collection_CannotVerifyIntersectionAgainstNull);
            }

            if (ReferenceEquals(Subject, null))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_DidNotExpectCollectionToIntersectWithXFormat + Resources.Common_CommaButFoundYFormat, otherCollection,
                        Subject);
            }

            if (ReferenceEquals(Subject, otherCollection))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(Resources.Collection_DidNotExpectCollectionXToIntersectWithYFormat + Resources.Common_CommaButBothReferenceSameObject,
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
                        Resources.Collection_DidNotExpectCollectionToIntersectWithXButFoundSharedItemsYFormat,
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
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation(Resources.Collection_ExpectedCollectionToStartWithXFormat, expected)
                .Given(() => actualItems)
                .AssertCollectionIsNotNull()
                .Then
                .AssertCollectionHasEnoughItems(expected.Length)
                .Then
                .AssertCollectionsHaveSameItems(expected, (a, e) => a.Take(e.Count).IndexOfFirstDifferenceWith(e, equalityComparison));
        }

        protected void AssertCollectionStartsWith<TActual, TExpected>(IEnumerable<TActual> actualItems, ICollection<TExpected> expected, Func<TActual, TExpected, bool> equalityComparison, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation(Resources.Collection_ExpectedCollectionToStartWithXFormat, expected)
                .Given(() => actualItems)
                .AssertCollectionIsNotNull()
                .Then
                .AssertCollectionHasEnoughItems(expected.Count)
                .Then
                .AssertCollectionsHaveSameItems(expected, (a, e) => a.Take(e.Count).IndexOfFirstDifferenceWith(e, equalityComparison));
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
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation(Resources.Collection_ExpectedCollectionToEndWithXFormat, expected)
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
                });
        }

        protected void AssertCollectionEndsWith<TActual, TExpected>(IEnumerable<TActual> actual, ICollection<TExpected> expected, Func<TActual, TExpected, bool> equalityComparison, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation(Resources.Collection_ExpectedCollectionToEndWithXFormat, expected)
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
                });
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
                .WithExpectation(Resources.Collection_ExpectedCollectionToHaveXPrecedeYFormat, expectation, successor)
                .Given(() => Subject.Cast<object>())
                .ForCondition(subject => subject.Any())
                .FailWith(Resources.Collection_ButTheCollectionIsEmpty)
                .Then
                .ForCondition(subject => HasPredecessor(successor, subject))
                .FailWith(Resources.Common_ButFoundNothing)
                .Then
                .Given(subject => PredecessorOf(successor, subject))
                .ForCondition(predecessor => predecessor.IsSameOrEqualTo(expectation))
                .FailWith(Resources.Common_ButFoundXFormat, predecessor => predecessor)
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
                .WithExpectation(Resources.Collection_ExpectedCollectionToHaveXSucceedYFormat, expectation, predecessor)
                .Given(() => Subject.Cast<object>())
                .ForCondition(subject => subject.Any())
                .FailWith(Resources.Collection_ButTheCollectionIsEmpty)
                .Then
                .ForCondition(subject => HasSuccessor(predecessor, subject))
                .FailWith(Resources.Common_ButFoundNothing)
                .Then
                .Given(subject => SuccessorOf(predecessor, subject))
                .ForCondition(successor => successor.IsSameOrEqualTo(expectation))
                .FailWith(Resources.Common_ButFoundXFormat, successor => successor);

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
                .WithExpectation(Resources.Collection_ExpectedTypeToBeXFormat, expectedType.FullName)
                .Given(() => Subject.Cast<object>())
                .ForCondition(subject => subject.All(x => x != null))
                .FailWith(Resources.Common_ButFoundANullElement)
                .Then
                .ForCondition(subject => subject.All(x => expectedType.GetTypeInfo().IsAssignableFrom(GetType(x).GetTypeInfo())))
                .FailWith(Resources.Common_ButFoundXFormat, subject => $"[{string.Join(", ", subject.Select(x => GetType(x).FullName))}]");

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
                .WithExpectation(Resources.Collection_ExpectedTypeToBeXFormat, expectedType.FullName)
                .Given(() => Subject.Cast<object>())
                .ForCondition(subject => subject.All(x => x != null))
                .FailWith(Resources.Common_ButFoundANullElement)
                .Then
                .ForCondition(subject => subject.All(x => expectedType == GetType(x)))
                .FailWith(Resources.Common_ButFoundXFormat, subject => $"[{string.Join(", ", subject.Select(x => GetType(x).FullName))}]");

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
