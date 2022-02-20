using System.Collections.Generic;
using System.Data;
using System.Linq;

using FluentAssertions.Collections;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions
{
    public static class DataTableCollectionAssertionExtensions
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
        public static AndConstraint<GenericCollectionAssertions<DataTable>> BeSameAs(this GenericCollectionAssertions<DataTable> assertion, DataTableCollection expected, string because = "", params object[] becauseArgs)
        {
            if (assertion.Subject is NonGenericCollectionWrapper<DataTableCollection, DataTable> wrapper)
            {
                var actualSubject = wrapper.UnderlyingCollection;

                Execute.Assertion
                    .UsingLineBreaks
                    .ForCondition(ReferenceEquals(actualSubject, expected))
                    .BecauseOf(because, becauseArgs)
                    .WithDefaultIdentifier("collection")
                    .FailWith("Expected {context} to refer to {0}{reason}, but found {1}.", expected, actualSubject);
            }
            else
            {
                Execute.Assertion
                    .UsingLineBreaks
                    .ForCondition(false)
                    .BecauseOf(because, becauseArgs)
                    .WithDefaultIdentifier("collection")
                    .FailWith("Expected {context} to refer to DataTableCollection{reason}, but found {1}.", expected);
            }

            return new AndConstraint<GenericCollectionAssertions<DataTable>>(assertion);
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
        public static AndConstraint<GenericCollectionAssertions<DataTable>> NotBeSameAs(this GenericCollectionAssertions<DataTable> assertion, DataTableCollection unexpected, string because = "", params object[] becauseArgs)
        {
            if (assertion.Subject is NonGenericCollectionWrapper<DataTableCollection, DataTable> wrapper)
            {
                var actualSubject = wrapper.UnderlyingCollection;

                Execute.Assertion
                    .UsingLineBreaks
                    .ForCondition(!ReferenceEquals(actualSubject, unexpected))
                    .BecauseOf(because, becauseArgs)
                    .WithDefaultIdentifier("collection")
                    .FailWith("Did not expect {context} to refer to {0}{reason}.", unexpected);
            }

            return new AndConstraint<GenericCollectionAssertions<DataTable>>(assertion);
        }

        /// <summary>
        /// Assert that the current <see cref="DataTableCollection"/> has the same number of tables as <paramref name="otherDataSet" />.
        /// </summary>
        /// <param name="otherDataSet">The other <see cref="DataSet"/> with the same expected number of tables</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataTable>> HaveSameCount(this GenericCollectionAssertions<DataTable> assertion, DataSet otherDataSet, string because = "",
            params object[] becauseArgs)
        {
            return assertion.HaveSameCount(otherDataSet.Tables, because, becauseArgs);
        }

        /// <summary>
        /// Assert that the current <see cref="DataTableCollection"/> does not have the same number of tables as <paramref name="otherDataSet" />.
        /// </summary>
        /// <param name="otherDataSet">The other <see cref="DataSet"/> with the unexpected number of tables</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataTable>> NotHaveSameCount(this GenericCollectionAssertions<DataTable> assertion, DataSet otherDataSet, string because = "",
            params object[] becauseArgs)
        {
            return assertion.NotHaveSameCount(otherDataSet.Tables, because, becauseArgs);
        }

        /// <summary>
        /// Assert that the current <see cref="DataTableCollection"/> has the same number of tables as <paramref name="otherCollection" />.
        /// </summary>
        /// <param name="otherCollection">The other <see cref="DataSet"/> with the same expected number of tables</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataTable>> HaveSameCount(this GenericCollectionAssertions<DataTable> assertion, DataTableCollection otherCollection, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(otherCollection, nameof(otherCollection), "Cannot verify count against a <null> collection.");

            if (assertion.Subject is ICollection<DataTable> collection)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .WithExpectation("Expected {context:collection} to have ")
                    .Given(() => (actual: collection.Count, expected: otherCollection.Count))
                    .ForCondition(count => count.actual == count.expected)
                    .FailWith("{0} table(s){reason}, but found {1} table(s).", count => count.expected, count => count.actual)
                    .Then
                    .ClearExpectation();
            }
            else
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .WithExpectation("Expected {context:collection} to have ")
                    .Given(() => assertion.Subject)
                    .ForCondition(subject => subject is not null)
                    .FailWith("the same count as {0}{reason}, but found <null>.", otherCollection)
                    .Then
                    .Given((subject) => (actual: subject.Count(), expected: otherCollection.Count))
                    .ForCondition(count => count.actual == count.expected)
                    .FailWith("{0} table(s){reason}, but found {1}.", count => count.expected, count => count.actual)
                    .Then
                    .ClearExpectation();
            }

            return new AndConstraint<GenericCollectionAssertions<DataTable>>(assertion);
        }

        /// <summary>
        /// Assert that the current <see cref="DataTableCollection"/> does not have the same number of tables as <paramref name="otherCollection" />.
        /// </summary>
        /// <param name="otherCollection">The other <see cref="DataTableCollection"/> with the unexpected number of tables</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataTable>> NotHaveSameCount(this GenericCollectionAssertions<DataTable> assertion, DataTableCollection otherCollection, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(otherCollection, nameof(otherCollection), "Cannot verify count against a <null> collection.");

            if (assertion.Subject is ICollection<DataTable> collection)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .WithExpectation("Expected {context:collection} to not have ")
                    .Given(() => (actual: collection.Count, expected: otherCollection.Count))
                    .ForCondition(count => count.actual != count.expected)
                    .FailWith("{0} table(s){reason}, but found {1} table(s).", count => count.expected, count => count.actual)
                    .Then
                    .ClearExpectation();
            }
            else
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .WithExpectation("Expected {context:collection} to not have ")
                    .Given(() => assertion.Subject)
                    .ForCondition(subject => subject is not null)
                    .FailWith("the same count as {0}{reason}, but found <null>.", otherCollection)
                    .Then
                    .Given((subject) => (actual: subject.Count(), expected: otherCollection.Count))
                    .ForCondition(count => count.actual != count.expected)
                    .FailWith("{0} table(s){reason}, but found {1}.", count => count.expected, count => count.actual)
                    .Then
                    .ClearExpectation();
            }

            return new AndConstraint<GenericCollectionAssertions<DataTable>>(assertion);
        }

        /// <summary>
        /// Asserts that the current <see cref="DataTableCollection"/> contains a <see cref="DataTable"/> with the specified <paramref name="expectedTableName"/> name.
        /// </summary>
        /// <param name="expectedTableName">A name for a <see cref="DataTable"/> that is expected to be in the <see cref="DataTableCollection"/>.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataTable>> ContainTableWithName(this GenericCollectionAssertions<DataTable> assertion, string expectedTableName, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(expectedTableName, nameof(expectedTableName), "Cannot verify that the collection contains a <null> DataTable.");

            if (assertion.Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expeected {context:collection} to contain table named {0}{reason}, but found {1}.", expectedTableName,
                        assertion.Subject);
            }

            bool containsTable;

            if (assertion.Subject is NonGenericCollectionWrapper<DataTableCollection, DataTable> wrapper)
            {
                containsTable = wrapper.UnderlyingCollection.Contains(expectedTableName);
            }
            else
            {
                containsTable = assertion.Subject.Any(table => table.TableName == expectedTableName);
            }

            if (!containsTable)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Expected {context:collection} to contain table named {0}{reason}, but it does not.",
                        expectedTableName);
            }

            return new AndConstraint<GenericCollectionAssertions<DataTable>>(assertion);
        }

        /// <summary>
        /// Asserts that the current <see cref="DataTableCollection"/> does not contain a table with the the supplied <paramref name="unexpectedTableName" />.
        /// </summary>
        /// <param name="unexpectedTableName">The table name that is not expected to be in the <see cref="DataTableCollection"/></param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataTable>> NotContainTableWithName(this GenericCollectionAssertions<DataTable> assertion, string unexpectedTableName, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(unexpectedTableName, nameof(unexpectedTableName), "Cannot verify that the collection does not contain a <null> DataTable.");

            if (assertion.Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expeected {context:collection} to not contain table named {0}{reason}, but found {1}.", unexpectedTableName,
                        assertion.Subject);
            }

            bool containsTable;

            if (assertion.Subject is NonGenericCollectionWrapper<DataTableCollection, DataTable> wrapper)
            {
                containsTable = wrapper.UnderlyingCollection.Contains(unexpectedTableName);
            }
            else
            {
                containsTable = assertion.Subject.Any(table => table.TableName == unexpectedTableName);
            }

            if (containsTable)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Expected {context:collection} to not contain table named {0}{reason}, but it does.",
                        unexpectedTableName);
            }

            return new AndConstraint<GenericCollectionAssertions<DataTable>>(assertion);
        }
    }
}
