using System.Diagnostics;
using System.Reflection;

namespace FluentAssertions.Types;

/// <summary>
/// Contains a number of methods to assert that a <see cref="ParameterInfo"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public class ParameterInfoAssertions : ReflectionAssertions<ParameterInfo, ParameterInfoAssertions>
{
    public ParameterInfoAssertions(ParameterInfo parameterInfo)
        : base(parameterInfo)
    {
    }

    internal override string SubjectDescription => Subject.Name;

    /// <summary>
    /// Returns the type of the subject the assertion applies on.
    /// </summary>
    protected override string Identifier => "parameter";
}
