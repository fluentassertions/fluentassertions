using System;
using System.Linq.Expressions;

namespace FluentAssertions;

public static class Value
{
    public static IEquivalencyMatcher ThatMatches<T>(Expression<Func<T, bool>> predicate)
    {
        return null;
    }
}

public interface IEquivalencyMatcher
{
}
