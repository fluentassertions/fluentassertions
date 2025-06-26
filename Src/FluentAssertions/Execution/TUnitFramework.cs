namespace FluentAssertions.Execution;

internal class TUnitFramework : LateBoundTestFramework
{
    public TUnitFramework()
    {
        LoadAssembly = true;
    }

    protected override string ExceptionFullName => "TUnit.Assertions.Exceptions.AssertionException";

    protected internal override string AssemblyName => "TUnit.Assertions";
}
