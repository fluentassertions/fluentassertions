using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Execution
{
    public class AssertionHelper
    {
        public static void Throw(string message)
        {
            throw new AssertFailedException(message);
        }
    }
}
