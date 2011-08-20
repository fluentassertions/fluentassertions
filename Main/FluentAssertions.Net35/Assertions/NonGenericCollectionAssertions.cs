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

        public AndConstraint<NonGenericCollectionAssertions> Contain(object expected)
        {
            return Contain(expected, string.Empty);
        }

        public AndConstraint<NonGenericCollectionAssertions> Contain(object expected, string reason,
                                                           params object[] reasonArgs)
        {
            if (expected is IEnumerable)
            {
                return base.Contain((IEnumerable)expected , reason, reasonArgs);
            } 
                
            return base.Contain(new[] { expected }, reason, reasonArgs);
        }
    }
}