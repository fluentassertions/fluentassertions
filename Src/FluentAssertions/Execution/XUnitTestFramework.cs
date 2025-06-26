namespace FluentAssertions.Execution;

/// <summary>
/// Implements the xUnit (version 2 and 3) test framework adapter.
/// </summary>
internal class XUnitTestFramework : LateBoundTestFramework
{
    private readonly string assemblyName;

    /// <summary>
    /// Implements the xUnit (version 2 and 3) test framework adapter.
    /// </summary>
    public XUnitTestFramework(string assemblyName)
    {
        this.assemblyName = assemblyName;
        LoadAssembly = true;
    }

    protected internal override string AssemblyName => assemblyName;

    protected override string ExceptionFullName => "Xunit.Sdk.XunitException";
}
