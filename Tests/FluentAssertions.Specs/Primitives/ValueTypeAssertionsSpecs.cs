using System;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives;

public class ValueTypeAssertionsSpecs
{
    [Fact]
    public void Succeeds_for_the_expected_value()
    {
        // Arrange
        var subject = new CoordinatesAssertions(new Coordinates(1, 2));

        // Act / Assert
        subject.Be(new Coordinates(1, 2));
    }

    [Fact]
    public void A_value_must_equal_the_expected_value()
    {
        // Arrange
        var subject = new CoordinatesAssertions(new Coordinates(1, 2));

        // Act
        Action act = () => subject.Be(new Coordinates(3, 4), "it is {0}", "required");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage(
                "Expected subject to be Coordinates(3, 4) because it is required, but found Coordinates(1, 2).");
    }

    [Fact]
    public void Succeeds_for_the_expected_nullable_value()
    {
        // Arrange
        var subject = new CoordinatesAssertions(new Coordinates(1, 2));

        // Act / Assert
        subject.Be((Coordinates?)new Coordinates(1, 2));
    }

    [Fact]
    public void A_value_must_equal_the_expected_nullable_value()
    {
        // Arrange
        var subject = new CoordinatesAssertions(new Coordinates(1, 2));

        // Act
        Action act = () => subject.Be((Coordinates?)new Coordinates(3, 4), "it is {0}", "required");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage(
                "Expected subject to be Coordinates(3, 4) because it is required, but found Coordinates(1, 2).");
    }

    [Fact]
    public void A_value_never_equals_a_null_expected_value()
    {
        // Arrange
        var subject = new CoordinatesAssertions(new Coordinates(1, 2));

        // Act
        Action act = () => subject.Be(null);

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected subject to be <null>, but found Coordinates(1, 2).");
    }

    [Fact]
    public void Succeeds_for_an_unexpected_value()
    {
        // Arrange
        var subject = new CoordinatesAssertions(new Coordinates(1, 2));

        // Act / Assert
        subject.NotBe(new Coordinates(3, 4));
    }

    [Fact]
    public void A_value_must_not_equal_the_unexpected_value()
    {
        // Arrange
        var subject = new CoordinatesAssertions(new Coordinates(1, 2));

        // Act
        Action act = () => subject.NotBe(new Coordinates(1, 2), "it is {0}", "required");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Did not expect subject to be Coordinates(1, 2) because it is required.");
    }

    [Fact]
    public void Succeeds_for_an_unexpected_nullable_value()
    {
        // Arrange
        var subject = new CoordinatesAssertions(new Coordinates(1, 2));

        // Act / Assert
        subject.NotBe((Coordinates?)new Coordinates(3, 4));
    }

    [Fact]
    public void A_value_must_not_equal_the_unexpected_nullable_value()
    {
        // Arrange
        var subject = new CoordinatesAssertions(new Coordinates(1, 2));

        // Act
        Action act = () => subject.NotBe((Coordinates?)new Coordinates(1, 2), "it is {0}", "required");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Did not expect subject to be Coordinates(1, 2) because it is required.");
    }

    [Fact]
    public void A_value_is_always_different_from_a_null_unexpected_value()
    {
        // Arrange
        var subject = new CoordinatesAssertions(new Coordinates(1, 2));

        // Act / Assert
        subject.NotBe(null);
    }

    [Fact]
    public void Succeeds_for_a_value_matching_the_predicate()
    {
        // Arrange
        var subject = new CoordinatesAssertions(new Coordinates(1, 2));

        // Act / Assert
        subject.Match(c => c.X == 1);
    }

    [Fact]
    public void A_value_must_satisfy_the_predicate()
    {
        // Arrange
        var subject = new CoordinatesAssertions(new Coordinates(1, 2));

        // Act
        Action act = () => subject.Match(c => c.X == 3, "it is {0}", "required");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected subject to match*because it is required, but found Coordinates(1, 2).");
    }

