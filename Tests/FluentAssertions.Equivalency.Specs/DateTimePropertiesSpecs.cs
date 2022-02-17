using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Equivalency.Specs
{
    public class DateTimePropertiesSpecs
    {
        [Fact]
        public void When_two_properties_are_datetime_and_both_are_nullable_and_both_are_null_it_should_succeed()
        {
            // Arrange
            var subject =
                new { Time = (DateTime?)null };

            var other =
                new { Time = (DateTime?)null };

            // Act
            Action act = () =>
                subject.Should().BeEquivalentTo(other);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_two_properties_are_datetime_and_both_are_nullable_and_are_equal_it_should_succeed()
        {
            // Arrange
            var subject =
                new { Time = (DateTime?)new DateTime(2013, 12, 9, 15, 58, 0) };

            var other =
                new { Time = (DateTime?)new DateTime(2013, 12, 9, 15, 58, 0) };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(other);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_two_properties_are_datetime_and_both_are_nullable_and_expectation_is_null_it_should_throw_and_state_the_difference()
        {
            // Arrange
            var subject =
                new { Time = (DateTime?)new DateTime(2013, 12, 9, 15, 58, 0) };

            var other =
                new { Time = (DateTime?)null };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(other);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected*Time to be <null>, but found <2013-12-09 15:58:00>.*");
        }

        [Fact]
        public void
            When_two_properties_are_datetime_and_both_are_nullable_and_subject_is_null_it_should_throw_and_state_the_difference()
        {
            // Arrange
            var subject =
                new { Time = (DateTime?)null };

            var other =
                new { Time = (DateTime?)new DateTime(2013, 12, 9, 15, 58, 0) };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(other);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected*Time*to be <2013-12-09 15:58:00>, but found <null>.*");
        }

        [Fact]
        public void When_two_properties_are_datetime_and_expectation_is_nullable_and_are_equal_it_should_succeed()
        {
            // Arrange
            var subject =
                new { Time = new DateTime(2013, 12, 9, 15, 58, 0) };

            var other =
                new { Time = (DateTime?)new DateTime(2013, 12, 9, 15, 58, 0) };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(other);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_two_properties_are_datetime_and_expectation_is_nullable_and_expectation_is_null_it_should_throw_and_state_the_difference()
        {
            // Arrange
            var subject =
                new { Time = new DateTime(2013, 12, 9, 15, 58, 0) };

            var other =
                new { Time = (DateTime?)null };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(other);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected*Time*to be <null>, but found <2013-12-09 15:58:00>.*");
        }

        [Fact]
        public void When_two_properties_are_datetime_and_subject_is_nullable_and_are_equal_it_should_succeed()
        {
            // Arrange
            var subject =
                new { Time = (DateTime?)new DateTime(2013, 12, 9, 15, 58, 0) };

            var other =
                new { Time = new DateTime(2013, 12, 9, 15, 58, 0) };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(other);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
            When_two_properties_are_datetime_and_subject_is_nullable_and_subject_is_null_it_should_throw_and_state_the_difference()
        {
            // Arrange
            var subject =
                new { Time = (DateTime?)null };

            var other =
                new { Time = new DateTime(2013, 12, 9, 15, 58, 0) };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(other);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected*Time*to be <2013-12-09 15:58:00>, but found <null>.*");
        }
    }
}
