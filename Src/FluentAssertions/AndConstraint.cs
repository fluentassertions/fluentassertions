using System.Diagnostics;

namespace FluentAssertions;

[StackTraceHidden]
[DebuggerNonUserCode]
public class AndConstraint<TParent>
{
    public TParent And { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AndConstraint{T}"/> class.
    /// </summary>
    public AndConstraint(TParent parent)
    {
        And = parent;
    }
}
