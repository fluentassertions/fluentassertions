using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

using FluentAssertions.Extensions;
using FluentAssertions.Primitives;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class ObjectAssertionSpecs
    {
        #region Be / NotBe

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

        #endregion

        #region BeNull / BeNotNull

        [Fact]
        public void Should_succeed_when_asserting_null_object_to_be_null()
        {
            // Arrange
            object someObject = null;

            // Act / Assert
            someObject.Should().BeNull();
        }

        [Fact]
        public void Should_fail_when_asserting_non_null_object_to_be_null()
        {
            // Arrange
            var someObject = new object();

            // Act
            Action act = () => someObject.Should().BeNull();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_a_non_null_object_is_expected_to_be_null_it_should_fail()
        {
            // Arrange
            var someObject = new object();

            // Act
            Action act = () => someObject.Should().BeNull("because we want to test the failure {0}", "message");

            // Assert
            act
                .Should().Throw<XunitException>()
                .Where(e => e.Message.StartsWith(
                    "Expected someObject to be <null> because we want to test the failure message, but found System.Object"));
        }

        [Fact]
        public void Should_succeed_when_asserting_non_null_object_not_to_be_null()
        {
            // Arrange
            var someObject = new object();

            // Act / Assert
            someObject.Should().NotBeNull();
        }

        [Fact]
        public void Should_fail_when_asserting_null_object_not_to_be_null()
        {
            // Arrange
            object someObject = null;

            // Act
            Action act = () => someObject.Should().NotBeNull();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_null_object_not_to_be_null()
        {
            // Arrange
            object someObject = null;

            // Act
            Action act = () => someObject.Should().NotBeNull("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected someObject not to be <null> because we want to test the failure message.");
        }

        #endregion

        #region BeOfType / NotBeOfType

        [Fact]
        public void When_object_type_is_matched_against_null_type_exactly_it_should_throw()
        {
            // Arrange
            var someObject = new object();

            // Act
            Action act = () => someObject.Should().BeOfType(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .Which.ParamName.Should().Be("expectedType");
        }

        [Fact]
        public void When_object_type_is_matched_negatively_against_null_type_exactly_it_should_throw()
        {
            // Arrange
            var someObject = new object();

            // Act
            Action act = () => someObject.Should().NotBeOfType(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .Which.ParamName.Should().Be("unexpectedType");
        }

        [Fact]
        public void When_object_type_is_exactly_equal_to_the_specified_type_it_should_not_fail()
        {
            // Arrange
            var someObject = new Exception();

            // Act
            Action act = () => someObject.Should().BeOfType<Exception>();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_object_type_is_value_type_and_matches_received_type_should_not_fail_and_assert_correctly()
        {
            // Arrange
            int valueTypeObject = 42;

            // Act
            Action act = () => valueTypeObject.Should().BeOfType(typeof(int));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_object_is_matched_against_a_null_type_it_should_throw()
        {
            // Arrange
            int valueTypeObject = 42;

            // Act
            Action act = () => valueTypeObject.Should().BeOfType(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .Which.ParamName.Should().Be("expectedType");
        }

        [Fact]
        public void When_null_object_is_matched_against_a_type_it_should_throw()
        {
            // Arrange
            int? valueTypeObject = null;

            // Act
            Action act = () => valueTypeObject.Should().BeOfType(typeof(int), "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*type to be System.Int32*because we want to test the failure message*");
        }

        [Fact]
        public void When_object_is_matched_negatively_against_a_null_type_it_should_throw()
        {
            // Arrange
            int valueTypeObject = 42;

            // Act
            Action act = () => valueTypeObject.Should().NotBeOfType(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .Which.ParamName.Should().Be("unexpectedType");
        }

        [Fact]
        public void When_object_type_is_value_type_and_doesnt_match_received_type_as_expected_should_not_fail_and_assert_correctly()
        {
            // Arrange
            int valueTypeObject = 42;

            // Act
            Action act = () => valueTypeObject.Should().NotBeOfType(typeof(double));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_null_object_is_matched_negatively_against_a_type_it_should_throw()
        {
            // Arrange
            int? valueTypeObject = null;

            // Act
            Action act = () => valueTypeObject.Should().NotBeOfType(typeof(int), "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*type not to be System.Int32*because we want to test the failure message*");
        }

        [Fact]
        public void When_object_type_is_value_type_and_matches_received_type_not_as_expected_should_fail()
        {
            // Arrange
            int valueTypeObject = 42;
            var expectedType = typeof(int);

            // Act
            Action act = () => valueTypeObject.Should().NotBeOfType(expectedType);

            // Assert
            act.Should().Throw<XunitException>().WithMessage($"Expected type not to be [{expectedType.AssemblyQualifiedName}], but it is.");
        }

        [Fact]
        public void When_object_type_is_value_type_and_doesnt_match_received_type_should_fail()
        {
            // Arrange
            int valueTypeObject = 42;
            var doubleType = typeof(double);

            // Act
            Action act = () => valueTypeObject.Should().BeOfType(doubleType);

            // Assert
            act.Should().Throw<XunitException>().WithMessage($"Expected type to be {doubleType}, but found {valueTypeObject.GetType()}.");
        }

        [Fact]
        public void When_object_is_of_the_expected_type_it_should_cast_the_returned_object_for_chaining()
        {
            // Arrange
            var someObject = new Exception("Actual Message");

            // Act
            Action act = () => someObject.Should().BeOfType<Exception>().Which.Message.Should().Be("Other Message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*Expected*Other*Actual*");
        }

        [Fact]
        public void When_object_type_is_different_than_expected_type_it_should_fail_with_descriptive_message()
        {
            // Arrange
            var someObject = new object();

            // Act
            Action act = () => someObject.Should().BeOfType<int>("because they are {0} {1}", "of different", "type");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected type to be System.Int32 because they are of different type, but found System.Object.");
        }

        [Fact]
        public void When_asserting_the_type_of_a_null_object_it_should_throw()
        {
            // Arrange
            object someObject = null;

            // Act
            Action act = () => someObject.Should().BeOfType<int>();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected someObject to be System.Int32, but found <null>.");
        }

        [Fact]
        public void Then_object_type_is_same_as_expected_type_but_in_different_assembly_it_should_fail_with_assembly_qualified_name
            ()
        {
            // Arrange
            var assertionsFromOtherAssembly = new object().Should();

            // Act
#pragma warning disable 436 // disable the warning on conflicting types, as this is the intention for the spec

            Action act = () =>
                assertionsFromOtherAssembly.Should().BeOfType<ObjectAssertions>();

#pragma warning restore 436

            // Assert
            const string expectedMessage =
                "Expected type to be [FluentAssertions.Primitives.ObjectAssertions, FluentAssertions.*]" +
                ", but found [FluentAssertions.Primitives.ObjectAssertions, FluentAssertions*].";

            act.Should().Throw<XunitException>().WithMessage(expectedMessage);
        }

        [Fact]
        public void When_object_type_is_a_subclass_of_the_expected_type_it_should_fail()
        {
            // Arrange
            var someObject = new DummyImplementingClass();

            // Act
            Action act = () => someObject.Should().BeOfType<DummyBaseClass>();

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected type to be FluentAssertions.Specs.DummyBaseClass, but found FluentAssertions.Specs.DummyImplementingClass.");
        }

        #endregion

        #region BeAssignableTo

        [Fact]
        public void When_object_type_is_matched_against_null_type_it_should_throw()
        {
            // Arrange
            var someObject = new object();

            // Act
            Action act = () => someObject.Should().BeAssignableTo(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .Which.ParamName.Should().Be("type");
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
            act.Should().Throw<XunitException>().WithMessage("*Expected*Other*Actual*");
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
            var someObject = new System.Collections.Generic.List<string>();

            // Act / Assert
            someObject.Should().BeAssignableTo(typeof(System.Collections.Generic.IList<>));
        }

        [Fact]
        public void When_a_null_instance_is_asserted_to_be_assignable_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            object someObject = null;
            Action act = () => someObject.Should().BeAssignableTo(typeof(DateTime), "because we want to test the failure {0}", "message");

            // Act / Assert
            act.Should().Throw<XunitException>()
                .WithMessage($"*assignable to {typeof(DateTime)}*failure message*found <null>*");
        }

        [Fact]
        public void When_an_unrelated_type_instance_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            var someObject = new DummyImplementingClass();
            Action act = () => someObject.Should().BeAssignableTo(typeof(DateTime), "because we want to test the failure {0}", "message");

            // Act / Assert
            act.Should().Throw<XunitException>()
                .WithMessage($"*assignable to {typeof(DateTime)}*failure message*{typeof(DummyImplementingClass)} is not*");
        }

        [Fact]
        public void When_unrelated_to_open_generic_type_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            var someObject = new DummyImplementingClass();
            Action act = () => someObject.Should().BeAssignableTo(typeof(System.Collections.Generic.IList<>), "because we want to test the failure {0}", "message");

            // Act / Assert
            act.Should().Throw<XunitException>()
                .WithMessage($"*assignable to {typeof(System.Collections.Generic.IList<>)}*failure message*{typeof(DummyImplementingClass)} is not*");
        }

        #endregion

        #region NotBeAssignableTo

        [Fact]
        public void When_object_type_is_matched_negatively_against_null_type_it_should_throw()
        {
            // Arrange
            var someObject = new object();

            // Act
            Action act = () => someObject.Should().NotBeAssignableTo(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .Which.ParamName.Should().Be("type");
        }

        [Fact]
        public void When_its_own_type_and_asserting_not_assignable_it_should_fail_with_a_useful_message()
        {
            // Arrange
            var someObject = new DummyImplementingClass();
            Action act = () => someObject.Should().NotBeAssignableTo<DummyImplementingClass>("because we want to test the failure {0}", "message");

            // Act / Assert
            act.Should().Throw<XunitException>()
                .WithMessage($"*not be assignable to {typeof(DummyImplementingClass)}*failure message*{typeof(DummyImplementingClass)} is*");
        }

        [Fact]
        public void When_its_base_type_and_asserting_not_assignable_it_should_fail_with_a_useful_message()
        {
            // Arrange
            var someObject = new DummyImplementingClass();
            Action act = () => someObject.Should().NotBeAssignableTo<DummyBaseClass>("because we want to test the failure {0}", "message");

            // Act / Assert
            act.Should().Throw<XunitException>()
                .WithMessage($"*not be assignable to {typeof(DummyBaseClass)}*failure message*{typeof(DummyImplementingClass)} is*");
        }

        [Fact]
        public void When_an_implemented_interface_type_and_asserting_not_assignable_it_should_fail_with_a_useful_message()
        {
            // Arrange
            var someObject = new DummyImplementingClass();
            Action act = () => someObject.Should().NotBeAssignableTo<IDisposable>("because we want to test the failure {0}", "message");

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
        public void When_not_to_the_unexpected_type_and_asserting_not_assignable_it_should_not_cast_the_returned_object_for_chaining()
        {
            // Arrange
            var someObject = new Exception("Actual Message");

            // Act
            Action act = () => someObject.Should().NotBeAssignableTo<DummyImplementingClass>()
                .And.Subject.Should().BeOfType<Exception>()
                .Which.Message.Should().Be("Other Message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*Expected*Other*Actual*");
        }

        [Fact]
        public void When_its_own_type_instance_and_asserting_not_assignable_it_should_fail_with_a_useful_message()
        {
            // Arrange
            var someObject = new DummyImplementingClass();
            Action act = () => someObject.Should().NotBeAssignableTo(typeof(DummyImplementingClass), "because we want to test the failure {0}", "message");

            // Act / Assert
            act.Should().Throw<XunitException>()
                .WithMessage($"*not be assignable to {typeof(DummyImplementingClass)}*failure message*{typeof(DummyImplementingClass)} is*");
        }

        [Fact]
        public void When_its_base_type_instance_and_asserting_not_assignable_it_should_fail_with_a_useful_message()
        {
            // Arrange
            var someObject = new DummyImplementingClass();
            Action act = () => someObject.Should().NotBeAssignableTo(typeof(DummyBaseClass), "because we want to test the failure {0}", "message");

            // Act / Assert
            act.Should().Throw<XunitException>()
                .WithMessage($"*not be assignable to {typeof(DummyBaseClass)}*failure message*{typeof(DummyImplementingClass)} is*");
        }

        [Fact]
        public void When_an_implemented_interface_type_instance_and_asserting_not_assignable_it_should_fail_with_a_useful_message()
        {
            // Arrange
            var someObject = new DummyImplementingClass();
            Action act = () => someObject.Should().NotBeAssignableTo(typeof(IDisposable), "because we want to test the failure {0}", "message");

            // Act / Assert
            act.Should().Throw<XunitException>()
                .WithMessage($"*not be assignable to {typeof(IDisposable)}*failure message*{typeof(DummyImplementingClass)} is*");
        }

        [Fact]
        public void When_an_implemented_open_generic_interface_type_instance_and_asserting_not_assignable_it_should_fail_with_a_useful_message()
        {
            // Arrange
            var someObject = new System.Collections.Generic.List<string>();
            Action act = () => someObject.Should().NotBeAssignableTo(typeof(System.Collections.Generic.IList<>), "because we want to test the failure {0}", "message");

            // Act / Assert
            act.Should().Throw<XunitException>()
                .WithMessage($"*not be assignable to {typeof(System.Collections.Generic.IList<>)}*failure message*{typeof(System.Collections.Generic.List<string>)} is*");
        }

        [Fact]
        public void When_a_null_instance_is_asserted_to_not_be_assignable_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            object someObject = null;
            Action act = () => someObject.Should().NotBeAssignableTo(typeof(DateTime), "because we want to test the failure {0}", "message");

            // Act / Assert
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
            someObject.Should().NotBeAssignableTo(typeof(System.Collections.Generic.IList<>), "because we want to test the failure {0}", "message");
        }

        #endregion

        #region HaveFlag / NotHaveFlag

        [Fact]
        public void When_enum_has_the_expected_flag_it_should_succeed()
        {
            // Arrange
            TestEnum someObject = TestEnum.One | TestEnum.Two;

            // Act / Assert
            someObject.Should().HaveFlag(TestEnum.Two);
        }

        [Fact]
        public void When_object_is_not_enum_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            object someObject = new object();

            // Act
            Action act = () => someObject.Should().HaveFlag(TestEnum.Three);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected*type*TestEnum*Object*");
        }

        [Fact]
        public void When_object_is_not_enum_and_null_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            object someObject = null;

            // Act
            Action act = () => someObject.Should().HaveFlag(TestEnum.Three);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected*type*but found <null>*");
        }

        [Fact]
        public void When_enum_does_not_have_specified_flag_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            TestEnum someObject = TestEnum.One | TestEnum.Two;

            // Act
            Action act = () => someObject.Should().HaveFlag(TestEnum.Three);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("The enum was expected to have flag Three but found One, Two.");
        }

        [Fact]
        public void When_enum_is_not_of_the_same_type_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            TestEnum someObject = TestEnum.One | TestEnum.Two;

            // Act
            Action act = () => someObject.Should().HaveFlag(OtherEnum.First);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected*type*OtherEnum*TestEnum*");
        }

        [Fact]
        public void When_enum_does_not_have_the_unexpected_flag_it_should_succeed()
        {
            // Arrange
            TestEnum someObject = TestEnum.One | TestEnum.Two;

            // Act / Assert
            someObject.Should().NotHaveFlag(TestEnum.Three);
        }

        [Fact]
        public void When_enum_does_have_specified_flag_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            TestEnum someObject = TestEnum.One | TestEnum.Two;

            // Act
            Action act = () => someObject.Should().NotHaveFlag(TestEnum.Two);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Did not expect the enum to have flag Two.");
        }

        [Fact]
        public void When_object_should_not_have_flag_but_is_not_an_enum_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            object someObject = new object();

            // Act
            Action act = () => someObject.Should().NotHaveFlag(TestEnum.Three);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected*type*TestEnum*Object*");
        }

        [Fact]
        public void When_object_should_not_have_flag_but_is_null_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            object someObject = null;

            // Act
            Action act = () => someObject.Should().NotHaveFlag(TestEnum.Three);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected*type*but found <null>*");
        }

        [Flags]
        public enum TestEnum
        {
            None = 0,
            One = 1,
            Two = 2,
            Three = 4
        }

        [Flags]
        public enum OtherEnum
        {
            Default,
            First,
            Second
        }

        #endregion

        #region Miscellaneous

        [Fact]
        public void Should_support_chaining_constraints_with_and()
        {
            // Arrange
            var someObject = new Exception();

            // Act / Assert
            someObject.Should()
                .BeOfType<Exception>()
                .And
                .NotBeNull();
        }

        #endregion

        #region BeBinarySerializable

        [Fact]
        public void When_an_object_is_binary_serializable_it_should_succeed()
        {
            // Arrange
            var subject = new SerializableClass
            {
                Name = "John",
                Id = 2
            };

            // Act
            Action act = () => subject.Should().BeBinarySerializable();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_an_object_is_binary_serializable_with_non_serializable_members_it_should_succeed()
        {
            // Arrange
            var subject = new SerializableClassWithNonSerializableMember()
            {
                Name = "John",
                NonSerializable = "Nonserializable value"
            };

            // Act
            Action act = () => subject.Should().BeBinarySerializable<SerializableClassWithNonSerializableMember>(options =>
                options.Excluding(s => s.NonSerializable));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_injecting_null_options_it_should_throw()
        {
            // Arrange
            var subject = new SerializableClassWithNonSerializableMember();

            // Act
            Action act = () => subject.Should().BeBinarySerializable<SerializableClassWithNonSerializableMember>(options: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().Be("options");
        }

        [Fact]
        public void When_an_object_is_not_binary_serializable_it_should_fail()
        {
            // Arrange
            var subject = new UnserializableClass
            {
                Name = "John"
            };

            // Act
            Action act = () => subject.Should().BeBinarySerializable("we need to store it on {0}", "disk");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*to be serializable because we need to store it on disk, but serialization failed with:*UnserializableClass*");
        }

        [Fact]
        public void When_an_object_is_binary_serializable_but_not_deserializable_it_should_fail()
        {
            // Arrange
            var subject = new BinarySerializableClassMissingDeserializationConstructor
            {
                Name = "John",
                BirthDay = 20.September(1973)
            };

            // Act
            Action act = () => subject.Should().BeBinarySerializable();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*to be serializable, but serialization failed with:*BinarySerializableClassMissingDeserializationConstructor*");
        }

        [Fact]
        public void When_an_object_is_binary_serializable_but_doesnt_restore_all_properties_it_should_fail()
        {
            // Arrange
            var subject = new BinarySerializableClassNotRestoringAllProperties
            {
                Name = "John",
                BirthDay = 20.September(1973)
            };

            // Act
            Action act = () => subject.Should().BeBinarySerializable();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*to be serializable, but serialization failed with:*member Name to be*");
        }

        [Fact]
        public void When_a_system_exception_is_asserted_to_be_serializable_it_should_compare_its_fields_and_properties()
        {
            // Arrange
            var subject = new Exception("some error");

            // Act
            Action act = () => subject.Should().BeBinarySerializable();

            // Assert
            act.Should().NotThrow();
        }

        internal class UnserializableClass
        {
            public string Name { get; set; }
        }

        [Serializable]
        public class SerializableClass
        {
            public string Name { get; set; }

            public int Id;
        }

        [Serializable]
        public class SerializableClassWithNonSerializableMember
        {
            [NonSerialized]
            private string nonSerializable;

            public string Name { get; set; }

            public string NonSerializable
            {
                get { return nonSerializable; }
                set { nonSerializable = value; }
            }
        }

        [Serializable]
        internal class BinarySerializableClassMissingDeserializationConstructor : ISerializable
        {
            public string Name { get; set; }
            public DateTime BirthDay { get; set; }

            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
            }
        }

        [Serializable]
        internal class BinarySerializableClassNotRestoringAllProperties : ISerializable
        {
            public string Name { get; set; }
            public DateTime BirthDay { get; set; }

            public BinarySerializableClassNotRestoringAllProperties()
            {
            }

            public BinarySerializableClassNotRestoringAllProperties(SerializationInfo info, StreamingContext context)
            {
                BirthDay = info.GetDateTime("BirthDay");
            }

            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("BirthDay", BirthDay);
            }
        }

        #endregion

        #region BeXmlSerializable

        [Fact]
        public void When_an_object_is_xml_serializable_it_should_succeed()
        {
            // Arrange
            var subject = new XmlSerializableClass
            {
                Name = "John",
                Id = 1
            };

            // Act
            Action act = () => subject.Should().BeXmlSerializable();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_an_object_is_not_xml_serializable_it_should_fail()
        {
            // Arrange
            var subject = new NonPublicClass
            {
                Name = "John"
            };

            // Act
            Action act = () => subject.Should().BeXmlSerializable("we need to store it on {0}", "disk");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*to be serializable because we need to store it on disk, but serialization failed with:*NonPublicClass*");
        }

        [Fact]
        public void When_an_object_is_xml_serializable_but_doesnt_restore_all_properties_it_should_fail()
        {
            // Arrange
            var subject = new XmlSerializableClassNotRestoringAllProperties
            {
                Name = "John",
                BirthDay = 20.September(1973)
            };

            // Act
            Action act = () => subject.Should().BeXmlSerializable();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*to be serializable, but serialization failed with:*member Name to be*");
        }

        internal class NonPublicClass
        {
            public string Name { get; set; }
        }

        public class XmlSerializableClass
        {
            public string Name { get; set; }

            public int Id;
        }

        public class XmlSerializableClassNotRestoringAllProperties : IXmlSerializable
        {
            public string Name { get; set; }
            public DateTime BirthDay { get; set; }

            public XmlSchema GetSchema()
            {
                return null;
            }

            public void ReadXml(XmlReader reader)
            {
                BirthDay = DateTime.Parse(reader.ReadElementContentAsString(), CultureInfo.InvariantCulture);
            }

            public void WriteXml(XmlWriter writer)
            {
                writer.WriteString(BirthDay.ToString(CultureInfo.InvariantCulture));
            }
        }

        #endregion

        #region BeDataContractSerializable

        [Fact]
        public void When_an_object_is_data_contract_serializable_it_should_succeed()
        {
            // Arrange
            var subject = new DataContractSerializableClass
            {
                Name = "John",
                Id = 1
            };

            // Act
            Action act = () => subject.Should().BeDataContractSerializable();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_an_object_is_not_data_contract_serializable_it_should_fail()
        {
            // Arrange
            var subject = new NonDataContractSerializableClass();

            // Act
            Action act = () => subject.Should().BeDataContractSerializable("we need to store it on {0}", "disk");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("*we need to store it on disk*EnumMemberAttribute*");
        }

        [Fact]
        public void When_an_object_is_data_contract_serializable_but_doesnt_restore_all_properties_it_should_fail()
        {
            // Arrange
            var subject = new DataContractSerializableClassNotRestoringAllProperties
            {
                Name = "John",
                BirthDay = 20.September(1973)
            };

            // Act
            Action act = () => subject.Should().BeDataContractSerializable();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*to be serializable, but serialization failed with:*member Name to be*");
        }

        [Fact]
        public void When_a_data_contract_serializable_object_doesnt_restore_an_ignored_property_it_should_succeed()
        {
            // Arrange
            var subject = new DataContractSerializableClassNotRestoringAllProperties
            {
                Name = "John",
                BirthDay = 20.September(1973)
            };

            // Act
            Action act = () => subject.Should().BeDataContractSerializable<DataContractSerializableClassNotRestoringAllProperties>(
                options => options.Excluding(x => x.Name));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_injecting_null_options_to_BeDataContractSerializable_it_should_throw()
        {
            // Arrange
            var subject = new DataContractSerializableClassNotRestoringAllProperties();

            // Act
            Action act = () => subject.Should().BeDataContractSerializable<DataContractSerializableClassNotRestoringAllProperties>(
                options: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().Be("options");
        }

        public enum Color
        {
            Red = 1,
            Yellow = 2
        }

        public class NonDataContractSerializableClass
        {
            public Color Color { get; set; }
        }

        public class DataContractSerializableClass
        {
            public string Name { get; set; }

            public int Id;
        }

        [DataContract]
        public class DataContractSerializableClassNotRestoringAllProperties
        {
            public string Name { get; set; }

            [DataMember]
            public DateTime BirthDay { get; set; }
        }

        #endregion

    }

    internal class DummyBaseClass
    {
    }

    internal class DummyImplementingClass : DummyBaseClass, IDisposable
    {
        public void Dispose()
        {
            // Ignore
        }
    }
}