    [Fact]
    public void Matching_against_a_null_predicate_is_not_allowed()
    {
        // Arrange
        var subject = new CoordinatesAssertions(new Coordinates(1, 2));

        // Act
        Action act = () => subject.Match(null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Cannot match a value against a <null> predicate.*");
    }

    [Fact]
    public void Throws_a_helpful_error_when_accidentally_using_equals()
    {
        // Arrange
        var subject = new CoordinatesAssertions(new Coordinates(1, 2));
        var expectation = new CoordinatesAssertions(new Coordinates(1, 2));

        // Act
        // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
        Action act = () => subject.Equals(expectation);

        // Assert
        act.Should().Throw<NotSupportedException>()
            .WithMessage("Equals is not part of Fluent Assertions. Did you mean Be() instead?");
    }
}

public class NullableValueTypeAssertionsSpecs
{
    [Fact]
    public void Succeeds_for_a_value_that_has_been_initialized()
    {
        // Arrange
        var subject = new NullableCoordinatesAssertions(new Coordinates(1, 2));

        // Act / Assert
        subject.NotBeNull();
    }

    [Fact]
    public void A_value_that_has_not_been_initialized_is_not_allowed_to_be_not_null()
    {
        // Arrange
        var subject = new NullableCoordinatesAssertions(null);

        // Act
        Action act = () => subject.NotBeNull("it is {0}", "required");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected subject not to be <null> because it is required.");
    }

    [Fact]
    public void Succeeds_for_a_value_that_has_not_been_initialized_yet()
    {
        // Arrange
        var subject = new NullableCoordinatesAssertions(null);

        // Act / Assert
        subject.BeNull();
    }

    [Fact]
    public void An_initialized_value_is_not_allowed_to_be_null()
    {
        // Arrange
        var subject = new NullableCoordinatesAssertions(new Coordinates(1, 2));

        // Act
        Action act = () => subject.BeNull("it is {0}", "required");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected subject to be <null> because it is required, but found Coordinates(1, 2).");
    }

    [Fact]
    public void HaveValue_is_equivalent_to_NotBeNull()
    {
        // Arrange
        var subject = new NullableCoordinatesAssertions(null);

        // Act
        Action act = () => subject.HaveValue("it is {0}", "required");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected subject not to be <null> because it is required.");
    }

    [Fact]
    public void NotHaveValue_is_equivalent_to_BeNull()
    {
        // Arrange
        var subject = new NullableCoordinatesAssertions(new Coordinates(1, 2));

        // Act
        Action act = () => subject.NotHaveValue("it is {0}", "required");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected subject to be <null> because it is required, but found Coordinates(1, 2).");
    }

    [Fact]
    public void Succeeds_for_the_expected_value()
    {
        // Arrange
        var subject = new NullableCoordinatesAssertions(new Coordinates(1, 2));

        // Act / Assert
        subject.Be(new Coordinates(1, 2));
    }

    [Fact]
    public void A_value_must_equal_the_expected_value()
    {
        // Arrange
        var subject = new NullableCoordinatesAssertions(new Coordinates(1, 2));

        // Act
        Action act = () => subject.Be(new Coordinates(3, 4), "it is {0}", "required");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage(
                "Expected subject to be Coordinates(3, 4) because it is required, but found Coordinates(1, 2).");
    }

    [Fact]
    public void A_null_value_never_equals_the_expected_value()
    {
        // Arrange
        var subject = new NullableCoordinatesAssertions(null);

        // Act
        Action act = () => subject.Be(new Coordinates(3, 4));

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected subject to be Coordinates(3, 4), but found <null>.");
    }

    [Fact]
    public void A_null_value_equals_a_null_expected_value()
    {
        // Arrange
        var subject = new NullableCoordinatesAssertions(null);

        // Act / Assert
        subject.Be(null);
    }

    [Fact]
    public void Succeeds_for_an_unexpected_value()
    {
        // Arrange
        var subject = new NullableCoordinatesAssertions(new Coordinates(1, 2));

        // Act / Assert
        subject.NotBe(new Coordinates(3, 4));
    }

    [Fact]
    public void A_value_must_not_equal_the_unexpected_value()
    {
        // Arrange
        var subject = new NullableCoordinatesAssertions(new Coordinates(1, 2));

        // Act
        Action act = () => subject.NotBe(new Coordinates(1, 2), "it is {0}", "required");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Did not expect subject to be Coordinates(1, 2) because it is required.");
    }

    [Fact]
    public void A_null_value_is_never_equal_to_the_unexpected_value()
    {
        // Arrange
        var subject = new NullableCoordinatesAssertions(null);

        // Act / Assert
        subject.NotBe(new Coordinates(3, 4));
    }

