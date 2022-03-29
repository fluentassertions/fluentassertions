using System;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class OccurrenceConstraintSpecs
    {
        public static object[][] PassingConstraints() => new object[][]
        {
            new object[] { AtLeast.Once(), 1 },
            new object[] { AtLeast.Once(), 2 },
            new object[] { AtLeast.Twice(), 2 },
            new object[] { AtLeast.Twice(), 3 },
            new object[] { AtLeast.Thrice(), 3 },
            new object[] { AtLeast.Thrice(), 4 },
            new object[] { AtLeast.Times(4), 4 },
            new object[] { AtLeast.Times(4), 5 },

            new object[] { AtMost.Once(), 0 },
            new object[] { AtMost.Once(), 1 },
            new object[] { AtMost.Twice(), 1 },
            new object[] { AtMost.Twice(), 2 },
            new object[] { AtMost.Thrice(), 2 },
            new object[] { AtMost.Thrice(), 3 },
            new object[] { AtMost.Times(4), 3 },
            new object[] { AtMost.Times(4), 4 },

            new object[] { Exactly.Once(), 1 },
            new object[] { Exactly.Twice(), 2 },
            new object[] { Exactly.Thrice(), 3 },
            new object[] { Exactly.Times(4), 4 },

            new object[] { LessThan.Twice(), 1 },
            new object[] { LessThan.Thrice(), 2 },
            new object[] { LessThan.Times(4), 3 },

            new object[] { MoreThan.Once(), 2 },
            new object[] { MoreThan.Twice(), 3 },
            new object[] { MoreThan.Thrice(), 4 },
            new object[] { MoreThan.Times(4), 5 },
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

        public static object[][] FailingConstraints() => new object[][]
        {
            new object[] { AtLeast.Once(), 0 },
            new object[] { AtLeast.Twice(), 1 },
            new object[] { AtLeast.Thrice(), 2 },
            new object[] { AtLeast.Times(4), 3 },

            new object[] { AtMost.Once(), 2 },
            new object[] { AtMost.Twice(), 3 },
            new object[] { AtMost.Thrice(), 4 },
            new object[] { AtMost.Times(4), 5 },

            new object[] { Exactly.Once(), 0 },
            new object[] { Exactly.Once(), 2 },
            new object[] { Exactly.Twice(), 1 },
            new object[] { Exactly.Twice(), 3 },
            new object[] { Exactly.Thrice(), 2 },
            new object[] { Exactly.Thrice(), 4 },
            new object[] { Exactly.Times(4), 3 },
            new object[] { Exactly.Times(4), 5 },

            new object[] { LessThan.Twice(), 2 },
            new object[] { LessThan.Twice(), 3 },
            new object[] { LessThan.Thrice(), 3 },
            new object[] { LessThan.Thrice(), 4 },
            new object[] { LessThan.Times(4), 4 },
            new object[] { LessThan.Times(4), 5 },

            new object[] { MoreThan.Once(), 0 },
            new object[] { MoreThan.Once(), 1 },
            new object[] { MoreThan.Twice(), 1 },
            new object[] { MoreThan.Twice(), 2 },
            new object[] { MoreThan.Thrice(), 2 },
            new object[] { MoreThan.Thrice(), 3 },
            new object[] { MoreThan.Times(4), 3 },
            new object[] { MoreThan.Times(4), 4 },
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
}
