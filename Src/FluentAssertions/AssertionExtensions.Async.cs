using System;
using System.Threading.Tasks;
using FluentAssertions.Specialized;

namespace FluentAssertions
{
    public static partial class AssertionExtensions
    {
        public static async Task<ExceptionAssertions<TException>> WithMessage<TException>(
            this Task<ExceptionAssertions<TException>> task,
            string expectedWildcardPattern,
            string because = "",
            params object[] becauseArgs)
            where TException : Exception
        {
            return (await task).WithMessage(expectedWildcardPattern, because, becauseArgs);
        }
    }
}
