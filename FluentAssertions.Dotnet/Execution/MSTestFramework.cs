namespace FluentAssertions.Execution
{
    internal class MSTestFramework : LateBoundTestFramework
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