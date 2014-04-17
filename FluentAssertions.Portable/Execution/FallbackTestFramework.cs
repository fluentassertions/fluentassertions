using System;

namespace FluentAssertions.Execution
{
    internal class AssertionFailedException : Exception
    {
        public AssertionFailedException(string message) : base(message)
        {
            
        }
    }
}