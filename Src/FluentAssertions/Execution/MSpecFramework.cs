using System.Diagnostics;

namespace FluentAssertions.Execution;

[StackTraceHidden]
internal class MSpecFramework : LateBoundTestFramework
{
    protected internal override string AssemblyName => "Machine.Specifications";

    protected override string ExceptionFullName => "Machine.Specifications.SpecificationException";
}
