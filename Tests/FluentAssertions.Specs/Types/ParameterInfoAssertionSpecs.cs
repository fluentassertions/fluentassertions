using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Types;

public class ParameterInfoAssertionSpecs
{
    public class BeDecortatedWithOfT
    {
        [Fact]
        public void When_asserting_a_parameter_is_decorated_with_attribute_and_it_is_it_succeeds()
        {
            // Arrange
            ParameterInfo parameterInfo =
                typeof(ParameterDecoratedWithDummyAttribute).GetMethod("ParameterWithOneAttribute")!.GetParameters().Single();

            // Act
            Action act = () =>
                parameterInfo.Should().BeDecoratedWith<DummyParameterAttribute>();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_parameter_is_decorated_with_an_attribute_it_allow_chaining_assertions()
        {
            // Arrange
            ParameterInfo parameterInfo =
                typeof(ParameterDecoratedWithDummyAttribute).GetMethod("ParameterWithOneAttribute")!.GetParameters().Single();

            // Act
            Action act = () =>
                parameterInfo.Should().BeDecoratedWith<DummyParameterAttribute>().Which.Value.Should().Be("OtherValue");

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected*Value*OtherValue*");
        }

        [Fact]
        public void
            When_a_parameter_is_decorated_with_an_attribute_and_multiple_attributes_match_continuation_using_the_matched_value_fail()
        {
            // Arrange
            ParameterInfo parameterInfo =
                typeof(ParameterDecoratedWithDummyAttribute).GetMethod("ParameterWithTwoAttributes")!.GetParameters().Single();

            // Act
            Action act = () =>
                parameterInfo.Should().BeDecoratedWith<DummyParameterAttribute>().Which.Value.Should().Be("OtherValue");

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_a_parameter_is_decorated_with_attribute_and_it_is_not_it_throw_with_useful_message()
        {
            // Arrange
            ParameterInfo parameterInfo =
                typeof(ParameterDecoratedWithDummyAttribute).GetMethod("ParameterWithoutAttributes")!.GetParameters().Single();

            // Act
            Action act = () =>
                parameterInfo.Should().BeDecoratedWith<DummyParameterAttribute>("because we want to test the error message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected parameter name to be decorated with " +
                    "FluentAssertions*DummyParameterAttribute because we want to test the error message, but that attribute was not found.");
        }

        [Fact]
        public void
            When_asserting_a_parameter_is_decorated_with_an_attribute_matching_a_predicate_but_it_is_not_it_throw_with_useful_message()
        {
            // Arrange
            ParameterInfo parameterInfo =
                typeof(ParameterDecoratedWithDummyAttribute).GetMethod("ParameterWithOneAttribute")!.GetParameters().Single();

            // Act
            Action act = () =>
                parameterInfo.Should().BeDecoratedWith<DummyParameterAttribute>(d => d.Value == "NotARealValue",
                    "because we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected parameter name to be decorated with " +
                    "FluentAssertions*DummyParameterAttribute that matches (d.Value == \"NotARealValue\") because we want to test the error message," +
                    " but no matching attribute was found.");
        }

        [Fact]
        public void When_asserting_a_parameter_is_decorated_with_attribute_matching_a_predicate_and_it_is_it_succeeds()
        {
            // Arrange
            ParameterInfo parameterInfo =
                typeof(ParameterDecoratedWithDummyAttribute).GetMethod("ParameterWithOneAttribute")!.GetParameters().Single();

            // Act
            Action act = () => parameterInfo.Should().BeDecoratedWith<DummyParameterAttribute>(d => d.Value == "Value");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_subject_is_null_be_decorated_withOfT_should_fail()
        {
            // Arrange
            ParameterInfo parameterInfo = null;

            // Act
            Action act = () =>
                parameterInfo.Should().BeDecoratedWith<DummyParameterAttribute>("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected parameter to be decorated with *DummyParameterAttribute *failure message*, but parameterInfo is <null>.");
        }

        [Fact]
        public void When_asserting_parameter_is_decorated_with_null_it_should_throw()
        {
            // Arrange
            ParameterInfo parameterInfo =
                typeof(ParameterDecoratedWithDummyAttribute).GetMethod("ParameterWithOneAttribute")!.GetParameters().Single();

            // Act
            Action act = () =>
                parameterInfo.Should().BeDecoratedWith((Expression<Func<DummyParameterAttribute, bool>>)null);

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
            ParameterInfo parameterInfo = null;

            // Act
            Action act = () =>
                parameterInfo.Should().NotBeDecoratedWith<DummyParameterAttribute>("we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected parameter to not be decorated with *DummyParameterAttribute *failure message*, but parameterInfo is <null>.");
        }

        [Fact]
        public void When_asserting_parameter_is_not_decorated_with_null_it_should_throw()
        {
            // Arrange
            ParameterInfo parameterInfo =
                typeof(ParameterDecoratedWithDummyAttribute).GetMethod("ParameterWithOneAttribute")!.GetParameters().Single();

            // Act
            Action act = () =>
                parameterInfo.Should().NotBeDecoratedWith((Expression<Func<DummyParameterAttribute, bool>>)null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("isMatchingAttributePredicate");
        }
    }

    #region Internal classes used in unit tests

    internal class ParameterDecoratedWithDummyAttribute
    {
        public void ParameterWithoutAttributes(string name)
        {
        }

        public void ParameterWithOneAttribute([DummyParameter("Value")] string name)
        {
        }

        public void ParameterWithTwoAttributes([DummyParameter("Value")] [DummyParameter("OtherValue")] string name)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = true)]
    public class DummyParameterAttribute : Attribute
    {
        public DummyParameterAttribute()
        {
        }

        public DummyParameterAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }

    #endregion
}
