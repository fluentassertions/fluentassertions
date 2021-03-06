using System;
using System.Collections.Generic;
using System.Text;

namespace FluentAssertions.Collections.MaximumMatching
{
    internal class IndexedElement<TElement>
    {
        public IndexedElement(int index, TElement value)
        {
            Index = index;
            Value = value;
        }

        public int Index { get; }

        public TElement Value { get; }

    }
}
