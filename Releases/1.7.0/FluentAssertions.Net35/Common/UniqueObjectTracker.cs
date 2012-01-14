using System.Collections.Generic;

using System.Linq;

namespace FluentAssertions.Common
{
    /// <summary>
    /// Simple class for detecting an attempt to process an object that were already processed.
    /// </summary>
    public class UniqueObjectTracker
    {
        private readonly IList<object> references = new List<object>();

        /// <summary>
        /// Tracks the specified reference but throws an <see cref="ObjectAlreadyTrackedException"/>
        /// if that reference was already tracked.
        /// </summary>
        public void Track(object reference)
        {
            if (references.Any(o => ReferenceEquals(o, reference)))
            {
                throw new ObjectAlreadyTrackedException();
            }

            references.Add(reference);
        }
    }
}