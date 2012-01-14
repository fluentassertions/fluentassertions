#if XUNIT

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions
{
    public class SpecificationMismatchException : AssertFailedException
    {
        public SpecificationMismatchException(string message) : base(message)
        {
            
        }
    }
}

#endif