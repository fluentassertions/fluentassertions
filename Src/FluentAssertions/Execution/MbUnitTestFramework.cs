namespace FluentAssertions.Execution
{
    internal class MbUnitTestFramework : LateBoundTestFramework
    {
        protected override string AssemblyName => "MbUnit.Framework";

        protected override string ExceptionFullName => "MbUnit.Core.Exceptions.AssertionException";
    }
}
