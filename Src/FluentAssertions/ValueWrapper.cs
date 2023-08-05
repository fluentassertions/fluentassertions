using System;

namespace FluentAssertions;

public sealed class ValueWrapper<TValue>
{
    public ValueWrapper(TValue value) => Value = value;

    public TValue Value { get; }

    public override string ToString() => Value?.ToString();
}
