using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class ObjectAssertionSpecs
{
    public class BeOneOf
    {
        [Fact]
        public void When_a_value_is_not_one_of_the_specified_values_it_should_throw()
        {
            // Arrange
            var value = new ClassWithCustomEqualMethod(3);

            // Act
            Action act = () => value.Should().BeOneOf(new ClassWithCustomEqualMethod(4), new ClassWithCustomEqualMethod(5));

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to be one of {ClassWithCustomEqualMethod(4), ClassWithCustomEqualMethod(5)}, but found ClassWithCustomEqualMethod(3).");
        }

        [Fact]
        public void When_a_value_is_not_one_of_the_specified_values_it_should_throw_with_descriptive_message()
        {
            // Arrange
            var value = new ClassWithCustomEqualMethod(3);

            // Act
            Action act = () =>
                value.Should().BeOneOf(new[] { new ClassWithCustomEqualMethod(4), new ClassWithCustomEqualMethod(5) },
                    "because those are the valid values");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to be one of {ClassWithCustomEqualMethod(4), ClassWithCustomEqualMethod(5)} because those are the valid values, but found ClassWithCustomEqualMethod(3).");
        }

        [Fact]
        public void When_a_value_is_one_of_the_specified_values_it_should_succeed()
        {
            // Arrange
            var value = new ClassWithCustomEqualMethod(4);

            // Act
            Action act = () => value.Should().BeOneOf(new ClassWithCustomEqualMethod(4), new ClassWithCustomEqualMethod(5));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void An_untyped_value_is_one_of_the_specified_values()
        {
            // Arrange
            object value = new SomeClass(5);

            // Act / Assert
            value.Should().BeOneOf(new[] { new SomeClass(4), new SomeClass(5) }, new SomeClassEqualityComparer());
        }

        [Fact]
        public void A_typed_value_is_one_of_the_specified_values()
        {
            // Arrange
            var value = new SomeClass(5);

            // Act / Assert
            value.Should().BeOneOf(new[] { new SomeClass(4), new SomeClass(5) }, new SomeClassEqualityComparer());
        }

        [Fact]
        public void An_untyped_value_is_not_one_of_the_specified_values()
        {
            // Arrange
            object value = new SomeClass(3);

            // Act
            Action act = () => value.Should().BeOneOf(new[] { new SomeClass(4), new SomeClass(5) },
                new SomeClassEqualityComparer(), "I said so");

            // Assert
            act.Should().Throw<XunitException>()
               .WithMessage("Expected value to be one of {SomeClass(4), SomeClass(5)}*I said so*SomeClass(3).");
        }

        [Fact]
        public void An_untyped_value_is_not_one_of_no_values()
        {
            // Arrange
            object value = new SomeClass(3);

            // Act
            Action act = () => value.Should().BeOneOf(Array.Empty<SomeClass>(), new SomeClassEqualityComparer());

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void A_typed_value_is_not_one_of_the_specified_values()
        {
            // Arrange
            var value = new SomeClass(3);

            // Act
            Action act = () => value.Should().BeOneOf(new[] { new SomeClass(4), new SomeClass(5) },
                new SomeClassEqualityComparer(), "I said so");

            // Assert
            act.Should().Throw<XunitException>()
               .WithMessage("Expected value to be one of {SomeClass(4), SomeClass(5)}*I said so*SomeClass(3).");
        }

        [Fact]
        public void A_typed_value_is_not_one_of_no_values()
        {
            // Arrange
            var value = new SomeClass(3);

            // Act
            Action act = () => value.Should().BeOneOf(Array.Empty<SomeClass>(), new SomeClassEqualityComparer());

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void A_typed_value_is_not_the_same_type_as_the_specified_values()
        {
            // Arrange
            var value = new ClassWithCustomEqualMethod(3);

            // Act
            Action act = () => value.Should().BeOneOf(new[] { new SomeClass(4), new SomeClass(5) },
                new SomeClassEqualityComparer(), "I said so");

            // Assert
            act.Should().Throw<XunitException>()
               .WithMessage("Expected value to be one of {SomeClass(4), SomeClass(5)}*I said so*ClassWithCustomEqualMethod(3).");
        }

        [Fact]
        public void An_untyped_value_requires_an_expectation()
        {
            // Arrange
            object value = new SomeClass(3);

            // Act
            Action act = () => value.Should().BeOneOf(null, new SomeClassEqualityComparer());

            // Assert
            act.Should().Throw<ArgumentNullException>().WithParameterName("validValues");
        }

        [Fact]
        public void A_typed_value_requires_an_expectation()
        {
            // Arrange
            var value = new SomeClass(3);

            // Act
            Action act = () => value.Should().BeOneOf(null, new SomeClassEqualityComparer());

            // Assert
            act.Should().Throw<ArgumentNullException>().WithParameterName("validValues");
        }

        [Fact]
        public void An_untyped_value_requires_a_comparer()
        {
            // Arrange
            object value = new SomeClass(3);

            // Act
            Action act = () => value.Should().BeOneOf(Array.Empty<SomeClass>(), comparer: null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithParameterName("comparer");
        }

        [Fact]
        public void A_typed_value_requires_a_comparer()
        {
            // Arrange
            var value = new SomeClass(3);

            // Act
            Action act = () => value.Should().BeOneOf(Array.Empty<SomeClass>(), comparer: null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithParameterName("comparer");
        }

        [Fact]
        public void Chaining_after_one_assertion()
        {
            // Arrange
            var value = new SomeClass(3);

            // Act / Assert
            value.Should().BeOneOf(value).And.NotBeNull();
        }

        [Fact]
        public void Can_chain_multiple_assertions()
        {
            // Arrange
            var value = new object();

            // Act / Assert
            value.Should().BeOneOf<object>(new[] { value }, new DumbObjectEqualityComparer()).And.NotBeNull();
        }
    }
}
