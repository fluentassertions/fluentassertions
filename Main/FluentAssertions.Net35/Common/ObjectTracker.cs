using System.Collections.Generic;

namespace FluentAssertions.Common
{
    /// <summary>
    /// Simple class for detecting an attempt to process an object that were already processed.
    /// </summary>
    internal class ObjectTracker
    {
        private static IList<object> objects;

        public void Reset()
        {
            objects = new List<object>();
        }

        public void Add(object value)
        {
            if (objects.Contains(value))
            {
                throw new ObjectAlreadyTrackedException();
            }

            objects.Add(value);
        }
    }
}