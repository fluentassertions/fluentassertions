using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions
{
    public class SpecificationMismatchException : UnitTestAssertException
    {
        public SpecificationMismatchException(string message) : base(message)
        {
            
        }
    }
}