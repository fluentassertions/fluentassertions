using System.Collections.Generic;

using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Keeps track of objects and their location within an object graph so that cyclic references can be detected
    /// and handled upon.
    /// </summary>
    internal class CyclicReferenceDetector : ICloneable2
    {
        #region Private Definitions

        private readonly CyclicReferenceHandling handling;
        private List<ObjectReference> orderedReferences = new List<ObjectReference>();

        #endregion

        public CyclicReferenceDetector(CyclicReferenceHandling handling)
        {
            this.handling = handling;
        }

        /// <summary>
        /// Determines whether the specified object reference is a cyclic reference to the same object earlier in the
        /// equivalency validation.
        /// </summary>
        /// <remarks>
        /// The behavior of a cyclic reference is determined by the <see cref="CyclicReferenceHandling"/> constructor
        /// parameter.
        /// </remarks>
        public bool IsCyclicReference(ObjectReference reference, string because = "", params object[] becauseArgs)
        {
            bool isCyclic = false;

            if (reference.IsComplexType)
            {
                if (orderedReferences.Contains(reference))
                {
                    isCyclic = true;
                    if (handling == CyclicReferenceHandling.ThrowException)
                    {
                        AssertionScope.Current
                            .BecauseOf(because, becauseArgs)
                            .FailWith(
                            "Expected {context:subject} to be {expectation}{reason}, but it contains a cyclic reference.");
                    }
                }
                else
                {
                    orderedReferences.Add(reference);
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
            return new CyclicReferenceDetector(handling)
            {
                orderedReferences = new List<ObjectReference>(orderedReferences)
            };
        }
    }
}
