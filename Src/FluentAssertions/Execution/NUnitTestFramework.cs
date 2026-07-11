using System.Diagnostics;

namespace FluentAssertions.Execution;

[StackTraceHidden]
internal class NUnitTestFramework : LateBoundTestFramework
{
    protected internal override string AssemblyName => "nunit.framework";

    protected override string ExceptionFullName => "NUnit.Framework.AssertionException";
}
