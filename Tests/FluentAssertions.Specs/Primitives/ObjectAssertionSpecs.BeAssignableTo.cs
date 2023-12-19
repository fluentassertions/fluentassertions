using System;
using System.Collections.Generic;
using FluentAssertionsAsync.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class ObjectAssertionSpecs
{
    public class BeAssignableTo
    {
        [Fact]
        public void When_object_type_is_matched_against_null_type_it_should_throw()
        {
            // Arrange
            var someObject = new object();

            // Act
            Action act = () => someObject.Should().BeAssignableTo(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("type");
        }

        [Fact]
        public void When_its_own_type_it_should_succeed()
        {
            // Arrange
            var someObject = new DummyImplementingClass();

            // Act / Assert
            someObject.Should().BeAssignableTo<DummyImplementingClass>();
        }

        [Fact]
        public void When_its_base_type_it_should_succeed()
        {
            // Arrange
            var someObject = new DummyImplementingClass();

            // Act / Assert
            someObject.Should().BeAssignableTo<DummyBaseClass>();
        }

        [Fact]
        public void When_an_implemented_interface_type_it_should_succeed()
        {
            // Arrange
            var someObject = new DummyImplementingClass();

            // Act / Assert
            someObject.Should().BeAssignableTo<IDisposable>();
        }

        [Fact]
        public void When_an_unrelated_type_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            var someObject = new DummyImplementingClass();
            Action act = () => someObject.Should().BeAssignableTo<DateTime>("because we want to test the failure {0}", "message");

            // Act / Assert
            act.Should().Throw<XunitException>()
                .WithMessage($"*assignable to {typeof(DateTime)}*failure message*{typeof(DummyImplementingClass)} is not*");
        }

        [Fact]
        public void When_to_the_expected_type_it_should_cast_the_returned_object_for_chaining()
        {
            // Arrange
            var someObject = new Exception("Actual Message");

            // Act
            Action act = () => someObject.Should().BeAssignableTo<Exception>().Which.Message.Should().Be("Other Message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*Expected*Actual*Other*");
        }

        [Fact]
        public void When_a_null_instance_is_asserted_to_be_assignableOfT_it_should_fail()
        {
            // Arrange
            object someObject = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                someObject.Should().BeAssignableTo<DateTime>("because we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage($"*assignable to {typeof(DateTime)}*failure message*found <null>*");
        }

        [Fact]
        public void When_its_own_type_instance_it_should_succeed()
        {
            // Arrange
            var someObject = new DummyImplementingClass();

            // Act / Assert
            someObject.Should().BeAssignableTo(typeof(DummyImplementingClass));
        }

        [Fact]
        public void When_its_base_type_instance_it_should_succeed()
        {
            // Arrange
            var someObject = new DummyImplementingClass();

            // Act / Assert
            someObject.Should().BeAssignableTo(typeof(DummyBaseClass));
        }

        [Fact]
        public void When_an_implemented_interface_type_instance_it_should_succeed()
        {
            // Arrange
            var someObject = new DummyImplementingClass();

            // Act / Assert
            someObject.Should().BeAssignableTo(typeof(IDisposable));
        }

        [Fact]
        public void When_an_implemented_open_generic_interface_type_instance_it_should_succeed()
        {
            // Arrange
            var someObject = new List<string>();

            // Act / Assert
            someObject.Should().BeAssignableTo(typeof(IList<>));
        }

        [Fact]
        public void When_a_null_instance_is_asserted_to_be_assignable_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            object someObject = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                someObject.Should().BeAssignableTo(typeof(DateTime), "because we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage($"*assignable to {typeof(DateTime)}*failure message*found <null>*");
        }

        [Fact]
        public void When_an_unrelated_type_instance_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            var someObject = new DummyImplementingClass();

            Action act = () =>
                someObject.Should().BeAssignableTo(typeof(DateTime), "because we want to test the failure {0}", "message");

            // Act / Assert
            act.Should().Throw<XunitException>()
                .WithMessage($"*assignable to {typeof(DateTime)}*failure message*{typeof(DummyImplementingClass)} is not*");
        }

        [Fact]
        public void When_unrelated_to_open_generic_type_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            var someObject = new DummyImplementingClass();

            Action act = () =>
                someObject.Should().BeAssignableTo(typeof(IList<>), "because we want to test the failure {0}", "message");

            // Act / Assert
            act.Should().Throw<XunitException>()
                .WithMessage($"*assignable to {typeof(IList<>)}*failure message*{typeof(DummyImplementingClass)} is not*");
        }

        [Fact]
        public void When_an_assertion_fails_on_BeAssignableTo_succeeding_message_should_be_included()
        {
            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                var item = string.Empty;
                item.Should().BeAssignableTo<int>();
                item.Should().BeAssignableTo<long>();
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected * to be assignable to System.Int32, but System.String is not.*" +
                    "Expected * to be assignable to System.Int64, but System.String is not.");
        }
    }

    public class NotBeAssignableTo
    {
        [Fact]
        public void When_object_type_is_matched_negatively_against_null_type_it_should_throw()
        {
            // Arrange
            var someObject = new object();

            // Act
            Action act = () => someObject.Should().NotBeAssignableTo(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("type");
        }

        [Fact]
        public void When_its_own_type_and_asserting_not_assignable_it_should_fail_with_a_useful_message()
        {
            // Arrange
            var someObject = new DummyImplementingClass();

            Action act = () =>
                someObject.Should()
                    .NotBeAssignableTo<DummyImplementingClass>("because we want to test the failure {0}", "message");

            // Act / Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    $"*not be assignable to {typeof(DummyImplementingClass)}*failure message*{typeof(DummyImplementingClass)} is*");
        }

        [Fact]
        public void When_its_base_type_and_asserting_not_assignable_it_should_fail_with_a_useful_message()
        {
            // Arrange
            var someObject = new DummyImplementingClass();

            Action act = () =>
                someObject.Should().NotBeAssignableTo<DummyBaseClass>("because we want to test the failure {0}", "message");

            // Act / Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    $"*not be assignable to {typeof(DummyBaseClass)}*failure message*{typeof(DummyImplementingClass)} is*");
        }

        [Fact]
        public void When_an_implemented_interface_type_and_asserting_not_assignable_it_should_fail_with_a_useful_message()
        {
            // Arrange
            var someObject = new DummyImplementingClass();

            Action act = () =>
                someObject.Should().NotBeAssignableTo<IDisposable>("because we want to test the failure {0}", "message");

            // Act / Assert
            act.Should().Throw<XunitException>()
                .WithMessage($"*not be assignable to {typeof(IDisposable)}*failure message*{typeof(DummyImplementingClass)} is*");
        }

        [Fact]
        public void When_an_unrelated_type_and_asserting_not_assignable_it_should_succeed()
        {
            // Arrange
            var someObject = new DummyImplementingClass();

            // Act / Assert
            someObject.Should().NotBeAssignableTo<DateTime>();
        }

        [Fact]
        public void
            When_not_to_the_unexpected_type_and_asserting_not_assignable_it_should_not_cast_the_returned_object_for_chaining()
        {
            // Arrange
            var someObject = new Exception("Actual Message");

            // Act
            Action act = () => someObject.Should().NotBeAssignableTo<DummyImplementingClass>()
                .And.Subject.Should().BeOfType<Exception>()
                .Which.Message.Should().Be("Other Message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*Expected*Actual*Other*");
        }

        [Fact]
        public void When_its_own_type_instance_and_asserting_not_assignable_it_should_fail_with_a_useful_message()
        {
            // Arrange
            var someObject = new DummyImplementingClass();

            Action act = () =>
                someObject.Should().NotBeAssignableTo(typeof(DummyImplementingClass), "because we want to test the failure {0}",
                    "message");

            // Act / Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    $"*not be assignable to {typeof(DummyImplementingClass)}*failure message*{typeof(DummyImplementingClass)} is*");
        }

        [Fact]
        public void When_its_base_type_instance_and_asserting_not_assignable_it_should_fail_with_a_useful_message()
        {
            // Arrange
            var someObject = new DummyImplementingClass();

            Action act = () =>
                someObject.Should()
                    .NotBeAssignableTo(typeof(DummyBaseClass), "because we want to test the failure {0}", "message");

            // Act / Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    $"*not be assignable to {typeof(DummyBaseClass)}*failure message*{typeof(DummyImplementingClass)} is*");
        }

        [Fact]
        public void
            When_an_implemented_interface_type_instance_and_asserting_not_assignable_it_should_fail_with_a_useful_message()
        {
            // Arrange
            var someObject = new DummyImplementingClass();

            Action act = () =>
                someObject.Should().NotBeAssignableTo(typeof(IDisposable), "because we want to test the failure {0}", "message");

            // Act / Assert
            act.Should().Throw<XunitException>()
                .WithMessage($"*not be assignable to {typeof(IDisposable)}*failure message*{typeof(DummyImplementingClass)} is*");
        }

        [Fact]
        public void
            When_an_implemented_open_generic_interface_type_instance_and_asserting_not_assignable_it_should_fail_with_a_useful_message()
        {
            // Arrange
            var someObject = new List<string>();

            Action act = () =>
                someObject.Should().NotBeAssignableTo(typeof(IList<>), "because we want to test the failure {0}", "message");

            // Act / Assert
            act.Should().Throw<XunitException>()
                .WithMessage($"*not be assignable to {typeof(IList<>)}*failure message*{typeof(List<string>)} is*");
        }

        [Fact]
        public void When_a_null_instance_is_asserted_to_not_be_assignable_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            object someObject = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                someObject.Should().NotBeAssignableTo(typeof(DateTime), "because we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage($"*not be assignable to {typeof(DateTime)}*failure message*found <null>*");
        }

        [Fact]
        public void When_an_unrelated_type_instance_and_asserting_not_assignable_it_should_succeed()
        {
            // Arrange
            var someObject = new DummyImplementingClass();

            // Act / Assert
            someObject.Should().NotBeAssignableTo(typeof(DateTime), "because we want to test the failure {0}", "message");
        }

        [Fact]
        public void When_unrelated_to_open_generic_type_and_asserting_not_assignable_it_should_succeed()
        {
            // Arrange
            var someObject = new DummyImplementingClass();

            // Act / Assert
            someObject.Should().NotBeAssignableTo(typeof(IList<>), "because we want to test the failure {0}", "message");
        }
    }
}
