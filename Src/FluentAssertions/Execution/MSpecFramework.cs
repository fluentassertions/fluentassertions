namespace FluentAssertions.Execution
{
    internal class MSpecFramework : LateBoundTestFramework
    {
        protected override string AssemblyName => "Machine.Specifications";

        protected override string ExceptionFullName => "Machine.Specifications.SpecificationException";
    }
}
