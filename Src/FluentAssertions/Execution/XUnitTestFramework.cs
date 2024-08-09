namespace FluentAssertions.Execution;

internal abstract class XUnitTestFramework : LateBoundTestFramework
{
    protected override string ExceptionFullName => "Xunit.Sdk.XunitException";
}

internal class XUnit2TestFramework : XUnitTestFramework
{
    protected internal override string AssemblyName => "xunit.assert";
}

internal class XUnit3TestFramework : XUnitTestFramework
{
    protected internal override string AssemblyName => "xunit.v3.assert";
}
