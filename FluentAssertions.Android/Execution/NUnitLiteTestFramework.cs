using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentAssertions.Execution
{
    class NUnitLiteTestFramework : LateBoundTestFramework
    {
        protected override string AssemblyName
        {
            get { return "xamarin.android.nunitlite"; }
        }

        protected override string ExceptionFullName
        {
            get { return "NUnit.Framework.AssertionException"; }
        }
    }
}