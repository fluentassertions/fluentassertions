namespace FluentAssertions.Execution;

/// <summary>
/// Implements the xUnit (version 2 and 3) test framework adapter.
/// </summary>
internal class XUnitTestFramework(string assemblyName) : LateBoundTestFramework(loadAssembly: true)
{
    protected internal override string AssemblyName => assemblyName;

    protected override string ExceptionFullName => "Xunit.Sdk.XunitException";
}
