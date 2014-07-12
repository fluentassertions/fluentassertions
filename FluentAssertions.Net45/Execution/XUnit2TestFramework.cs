namespace FluentAssertions.Execution
{
    internal class XUnit2TestFramework : LateBoundTestFramework
    {
        protected override string AssemblyName
        {
            get { return "xunit.assert"; }
        }

        protected override string ExceptionFullName
        {
            get { return "Xunit.Sdk.XunitException"; }
        }
    }
}