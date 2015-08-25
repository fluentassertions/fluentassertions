using FluentAssertions.Common;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.specs
{
    [TestClass]
    public class Initialization
    {
        [AssemblyInitialize]
        public static void Foo(TestContext context)
        {
            Services.ThrowException = message => { throw new AssertFailedException(message); };
        }
    }
}