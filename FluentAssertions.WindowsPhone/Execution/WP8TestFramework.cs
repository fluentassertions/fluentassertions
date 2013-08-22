namespace FluentAssertions.Execution
{
    internal class WP8TestFramework : LateBoundTestFramework
    {
        protected override string AssemblyName
        {
            get { return "Microsoft.VisualStudio.QualityTools.UnitTesting.Phone"; }
        }

        protected override string ExceptionFullName
        {
            get { return "Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AssertFailedException"; }
        }
    }
}