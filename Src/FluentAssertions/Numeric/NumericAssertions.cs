using System;
using System.Diagnostics;
using FluentAssertions.Execution;

namespace FluentAssertions.Numeric;

/// <summary>
/// Contains a number of methods to assert that an <see cref="IComparable{T}"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public class NumericAssertions<T> : NumericAssertions<T, NumericAssertions<T>>
    where T : struct, IComparable<T>
{
    public NumericAssertions(T value, AssertionChain assertionChain)
        : base(value, assertionChain)
    {
    }
}

/// <summary>
/// Contains a number of methods to assert that an <see cref="IComparable{T}"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public class NumericAssertions<T, TAssertions> : NumericAssertionsBase<T, T, TAssertions>
    where T : struct, IComparable<T>
    where TAssertions : NumericAssertions<T, TAssertions>
{
    public NumericAssertions(T value, AssertionChain assertionChain)
        : base(assertionChain)
    {
        Subject = value;
    }

    public override T Subject { get; }
}
