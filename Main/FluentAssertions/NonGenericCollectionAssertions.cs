using System;
using System.Collections;

namespace FluentAssertions
{
    public class NonGenericCollectionAssertions : CollectionAssertions<IEnumerable, NonGenericCollectionAssertions>
    {
        internal NonGenericCollectionAssertions(IEnumerable collection)
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
                                                           params object[] reasonParameters)
        {
            if (expected is IEnumerable)
            {
                return base.Contain((IEnumerable)expected , reason, reasonParameters);
            } 
                
            return base.Contain(new[] { expected }, reason, reasonParameters);
        }
    }
}