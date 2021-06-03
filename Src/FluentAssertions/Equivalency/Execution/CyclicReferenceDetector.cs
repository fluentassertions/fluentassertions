using System.Collections.Generic;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Execution
{
    /// <summary>
    /// Keeps track of objects and their location within an object graph so that cyclic references can be detected
    /// and handled upon.
    /// </summary>
    internal class CyclicReferenceDetector : ICloneable2
    {
        #region Private Definitions

        private HashSet<ObjectReference> observedReferences = new();

        #endregion

        /// <summary>
        /// Determines whether the specified object reference is a cyclic reference to the same object earlier in the
        /// equivalency validation.
        /// </summary>
        /// <remarks>
        /// The behavior of a cyclic reference is determined by the <paramref name="handling"/> parameter.
        /// </remarks>
        public bool IsCyclicReference(ObjectReference reference, CyclicReferenceHandling handling, Reason reason = null)
        {
            bool isCyclic = false;

            if (reference.IsComplexType)
            {
                isCyclic = !observedReferences.Add(reference);

                if (isCyclic && (handling == CyclicReferenceHandling.ThrowException))
                {
                    AssertionScope.Current
                        .BecauseOf(reason)
                        .FailWith(
                            "Expected {context:subject} to be {expectation}{reason}, but it contains a cyclic reference.");
                }
            }

            return isCyclic;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public object Clone()
        {
            return new CyclicReferenceDetector
            {
                observedReferences = new HashSet<ObjectReference>(observedReferences)
            };
        }
    }
}
