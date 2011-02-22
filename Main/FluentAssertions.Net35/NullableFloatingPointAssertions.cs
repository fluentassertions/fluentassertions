using System;

namespace FluentAssertions
{
    public class NullableFloatingPointAssertions<T> : NullableNumericAssertions<T> where T : struct, IComparable
    {
        public NullableFloatingPointAssertions(T? expected) : base(expected)
        {
        }
    }
}