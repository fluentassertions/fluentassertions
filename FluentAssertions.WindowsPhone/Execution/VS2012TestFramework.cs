namespace FluentAssertions.Execution
{
    internal class VS2012TestFramework : LateBoundTestFramework
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