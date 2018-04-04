namespace FluentAssertions.Execution
{
    internal class NUnitPclTestFramework : LateBoundTestFramework
    {
        protected override string AssemblyName => "nunit.framework";

        protected override string ExceptionFullName => "NUnit.Framework.AssertionException";
    }
}
