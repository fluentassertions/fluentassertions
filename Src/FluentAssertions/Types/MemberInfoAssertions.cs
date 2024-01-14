using System.Diagnostics;
using System.Reflection;

namespace FluentAssertions.Types;

/// <summary>
/// Contains a number of methods to assert that a <see cref="MemberInfo"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public abstract class MemberInfoAssertions<TSubject, TAssertions> : ReflectionAssertions<TSubject, TAssertions>
    where TSubject : MemberInfo
    where TAssertions : MemberInfoAssertions<TSubject, TAssertions>
{
    protected MemberInfoAssertions(TSubject subject)
        : base(subject)
    {
    }

    protected override string Identifier => "member";

    internal override string SubjectDescription => $"{Subject.DeclaringType}.{Subject.Name}";
}
