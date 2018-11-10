using System.Diagnostics;
using FluentAssertions.Execution;

namespace FluentAssertions
{
    [DebuggerNonUserCode]
    public class StringWithOccurenceConstraint
    {
        public delegate bool StringOccurrenceComparer(int occuredTimes, int expectedTimes);

        private readonly StringOccurrenceComparer comparator;
        private readonly string occurrenceMode;
        private readonly StringContainmentConstraints parentConstraint;

        public StringWithOccurenceConstraint(StringContainmentConstraints parentConstraint,
            string occurrenceMode,
            StringOccurrenceComparer comparator)
        {
            this.parentConstraint = parentConstraint;
            this.occurrenceMode = occurrenceMode;
            this.comparator = comparator;
        }

        public AndConstraint<StringContainmentConstraints> Once(string because = "", params object[] becauseArgs) =>
            Times(1, "once", because, becauseArgs);

        public AndConstraint<StringContainmentConstraints> Twice(string because = "", params object[] becauseArgs) =>
            Times(2, "twice", because, becauseArgs);

        public AndConstraint<StringContainmentConstraints> Thrice(string because = "", params object[] becauseArgs) =>
            Times(3, "thrice", because, becauseArgs);

        public AndConstraint<StringContainmentConstraints> Times(int times, string because = "", params object[] becauseArgs) =>
            Times(times, times == 1 ? "1 time" : $"{times} times", because, becauseArgs);

        private AndConstraint<StringContainmentConstraints> Times(int times, string displayValue, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(comparator(parentConstraint.Occurrences, times))
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    $"Expected {{context:string}} {{0}} to {parentConstraint.ContainmentMode} {{1}} {occurrenceMode} {displayValue}{{reason}}, but found {(parentConstraint.Occurrences == 1 ? "1 time" : $"{parentConstraint.Occurrences} times")}.",
                    parentConstraint.Subject, parentConstraint.Expected);

            return new AndConstraint<StringContainmentConstraints>(parentConstraint);
        }
    }
}
