using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentAssertions.Execution;

namespace FluentAssertions.Collections;

[DebuggerNonUserCode]
public class SubsequentOrderingAssertions<T>
    : SubsequentOrderingGenericCollectionAssertions<IEnumerable<T>, T, SubsequentOrderingAssertions<T>>
{
    public SubsequentOrderingAssertions(IEnumerable<T> actualValue, IOrderedEnumerable<T> previousOrderedEnumerable, Assertion assertion)
        : base(actualValue, previousOrderedEnumerable, assertion)
    {
    }
}
