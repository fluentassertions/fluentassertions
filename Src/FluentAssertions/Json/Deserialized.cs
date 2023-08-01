namespace FluentAssertions.Json;

public sealed class Deserialized<T>
{
    internal Deserialized(T value) => Value = value;

    public T Value { get; }
}
