using FluentAssertions.Common;

namespace FluentAssertions.Execution
{
    internal static class AssertionHelper
    {
        public static void Throw(string message)
        {
            Services.TestFramework.Throw(message);
        }
    }
}