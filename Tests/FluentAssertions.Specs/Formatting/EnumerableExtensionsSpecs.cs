using System.Collections.Generic;
using FluentAssertionsAsync.Formatting;
using Xunit;

namespace FluentAssertionsAsync.Specs.Formatting;

public class EnumerableExtensionsSpecs
{
    public static TheoryData<IEnumerable<object>, string> JoinUsingWritingStyleTestCases => new()
    {
        { new object[0], "" },
        { new object[] { "test" }, "test" },
        { new object[] { "test", "test2" }, "test and test2" },
        { new object[] { "test", "test2", "test3" }, "test, test2 and test3" },
        { new object[] { "test", "test2", "test3", "test4" }, "test, test2, test3 and test4" }
    };

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
