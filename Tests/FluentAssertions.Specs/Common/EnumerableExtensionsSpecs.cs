using FluentAssertions.Common;
using Xunit;

namespace FluentAssertions.Specs.Common;

public class EnumerableExtensionsSpecs
{
    [Fact]
    public void A_shorter_first_sequence_differs_at_its_length()
    {
        // Arrange
        int[] first = [1, 2];
        int[] second = [1, 2, 3];

        // Act
        int index = first.IndexOfFirstDifferenceWith(second, (left, right) => left == right);

        // Assert
        index.Should().Be(2);
    }

    [Fact]
    public void A_shorter_second_sequence_differs_at_its_length()
    {
        // Arrange
        int[] first = [1, 2, 3];
        int[] second = [1, 2];

        // Act
        int index = first.IndexOfFirstDifferenceWith(second, (left, right) => left == right);

        // Assert
        index.Should().Be(2);
    }
}
