using System;
using Xunit;
using Xunit.Sdk;

#if NET6_0_OR_GREATER

namespace FluentAssertions.Specs.Primitives;

public class DateOnlyAssertionSpecs
{
    [Fact]
    public void Should_succeed_when_asserting_nullable_dateonly_value_with_value_to_have_a_value()
    {
        // Arrange
        DateOnly? dateOnly = new(2016, 06, 04);

        // Act/Assert
        dateOnly.Should().HaveValue();
    }

    [Fact]
    public void Should_succeed_when_asserting_nullable_dateonly_value_with_value_to_not_be_null()
    {
        // Arrange
        DateOnly? dateOnly = new(2016, 06, 04);

        // Act/Assert
        dateOnly.Should().NotBeNull();
    }

    public class Be
    {
        [Fact]
        public void Should_succeed_when_asserting_dateonly_value_is_equal_to_the_same_value()
        {
            // Arrange
            DateOnly dateOnly = new(2016, 06, 04);
            DateOnly sameDateOnly = new(2016, 06, 04);

            // Act/Assert
            dateOnly.Should().Be(sameDateOnly);
        }

        [Fact]
        public void When_dateonly_value_is_equal_to_the_same_nullable_value_be_should_succeed()
        {
            // Arrange
            DateOnly dateOnly = new(2016, 06, 04);
            DateOnly? sameDateOnly = new(2016, 06, 04);

            // Act/Assert
            dateOnly.Should().Be(sameDateOnly);
        }

        [Fact]
        public void When_both_values_are_at_their_minimum_then_it_should_succeed()
        {
            // Arrange
            DateOnly dateOnly = DateOnly.MinValue;
            DateOnly sameDateOnly = DateOnly.MinValue;

            // Act/Assert
            dateOnly.Should().Be(sameDateOnly);
        }

        [Fact]
        public void When_both_values_are_at_their_maximum_then_it_should_succeed()
        {
            // Arrange
            DateOnly dateOnly = DateOnly.MaxValue;
            DateOnly sameDateOnly = DateOnly.MaxValue;

            // Act/Assert
            dateOnly.Should().Be(sameDateOnly);
        }

        [Fact]
        public void Should_fail_when_asserting_dateonly_value_is_equal_to_the_different_value()
        {
            // Arrange
            var dateOnly = new DateOnly(2012, 03, 10);
            var otherDateOnly = new DateOnly(2012, 03, 11);

            // Act
            Action act = () => dateOnly.Should().Be(otherDateOnly, "because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected dateOnly to be <2012-03-11>*failure message, but found <2012-03-10>.");
        }

        [Fact]
        public void Should_succeed_when_asserting_nullable_numeric_value_equals_the_same_value()
        {
            // Arrange
            DateOnly? nullableDateOnlyA = new DateOnly(2016, 06, 04);
            DateOnly? nullableDateOnlyB = new DateOnly(2016, 06, 04);

            // Act/Assert
            nullableDateOnlyA.Should().Be(nullableDateOnlyB);
        }

        [Fact]
        public void Should_succeed_when_asserting_nullable_numeric_null_value_equals_null()
        {
            // Arrange
            DateOnly? nullableDateOnlyA = null;
            DateOnly? nullableDateOnlyB = null;

            // Act/Assert
            nullableDateOnlyA.Should().Be(nullableDateOnlyB);
        }

        [Fact]
        public void Should_fail_when_asserting_nullable_numeric_value_equals_a_different_value()
        {
            // Arrange
            DateOnly? nullableDateOnlyA = new DateOnly(2016, 06, 04);
            DateOnly? nullableDateOnlyB = new DateOnly(2016, 06, 06);

            // Act
            Action action = () =>
                nullableDateOnlyA.Should().Be(nullableDateOnlyB);

            // Assert
            action.Should().Throw<XunitException>();
        }

        [Fact]
        public void Should_fail_with_descriptive_message_when_asserting_dateonly_null_value_is_equal_to_another_value()
        {
            // Arrange
            DateOnly? nullableDateOnly = null;

            // Act
            Action action = () =>
                nullableDateOnly.Should().Be(new DateOnly(2016, 06, 04), "because we want to test the failure {0}",
                    "message");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected nullableDateOnly to be <2016-06-04> because we want to test the failure message, but found <null>.");
        }

