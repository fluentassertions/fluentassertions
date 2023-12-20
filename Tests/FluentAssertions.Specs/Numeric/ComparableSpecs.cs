using System;
using System.Globalization;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Numeric;

public class ComparableSpecs
{
    public class Be
    {
        [Fact]
        public void When_two_instances_are_equal_it_should_succeed()
        {
            // Arrange
            var subject = new EquatableOfInt(1);
            var other = new EquatableOfInt(1);

            // Act / Assert
            subject.Should().Be(other);
        }

        [Fact]
        public void When_two_instances_are_the_same_reference_but_are_not_considered_equal_it_should_succeed()
        {
            // Arrange
            var subject = new SameInstanceIsNotEqualClass();
            var other = subject;

            // Act
            Action act = () => subject.Should().Be(other);

            // Assert
            act.Should().NotThrow(
                "This is inconsistent with the behavior ObjectAssertions.Be but is how ComparableTypeAssertions.Be has always worked.");
        }

        [Fact]
        public void When_two_instances_are_not_equal_it_should_throw()
        {
            // Arrange
            var subject = new EquatableOfInt(1);
            var other = new EquatableOfInt(2);

            // Act
            Action act = () => subject.Should().Be(other, "they have the same property values");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected*2*because they have the same property values, but found*1*.");
        }
    }

    public class NotBe
    {
        [Fact]
        public void When_two_references_to_the_same_instance_are_not_equal_it_should_throw()
        {
            // Arrange
            var subject = new SameInstanceIsNotEqualClass();
            var other = subject;

            // Act
            Action act = () => subject.Should().NotBe(other);

            // Assert
            act.Should().Throw<XunitException>(
                "This is inconsistent with the behavior ObjectAssertions.Be but is how ComparableTypeAssertions.Be has always worked.");
        }

        [Fact]
        public void When_two_equal_objects_should_not_be_equal_it_should_throw()
        {
            // Arrange
            var subject = new EquatableOfInt(1);
            var other = new EquatableOfInt(1);

            // Act
            Action act = () => subject.Should().NotBe(other, "they represent different things");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "*Did not expect subject to be equal to*1*because they represent different things.*");
        }

