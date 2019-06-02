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

    public class AssertionFailedException : Exception
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