        [Fact]
        public void Should_succeed_when_asserting_dateonly_value_is_not_equal_to_a_different_value()
        {
            // Arrange
            DateOnly dateOnly = new(2016, 06, 04);
            DateOnly otherDateOnly = new(2016, 06, 05);

            // Act/Assert
            dateOnly.Should().NotBe(otherDateOnly);
        }
    }

    public class BeBefore
    {
        [Fact]
        public void When_asserting_subject_is_not_before_earlier_expected_dateonly_it_should_succeed()
        {
            // Arrange
            DateOnly expected = new(2016, 06, 03);
            DateOnly subject = new(2016, 06, 04);

            // Act/Assert
            subject.Should().NotBeBefore(expected);
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_before_the_same_dateonly_it_should_throw()
        {
            // Arrange
            DateOnly expected = new(2016, 06, 04);
            DateOnly subject = new(2016, 06, 04);

            // Act
            Action act = () => subject.Should().BeBefore(expected);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be before <2016-06-04>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_not_before_the_same_dateonly_it_should_succeed()
        {
            // Arrange
            DateOnly expected = new(2016, 06, 04);
            DateOnly subject = new(2016, 06, 04);

            // Act/Assert
            subject.Should().NotBeBefore(expected);
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_on_or_before_expected_dateonly_should_succeed()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 05);

            // Act/Assert
            subject.Should().BeOnOrBefore(expectation);
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_on_or_before_expected_dateonly_should_throw()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 05);

            // Act
            Action act = () => subject.Should().NotBeOnOrBefore(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be after <2016-06-05>, but found <2016-06-04>.");
        }

        [Fact]
        public void
            When_asserting_subject_dateonly_is_on_or_before_the_same_date_as_the_expected_dateonly_should_succeed()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 04);

            // Act/Assert
            subject.Should().BeOnOrBefore(expectation);
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_on_or_before_the_same_date_as_the_expected_dateonly_should_throw()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 04);

            // Act
            Action act = () => subject.Should().NotBeOnOrBefore(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be after <2016-06-04>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_not_on_or_before_earlier_expected_dateonly_should_throw()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 03);

            // Act
            Action act = () => subject.Should().BeOnOrBefore(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be on or before <2016-06-03>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_not_on_or_before_earlier_expected_dateonly_should_succeed()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 03);