    [Fact]
    public void A_null_value_never_differs_from_a_null_unexpected_value()
    {
        // Arrange
        var subject = new NullableCoordinatesAssertions(null);

        // Act
        Action act = () => subject.NotBe(null);

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Did not expect subject to be <null>.");
    }

    [Fact]
    public void Succeeds_for_a_value_matching_the_predicate()
    {
        // Arrange
        var subject = new NullableCoordinatesAssertions(new Coordinates(1, 2));

        // Act / Assert
        subject.Match(c => c.X == 1);
    }

    [Fact]
    public void A_value_must_satisfy_the_predicate()
    {
        // Arrange
        var subject = new NullableCoordinatesAssertions(new Coordinates(1, 2));

        // Act
        Action act = () => subject.Match(c => c.X == 3, "it is {0}", "required");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected subject to match*because it is required, but found Coordinates(1, 2).");
    }

    [Fact]
    public void A_null_value_never_satisfies_the_predicate()
    {
        // Arrange
        var subject = new NullableCoordinatesAssertions(null);

        // Act
        Action act = () => subject.Match(c => c.X == 1);

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected subject to match*but found <null>.");
    }

    [Fact]
    public void Matching_against_a_null_predicate_is_not_allowed()
    {
        // Arrange
        var subject = new NullableCoordinatesAssertions(new Coordinates(1, 2));

        // Act
        Action act = () => subject.Match((Expression<Func<Coordinates, bool>>)null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Cannot match a value against a <null> predicate.*");
    }

    [Fact]
    public void Succeeds_for_a_null_value_matching_the_nullable_predicate()
    {
        // Arrange
        var subject = new NullableCoordinatesAssertions(null);

        // Act / Assert
        subject.Match(c => c == null);
    }

    [Fact]
    public void Succeeds_for_a_value_matching_the_nullable_predicate()
    {
        // Arrange
        var subject = new NullableCoordinatesAssertions(new Coordinates(1, 2));

        // Act / Assert
        subject.Match(c => c.Value.X == 1);
    }

    [Fact]
    public void A_value_must_satisfy_the_nullable_predicate()
    {
        // Arrange
        var subject = new NullableCoordinatesAssertions(new Coordinates(1, 2));

        // Act
        Action act = () => subject.Match(c => c.Value.X == 3, "it is {0}", "required");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected subject to match*because it is required, but found Coordinates(1, 2).");
    }

    [Fact]
    public void Matching_against_a_null_nullable_predicate_is_not_allowed()
    {
        // Arrange
        var subject = new NullableCoordinatesAssertions(new Coordinates(1, 2));

        // Act
        Action act = () => subject.Match(null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Cannot match a value against a <null> predicate.*");
    }

    [Fact]
    public void Throws_a_helpful_error_when_accidentally_using_equals()
    {
        // Arrange
        var subject = new NullableCoordinatesAssertions(new Coordinates(1, 2));
        var expectation = new NullableCoordinatesAssertions(new Coordinates(1, 2));

        // Act
        // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
        Action act = () => subject.Equals(expectation);

        // Assert
        act.Should().Throw<NotSupportedException>()
            .WithMessage("Equals is not part of Fluent Assertions. Did you mean Be() instead?");
    }
}

[StructLayout(LayoutKind.Auto)]
public readonly struct Coordinates(int x, int y) : IEquatable<Coordinates>
{
    public int X { get; } = x;

    // ReSharper disable once MemberCanBePrivate.Global
    public int Y { get; } = y;

    public bool Equals(Coordinates other) => X == other.X && Y == other.Y;

    public override bool Equals(object obj) => obj is Coordinates other && Equals(other);

    public override int GetHashCode() => (X * 397) ^ Y;

    public static bool operator ==(Coordinates left, Coordinates right) => left.Equals(right);

    public static bool operator !=(Coordinates left, Coordinates right) => !left.Equals(right);

    public override string ToString() => $"Coordinates({X}, {Y})";
}

public class CoordinatesAssertions(Coordinates subject)
    : ValueTypeAssertions<Coordinates, CoordinatesAssertions>(subject, AssertionChain.GetOrCreate())
{
    protected override string Identifier => "subject";
}

public class NullableCoordinatesAssertions(Coordinates? subject)
    : NullableValueTypeAssertions<Coordinates, NullableCoordinatesAssertions>(subject, AssertionChain.GetOrCreate())
{
    protected override string Identifier => "subject";
}
