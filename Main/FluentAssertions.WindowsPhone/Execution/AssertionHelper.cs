using System;

namespace FluentAssertions.Execution
{
    public class AssertionHelper
    {
        public static void Throw(string message)
        {
            var testFramework = new WindowsPhoneTestFramework();
            if (testFramework.IsAvailable)
            {
                testFramework.Throw(message);
            }
            else
            {
                throw new InvalidOperationException("Could not find the Windows Phone test framework");
            }
        }
    }
}