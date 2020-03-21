using System;
using System.Collections.Generic;
using FluentAssertions.Collections;

namespace FluentAssertions.Specialized
{
#if NETSTANDARD2_1||NETCOREAPP3_0

    public class SpanAssertions<T> : SelfReferencingCollectionAssertions<T, SpanAssertions<T>>
    {
        public SpanAssertions(IEnumerable<T> subject) : base(subject)
        {
        }
    }
#endif
}
