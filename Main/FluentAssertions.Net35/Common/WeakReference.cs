using System;

namespace FluentAssertions.Common
{
    internal class WeakReference<T> where T : class
    {
        private readonly WeakReference innerReference;

        public static WeakReference<T> Create(T target)
        {
            if (target == null)
            {
                return WeakNullReference<T>.Singleton;
            }

            return new WeakReference<T>(target);
        }

        protected WeakReference(T target)
        {
            innerReference = new WeakReference(target);
        }

        public T Target
        {
            get { return (T)innerReference.Target; }
        }

        public virtual bool IsAlive
        {
            get { return innerReference.IsAlive; }
        }
    }
}