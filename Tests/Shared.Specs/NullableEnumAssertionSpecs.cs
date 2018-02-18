using Xunit;

namespace FluentAssertions.Specs
{
    public enum MyEnum
    {
        Dummy,
    }

    public class NullableEnumAssertionSpecs
    {
        [Fact]
        public void When_a_nullable_enum_is_asserted_on_is_should_use_object_assertions()
        {
            // Arrange
            MyEnum? myEnum = MyEnum.Dummy;
            string expectedAssertionType = "FluentAssertions.Primitives.ObjectAssertions";

            // Act
            string assertionType = myEnum.Should().GetType().FullName;

            // Assert
            assertionType.Should().Be(expectedAssertionType,
                "it is a breaking change, if nullable enums do not end up in ObjectAssertions");
        }
    }
}
