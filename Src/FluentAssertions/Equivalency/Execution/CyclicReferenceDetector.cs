using System.Collections.Generic;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Execution;

/// <summary>
/// Keeps track of objects and their location within an object graph so that cyclic references can be detected
/// and handled upon.
/// </summary>
[System.Diagnostics.StackTraceHidden]
internal class CyclicReferenceDetector : ICloneable2
{
    #region Private Definitions

    private HashSet<ObjectReference> observedReferences = [];

    #endregion

    /// <summary>
    /// Determines whether the specified object reference is a cyclic reference to the same object earlier in the
    /// equivalency validation.
    /// </summary>
    public bool IsCyclicReference(ObjectReference reference)
    {
        bool isCyclic = false;

        if (reference.CompareByMembers)
        {
            isCyclic = !observedReferences.Add(reference);
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

