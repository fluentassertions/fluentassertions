using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Collections
{
    public class StringCollectionAssertions :
        SelfReferencingCollectionAssertions<string, StringCollectionAssertions>
    {
        public StringCollectionAssertions(IEnumerable<string> actualValue)
            : base(actualValue)
        {
        }

        /// <summary>
        /// Expects the current collection to contain all the same elements in the same order as the collection identified by 
        /// <paramref name="expected" />. Elements are compared using their <see cref="object.Equals(object)" />.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable"/> with the expected elements.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<StringCollectionAssertions> Equal(params string[] expected)
        {
            return base.Equal(expected.AsEnumerable());
        }

        /// <summary>
        /// Expects the current collection to contain all the same elements in the same order as the collection identified by 
        /// <paramref name="expected" />. Elements are compared using their <see cref="object.Equals(object)" />.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable"/> with the expected elements.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<StringCollectionAssertions> Equal(IEnumerable<string> expected)
        {
            return base.Equal(expected);
        }

        /// <summary>
        /// Expects the current collection to contain all elements of the collection identified by <paramref name="elements" />,
        /// regardless of the order. Elements are compared using their <see cref="object.Equals(object)" />.
        /// </summary>
        /// <param name="elements">A params array with the expected elements.</param>
        public AndConstraint<StringCollectionAssertions> BeEquivalentTo(params string[] elements)
        {
            return base.BeEquivalentTo(elements.AsEnumerable());
        }

        /// <summary>
        /// Expects the current collection to contain all elements of the collection identified by <paramref name="expected" />,
        /// regardless of the order. Elements are compared using their <see cref="object.Equals(object)" />.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable"/> with the expected elements.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<StringCollectionAssertions> BeEquivalentTo(IEnumerable<string> expected, string reason = "",
            params object[] reasonArgs)
        {
            return base.BeEquivalentTo(expected, reason, reasonArgs);
        }

        /// <summary>
        /// Expects the current collection to contain the specified elements in the exact same order. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable"/> with the expected elements.</param>
        public AndConstraint<StringCollectionAssertions> ContainInOrder(params string[] expected)
        {
            return base.ContainInOrder(expected.AsEnumerable());
        }

        /// <summary>
        /// Expects the current collection to contain the specified elements in the exact same order. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable"/> with the expected elements.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<StringCollectionAssertions> ContainInOrder(IEnumerable<string> expected, string reason = "",
            params object[] reasonArgs)
        {
            return base.ContainInOrder(expected, reason, reasonArgs);
        }

        /// <summary>
        /// Expects the current collection to contain the specified elements in any order. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable"/> with the expected elements.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<StringCollectionAssertions> Contain(IEnumerable<string> expected)
        {
            return base.Contain(expected);
        }

        /// <summary>
        /// Expects the current collection to contain the specified elements in any order. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable"/> with the expected elements.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<StringCollectionAssertions> Contain(IEnumerable<string> expected, string reason, object reasonArg,
            params object[] reasonArgs)
        {
            var args = new List<object> { reasonArg };
            args.AddRange(reasonArgs);
            return base.Contain(expected, reason, args.ToArray());
        }

        /// <summary>
        /// Asserts that the collection contains some extra items in addition to the original items.
        /// </summary>
        /// <param name="expectedItemsList">An <see cref="IEnumerable{T}"/> of expectation items.</param>
        /// <param name="additionalExpectedItems">Additional items that are expectation to be contained by the collection.</param>
        public AndConstraint<StringCollectionAssertions> Contain(IEnumerable<string> expectedItemsList,
            string additionalExpectedItem)
        {
            var list = new List<string>(expectedItemsList);
            list.Add(additionalExpectedItem);

            return base.Contain(list);
        }

        /// <summary>
        /// Asserts that the collection contains some extra items in addition to the original items.
        /// </summary>
        /// <param name="expectedItemsList">An <see cref="IEnumerable{T}"/> of expectation items.</param>
        /// <param name="additionalExpectedItems">Additional items that are expectation to be contained by the collection.</param>
        public AndConstraint<StringCollectionAssertions> Contain(IEnumerable<string> expectedItemsList,
            IEnumerable<string> additionalExpectedItems)
        {
            var list = new List<string>(expectedItemsList);
            list.AddRange(additionalExpectedItems);

            return base.Contain(list);
        }
    }
}