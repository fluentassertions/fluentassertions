using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Execution
{
    internal class SilverlightTestFramework : ITestFramework
    {
        public bool IsAvailable
        {
            get { return true; }
        }

        public void Throw(string message)
        {
            throw new AssertFailedException(message);
        }
    }
}