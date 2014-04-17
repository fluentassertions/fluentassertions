using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentAssertions.Common;
using FluentAssertions.Execution;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.specs
{
    [TestClass]
    public class Initialization
    {
        [AssemblyInitialize]
        public static void Foo(TestContext context)
        {
            Services.TestFramework = new MsTestFramework();
        }
    }

    internal class MsTestFramework : ITestFramework
    {
        public bool IsAvailable { get; private set; }

        public void Throw(string message)
        {
            throw new AssertFailedException(message);
        }
    }
}
