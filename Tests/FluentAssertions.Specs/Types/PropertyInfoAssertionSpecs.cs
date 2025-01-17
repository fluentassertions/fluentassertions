using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Types;

public class PropertyInfoAssertionSpecs
{
    public class BeVirtual
    {
        [Fact]
        public void When_asserting_that_a_property_is_virtual_and_it_is_then_it_succeeds()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithAllPropertiesVirtual).GetRuntimeProperty("PublicVirtualProperty");

            // Act
            Action act = () =>
                propertyInfo.Should().BeVirtual();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_that_a_property_is_virtual_and_it_is_not_then_it_fails_with_useful_message()
        {
            // Arrange
            PropertyInfo propertyInfo =
                typeof(ClassWithNonVirtualPublicProperties).GetRuntimeProperty("PublicNonVirtualProperty");

            // Act
            Action act = () =>
                propertyInfo.Should().BeVirtual("we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected property String FluentAssertions*ClassWithNonVirtualPublicProperties.PublicNonVirtualProperty" +
                    " to be virtual because we want to test the error message," +
                    " but it is not.");
        }

        [Fact]
        public void When_subject_is_null_be_virtual_should_fail()
        {
            // Arrange
            PropertyInfo propertyInfo = null;

            // Act
            Action act = () =>
                propertyInfo.Should().BeVirtual("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected property to be virtual *failure message*, but propertyInfo is <null>.");
        }
    }

    public class NotBeVirtual
    {
        [Fact]
        public void When_asserting_that_a_property_is_not_virtual_and_it_is_not_then_it_succeeds()
        {
            // Arrange
            PropertyInfo propertyInfo =
                typeof(ClassWithNonVirtualPublicProperties).GetRuntimeProperty("PublicNonVirtualProperty");

            // Act
            Action act = () =>
                propertyInfo.Should().NotBeVirtual();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_that_a_property_is_not_virtual_and_it_is_then_it_fails_with_useful_message()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithAllPropertiesVirtual).GetRuntimeProperty("PublicVirtualProperty");

            // Act
            Action act = () =>
                propertyInfo.Should().NotBeVirtual("we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected property *ClassWithAllPropertiesVirtual.PublicVirtualProperty" +
                    " not to be virtual because we want to test the error message," +
                    " but it is.");
        }

        [Fact]
        public void When_subject_is_null_not_be_virtual_should_fail()
        {
            // Arrange
            PropertyInfo propertyInfo = null;

            // Act
            Action act = () =>
                propertyInfo.Should().NotBeVirtual("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected property not to be virtual *failure message*, but propertyInfo is <null>.");
        }
    }

    public class BeDecortatedWithOfT
    {
        [Fact]
        public void When_asserting_a_property_is_decorated_with_attribute_and_it_is_it_succeeds()
        {
            // Arrange
            PropertyInfo propertyInfo =
                typeof(ClassWithAllPropertiesDecoratedWithDummyAttribute).GetRuntimeProperty("PublicProperty");

            // Act
            Action act = () =>
                propertyInfo.Should().BeDecoratedWith<DummyPropertyAttribute>();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_property_is_decorated_with_an_attribute_it_allow_chaining_assertions()
        {
            // Arrange
            PropertyInfo propertyInfo =
                typeof(ClassWithAllPropertiesDecoratedWithDummyAttribute).GetRuntimeProperty("PublicProperty");

            // Act
            Action act = () =>
                propertyInfo.Should().BeDecoratedWith<DummyPropertyAttribute>().Which.Value.Should().Be("OtherValue");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected*Value*OtherValue*");
        }

        [Fact]
        public void
            When_a_property_is_decorated_with_an_attribute_and_multiple_attributes_match_continuation_using_the_matched_value_fail()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithAllPropertiesDecoratedWithDummyAttribute).GetRuntimeProperty(
                    "PublicPropertyWithSameAttributeTwice");

            // Act
            Action act = () =>
                propertyInfo.Should().BeDecoratedWith<DummyPropertyAttribute>().Which.Value.Should().Be("OtherValue");

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_a_property_is_decorated_with_attribute_and_it_is_not_it_throw_with_useful_message()
        {
            // Arrange
            PropertyInfo propertyInfo =
                typeof(ClassWithPropertiesThatAreNotDecoratedWithDummyAttribute).GetRuntimeProperty("PublicProperty");

            // Act
            Action act = () =>
                propertyInfo.Should().BeDecoratedWith<DummyPropertyAttribute>("because we want to test the error message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected property String " +
                    "FluentAssertions*ClassWithPropertiesThatAreNotDecoratedWithDummyAttribute.PublicProperty to be decorated with " +
                    "FluentAssertions*DummyPropertyAttribute because we want to test the error message, but that attribute was not found.");
        }

        [Fact]
        public void
            When_asserting_a_property_is_decorated_with_an_attribute_matching_a_predicate_but_it_is_not_it_throw_with_useful_message()
        {
            // Arrange
            PropertyInfo propertyInfo =
                typeof(ClassWithPropertiesThatAreNotDecoratedWithDummyAttribute).GetRuntimeProperty("PublicProperty");

            // Act
            Action act = () =>
                propertyInfo.Should().BeDecoratedWith<DummyPropertyAttribute>(d => d.Value == "NotARealValue",
                    "because we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected property String FluentAssertions*ClassWithPropertiesThatAreNotDecoratedWithDummyAttribute.PublicProperty to be decorated with " +
                    "FluentAssertions*DummyPropertyAttribute because we want to test the error message," +
                    " but that attribute was not found.");
        }

        [Fact]
        public void When_asserting_a_property_is_decorated_with_attribute_matching_a_predicate_and_it_is_it_succeeds()
        {
            // Arrange
            PropertyInfo propertyInfo =
                typeof(ClassWithAllPropertiesDecoratedWithDummyAttribute).GetRuntimeProperty("PublicProperty");

            // Act
            Action act = () => propertyInfo.Should().BeDecoratedWith<DummyPropertyAttribute>(d => d.Value == "Value");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_subject_is_null_be_decorated_withOfT_should_fail()
        {
            // Arrange
            PropertyInfo propertyInfo = null;

            // Act
            Action act = () =>
                propertyInfo.Should().BeDecoratedWith<DummyPropertyAttribute>("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected property to be decorated with *.DummyPropertyAttribute *failure message*, but propertyInfo is <null>.");
        }

        [Fact]
        public void When_asserting_property_is_decorated_with_null_it_should_throw()
        {
            // Arrange
            PropertyInfo propertyInfo =
                typeof(ClassWithAllPropertiesDecoratedWithDummyAttribute).GetRuntimeProperty("PublicProperty");

            // Act
            Action act = () =>
                propertyInfo.Should().BeDecoratedWith((Expression<Func<DummyPropertyAttribute, bool>>)null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("isMatchingAttributePredicate");
        }
    }

    public class NotBeDecoratedWithOfT
    {
        [Fact]
        public void When_subject_is_null_not_be_decorated_withOfT_should_fail()
        {
            // Arrange
            PropertyInfo propertyInfo = null;

            // Act
            Action act = () =>
                propertyInfo.Should().NotBeDecoratedWith<DummyPropertyAttribute>("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected property to not be decorated with *.DummyPropertyAttribute *failure message*, but propertyInfo is <null>.");
        }

        [Fact]
        public void When_asserting_property_is_not_decorated_with_null_it_should_throw()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("StringProperty");

            // Act
            Action act = () =>
                propertyInfo.Should().NotBeDecoratedWith((Expression<Func<DummyPropertyAttribute, bool>>)null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("isMatchingAttributePredicate");
        }
    }

    public class BeWritable
    {
        [Fact]
        public void When_asserting_a_readonly_property_is_writable_it_fails_with_useful_message()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("ReadOnlyProperty");

            // Act
            Action action = () => propertyInfo.Should().BeWritable("we want to test the error {0}", "message");

            // Assert
            action
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected propertyInfo ReadOnlyProperty to have a setter because we want to test the error message.");
        }

        [Fact]
        public void When_asserting_a_readwrite_property_is_writable_it_succeeds()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("ReadWriteProperty");

            // Act
            Action action = () => propertyInfo.Should().BeWritable("that's required");

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_writeonly_property_is_writable_it_succeeds()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("WriteOnlyProperty");

            // Act
            Action action = () => propertyInfo.Should().BeWritable("that's required");

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_subject_is_null_be_writable_should_fail()
        {
            // Arrange
            PropertyInfo propertyInfo = null;

            // Act
            Action act = () =>
                propertyInfo.Should().BeWritable("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected property to have a setter *failure message*, but propertyInfo is <null>.");
        }
    }

    public class BeReadable
    {
        [Fact]
        public void When_asserting_a_readonly_property_is_readable_it_succeeds()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("ReadOnlyProperty");

            // Act
            Action action = () => propertyInfo.Should().BeReadable("that's required");

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_readwrite_property_is_readable_it_succeeds()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithReadOnlyProperties).GetRuntimeProperty("ReadWriteProperty");

            // Act
            Action action = () => propertyInfo.Should().BeReadable("that's required");

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_writeonly_property_is_readable_it_fails_with_useful_message()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("WriteOnlyProperty");

            // Act
            Action action = () => propertyInfo.Should().BeReadable("we want to test the error {0}", "message");

            // Assert
            action
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected property WriteOnlyProperty to have a getter because we want to test the error message, but it does not.");
        }

        [Fact]
        public void When_subject_is_null_be_readable_should_fail()
        {
            // Arrange
            PropertyInfo propertyInfo = null;

            // Act
            Action act = () =>
                propertyInfo.Should().BeReadable("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected property to have a getter *failure message*, but propertyInfo is <null>.");
        }
    }

    public class NotBeWritable
    {
        [Fact]
        public void When_asserting_a_readonly_property_is_not_writable_it_succeeds()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithReadOnlyProperties).GetRuntimeProperty("ReadOnlyProperty");

            // Act
            Action action = () => propertyInfo.Should().NotBeWritable("that's required");

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_readwrite_property_is_not_writable_it_fails_with_useful_message()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithReadOnlyProperties).GetRuntimeProperty("ReadWriteProperty");

            // Act
            Action action = () => propertyInfo.Should().NotBeWritable("we want to test the error {0}", "message");

            // Assert
            action
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected propertyInfo ReadWriteProperty not to have a setter because we want to test the error message.");
        }

        [Fact]
        public void When_asserting_a_writeonly_property_is_not_writable_it_fails_with_useful_message()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("WriteOnlyProperty");

            // Act
            Action action = () => propertyInfo.Should().NotBeWritable("we want to test the error {0}", "message");

            // Assert
            action
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected propertyInfo WriteOnlyProperty not to have a setter because we want to test the error message.");
        }

        [Fact]
        public void When_subject_is_null_not_be_writable_should_fail()
        {
            // Arrange
            PropertyInfo propertyInfo = null;

            // Act
            Action act = () =>
                propertyInfo.Should().NotBeWritable("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected property not to have a setter *failure message*, but propertyInfo is <null>.");
        }
    }

    public class NotBeReadable
    {
        [Fact]
        public void When_asserting_a_readonly_property_is_not_readable_it_fails_with_useful_message()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithReadOnlyProperties).GetRuntimeProperty("ReadOnlyProperty");

            // Act
            Action action = () => propertyInfo.Should().NotBeReadable("we want to test the error {0}", "message");

            // Assert
            action
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected propertyInfo ReadOnlyProperty not to have a getter because we want to test the error message.");
        }

        [Fact]
        public void When_asserting_a_readwrite_property_is_not_readable_it_fails_with_useful_message()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithReadOnlyProperties).GetRuntimeProperty("ReadWriteProperty");

            // Act
            Action action = () => propertyInfo.Should().NotBeReadable("we want to test the error {0}", "message");

            // Assert
            action
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected propertyInfo ReadWriteProperty not to have a getter because we want to test the error message.");
        }

        [Fact]
        public void When_asserting_a_writeonly_property_is_not_readable_it_succeeds()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("WriteOnlyProperty");

            // Act
            Action action = () => propertyInfo.Should().NotBeReadable("that's required");

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_subject_is_null_not_be_readable_should_fail()
        {
            // Arrange
            PropertyInfo propertyInfo = null;

            // Act
            Action act = () =>
                propertyInfo.Should().NotBeReadable("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected property not to have a getter *failure message*, but propertyInfo is <null>.");
        }
    }

    public class BeReadableAccessModifier
    {
        [Fact]
        public void When_asserting_a_public_read_private_write_property_is_public_readable_it_succeeds()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("ReadPrivateWriteProperty");

            // Act
            Action action = () => propertyInfo.Should().BeReadable(CSharpAccessModifier.Public, "that's required");

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_private_read_public_write_property_is_public_readable_it_fails_with_useful_message()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("WritePrivateReadProperty");

            // Act
            Action action = () =>
                propertyInfo.Should().BeReadable(CSharpAccessModifier.Public, "we want to test the error {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected method get_WritePrivateReadProperty to be Public because we want to test the error message, but it is Private.");
        }

        [Fact]
        public void Do_not_the_check_access_modifier_when_the_property_is_not_readable()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("WriteOnlyProperty");

            // Act
            Action action = () =>
            {
                using var _ = new AssertionScope();
                propertyInfo.Should().BeReadable(CSharpAccessModifier.Private);
            };

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected property WriteOnlyProperty to have a getter, but it does not.");
        }

        [Fact]
        public void When_subject_is_null_be_readable_with_accessmodifier_should_fail()
        {
            // Arrange
            PropertyInfo propertyInfo = null;

            // Act
            Action act = () =>
                propertyInfo.Should().BeReadable(CSharpAccessModifier.Public, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected property to be Public *failure message*, but propertyInfo is <null>.");
        }

        [Fact]
        public void When_asserting_is_readable_with_an_invalid_enum_value_it_should_throw()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("StringProperty");

            // Act
            Action act = () =>
                propertyInfo.Should().BeReadable((CSharpAccessModifier)int.MaxValue);

            // Assert
            act.Should().ThrowExactly<ArgumentOutOfRangeException>()
                .WithParameterName("accessModifier");
        }
    }

    public class BeWritableAccessModifier
    {
        [Fact]
        public void When_asserting_a_public_write_private_read_property_is_public_writable_it_succeeds()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("WritePrivateReadProperty");

            // Act
            Action action = () => propertyInfo.Should().BeWritable(CSharpAccessModifier.Public, "that's required");

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_private_write_public_read_property_is_public_writable_it_fails_with_useful_message()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("ReadPrivateWriteProperty");

            // Act
            Action action = () =>
                propertyInfo.Should().BeWritable(CSharpAccessModifier.Public, "we want to test the error {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected method set_ReadPrivateWriteProperty to be Public because we want to test the error message, but it is Private.");
        }

        [Fact]
        public void Do_not_the_check_access_modifier_when_the_property_is_not_writable()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("ReadOnlyProperty");

            // Act
            Action action = () =>
            {
                using var _ = new AssertionScope();
                propertyInfo.Should().BeWritable(CSharpAccessModifier.Private);
            };

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected propertyInfo ReadOnlyProperty to have a setter.");
        }

        [Fact]
        public void When_subject_is_null_be_writable_with_accessmodifier_should_fail()
        {
            // Arrange
            PropertyInfo propertyInfo = null;

            // Act
            Action act = () =>
                propertyInfo.Should().BeWritable(CSharpAccessModifier.Public, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected property to be Public *failure message*, but propertyInfo is <null>.");
        }

        [Fact]
        public void When_asserting_is_writable_with_an_invalid_enum_value_it_should_throw()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("StringProperty");

            // Act
            Action act = () =>
                propertyInfo.Should().BeWritable((CSharpAccessModifier)int.MaxValue);

            // Assert
            act.Should().ThrowExactly<ArgumentOutOfRangeException>()
                .WithParameterName("accessModifier");
        }
    }

    public class Return
    {
        [Fact]
        public void When_asserting_a_String_property_returns_a_String_it_succeeds()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("StringProperty");

            // Act
            Action action = () => propertyInfo.Should().Return(typeof(string));

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_String_property_returns_an_Int32_it_throw_with_a_useful_message()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("StringProperty");

            // Act
            Action action = () => propertyInfo.Should().Return(typeof(int), "we want to test the error {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected Type of property StringProperty to be System.Int32 because we want to test the error " +
                    "message, but it is System.String.");
        }

        [Fact]
        public void When_subject_is_null_return_should_fail()
        {
            // Arrange
            PropertyInfo propertyInfo = null;

            // Act
            Action act = () =>
                propertyInfo.Should().Return(typeof(int), "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type of property to be *.Int32 *failure message*, but propertyInfo is <null>.");
        }

        [Fact]
        public void When_asserting_property_type_is_null_it_should_throw()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("StringProperty");

            // Act
            Action act = () =>
                propertyInfo.Should().Return(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("propertyType");
        }
    }

    public class ReturnOfT
    {
        [Fact]
        public void When_asserting_a_String_property_returnsOfT_a_String_it_succeeds()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("StringProperty");

            // Act
            Action action = () => propertyInfo.Should().Return<string>();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_String_property_returnsOfT_an_Int32_it_throw_with_a_useful_message()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("StringProperty");

            // Act
            Action action = () => propertyInfo.Should().Return<int>("we want to test the error {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected Type of property StringProperty to be System.Int32 because we want to test the error " +
                    "message, but it is System.String.");
        }
    }

    public class NotReturn
    {
        [Fact]
        public void When_asserting_a_String_property_does_not_returns_an_Int32_it_succeeds()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("StringProperty");

            // Act
            Action action = () => propertyInfo.Should().NotReturn(typeof(int));

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_String_property_does_not_return_a_String_it_throw_with_a_useful_message()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("StringProperty");

            // Act
            Action action = () => propertyInfo.Should().NotReturn(typeof(string), "we want to test the error {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected Type of property StringProperty not to be*String*because we want to test the error " +
                    "message, but it is.");
        }

        [Fact]
        public void When_subject_is_null_not_return_should_fail()
        {
            // Arrange
            PropertyInfo propertyInfo = null;

            // Act
            Action act = () =>
                propertyInfo.Should().NotReturn(typeof(int), "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected type of property not to be *.Int32 *failure message*, but propertyInfo is <null>.");
        }

        [Fact]
        public void When_asserting_property_type_is_not_null_it_should_throw()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("StringProperty");

            // Act
            Action act = () =>
                propertyInfo.Should().NotReturn(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("propertyType");
        }
    }

    public class NotReturnOfT
    {
        [Fact]
        public void When_asserting_a_String_property_does_not_returnOfT_an_Int32_it_succeeds()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("StringProperty");

            // Act
            Action action = () => propertyInfo.Should().NotReturn<int>();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_String_property_does_not_returnsOfT_a_String_it_throw_with_a_useful_message()
        {
            // Arrange
            PropertyInfo propertyInfo = typeof(ClassWithProperties).GetRuntimeProperty("StringProperty");

            // Act
            Action action = () => propertyInfo.Should().NotReturn<string>("we want to test the error {0}", "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected Type of property StringProperty not to be*String*because we want to test the error " +
                    "message, but it is.");
        }
    }

    #region Internal classes used in unit tests

    private class ClassWithProperties
    {
        public string ReadOnlyProperty { get { return ""; } }

        public string ReadPrivateWriteProperty { get; private set; }

        public string ReadWriteProperty { get; set; }

        public string WritePrivateReadProperty { private get; set; }

        public string WriteOnlyProperty { set { } }

        public string StringProperty { get; set; }
    }

    #endregion
}
