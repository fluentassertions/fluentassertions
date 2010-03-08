using System;

namespace FluentAssertions
{
    public class SpecificationMismatchException : Exception
    {
        public SpecificationMismatchException(string message) : base(message)
        {
            
        }
    }
}