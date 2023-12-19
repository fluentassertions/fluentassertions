using System.Diagnostics;

namespace FluentAssertionsAsync;

[DebuggerNonUserCode]
public class AndConstraint<T>
{
    public T And { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AndConstraint{T}"/> class.
    /// </summary>
    public AndConstraint(T parentConstraint)
    {
        And = parentConstraint;
    }
}
