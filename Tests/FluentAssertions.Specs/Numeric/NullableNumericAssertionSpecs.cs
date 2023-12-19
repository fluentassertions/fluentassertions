using Xunit;

namespace FluentAssertionsAsync.Specs.Numeric;

public partial class NullableNumericAssertionSpecs
{
    [Fact]
    public void Should_support_chaining_constraints_with_and()
    {
        // Arrange
        int? nullableInteger = 1;

        // Act / Assert
        nullableInteger.Should()
            .HaveValue()
            .And
            .BePositive();
    }
}
