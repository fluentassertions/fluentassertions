namespace FluentAssertions.Execution
{
    internal class MSTestFrameworkRT : LateBoundTestFramework
    {
        protected override string AssemblyName
        {
            get { return "Microsoft.VisualStudio.TestPlatform.UnitTestFramework"; }
        }

        protected override string ExceptionFullName
        {
            get { return "Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AssertFailedException"; }
        }
    }
}