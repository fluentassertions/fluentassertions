using Xunit;

namespace FluentAssertions.Specs.Collections;

public partial class CollectionAssertionSpecs
{
    public class AllBeEquivalentTo
    {
        [Fact]
        public void Can_ignore_casing_while_comparing_collections_of_strings()
        {
            // Arrange
            var actual = new[] { "test", "tEst", "Test", "TEst", "teST" };
            var expectation = "test";

            // Act / Assert
            actual.Should().AllBeEquivalentTo(expectation, o => o.IgnoringCase());
        }

        [Fact]
        public void Can_ignore_leading_whitespace_while_comparing_collections_of_strings()
        {
            // Arrange
            var actual = new[] { " test", "test", "\ttest", "\ntest", "  \t \n test" };
            var expectation = "test";

            // Act / Assert
            actual.Should().AllBeEquivalentTo(expectation, o => o.IgnoringLeadingWhitespace());
        }

        [Fact]
        public void Can_ignore_trailing_whitespace_while_comparing_collections_of_strings()
        {
            // Arrange
            var actual = new[] { "test ", "test", "test\t", "test\n", "test  \t \n " };
            var expectation = "test";

            // Act / Assert
            actual.Should().AllBeEquivalentTo(expectation, o => o.IgnoringTrailingWhitespace());
        }
    }
}
