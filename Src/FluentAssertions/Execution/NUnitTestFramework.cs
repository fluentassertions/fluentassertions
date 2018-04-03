namespace FluentAssertions.Execution
{
    internal class NUnitTestFramework : LateBoundTestFramework
    {
        protected override string AssemblyName => "nunit.framework";

        protected override string ExceptionFullName => "NUnit.Framework.AssertionException";
    }
}
