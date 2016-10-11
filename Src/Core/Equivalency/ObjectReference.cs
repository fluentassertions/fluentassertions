using System;
using System.Linq;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Represents  an object tracked by the <see cref="ObjectTracker"/> including it's location within an object graph.
    /// </summary>
    internal class ObjectReference
    {
        private readonly object @object;
        private readonly string[] path;

        public ObjectReference(object @object, string path)
        {
            this.@object = @object;
            this.path = path.ToLower().Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            var other = (ObjectReference) obj;

            return ReferenceEquals(@object, other.@object) && IsParentOf(other);
        }

        private bool IsParentOf(ObjectReference other)
        {
            return (other.path.Length > path.Length) && other.path.Take(path.Length).SequenceEqual(path);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                return (@object.GetHashCode()*397) ^ path.GetHashCode();
            }
        }

        public override string ToString()
        {
            return string.Format("{{\"{0}\", {1}}}", path, @object);
        }

        public bool IsComplexType
        {
            get { return !ReferenceEquals(@object, null) && @object.GetType().IsComplexType(); }
        }
    }
}