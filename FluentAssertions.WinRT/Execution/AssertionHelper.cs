using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

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
