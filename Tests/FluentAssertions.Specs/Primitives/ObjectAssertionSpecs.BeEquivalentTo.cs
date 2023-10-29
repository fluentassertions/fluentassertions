using Xunit;

namespace FluentAssertions.Specs.Primitives;

public partial class ObjectAssertionSpecs
{
    public class BeEquivalentTo
    {
        [Fact]
        public void Succeed_for_object_containing_case_different_strings_when_IgnoringCase()
        {
            // Arrange
            var actual = new { foo = "test" };
            var expect = new { foo = "TEST" };

            // Act / Assert
            actual.Should().BeEquivalentTo(expect, o => o.IgnoringCase());
        }

        [Fact]
        public void Succeed_for_object_containing_leading_whitespace_different_strings_when_IgnoringLeadingWhitespace()
        {
            // Arrange
            var actual = new { foo = "  test" };
            var expect = new { foo = "test" };

            // Act / Assert
            actual.Should().BeEquivalentTo(expect, o => o.IgnoringLeadingWhitespace());
        }

        [Fact]
        public void Succeed_for_object_containing_trailing_whitespace_different_strings_when_IgnoringTrailingWhitespace()
        {
            // Arrange
            var actual = new { foo = "test  " };
            var expect = new { foo = "test" };

            // Act / Assert
            actual.Should().BeEquivalentTo(expect, o => o.IgnoringTrailingWhitespace());
        }

        [Fact]
        public void Succeed_for_object_containing_newline_different_strings_when_IgnoringNewlines()
        {
            // Arrange
            var actual = new { foo = "\rA\nB\r\nC\n" };
            var expect = new { foo = "ABC" };

            // Act / Assert
            actual.Should().BeEquivalentTo(expect, o => o.IgnoringNewlines());
        }
    }
}
