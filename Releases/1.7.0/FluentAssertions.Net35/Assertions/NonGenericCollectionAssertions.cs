using System.Collections;
using System.Diagnostics;

namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="IEnumerable"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class NonGenericCollectionAssertions : CollectionAssertions<IEnumerable, NonGenericCollectionAssertions>
    {
        protected internal NonGenericCollectionAssertions(IEnumerable collection)
        {
            if (collection != null)
            {
                Subject = collection;
            }
        }

        /// <summary>
        /// Asserts that the current collection contains the specified <paramref name="expected"/> object. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">An object, or <see cref="IEnumerable"/> of objects that are expected to be in the collection.</param>
        public AndConstraint<NonGenericCollectionAssertions> Contain(object expected)
        {
            return Contain(expected, string.Empty);
        }

        /// <summary>
        /// Asserts that the current collection contains the specified <paramref name="expected"/> object. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">An object, or <see cref="IEnumerable"/> of objects that are expected to be in the collection.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<NonGenericCollectionAssertions> Contain(object expected, string reason,
            params object [] reasonArgs)
        {
            if (expected is IEnumerable)
            {
                return base.Contain((IEnumerable) expected, reason, reasonArgs);
            }

            return base.Contain(new [] { expected }, reason, reasonArgs);
        }
    }
}