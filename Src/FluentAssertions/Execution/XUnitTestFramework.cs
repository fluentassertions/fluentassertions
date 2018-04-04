namespace FluentAssertions.Execution
{
    internal class XUnitTestFramework : LateBoundTestFramework
    {
        protected override string AssemblyName => "xunit";

        protected override string ExceptionFullName => "Xunit.Sdk.AssertException";
    }
}
