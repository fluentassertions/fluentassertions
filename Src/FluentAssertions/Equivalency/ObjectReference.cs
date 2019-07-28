using System;
using System.Linq;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Represents  an object tracked by the <see cref="CyclicReferenceDetector"/> including it's location within an object graph.
    /// </summary>
    internal class ObjectReference
    {
        private readonly object @object;
        private readonly string path;
        private readonly bool? isComplexType;
        private string[] pathElements;

        public ObjectReference(object @object, string path, bool? isComplexType = null)
        {
            this.@object = @object;
            this.path = path;
            this.isComplexType = isComplexType;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="System.Object"/> is equal to the current <see cref="System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="System.Object"/>. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (!(obj is ObjectReference other))
            {
                return false;
            }

            return ReferenceEquals(@object, other.@object) && IsParentOf(other);
        }

        private string[] GetPathElements() => pathElements
            ?? (pathElements = path.ToUpperInvariant().Replace("][", "].[").Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries));

        private bool IsParentOf(ObjectReference other)
        {
            string[] path = GetPathElements();
            string[] otherPath = other.GetPathElements();
            return (otherPath.Length > path.Length) && otherPath.Take(path.Length).SequenceEqual(path);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(@object);
        }

        public override string ToString()
        {
            return $"{{\"{path}\", {@object}}}";
        }

        public bool IsComplexType => isComplexType ?? (!(@object is null) && !@object.GetType().OverridesEquals());
    }
}