            // Act/Assert
            subject.Should().NotBeOnOrBefore(expectation);
        }
    }

    public class BeAfter
    {
        [Fact]
        public void When_asserting_subject_dateonly_is_after_earlier_expected_dateonly_should_succeed()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 03);

            // Act/Assert
            subject.Should().BeAfter(expectation);
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_not_after_earlier_expected_dateonly_should_throw()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 03);

            // Act
            Action act = () => subject.Should().NotBeAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be on or before <2016-06-03>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_after_later_expected_dateonly_should_throw()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 05);

            // Act
            Action act = () => subject.Should().BeAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be after <2016-06-05>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_not_after_later_expected_dateonly_should_succeed()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 05);

            // Act/Assert
            subject.Should().NotBeAfter(expectation);
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_after_the_same_expected_dateonly_should_throw()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 04);

            // Act
            Action act = () => subject.Should().BeAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be after <2016-06-04>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_not_after_the_same_expected_dateonly_should_succeed()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 04);

            // Act/Assert
            subject.Should().NotBeAfter(expectation);
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_on_or_after_earlier_expected_dateonly_should_succeed()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 03);

            // Act/Assert
            subject.Should().BeOnOrAfter(expectation);
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_not_on_or_after_earlier_expected_dateonly_should_throw()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 03);

            // Act
            Action act = () => subject.Should().NotBeOnOrAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be before <2016-06-03>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_on_or_after_the_same_expected_dateonly_should_succeed()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 04);

            // Act/Assert
            subject.Should().BeOnOrAfter(expectation);
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_not_on_or_after_the_same_expected_dateonly_should_throw()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 04);

            // Act
            Action act = () => subject.Should().NotBeOnOrAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be before <2016-06-04>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_on_or_after_later_expected_dateonly_should_throw()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 05);

            // Act
            Action act = () => subject.Should().BeOnOrAfter(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be on or after <2016-06-05>, but found <2016-06-04>.");
        }

        [Fact]
        public void When_asserting_subject_dateonly_is_not_on_or_after_later_expected_dateonly_should_succeed()
        {
            // Arrange
            DateOnly subject = new(2016, 06, 04);
            DateOnly expectation = new(2016, 06, 05);

            // Act/Assert
            subject.Should().NotBeOnOrAfter(expectation);
        }
    }

    public class HaveYear
    {
        [Fact]
        public void When_asserting_subject_dateonly_should_have_year_with_the_same_value_should_succeed()
        {
            // Arrange
            DateOnly subject = new(2009, 12, 31);
            const int expectation = 2009;

            // Act/Assert
            subject.Should().HaveYear(expectation);
        }

        [Fact]
        public void When_asserting_subject_dateonly_should_not_have_year_with_the_same_value_should_throw()
        {
            // Arrange
            DateOnly subject = new(2009, 12, 31);
            const int expectation = 2009;

            // Act
            Action act = () => subject.Should().NotHaveYear(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the year part of subject to be 2009, but it was.");
        }

        [Fact]
        public void When_asserting_subject_dateonly_should_have_year_with_a_different_value_should_throw()
        {
            // Arrange
            DateOnly subject = new(2009, 12, 31);
            const int expectation = 2008;

            // Act
            Action act = () => subject.Should().HaveYear(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the year part of subject to be 2008, but found 2009.");
        }

        [Fact]
        public void When_asserting_subject_dateonly_should_not_have_year_with_a_different_value_should_succeed()
        {
            // Arrange
            DateOnly subject = new(2009, 12, 31);
            const int expectation = 2008;

            // Act/Assert
            subject.Should().NotHaveYear(expectation);
        }

        [Fact]
        public void When_asserting_subject_null_dateonly_should_have_year_should_throw()
        {
            // Arrange
            DateOnly? subject = null;
            const int expectation = 2008;

            // Act
            Action act = () => subject.Should().HaveYear(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the year part of subject to be 2008, but found <null>.");
        }

        [Fact]
        public void When_asserting_subject_null_dateonly_should_not_have_year_should_throw()
        {
            // Arrange
            DateOnly? subject = null;
            const int expectation = 2008;

            // Act
            Action act = () => subject.Should().NotHaveYear(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the year part of subject to be 2008, but found a <null> DateOnly.");
        }
    }

    public class HaveMonth
    {
        [Fact]
        public void When_asserting_subject_dateonly_should_have_month_with_the_same_value_it_should_succeed()
        {
            // Arrange
            DateOnly subject = new(2009, 12, 31);
            const int expectation = 12;

            // Act/Assert
            subject.Should().HaveMonth(expectation);
        }

        [Fact]
        public void When_asserting_subject_dateonly_should_not_have_month_with_the_same_value_it_should_throw()
        {
            // Arrange
            DateOnly subject = new(2009, 12, 31);
            const int expectation = 12;

            // Act
            Action act = () => subject.Should().NotHaveMonth(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the month part of subject to be 12, but it was.");
        }

        [Fact]
        public void When_asserting_subject_dateonly_should_have_a_month_with_a_different_value_it_should_throw()
        {
            // Arrange
            DateOnly subject = new(2009, 12, 31);
            const int expectation = 11;

            // Act
            Action act = () => subject.Should().HaveMonth(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the month part of subject to be 11, but found 12.");
        }

        [Fact]
        public void When_asserting_subject_dateonly_should_not_have_a_month_with_a_different_value_it_should_succeed()
        {
            // Arrange
            DateOnly subject = new(2009, 12, 31);
            const int expectation = 11;

            // Act/Assert
            subject.Should().NotHaveMonth(expectation);
        }

        [Fact]
        public void When_asserting_subject_null_dateonly_should_have_month_should_throw()
        {
            // Arrange
            DateOnly? subject = null;
            const int expectation = 12;

            // Act
            Action act = () => subject.Should().HaveMonth(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the month part of subject to be 12, but found a <null> DateOnly.");
        }

        [Fact]
        public void When_asserting_subject_null_dateonly_should_not_have_month_should_throw()
        {
            // Arrange
            DateOnly? subject = null;
            const int expectation = 12;

            // Act
            Action act = () => subject.Should().NotHaveMonth(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the month part of subject to be 12, but found a <null> DateOnly.");
        }
    }

    public class HaveDay
    {
        [Fact]
        public void When_asserting_subject_dateonly_should_have_day_with_the_same_value_it_should_succeed()
        {
            // Arrange
            DateOnly subject = new(2009, 12, 31);
            const int expectation = 31;

            // Act/Assert
            subject.Should().HaveDay(expectation);
        }

        [Fact]
        public void When_asserting_subject_dateonly_should_not_have_day_with_the_same_value_it_should_throw()
        {
            // Arrange
            DateOnly subject = new(2009, 12, 31);
            const int expectation = 31;

            // Act
            Action act = () => subject.Should().NotHaveDay(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the day part of subject to be 31, but it was.");
        }

        [Fact]
        public void When_asserting_subject_dateonly_should_have_day_with_a_different_value_it_should_throw()
        {
            // Arrange
            DateOnly subject = new(2009, 12, 31);
            const int expectation = 30;

            // Act
            Action act = () => subject.Should().HaveDay(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the day part of subject to be 30, but found 31.");
        }

        [Fact]
        public void When_asserting_subject_dateonly_should_not_have_day_with_a_different_value_it_should_succeed()
        {
            // Arrange
            DateOnly subject = new(2009, 12, 31);
            const int expectation = 30;

            // Act/Assert
            subject.Should().NotHaveDay(expectation);
        }

        [Fact]
        public void When_asserting_subject_null_dateonly_should_have_day_should_throw()
        {
            // Arrange
            DateOnly? subject = null;
            const int expectation = 22;

            // Act
            Action act = () => subject.Should().HaveDay(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected the day part of subject to be 22, but found a <null> DateOnly.");
        }

        [Fact]
        public void When_asserting_subject_null_dateonly_should_not_have_day_should_throw()
        {
            // Arrange
            DateOnly? subject = null;
            const int expectation = 22;

            // Act
            Action act = () => subject.Should().NotHaveDay(expectation);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Did not expect the day part of subject to be 22, but found a <null> DateOnly.");
        }
    }

    public class BeOneOf
    {
        [Fact]
        public void When_a_value_is_not_one_of_the_specified_values_it_should_throw()
        {
            // Arrange
            DateOnly value = new(2016, 12, 20);

            // Act
            Action action = () => value.Should().BeOneOf(value.AddDays(1), value.AddMonths(-1));

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected value to be one of {<2016-12-21>, <2016-11-20>}, but found <2016-12-20>.");
        }

        [Fact]
        public void When_a_value_is_not_one_of_the_specified_values_it_should_throw_with_descriptive_message()
        {
            // Arrange
            DateOnly value = new(2016, 12, 20);

            // Act
            Action action = () =>
                value.Should().BeOneOf(new[] { value.AddDays(1), value.AddDays(2) }, "because it's true");

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected value to be one of {<2016-12-21>, <2016-12-22>} because it's true, but found <2016-12-20>.");
        }

        [Fact]
        public void When_a_value_is_one_of_the_specified_values_it_should_succeed()
        {
            // Arrange
            DateOnly value = new(2016, 12, 30);

            // Act/Assert
            value.Should().BeOneOf(new DateOnly(2216, 1, 30), new DateOnly(2016, 12, 30));
        }

        [Fact]
        public void When_a_null_value_is_not_one_of_the_specified_values_it_should_throw()
        {
            // Arrange
            DateOnly? value = null;

            // Act
            Action action = () => value.Should().BeOneOf(new DateOnly(2216, 1, 30), new DateOnly(1116, 4, 10));

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected value to be one of {<2216-01-30>, <1116-04-10>}, but found <null>.");
        }

        [Fact]
        public void When_a_value_is_one_of_the_specified_values_it_should_succeed_when_dateonly_is_null()
        {
            // Arrange
            DateOnly? value = null;

            // Act/Assert
            value.Should().BeOneOf(new DateOnly(2216, 1, 30), null);
        }
    }

    public class AndChaining
    {
        [Fact]
        public void Should_support_chaining_constraints_with_and()
        {
            // Arrange
            DateOnly earlierDateOnly = new(2016, 06, 03);
            DateOnly? nullableDateOnly = new(2016, 06, 04);

            // Act/Assert
            nullableDateOnly.Should()
                .HaveValue()
                .And
                .BeAfter(earlierDateOnly);
        }
    }
}

#endif
