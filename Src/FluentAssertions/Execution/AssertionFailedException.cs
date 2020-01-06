using System;

#if !NETSTANDARD1_3 && !NETSTANDARD1_6
using System.Runtime.Serialization;
#endif

namespace FluentAssertions.Execution
{
    /// <summary>
    /// Represents the default exception in case no test framework is configured.
    /// </summary>
#if !NETSTANDARD1_3 && !NETSTANDARD1_6
    [Serializable]
#endif

#pragma warning disable CA1032, RCS1194 // AssertionFailedException should never be constructed with an empty message
    public class AssertionFailedException : Exception
#pragma warning restore CA1032, RCS1194
    {
        public AssertionFailedException(string message)
            : base(message)
        {
        }

#if !NETSTANDARD1_3 && !NETSTANDARD1_6
        protected AssertionFailedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif

    }
}
