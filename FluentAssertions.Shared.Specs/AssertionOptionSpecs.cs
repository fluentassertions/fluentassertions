using Chill.Autofac;
using Chill;
using System;
using System.Collections.Generic;
using System.Text;

using FluentAssertions.Execution;
#if !OLD_MSTEST
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace FluentAssertions.Specs
{
    namespace AssertionOptionsSpecs
    {
        [TestClass]
        public class When_configuring_global_assertion_options : GivenWhenThen, IDisposable
        {
            public When_configuring_global_assertion_options()
            {
                Given(() =>
                {

                });
                
                When(() =>
                {
                    
                });
            }

            [TestMethod]
            public void Then_it_should()
            {
                
            }

            protected override void Dispose(bool disposing)
            {


                base.Dispose(disposing);
            }
        }
}
}
