namespace FluentAssertions.Frameworks
{
    internal class MSTestFramework : LateBoundTestFramework
    {
        protected override string ExceptionFullName
        {
            get { return "Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException"; }
        }

        protected override string AssemblyName
        {
            get { return "Microsoft.VisualStudio.QualityTools.UnitTestFramework"; }
        }
    }
}