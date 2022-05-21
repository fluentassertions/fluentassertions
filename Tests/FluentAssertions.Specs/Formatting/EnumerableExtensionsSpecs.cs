using System.Collections.Generic;
using FluentAssertions.Formatting;
using Xunit;

namespace FluentAssertions.Specs.Formatting;

public class EnumerableExtensionsSpecs
{
    public static IEnumerable<object[]> JoinUsingWritingStyleTestCases
    {
        get
        {
            yield return new object[] { new object[0], "" };
            yield return new object[] { new object[] { "test" }, "test" };
            yield return new object[] { new object[] { "test", "test2" }, "test and test2" };
            yield return new object[] { new object[] { "test", "test2", "test3" }, "test, test2 and test3" };
            yield return new object[] { new object[] { "test", "test2", "test3", "test4" }, "test, test2, test3 and test4" };
        }
    }

    [Theory]
    [MemberData(nameof(JoinUsingWritingStyleTestCases))]
    public void JoinUsingWritingStyle_should_format_correctly(IEnumerable<object> input, string expectation)
    {
        // Act
        var result = input.JoinUsingWritingStyle();

        // Assert
        result.Should().Be(expectation);
    }
}
