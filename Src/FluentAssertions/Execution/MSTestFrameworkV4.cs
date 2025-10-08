namespace FluentAssertions.Execution;

internal sealed class MSTestFrameworkV4 : LateBoundTestFramework
{
    protected override string ExceptionFullName => "Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException";

    protected internal override string AssemblyName => "MSTest.TestFramework";
}
