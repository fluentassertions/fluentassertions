using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FluentAssertionsAsync.Collections;

[DebuggerNonUserCode]
public class SubsequentOrderingAssertions<T>
    : SubsequentOrderingGenericCollectionAssertions<IEnumerable<T>, T, SubsequentOrderingAssertions<T>>
{
    public SubsequentOrderingAssertions(IEnumerable<T> actualValue, IOrderedEnumerable<T> previousOrderedEnumerable)
        : base(actualValue, previousOrderedEnumerable)
    {
    }
}
