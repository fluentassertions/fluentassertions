using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class GuidAssertionSpecs
    {
        #region BeEmpty / NotBeEmpty

        [Fact]
        public void Should_succeed_when_asserting_empty_guid_is_empty()
        {
            // Arrange
            Guid guid = Guid.Empty;

            // Act / Assert
            guid.Should().BeEmpty();
        }

        [Fact]
        public void Should_fail_when_asserting_non_empty_guid_is_empty()
        {
            // Arrange
            var guid = new Guid("12345678-1234-1234-1234-123456789012");

            // Act
            Action act = () => guid.Should().BeEmpty("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected Guid to be empty because we want to test the failure message, but found {12345678-1234-1234-1234-123456789012}.");
        }

        [Fact]
        public void Should_succeed_when_asserting_non_empty_guid_is_not_empty()
        {
            // Arrange
            var guid = new Guid("12345678-1234-1234-1234-123456789012");

            // Act
            Action act = () => guid.Should().NotBeEmpty();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Should_fail_when_asserting_empty_guid_is_not_empty()
        {
            // Act
            Action act = () => Guid.Empty.Should().NotBeEmpty("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect Guid.Empty to be empty because we want to test the failure message.");
        }

        #endregion

        #region Be / NotBe

        [Fact]
        public void Should_succeed_when_asserting_guid_equals_the_same_guid()
        {
            // Arrange
            var guid = new Guid("11111111-aaaa-bbbb-cccc-999999999999");
            var sameGuid = new Guid("11111111-aaaa-bbbb-cccc-999999999999");

            // Act
            Action act = () => guid.Should().Be(sameGuid);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Should_succeed_when_asserting_guid_equals_the_same_guid_in_string_format()
        {
            // Arrange
            var guid = new Guid("11111111-aaaa-bbbb-cccc-999999999999");

            // Act
            Action act = () => guid.Should().Be("11111111-aaaa-bbbb-cccc-999999999999");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Should_fail_when_asserting_guid_equals_a_different_guid()
        {
            // Arrange
            var guid = new Guid("11111111-aaaa-bbbb-cccc-999999999999");
            var differentGuid = new Guid("55555555-ffff-eeee-dddd-444444444444");

            // Act
            Action act = () => guid.Should().Be(differentGuid, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected Guid to be {55555555-ffff-eeee-dddd-444444444444} because we want to test the failure message, but found {11111111-aaaa-bbbb-cccc-999999999999}.");
        }

        [Fact]
        public void Should_succeed_when_asserting_guid_does_not_equal_a_different_guid()
        {
            // Arrange
            var guid = new Guid("11111111-aaaa-bbbb-cccc-999999999999");
            var differentGuid = new Guid("55555555-ffff-eeee-dddd-444444444444");

            // Act
            Action act = () =>
                guid.Should().NotBe(differentGuid);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Should_succeed_when_asserting_guid_does_not_equal_the_same_guid()
        {
            // Arrange
            var guid = new Guid("11111111-aaaa-bbbb-cccc-999999999999");
            var sameGuid = new Guid("11111111-aaaa-bbbb-cccc-999999999999");

            // Act
            Action act = () => guid.Should().NotBe(sameGuid, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Did not expect Guid to be {11111111-aaaa-bbbb-cccc-999999999999} because we want to test the failure message.");
        }

        #endregion

        [Fact]
        public void Should_support_chaining_constraints_with_and()
        {
            // Arrange
            Guid guid = Guid.NewGuid();

            // Act / Assert
            guid.Should()
                .NotBeEmpty()
                .And.Be(guid);
        }
    }
}
