using System;
using System.Collections.Generic;
using System.Text;

namespace FluentAssertions.Collections.MaximumMatching
{
    internal class IndexedElement<TElement>
    {
        public IndexedElement(TElement value, int index)
        {
            Index = index;
            Value = value;
        }

        public int Index { get; }

        public TElement Value { get; }

    }
}
