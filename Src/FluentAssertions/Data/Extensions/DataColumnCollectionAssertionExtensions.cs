using System.Data;
using System.Linq;

using FluentAssertions.Collections;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions
{
    public static class DataColumnCollectionAssertionExtensions
    {
        /// <summary>
        /// Assert that the current collection has the same number of elements as <paramref name="otherCollection" />.
        /// </summary>
        /// <param name="otherCollection">The other collection with the same expected number of elements</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataColumn>> HaveSameCount(this GenericCollectionAssertions<DataColumn> assertion, DataColumnCollection otherCollection, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(otherCollection, nameof(otherCollection), "Cannot verify count against a <null> collection.");

            if (assertion.Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to have the same count as {0}{reason}, but found {1}.",
                        otherCollection,
                        assertion.Subject);
            }

            int actualCount = assertion.Subject.Count();
            int expectedCount = otherCollection.Count;

            Execute.Assertion
                .ForCondition(actualCount == expectedCount)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} to have {0} column(s){reason}, but found {1}.", expectedCount, actualCount);

            return new AndConstraint<GenericCollectionAssertions<DataColumn>>(assertion);
        }

        /// <summary>
        /// Assert that the current <see cref="DataColumnCollection"/> does not have the same number of columns as <paramref name="otherCollection" />.
        /// </summary>
        /// <param name="otherCollection">The other <see cref="DataColumnCollection"/> with the unexpected number of elements</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataColumn>> NotHaveSameCount(this GenericCollectionAssertions<DataColumn> assertion, DataColumnCollection otherCollection, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(otherCollection, nameof(otherCollection), "Cannot verify count against a <null> collection.");

            if (assertion.Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} to not have the same count as {0}{reason}, but found {1}.",
                        otherCollection,
                        assertion.Subject);
            }

            if (ReferenceEquals(assertion.Subject, otherCollection))
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:collection} {0} to not have the same count as {1}{reason}, but they both reference the same object.",
                        assertion.Subject,
                        otherCollection);
            }

            int actualCount = assertion.Subject.Count();
            int expectedCount = otherCollection.Count;

            Execute.Assertion
                .ForCondition(actualCount != expectedCount)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:collection} to not have {0} column(s){reason}, but found {1}.", expectedCount, actualCount);

            return new AndConstraint<GenericCollectionAssertions<DataColumn>>(assertion);
        }

        /// <summary>
        /// Asserts that the current <see cref="DataColumnCollection"/> contains a <see cref="DataColumn"/> with the specified <paramref name="expectedColumnName"/> name.
        /// </summary>
        /// <param name="expectedColumnName">A name for a <see cref="DataColumn"/> that is expected to be in the <see cref="DataColumnCollection"/>.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataColumn>> ContainColumnWithName(this GenericCollectionAssertions<DataColumn> assertion, string expectedColumnName, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(expectedColumnName, nameof(expectedColumnName), "Cannot verify that the collection contains a <null> DataColumn.");

            if (assertion.Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expeected {context:collection} to contain column named {0}{reason}, but found {1}.", expectedColumnName,
                        assertion.Subject);
            }

            bool containsColumn;

            if (assertion.Subject is NonGenericCollectionWrapper<DataColumnCollection, DataColumn> wrapper)
            {
                containsColumn = wrapper.UnderlyingCollection.Contains(expectedColumnName);
            }
            else
            {
                containsColumn = assertion.Subject.Any(column => column.ColumnName == expectedColumnName);
            }

            if (!containsColumn)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Expected {context:collection} to contain column named {0}{reason}, but it does not.",
                        expectedColumnName);
            }

            return new AndConstraint<GenericCollectionAssertions<DataColumn>>(assertion);
        }

        /// <summary>
        /// Asserts that the current <see cref="DataColumnCollection"/> does not contain a column with the the supplied <paramref name="unexpectedColumnName" />.
        /// </summary>
        /// <param name="unexpectedColumnName">The column name that is not expected to be in the <see cref="DataColumnCollection"/></param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<DataColumn>> NotContainColumnWithName(this GenericCollectionAssertions<DataColumn> assertion, string unexpectedColumnName, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(unexpectedColumnName, nameof(unexpectedColumnName), "Cannot verify that the collection does not contain a <null> DataColumn.");

            if (assertion.Subject is null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expeected {context:collection} to not contain column named {0}{reason}, but found {1}.", unexpectedColumnName,
                        assertion.Subject);
            }

            bool containsColumn;

            if (assertion.Subject is NonGenericCollectionWrapper<DataColumnCollection, DataColumn> wrapper)
            {
                containsColumn = wrapper.UnderlyingCollection.Contains(unexpectedColumnName);
            }
            else
            {
                containsColumn = assertion.Subject.Any(column => column.ColumnName == unexpectedColumnName);
            }

            if (containsColumn)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Expected {context:collection} to not contain column named {0}{reason}, but it does.",
                        unexpectedColumnName);
            }

            return new AndConstraint<GenericCollectionAssertions<DataColumn>>(assertion);
        }
    }
}
