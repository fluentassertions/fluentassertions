using System;

using FluentAssertions.Assertions;

namespace FluentAssertions
{
    // Assertions that MSTest lacks
    public static class AssertEx
    {
        public static ExceptionAssertions<TException> Throws<TException>(Action action)
            where TException : Exception
        {
            return action.ShouldThrow<TException>();
        }
    }
}