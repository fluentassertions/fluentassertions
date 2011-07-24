namespace FluentAssertions.Frameworks
{
    internal class MSpecFramework : LateBoundTestFramework
    {
        protected override string AssemblyName
        {
            get { return "Machine.Specifications"; }
        }

        protected override string ExceptionFullName
        {
            get { return "Machine.Specifications.SpecificationException"; }
        }
    }
}