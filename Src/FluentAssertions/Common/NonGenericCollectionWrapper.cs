using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Common
{
    internal class NonGenericCollectionWrapper<TCollection, TItem> : ICollection<TItem>, IEnumerable<TItem>
        where TCollection : ICollection, IEnumerable
    {
        public TCollection UnderlyingCollection { get; private set; }

        public NonGenericCollectionWrapper(TCollection collection)
        {
            UnderlyingCollection = collection;
        }

        public int Count => UnderlyingCollection.Count;

        public bool IsReadOnly => true;

        public IEnumerator<TItem> GetEnumerator() => UnderlyingCollection.Cast<TItem>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => UnderlyingCollection.GetEnumerator();

        public bool Contains(TItem item) => UnderlyingCollection.Cast<TItem>().Contains(item);

        public void CopyTo(TItem[] array, int arrayIndex) => UnderlyingCollection.CopyTo(array, arrayIndex);

        /*

        Mutation is not supported, but these methods must be implemented to satisfy ICollection<T>:
        * Add
        * Clear
        * Remove

        */

        public void Add(TItem item) => throw new NotSupportedException();

        public void Clear() => throw new NotSupportedException();

        public bool Remove(TItem item) => throw new NotSupportedException();
    }
}
