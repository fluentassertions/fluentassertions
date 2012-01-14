using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Frameworks
{
    public class AssertionHelper
    {
        public static void Throw(string message)
        {
            throw new AssertFailedException(message);
        }
    }
}
