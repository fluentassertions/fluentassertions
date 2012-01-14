#if NUNIT

using NUnit.Framework;

namespace FluentAssertions
{
    public class SpecificationMismatchException : AssertionException
    {
        public SpecificationMismatchException(string message) : base(message)
        {
            
        }
    }
}

#endif