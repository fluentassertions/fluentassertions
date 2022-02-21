using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using FluentAssertions.Collections;
using FluentAssertions.Common;
using FluentAssertions.Data;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;

namespace FluentAssertions
{
    public static class DataRowCollectionAssertionExtensions
    {
        /// <summary>
        /// Asserts that an object reference refers to the exact same object as another object reference.
        /// </summary>
        /// <param name="expected">The expected object</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataRow>> BeSameAs(this GenericCollectionAssertions<DataRow> assertion, DataRowCollection expected, string because = "", params object[] becauseArgs)
        {
            if (assertion.Subject is NonGenericCollectionWrapper<DataRowCollection, DataRow> wrapper)
            {
                var actualSubject = wrapper.UnderlyingCollection;

                Execute.Assertion
                    .UsingLineBreaks
                    .ForCondition(ReferenceEquals(actualSubject, expected))
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:row collection} to refer to {0}{reason}, but found {1} (different underlying object).", expected, actualSubject);
            }
            else
            {
                Execute.Assertion
                    .UsingLineBreaks
                    .ForCondition(false)
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:row collection} to refer to DataRowCollection{reason}, but found {0} (different type).", expected);
            }

            return new AndConstraint<GenericCollectionAssertions<DataRow>>(assertion);
        }

        /// <summary>
        /// Asserts that an object reference refers to a different object than another object reference refers to.
        /// </summary>
        /// <param name="unexpected">The unexpected object</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataRow>> NotBeSameAs(this GenericCollectionAssertions<DataRow> assertion, DataRowCollection unexpected, string because = "", params object[] becauseArgs)
        {
            if (assertion.Subject is NonGenericCollectionWrapper<DataRowCollection, DataRow> wrapper)
            {
                var actualSubject = wrapper.UnderlyingCollection;

                Execute.Assertion
                    .UsingLineBreaks
                    .ForCondition(!ReferenceEquals(actualSubject, unexpected))
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Did not expect {context:row collection} to refer to {0}{reason}.", unexpected);
            }

            return new AndConstraint<GenericCollectionAssertions<DataRow>>(assertion);
        }

        /// <summary>
        /// Assert that the current collection of <see cref="DataRow"/>s has the same number of rows as <paramref name="otherCollection" />.
        /// </summary>
        /// <param name="otherCollection">The other collection with the same expected number of elements</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataRow>> HaveSameCount(this GenericCollectionAssertions<DataRow> assertion, DataRowCollection otherCollection, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(otherCollection, nameof(otherCollection), "Cannot verify count against a <null> collection.");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:collection} to have ")
                .Given(() => assertion.Subject)
                .ForCondition(subject => subject is not null)
                .FailWith("the same count as {0}{reason}, but found <null>.", otherCollection)
                .Then
                .Given((subject) => (actual: subject.Count(), expected: otherCollection.Count))
                .ForCondition(count => count.actual == count.expected)
                .FailWith("{0} row(s){reason}, but found {1}.", count => count.expected, count => count.actual)
                .Then
                .ClearExpectation();

            return new AndConstraint<GenericCollectionAssertions<DataRow>>(assertion);
        }

        /// <summary>
        /// Assert that the current collection of <see cref="DataRow"/>s does not have the same number of rows as <paramref name="otherCollection" />.
        /// </summary>
        /// <param name="otherCollection">The other <see cref="DataRowCollection"/> with the unexpected number of elements</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataRow>> NotHaveSameCount(this GenericCollectionAssertions<DataRow> assertion, DataRowCollection otherCollection, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(otherCollection, nameof(otherCollection), "Cannot verify count against a <null> collection.");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected {context:collection} to not have ")
                .Given(() => assertion.Subject)
                .ForCondition(subject => subject is not null)
                .FailWith("the same count as {0}{reason}, but found <null>.", otherCollection)
                .Then
                .Given((subject) => (actual: subject.Count(), expected: otherCollection.Count))
                .ForCondition(count => count.actual != count.expected)
                .FailWith("{0} row(s){reason}, but found {1}.", count => count.expected, count => count.actual)
                .Then
                .ClearExpectation();

            return new AndConstraint<GenericCollectionAssertions<DataRow>>(assertion);
        }

        /// <summary>
        /// Asserts that a <see cref="DataRowCollection"/> contains at least one <see cref="DataRow"/> equivalent to another <see cref="DataRow"/>.
        /// </summary>
        /// <remarks>
        /// See the documentation for <see cref="DataRowAssertions{TDataRow}"/> for information about when <see cref="DataRow"/>
        /// objects are considered equivalent.
        /// </remarks>
        /// <param name="expected">The expected <see cref="DataRow"/>.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataRow>> ContainEquivalentOf(this GenericCollectionAssertions<DataRow> assertion, DataRow expected, string because = "",
            params object[] becauseArgs)
        {
            return ContainEquivalentOf(assertion, expected, config => config, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that a <see cref="DataRowCollection"/> contains at least one object equivalent to another object.
        /// </summary>
        /// <remarks>
        /// See the documentation for <see cref="DataRowAssertions{TDataRow}"/> for information about when <see cref="DataRow"/>
        /// objects are considered equivalent.
        /// </remarks>
        /// <param name="expected">The expected <see cref="DataRow"/>.</param>
        /// <param name="config">
        /// A reference to the <see cref="EquivalencyAssertionOptions{DataRow}"/> configuration object that can be used
        /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
        /// <see cref="EquivalencyAssertionOptions{DataRow}"/> class. The global defaults are determined by the
        /// <see cref="AssertionOptions"/> class.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataRow>> ContainEquivalentOf(this GenericCollectionAssertions<DataRow> assertion, DataRow expected, Func<EquivalencyAssertionOptions<DataRow>,
                EquivalencyAssertionOptions<DataRow>> config, string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(config, nameof(config));

            if (assertion.Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to contain equivalent of {0}{reason}, but found <null>.", expected);
            }

            EquivalencyAssertionOptions<DataRow> options = config(AssertionOptions.CloneDefaults<DataRow>());
            IEnumerable<object> actualItems = assertion.Subject.Cast<object>();

            using (var scope = new AssertionScope())
            {
                scope.AddReportable("configuration", options.ToString());

                foreach (object actualItem in actualItems)
                {
                    var context = new EquivalencyValidationContext(Node.From<DataRow>(() => CallerIdentifier.DetermineCallerIdentity()), options)
                    {
                        Reason = new Reason(because, becauseArgs),
                        TraceWriter = options.TraceWriter
                    };

                    var comparands = new Comparands
                    {
                        Subject = actualItem,
                        Expectation = expected,
                        CompileTimeType = typeof(DataRow),
                    };

                    new EquivalencyValidator().AssertEquality(comparands, context);

                    string[] failures = scope.Discard();

                    if (!failures.Any())
                    {
                        return new AndConstraint<GenericCollectionAssertions<DataRow>>(assertion);
                    }
                }

                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} {0} to contain equivalent of {1}{reason}.", assertion.Subject, expected);
            }

            return new AndConstraint<GenericCollectionAssertions<DataRow>>(assertion);
        }

        /// <summary>
        /// Asserts that a <see cref="DataRowCollection"/> does not contain any table equivalent to supplied <see cref="DataRow"/>.
        /// </summary>
        /// <remarks>
        /// See the documentation for <see cref="DataRowAssertions{TDataRow}"/> for information about when <see cref="DataRow"/>
        /// objects are considered equivalent.
        /// </remarks>
        /// <param name="unexpected">The unexpected <see cref="DataRow"/>.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataRow>> NotContainEquivalentOf(this GenericCollectionAssertions<DataRow> assertion, DataRow unexpected, string because = "",
            params object[] becauseArgs)
        {
            return NotContainEquivalentOf(assertion, unexpected, config => config, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that a <see cref="DataRowCollection"/> does not contain any table equivalent to supplied <see cref="DataRow"/>.
        /// </summary>
        /// <remarks>
        /// See the documentation for <see cref="DataRowAssertions{TDataRow}"/> for information about when <see cref="DataRow"/>
        /// objects are considered equivalent.
        /// </remarks>
        /// <param name="unexpected">The unexpected <see cref="DataRow"/>.</param>
        /// <param name="config">
        /// A reference to the <see cref="EquivalencyAssertionOptions{DataRow}"/> configuration object that can be used
        /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
        /// <see cref="EquivalencyAssertionOptions{DataRow}"/> class. The global defaults are determined by the
        /// <see cref="AssertionOptions"/> class.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataRow>> NotContainEquivalentOf(this GenericCollectionAssertions<DataRow> assertion, DataRow unexpected, Func<EquivalencyAssertionOptions<DataRow>,
            EquivalencyAssertionOptions<DataRow>> config, string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(config, nameof(config));

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(assertion.Subject is not null)
                .FailWith("Expected {context:collection} not to contain equivalent of {0}{reason}, but collection is <null>.", unexpected);

            EquivalencyAssertionOptions<DataRow> options = config(AssertionOptions.CloneDefaults<DataRow>());

            var foundIndices = new List<int>();
            using (var scope = new AssertionScope())
            {
                int index = 0;
                foreach (object actualItem in assertion.Subject)
                {
                    var context = new EquivalencyValidationContext(Node.From<DataRow>(CallerIdentifier.DetermineCallerIdentity), options)
                    {
                        Reason = new Reason(because, becauseArgs),
                        TraceWriter = options.TraceWriter
                    };

                    var comparands = new Comparands
                    {
                        Subject = actualItem,
                        Expectation = unexpected,
                        CompileTimeType = typeof(DataRow),
                    };

                    new EquivalencyValidator().AssertEquality(comparands, context);

                    string[] failures = scope.Discard();

                    if (!failures.Any())
                    {
                        foundIndices.Add(index);
                    }

                    index++;
                }
            }

            if (foundIndices.Count > 0)
            {
                using (new AssertionScope())
                {
                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .WithExpectation("Expected {context:collection} {0} not to contain equivalent of {1}{reason}, ", assertion.Subject, unexpected)
                        .AddReportable("configuration", options.ToString());

                    if (foundIndices.Count == 1)
                    {
                        Execute.Assertion
                            .FailWith("but found one at index {0}.", foundIndices[0]);
                    }
                    else
                    {
                        Execute.Assertion
                            .FailWith("but found several at indices {0}.", foundIndices);
                    }
                }
            }

            return new AndConstraint<GenericCollectionAssertions<DataRow>>(assertion);
        }
    }
}
