using System.Collections.Generic;
using System.Reflection;

namespace FluentAssertions.Assertions
{
    internal class CyclicReferenceTracker
    {
        private static IList<object> referencedObjects;

        public void Initialize()
        {
            referencedObjects = new List<object>();
        }

        public void AssertNoCyclicReferenceFor(object value)
        {
            if (referencedObjects.Contains(value))
            {
                throw new CyclicReferenceInRecursionException();
            }

            referencedObjects.Add(value);
        }
    }
}