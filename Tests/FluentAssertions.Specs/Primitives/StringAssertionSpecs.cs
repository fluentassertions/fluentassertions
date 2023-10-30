using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace FluentAssertions.Specs.Primitives;

public partial class StringAssertionSpecs
{
    [Fact]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public void When_chaining_multiple_assertions_it_should_assert_all_conditions()
    {
        // Arrange
        string actual = "ABCDEFGHI";
        string prefix = "AB";
        string suffix = "HI";
        string substring = "EF";
        int length = 9;

        // Act / Assert
        actual.Should()
            .StartWith(prefix).And
            .EndWith(suffix).And
            .Contain(substring).And
            .HaveLength(length);
    }

    private sealed class MatchingEqualityComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return true;
        }

        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }

    private sealed class NotMatchingEqualityComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return false;
        }

        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }
}