        [Fact]
        public void When_two_unequal_objects_should_not_be_equal_it_should_not_throw()
        {
            // Arrange
            var subject = new EquatableOfInt(1);
            var other = new EquatableOfInt(2);

            // Act
            Action act = () => subject.Should().NotBe(other);

            // Assert
            act.Should().NotThrow();
        }
    }

    public class BeOneOf
    {
        [Fact]
        public void When_a_value_is_not_equal_to_one_of_the_specified_values_it_should_throw()
        {
            // Arrange
            var value = new EquatableOfInt(3);

            // Act
            Action act = () => value.Should().BeOneOf(new[] { new EquatableOfInt(4), new EquatableOfInt(5) },
                "because those are the valid {0}", "values");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected value to be one of {4, 5} because those are the valid values, but found 3.");
        }

        [Fact]
        public void When_two_instances_are_the_same_reference_but_are_not_considered_equal_it_should_succeed()
        {
            // Arrange
            var subject = new SameInstanceIsNotEqualClass();
            var other = subject;

            // Act
            Action act = () => subject.Should().BeOneOf(other);

            // Assert
            act.Should().NotThrow(
                "This is inconsistent with the behavior ObjectAssertions.Be but is how ComparableTypeAssertions.Be has always worked.");
        }

        [Fact]
        public void When_a_value_is_equal_to_one_of_the_specified_values_it_should_succeed()
        {
            // Arrange
            var value = new EquatableOfInt(4);

            // Act
            Action act = () => value.Should().BeOneOf(new EquatableOfInt(4), new EquatableOfInt(5));

            // Assert
            act.Should().NotThrow();
        }
    }

    public class BeEquivalentToAsync
    {
        [Fact]
        public async Task When_two_instances_are_equivalent_it_should_succeed()
        {
            // Arrange
            var subject = new ComparableCustomer(42);
            var expected = new CustomerDto(42);

            // Act / Assert
            await subject.Should().BeEquivalentToAsync(expected);
        }

        [Fact]
        public async Task When_two_instances_are_compared_it_should_allow_chaining()
        {
            // Arrange
            var subject = new ComparableCustomer(42);
            var expected = new CustomerDto(42);

            // Act / Assert
            (await subject.Should().BeEquivalentToAsync(expected))
                .And.NotBeNull();
        }

        [Fact]
        public async Task When_two_instances_are_compared_with_config_it_should_allow_chaining()
        {
            // Arrange
            var subject = new ComparableCustomer(42);
            var expected = new CustomerDto(42);

            // Act / Assert
            (await subject.Should().BeEquivalentToAsync(expected, opt => opt))
                .And.NotBeNull();
        }

        [Fact]
        public async Task When_two_instances_are_equivalent_due_to_exclusion_it_should_succeed()
        {
            // Arrange
            var subject = new ComparableCustomer(42);

            var expected = new AnotherCustomerDto(42)
            {
                SomeOtherProperty = 1337
            };

            // Act / Assert
            await subject.Should().BeEquivalentToAsync(expected,
                options => options.Excluding(x => x.SomeOtherProperty),
                "they have the same property values");
        }

        [Fact]
        public async Task When_injecting_a_null_config_it_should_throw()
        {
            // Arrange
            var subject = new ComparableCustomer(42);
            var expected = new AnotherCustomerDto(42);

            // Act
            Func<Task> act = () => subject.Should().BeEquivalentToAsync(expected, config: null);

            // Assert
            await act.Should().ThrowExactlyAsync<ArgumentNullException>()
                .WithParameterName("config");
        }

        [Fact]
        public async Task When_two_instances_are_not_equivalent_it_should_throw()
        {
            // Arrange
            var subject = new ComparableCustomer(42);

            var expected = new AnotherCustomerDto(42)
            {
                SomeOtherProperty = 1337
            };

            // Act
            Func<Task> act = () => subject.Should().BeEquivalentToAsync(expected, "they have the same property values");

            // Assert
            await act
                .Should().ThrowAsync<XunitException>()
                .WithMessage(
                    "Expectation has property subject.SomeOtherProperty*that the other object does not have*");
        }
    }

    public class BeNull
    {
        [Fact]
        public void When_assertion_an_instance_to_be_null_and_it_is_null_it_should_succeed()
        {
            // Arrange
            ComparableOfString subject = null;

            // Act
            Action action = () =>
                subject.Should().BeNull();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_assertion_an_instance_to_be_null_and_it_is_not_null_it_should_throw()
        {
            // Arrange
            var subject = new ComparableOfString("");

            // Act
            Action action = () =>
                subject.Should().BeNull();

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be <null>, but found*");
        }
    }

    public class NotBeNull
    {
        [Fact]
        public void When_assertion_an_instance_not_to_be_null_and_it_is_not_null_it_should_succeed()
        {
            // Arrange
            var subject = new ComparableOfString("");

            // Act
            Action action = () =>
                subject.Should().NotBeNull();

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_assertion_an_instance_not_to_be_null_and_it_is_null_it_should_throw()
        {
            // Arrange
            ComparableOfString subject = null;

            // Act
            Action action = () =>
                subject.Should().NotBeNull();

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected subject not to be <null>.");
        }
    }

    public class BeInRange
    {
        [Fact]
        public void When_assertion_an_instance_to_be_in_a_certain_range_and_it_is_it_should_succeed()
        {
            // Arrange
            var subject = new ComparableOfInt(1);

            // Act
            Action action = () =>
                subject.Should().BeInRange(new ComparableOfInt(1), new ComparableOfInt(2));

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_an_instance_to_be_in_a_certain_range_and_it_is_it_should_succeed()
        {
            // Arrange
            var subject = new ComparableOfInt(2);

            // Act
            Action action = () =>
                subject.Should().BeInRange(new ComparableOfInt(1), new ComparableOfInt(2));

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_assertion_an_instance_to_be_in_a_certain_range_but_it_is_not_it_should_throw()
        {
            // Arrange
            var subject = new ComparableOfInt(3);

            // Act
            Action action = () =>
                subject.Should().BeInRange(new ComparableOfInt(1), new ComparableOfInt(2));

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected subject to be between*and*, but found *.");
        }
    }

    public class NotBeInRange
    {
        [Fact]
        public void When_assertion_an_instance_to_not_be_in_a_certain_range_and_it_is_not_it_should_succeed()
        {
            // Arrange
            var subject = new ComparableOfInt(3);

            // Act
            Action action = () =>
                subject.Should().NotBeInRange(new ComparableOfInt(1), new ComparableOfInt(2));

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void When_assertion_an_instance_to_not_be_in_a_certain_range_but_it_is_not_it_should_throw()
        {
            // Arrange
            var subject = new ComparableOfInt(2);

            // Act
            Action action = () =>
                subject.Should().NotBeInRange(new ComparableOfInt(1), new ComparableOfInt(2));

            // Assert
            action.Should().Throw<XunitException>()
                .WithMessage("Expected subject to not be between*and*, but found *.");
        }

        [Fact]
        public void When_asserting_an_instance_to_not_be_in_a_certain_range_but_it_is_not_it_should_throw()
        {
            // Arrange
            var subject = new ComparableOfInt(1);

            // Act
            Action action = () =>
                subject.Should().NotBeInRange(new ComparableOfInt(1), new ComparableOfInt(2));

            // Assert
            action.Should().Throw<XunitException>();
        }
    }

    public class BeRankedEquallyTo
    {
        [Fact]
        public void When_subect_is_ranked_equal_to_another_subject_and_that_is_expected_it_should_not_throw()
        {
            // Arrange
            var subject = new ComparableOfString("Hello");
            var other = new ComparableOfString("Hello");

            // Act
            Action act = () => subject.Should().BeRankedEquallyTo(other);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_subject_is_not_ranked_equal_to_another_subject_but_that_is_expected_it_should_throw()
        {
            // Arrange
            var subject = new ComparableOfString("42");
            var other = new ComparableOfString("Forty two");

            // Act
            Action act = () => subject.Should().BeRankedEquallyTo(other, "they represent the same number");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected subject*42*to be ranked as equal to*Forty two*because they represent the same number.");
        }
    }

    public class NotBeRankedEquallyTo
    {
        [Fact]
        public void When_subect_is_not_ranked_equal_to_another_subject_and_that_is_expected_it_should_not_throw()
        {
            // Arrange
            var subject = new ComparableOfString("Hello");
            var other = new ComparableOfString("Hi");

            // Act
            Action act = () => subject.Should().NotBeRankedEquallyTo(other);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_subject_is_ranked_equal_to_another_subject_but_that_is_not_expected_it_should_throw()
        {
            // Arrange
            var subject = new ComparableOfString("Lead");
            var other = new ComparableOfString("Lead");

            // Act
            Action act = () => subject.Should().NotBeRankedEquallyTo(other, "they represent different concepts");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage(
                    "Expected subject*Lead*not to be ranked as equal to*Lead*because they represent different concepts.");
        }
    }

    public class BeLessThan
    {
        [Fact]
        public void When_subject_is_less_than_another_subject_and_that_is_expected_it_should_not_throw()
        {
            // Arrange
            var subject = new ComparableOfString("City");
            var other = new ComparableOfString("World");

            // Act
            Action act = () => subject.Should().BeLessThan(other);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_subject_is_not_less_than_another_subject_but_that_is_expected_it_should_throw()
        {
            // Arrange
            var subject = new ComparableOfString("World");
            var other = new ComparableOfString("City");

            // Act
            Action act = () => subject.Should().BeLessThan(other, "a city is smaller than the world");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected subject*World*to be less than*City*because a city is smaller than the world.");
        }

        [Fact]
        public void When_subject_is_equal_to_another_subject_and_expected_to_be_less_it_should_throw()
        {
            // Arrange
            var subject = new ComparableOfString("City");
            var other = new ComparableOfString("City");

            // Act
            Action act = () => subject.Should().BeLessThan(other);

            // Assert
            act.Should().Throw<XunitException>();
        }
    }

    public class BeLessThanOrEqualTo
    {
        [Fact]
        public void When_subject_is_greater_than_another_subject_and_that_is_not_expected_it_should_throw()
        {
            // Arrange
            var subject = new ComparableOfString("World");
            var other = new ComparableOfString("City");

            // Act
            Action act = () => subject.Should().BeLessThanOrEqualTo(other, "we want to order them");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected subject*World*to be less than or equal to*City*because we want to order them.");
        }

        [Fact]
        public void When_subject_is_equal_to_another_subject_and_that_is_expected_it_should_not_throw()
        {
            // Arrange
            var subject = new ComparableOfString("World");
            var other = new ComparableOfString("World");

            // Act
            Action act = () => subject.Should().BeLessThanOrEqualTo(other);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_subject_is_less_than_another_subject_and_less_than_or_equal_is_expected_it_should_not_throw()
        {
            // Arrange
            var subject = new ComparableOfString("City");
            var other = new ComparableOfString("World");

            // Act
            Action act = () => subject.Should().BeLessThanOrEqualTo(other);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Chaining_after_one_assertion()
        {
            // Arrange
            var subject = new ComparableOfString("World");
            var other = new ComparableOfString("World");

            // Act / Assert
            subject.Should().BeLessThanOrEqualTo(other).And.NotBeNull();
        }
    }

    public class BeGreaterThan
    {
        [Fact]
        public void When_subject_is_greater_than_another_subject_and_that_is_expected_it_should_not_throw()
        {
            // Arrange
            var subject = new ComparableOfString("efg");
            var other = new ComparableOfString("abc");

            // Act
            Action act = () => subject.Should().BeGreaterThan(other);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_subject_is_equal_to_another_subject_and_expected_to_be_greater_it_should_throw()
        {
            // Arrange
            var subject = new ComparableOfString("efg");
            var other = new ComparableOfString("efg");

            // Act
            Action act = () => subject.Should().BeGreaterThan(other);

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_subject_is_not_greater_than_another_subject_but_that_is_expected_it_should_throw()
        {
            // Arrange
            var subject = new ComparableOfString("abc");
            var other = new ComparableOfString("def");

            // Act
            Action act = () => subject.Should().BeGreaterThan(other, "'a' is smaller then 'e'");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected subject*abc*to be greater than*def*because 'a' is smaller then 'e'.");
        }
    }

    public class BeGreaterThanOrEqualTo
    {
        [Fact]
        public void When_subject_is_less_than_another_subject_and_that_is_not_expected_it_should_throw()
        {
            // Arrange
            var subject = new ComparableOfString("abc");
            var other = new ComparableOfString("def");

            // Act
            Action act = () => subject.Should().BeGreaterThanOrEqualTo(other, "'d' is bigger then 'a'");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("Expected subject*abc*to be greater than or equal to*def*because 'd' is bigger then 'a'.");
        }

        [Fact]
        public void When_subject_is_equal_to_another_subject_and_that_is_equal_or_greater_it_should_not_throw()
        {
            // Arrange
            var subject = new ComparableOfString("def");
            var other = new ComparableOfString("def");

            // Act
            Action act = () => subject.Should().BeGreaterThanOrEqualTo(other);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_subject_is_greater_than_another_subject_and_greater_than_or_equal_is_expected_it_should_not_throw()
        {
            // Arrange
            var subject = new ComparableOfString("xyz");
            var other = new ComparableOfString("abc");

            // Act
            Action act = () => subject.Should().BeGreaterThanOrEqualTo(other);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Chaining_after_one_assertion()
        {
            // Arrange
            var subject = new ComparableOfString("def");
            var other = new ComparableOfString("def");

            // Act / Assert
            subject.Should().BeGreaterThanOrEqualTo(other).And.NotBeNull();
        }
    }
}

public class ComparableOfString : IComparable<ComparableOfString>
{
    public string Value { get; set; }

    public ComparableOfString(string value)
    {
        Value = value;
    }

    public int CompareTo(ComparableOfString other)
    {
        return string.CompareOrdinal(Value, other.Value);
    }

    public override string ToString()
    {
        return Value;
    }
}

public class SameInstanceIsNotEqualClass : IComparable<SameInstanceIsNotEqualClass>
{
    public override bool Equals(object obj)
    {
        return false;
    }

    public override int GetHashCode()
    {
        return 1;
    }

    int IComparable<SameInstanceIsNotEqualClass>.CompareTo(SameInstanceIsNotEqualClass other) =>
        throw new NotSupportedException("This type is meant for assertions using Equals()");
}

public class EquatableOfInt : IComparable<EquatableOfInt>
{
    public int Value { get; set; }

    public EquatableOfInt(int value)
    {
        Value = value;
    }

    public override bool Equals(object obj)
    {
        return Value == ((EquatableOfInt)obj).Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }

    int IComparable<EquatableOfInt>.CompareTo(EquatableOfInt other) =>
        throw new NotSupportedException("This type is meant for assertions using Equals()");
}

public class ComparableOfInt : IComparable<ComparableOfInt>
{
    public int Value { get; set; }

    public ComparableOfInt(int value)
    {
        Value = value;
    }

    public int CompareTo(ComparableOfInt other)
    {
        return Value.CompareTo(other.Value);
    }

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }
}

public class ComparableCustomer : IComparable<ComparableCustomer>
{
    public ComparableCustomer(int id)
    {
        Id = id;
    }

    public int Id { get; }

    public int CompareTo(ComparableCustomer other)
    {
        return Id.CompareTo(other.Id);
    }
}

public class CustomerDto
{
    public CustomerDto(int id)
    {
        Id = id;
    }

    public int Id { get; }
}

public class AnotherCustomerDto
{
    public AnotherCustomerDto(int id)
    {
        Id = id;
    }

    public int Id { get; }

    public int SomeOtherProperty { get; set; }
}
