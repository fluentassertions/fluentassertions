namespace FluentAssertions.Execution
{
    internal class WP75TestFramework : LateBoundTestFramework
    {
        protected override string AssemblyName
        {
            get { return "Microsoft.VisualStudio.QualityTools.UnitTesting.Silverlight"; }
        }

        protected override string ExceptionFullName
        {
            get { return "Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException"; }
        }
    }
}