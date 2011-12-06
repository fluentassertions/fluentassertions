using System;

namespace FluentAssertions.Common
{
    /// <summary>
    /// Is thrown when the <see cref="UniqueObjectTracker"/> detects an object that was already processed.
    /// </summary>
    internal class ObjectAlreadyTrackedException : Exception
    {
    }
}