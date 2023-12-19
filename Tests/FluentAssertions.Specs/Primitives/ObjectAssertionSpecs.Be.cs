using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class ObjectAssertionSpecs
{
    public class Be
    {
        [Fact]
        public void When_two_equal_object_are_expected_to_be_equal_it_should_not_fail()
        {
            // Arrange
            var someObject = new ClassWithCustomEqualMethod(1);
            var equalObject = new ClassWithCustomEqualMethod(1);

            // Act / Assert
            someObject.Should().Be(equalObject);
        }

        [Fact]
        public void When_two_different_objects_are_expected_to_be_equal_it_should_fail_with_a_clear_explanation()
        {
            // Arrange
            var someObject = new ClassWithCustomEqualMethod(1);
            var nonEqualObject = new ClassWithCustomEqualMethod(2);

            // Act
            Action act = () => someObject.Should().Be(nonEqualObject);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected someObject to be ClassWithCustomEqualMethod(2), but found ClassWithCustomEqualMethod(1).");
        }

        [Fact]
        public void When_both_subject_and_expected_are_null_it_should_succeed()
        {
            // Arrange
            object someObject = null;
            object expectedObject = null;

            // Act / Assert
            someObject.Should().Be(expectedObject);
        }

        [Fact]
        public void When_the_subject_is_null_it_should_fail()
        {
            // Arrange
            object someObject = null;
            var nonEqualObject = new ClassWithCustomEqualMethod(2);

            // Act
            Action act = () => someObject.Should().Be(nonEqualObject);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected someObject to be ClassWithCustomEqualMethod(2), but found <null>.");
        }

        [Fact]
        public void When_two_different_objects_are_expected_to_be_equal_it_should_fail_and_use_the_reason()
        {
            // Arrange
            var someObject = new ClassWithCustomEqualMethod(1);
            var nonEqualObject = new ClassWithCustomEqualMethod(2);

            // Act
            Action act = () => someObject.Should().Be(nonEqualObject, "because it should use the {0}", "reason");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected someObject to be ClassWithCustomEqualMethod(2) because it should use the reason, but found ClassWithCustomEqualMethod(1).");
        }

        [Fact]
        public void When_comparing_a_numeric_and_an_enum_for_equality_it_should_throw()
        {
            // Arrange
            object subject = 1;
            MyEnum expected = MyEnum.One;

            // Act
            Action act = () => subject.Should().Be(expected);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void An_untyped_value_is_equal_to_another_according_to_a_comparer()
        {
            // Arrange
            object value = new SomeClass(5);

            // Act / Assert
            value.Should().Be(new SomeClass(5), new SomeClassEqualityComparer());
        }

        [Fact]
        public void A_typed_value_is_equal_to_another_according_to_a_comparer()
        {
            // Arrange
            var value = new SomeClass(5);

            // Act / Assert
            value.Should().Be(new SomeClass(5), new SomeClassEqualityComparer());
        }

        [Fact]
        public void An_untyped_value_is_not_equal_to_another_according_to_a_comparer()
        {
            // Arrange
            object value = new SomeClass(3);

            // Act
            Action act = () => value.Should().Be(new SomeClass(4), new SomeClassEqualityComparer(), "I said so");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected value to be SomeClass(4)*I said so*found SomeClass(3).");
        }

        [Fact]
        public void A_typed_value_is_not_equal_to_another_according_to_a_comparer()
        {
            // Arrange
            var value = new SomeClass(3);

            // Act
            Action act = () => value.Should().Be(new SomeClass(4), new SomeClassEqualityComparer(), "I said so");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected value to be SomeClass(4)*I said so*found SomeClass(3).");
        }

        [Fact]
        public void A_typed_value_is_not_of_the_same_type()
        {
            // Arrange
            var value = new ClassWithCustomEqualMethod(3);

            // Act
            Action act = () => value.Should().Be(new SomeClass(3), new SomeClassEqualityComparer(), "I said so");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected value to be SomeClass(3)*I said so*found ClassWithCustomEqualMethod(3).");
        }

        [Fact]
        public void A_untyped_value_requires_a_comparer()
        {
            // Arrange
            object value = new SomeClass(3);

            // Act
            Action act = () => value.Should().Be(new SomeClass(3), comparer: null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithParameterName("comparer");
        }

        [Fact]
        public void A_typed_value_requires_a_comparer()
        {
            // Arrange
            var value = new SomeClass(3);

            // Act
            Action act = () => value.Should().Be(new SomeClass(3), comparer: null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithParameterName("comparer");
        }

        [Fact]
        public void Chaining_after_one_assertion()
        {
            // Arrange
            var value = new SomeClass(3);

            // Act / Assert
            value.Should().Be(value).And.NotBeNull();
        }

        [Fact]
        public void Can_chain_multiple_assertions()
        {
            // Arrange
            var value = new object();

            // Act / Assert
            value.Should().Be<object>(value, new DumbObjectEqualityComparer()).And.NotBeNull();
        }
    }

    public class NotBe
    {
        [Fact]
        public void When_non_equal_objects_are_expected_to_be_not_equal_it_should_not_fail()
        {
            // Arrange
            var someObject = new ClassWithCustomEqualMethod(1);
            var nonEqualObject = new ClassWithCustomEqualMethod(2);

            // Act / Assert
            someObject.Should().NotBe(nonEqualObject);
        }

        [Fact]
        public void When_two_equal_objects_are_expected_not_to_be_equal_it_should_fail_with_a_clear_explanation()
        {
            // Arrange
            var someObject = new ClassWithCustomEqualMethod(1);
            var equalObject = new ClassWithCustomEqualMethod(1);

            // Act
            Action act = () =>
                someObject.Should().NotBe(equalObject);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect someObject to be equal to ClassWithCustomEqualMethod(1).");
        }

        [Fact]
        public void When_two_equal_objects_are_expected_not_to_be_equal_it_should_fail_and_use_the_reason()
        {
            // Arrange
            var someObject = new ClassWithCustomEqualMethod(1);
            var equalObject = new ClassWithCustomEqualMethod(1);

            // Act
            Action act = () =>
                someObject.Should().NotBe(equalObject, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect someObject to be equal to ClassWithCustomEqualMethod(1) " +
                "because we want to test the failure message.");
        }

        [Fact]
        public void An_untyped_value_is_not_equal_to_another_according_to_a_comparer()
        {
            // Arrange
            object value = new SomeClass(5);

            // Act / Assert
            value.Should().NotBe(new SomeClass(4), new SomeClassEqualityComparer());
        }

        [Fact]
        public void A_typed_value_is_not_equal_to_another_according_to_a_comparer()
        {
            // Arrange
            var value = new SomeClass(5);

            // Act / Assert
            value.Should().NotBe(new SomeClass(4), new SomeClassEqualityComparer());
        }

        [Fact]
        public void An_untyped_value_is_equal_to_another_according_to_a_comparer()
        {
            // Arrange
            object value = new SomeClass(3);

            // Act
            Action act = () => value.Should().NotBe(new SomeClass(3), new SomeClassEqualityComparer(), "I said so");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Did not expect value to be equal to SomeClass(3)*I said so*");
        }

        [Fact]
        public void A_typed_value_is_equal_to_another_according_to_a_comparer()
        {
            // Arrange
            var value = new SomeClass(3);

            // Act
            Action act = () => value.Should().NotBe(new SomeClass(3), new SomeClassEqualityComparer(), "I said so");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Did not expect value to be equal to SomeClass(3)*I said so*");
        }

        [Fact]
        public void A_typed_value_is_not_of_the_same_type()
        {
            // Arrange
            var value = new ClassWithCustomEqualMethod(3);

            // Act / Assert
            value.Should().NotBe(new SomeClass(3), new SomeClassEqualityComparer(), "I said so");
        }

        [Fact]
        public void An_untyped_value_requires_a_comparer()
        {
            // Arrange
            object value = new SomeClass(3);

            // Act
            Action act = () => value.Should().NotBe(new SomeClass(3), comparer: null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithParameterName("comparer");
        }

        [Fact]
        public void A_typed_value_requires_a_comparer()
        {
            // Arrange
            var value = new SomeClass(3);

            // Act
            Action act = () => value.Should().NotBe(new SomeClass(3), comparer: null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithParameterName("comparer");
        }

        [Fact]
        public void Chaining_after_one_assertion()
        {
            // Arrange
            var value = new SomeClass(3);

            // Act / Assert
            value.Should().NotBe(new SomeClass(3)).And.NotBeNull();
        }

        [Fact]
        public void Can_chain_multiple_assertions()
        {
            // Arrange
            var value = new object();

            // Act / Assert
            value.Should().NotBe<object>(new object(), new DumbObjectEqualityComparer()).And.NotBeNull();
        }
    }

    private enum MyEnum
    {
        One = 1,
        Two = 2
    }
}
