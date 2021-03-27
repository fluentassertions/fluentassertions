using System;
using System.Collections;
using System.Collections.Generic;

namespace FluentAssertions.Common
{
    /// <summary>
    /// A smarter enumerator that can provide information about the relative location (current, first, last)
    /// of the current item within the collection without unnecessarily iterating the collection.
    /// </summary>
    internal class Iterator<T> : IEnumerator<T>
    {
        private const int InitialIndex = -1;
        private readonly IEnumerable<T> enumerable;
        private readonly int? maxItems;
        private IEnumerator<T> enumerator;
        private T current;
        private T next;

        private bool hasNext;
        private bool hasCurrent;

        private bool hasCompleted;

        public Iterator(IEnumerable<T> enumerable, int maxItems = int.MaxValue)
        {
            this.enumerable = enumerable;
            this.maxItems = maxItems;

            Reset();
        }

        public void Reset()
        {
            Index = InitialIndex;

            enumerator = enumerable.GetEnumerator();
            hasCurrent = false;
            hasNext = false;
            hasCompleted = false;
            current = default;
            next = default;
        }

        public int Index { get; private set; }

        public bool IsFirst => Index == 0;

        public bool IsLast => (hasCurrent && !hasNext) || HasReachedMaxItems;

        object IEnumerator.Current => Current;

        public T Current
        {
            get
            {
                if (!hasCurrent)
                {
                    throw new InvalidOperationException($"Please call {nameof(MoveNext)} first");
                }

                return current;
            }

            private set
            {
                current = value;
                hasCurrent = true;
            }
        }

        public bool MoveNext()
        {
            if (!hasCompleted)
            {
                if (FetchCurrent())
                {
                    PrefetchNext();
                    return true;
                }
            }

            hasCompleted = true;
            return false;
        }

        private bool FetchCurrent()
        {
            if (hasNext && !HasReachedMaxItems)
            {
                Current = next;
                Index++;

                return true;
            }

            if (enumerator.MoveNext() && !HasReachedMaxItems)
            {
                Current = enumerator.Current;
                Index++;

                return true;
            }
            else
            {
                hasCompleted = true;
                return false;
            }
        }

        public bool HasReachedMaxItems => Index == maxItems;

        private void PrefetchNext()
        {
            if (enumerator.MoveNext())
            {
                next = enumerator.Current;
                hasNext = true;
            }
            else
            {
                next = default;
                hasNext = false;
            }
        }

        public bool IsEmpty
        {
            get
            {
                if (!hasCurrent && !hasCompleted)
                {
                    throw new InvalidOperationException($"Please call {nameof(MoveNext)} first");
                }

                return Index == InitialIndex;
            }
        }

        public void Dispose()
        {
            enumerator.Dispose();
        }
    }
}
