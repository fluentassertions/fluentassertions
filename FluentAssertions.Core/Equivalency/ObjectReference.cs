using System;

using FluentAssertions.Common;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Represents  an object tracked by the <see cref="ObjectTracker"/> including it's location within an object graph.
    /// </summary>
    internal class ObjectReference
    {
        private readonly object @object;
        private readonly string propertyPath;

        public ObjectReference(object @object, string propertyPath)
        {
            this.@object = @object;
            this.propertyPath = propertyPath;
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
            var other = (ObjectReference)obj;

            int firstDot = other.propertyPath.IndexOf(".");
            string firstPartOfOtherPropertyPath = (firstDot >= 0) ? other.propertyPath.Substring(0, firstDot) : "";

            return ReferenceEquals(@object, other.@object) &&
                   (propertyPath.Length > 0) && 
                   firstPartOfOtherPropertyPath.Equals(propertyPath, StringComparison.CurrentCultureIgnoreCase);
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
                return (@object.GetHashCode() * 397) ^ propertyPath.GetHashCode();
            }
        }

        public bool IsReference
        {
            get { return !ReferenceEquals(@object, null) && @object.GetType().IsComplexType(); }
        }
    }
}