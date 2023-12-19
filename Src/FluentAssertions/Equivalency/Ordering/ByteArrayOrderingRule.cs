using System.Collections.Generic;
using FluentAssertionsAsync.Common;

namespace FluentAssertionsAsync.Equivalency.Ordering;

/// <summary>
/// Ordering rule that ensures that byte arrays are always compared in strict ordering since it would cause a
/// severe performance impact otherwise.
/// </summary>
internal class ByteArrayOrderingRule : IOrderingRule
{
    public OrderStrictness Evaluate(IObjectInfo objectInfo)
    {
        return objectInfo.CompileTimeType.IsSameOrInherits(typeof(IEnumerable<byte>))
            ? OrderStrictness.Strict
            : OrderStrictness.Irrelevant;
    }

    public override string ToString()
    {
        return "Be strict about the order of items in byte arrays";
    }
}
