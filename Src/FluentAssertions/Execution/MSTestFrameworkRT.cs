namespace FluentAssertions.Execution
{
    internal class MSTestFrameworkRT : LateBoundTestFramework
    {
        protected override string AssemblyName => "Microsoft.VisualStudio.TestPlatform.UnitTestFramework";

        protected override string ExceptionFullName => "Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AssertFailedException";
    }
}
