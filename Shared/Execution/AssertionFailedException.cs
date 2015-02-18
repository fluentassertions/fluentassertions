using System;

namespace FluentAssertions.Execution
{
    /// <summary>
    /// Represents the default exception in case no test framework is configured.
    /// </summary>
#if NET40 || NET45
    [Serializable]
#endif
    public class AssertionFailedException : Exception
    {
        public AssertionFailedException(string message) : base(message)
        {
            
        }
    }
}