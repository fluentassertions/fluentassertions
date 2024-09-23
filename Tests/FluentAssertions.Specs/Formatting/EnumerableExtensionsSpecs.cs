using FluentAssertions.Formatting;
using Xunit;

namespace FluentAssertions.Specs.Formatting;

public class EnumerableExtensionsSpecs
{
    public static TheoryData<string[], string> JoinUsingWritingStyleTestCases => new()
    {
        { [], "" },
        { ["test"], "test" },
        { ["test", "test2"], "test and test2" },
        { ["test", "test2", "test3"], "test, test2 and test3" },
        { ["test", "test2", "test3", "test4"], "test, test2, test3 and test4" }
    };

    [Theory]
    [MemberData(nameof(JoinUsingWritingStyleTestCases))]
    public void JoinUsingWritingStyle_should_format_correctly(string[] input, string expectation)
    {
        // Act
        var result = input.JoinUsingWritingStyle();

        // Assert
        result.Should().Be(expectation);
    }
}
