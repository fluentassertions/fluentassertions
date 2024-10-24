namespace FluentAssertions.Execution;

/// <summary>
/// Implements the XUnit (version 2) test framework adapter.
/// </summary>
internal class XUnit2TestFramework : LoadableTestFramework
{
    protected internal override string AssemblyName => "xunit.assert";

    protected override string ExceptionFullName => "Xunit.Sdk.XunitException";
}
