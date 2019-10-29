using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class BasicNonEquivalencySpecs
    {
        [Fact]
        public void When_asserting_inequivalence_of_equal_ints_as_object_it_should_fail()
        {
            // Arrange
            object i1 = 1;
            object i2 = 1;

            // Act
            Action act = () => i1.Should().NotBeEquivalentTo(i2);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_inequivalence_of_unequal_ints_as_object_it_should_succeed()
        {
            // Arrange
            object i1 = 1;
            object i2 = 2;

            // Act
            Action act = () => i1.Should().NotBeEquivalentTo(i2);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_inequivalence_of_equal_strings_as_object_it_should_fail()
        {
            // Arrange
            object s1 = "A";
            object s2 = "A";

            // Act
            Action act = () => s1.Should().NotBeEquivalentTo(s2);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_inequivalence_of_unequal_strings_as_object_it_should_succeed()
        {
            // Arrange
            object s1 = "A";
            object s2 = "B";

            // Act
            Action act = () => s1.Should().NotBeEquivalentTo(s2);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_inequivalence_of_equal_classes_it_should_fail()
        {
            // Arrange
            var o1 = new { Name = "A" };
            var o2 = new { Name = "A" };

            // Act
            Action act = () => o1.Should().NotBeEquivalentTo(o2);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_inequivalence_of_unequal_classes_it_should_succeed()
        {
            // Arrange
            var o1 = new { Name = "A" };
            var o2 = new { Name = "B" };

            // Act
            Action act = () => o1.Should().NotBeEquivalentTo(o2);

            // Assert
            act.Should().NotThrow();
        }
    }
}
