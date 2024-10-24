namespace FluentAssertions.Execution;

internal class TUnitFramework : LoadableTestFramework
{
    protected override string ExceptionFullName => "TUnit.Assertions.Exceptions.AssertionException";

    protected internal override string AssemblyName => "TUnit.Assertions";
}
