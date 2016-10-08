namespace FluentAssertions.Execution
{
    internal class MSTestFrameworkV2 : LateBoundTestFramework
    {
        protected override string ExceptionFullName
        {
            get { return "Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException"; }
        }

        protected override string AssemblyName
        {
            get { return "Microsoft.VisualStudio.TestPlatform.TestFramework"; }
        }
    }
}