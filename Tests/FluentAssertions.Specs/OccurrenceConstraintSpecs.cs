using System;
using FluentAssertionsAsync.Execution;
using FluentAssertionsAsync.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs;

public class OccurrenceConstraintSpecs
{
    public static TheoryData<OccurrenceConstraint, int> PassingConstraints => new()
    {
        { AtLeast.Once(), 1 },
        { AtLeast.Once(), 2 },
        { AtLeast.Twice(), 2 },
        { AtLeast.Twice(), 3 },
        { AtLeast.Thrice(), 3 },
        { AtLeast.Thrice(), 4 },
        { AtLeast.Times(4), 4 },
        { AtLeast.Times(4), 5 },
        { 4.TimesOrMore(), 4 },
        { 4.TimesOrMore(), 5 },
        { AtMost.Once(), 0 },
        { AtMost.Once(), 1 },
        { AtMost.Twice(), 1 },
        { AtMost.Twice(), 2 },
        { AtMost.Thrice(), 2 },
        { AtMost.Thrice(), 3 },
        { AtMost.Times(4), 3 },
        { AtMost.Times(4), 4 },
        { 4.TimesOrLess(), 4 },
        { 4.TimesOrLess(), 1 },
        { Exactly.Once(), 1 },
        { Exactly.Twice(), 2 },
        { Exactly.Thrice(), 3 },
        { Exactly.Times(4), 4 },
        { 4.TimesExactly(), 4 },
        { LessThan.Twice(), 1 },
        { LessThan.Thrice(), 2 },
        { LessThan.Times(4), 3 },
        { MoreThan.Once(), 2 },
        { MoreThan.Twice(), 3 },
        { MoreThan.Thrice(), 4 },
        { MoreThan.Times(4), 5 }
    };

    [Theory]
    [MemberData(nameof(PassingConstraints))]
    public void Occurrence_constraint_passes(OccurrenceConstraint constraint, int occurrences)
    {
        // Act / Assert
        Execute.Assertion
            .ForConstraint(constraint, occurrences)
            .FailWith("");
    }

    public static TheoryData<OccurrenceConstraint, int> FailingConstraints => new()
    {
        { AtLeast.Once(), 0 },
        { AtLeast.Twice(), 1 },
        { AtLeast.Thrice(), 2 },
        { AtLeast.Times(4), 3 },
        { 4.TimesOrMore(), 3 },
        { AtMost.Once(), 2 },
        { AtMost.Twice(), 3 },
        { AtMost.Thrice(), 4 },
        { AtMost.Times(4), 5 },
        { 4.TimesOrLess(), 5 },
        { Exactly.Once(), 0 },
        { Exactly.Once(), 2 },
        { Exactly.Twice(), 1 },
        { Exactly.Twice(), 3 },
        { Exactly.Thrice(), 2 },
        { Exactly.Thrice(), 4 },
        { Exactly.Times(4), 3 },
        { Exactly.Times(4), 5 },
        { 4.TimesExactly(), 1 },
        { LessThan.Twice(), 2 },
        { LessThan.Twice(), 3 },
        { LessThan.Thrice(), 3 },
        { LessThan.Thrice(), 4 },
        { LessThan.Times(4), 4 },
        { LessThan.Times(4), 5 },
        { MoreThan.Once(), 0 },
        { MoreThan.Once(), 1 },
        { MoreThan.Twice(), 1 },
        { MoreThan.Twice(), 2 },
        { MoreThan.Thrice(), 2 },
        { MoreThan.Thrice(), 3 },
        { MoreThan.Times(4), 3 },
        { MoreThan.Times(4), 4 },
    };

    [Theory]
    [MemberData(nameof(FailingConstraints))]
    public void Occurrence_constraint_fails(OccurrenceConstraint constraint, int occurrences)
    {
        // Act
        Action act = () => Execute.Assertion
            .ForConstraint(constraint, occurrences)
            .FailWith($"Expected occurrence to be {constraint.Mode} {constraint.ExpectedCount}, but it was {occurrences}");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected occurrence to be *, but it was *");
    }
}
