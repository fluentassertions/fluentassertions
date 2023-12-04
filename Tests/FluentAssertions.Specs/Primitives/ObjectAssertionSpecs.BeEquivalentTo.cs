using Xunit;

namespace FluentAssertions.Specs.Primitives;

public partial class ObjectAssertionSpecs
{
    public class BeEquivalentTo
    {
        [Fact]
        public void Can_ignore_casing_while_comparing_objects_with_string_properties()
        {
            // Arrange
            var actual = new { foo = "test" };
            var expectation = new { foo = "TEST" };

            // Act / Assert
            actual.Should().BeEquivalentTo(expectation, o => o.IgnoringCase());
        }

        [Fact]
        public void Can_ignore_leading_whitespace_while_comparing_objects_with_string_properties()
        {
            // Arrange
            var actual = new { foo = "  test" };
            var expectation = new { foo = "test" };

            // Act / Assert
            actual.Should().BeEquivalentTo(expectation, o => o.IgnoringLeadingWhitespace());
        }

        [Fact]
        public void Can_ignore_trailing_whitespace_while_comparing_objects_with_string_properties()
        {
            // Arrange
            var actual = new { foo = "test  " };
            var expectation = new { foo = "test" };

            // Act / Assert
            actual.Should().BeEquivalentTo(expectation, o => o.IgnoringTrailingWhitespace());
        }
    }
}
