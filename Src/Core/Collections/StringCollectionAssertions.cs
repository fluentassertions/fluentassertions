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
        public new AndConstraint<StringCollectionAssertions> Equal(params string[] expected)
        {
            return base.Equal(expected.AsEnumerable());
        }

        /// <summary>
        /// Expects the current collection to contain all the same elements in the same order as the collection identified by 
        /// <paramref name="expected" />. Elements are compared using their <see cref="object.Equals(object)" />.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable"/> with the expected elements.</param>
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
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringCollectionAssertions> BeEquivalentTo(IEnumerable<string> expected, string because = "",
            params object[] becauseArgs)
        {
            return base.BeEquivalentTo(expected, because, becauseArgs);
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
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringCollectionAssertions> ContainInOrder(IEnumerable<string> expected, string because = "",
            params object[] becauseArgs)
        {
            return base.ContainInOrder(expected, because, becauseArgs);
        }

        /// <summary>
        /// Expects the current collection to contain the specified elements in any order. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable"/> with the expected elements.</param>
        public AndConstraint<StringCollectionAssertions> Contain(IEnumerable<string> expected)
        {
            return base.Contain(expected);
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
        public AndConstraint<StringCollectionAssertions> Contain(IEnumerable<string> expected, string because = null,
            object becauseArg = null,
            params object[] becauseArgs)
        {
            var args = new List<object> {becauseArg};
            args.AddRange(becauseArgs);
            return base.Contain(expected, because, args.ToArray());
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
        public AndConstraint<StringCollectionAssertions> NotContain(IEnumerable<string> unexpected, string because = null,
            object becauseArg = null,
            params object[] becauseArgs)
        {
            var args = new List<object> { becauseArg };
            args.AddRange(becauseArgs);
            return base.NotContain(unexpected, because, args.ToArray());
        }
    }
}