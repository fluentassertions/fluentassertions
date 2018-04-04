namespace FluentAssertions.Execution
{
    internal class MSTestFramework : LateBoundTestFramework
    {
        protected override string ExceptionFullName => "Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException";

        protected override string AssemblyName => "Microsoft.VisualStudio.QualityTools.UnitTestFramework";
    }
}
