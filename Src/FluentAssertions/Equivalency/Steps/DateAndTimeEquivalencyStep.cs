using System;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Steps;

// AV1000: Type 'DateAndTimeEquivalencyStep' contains the word 'and', which suggests it has multiple purpose
#pragma warning disable AV1000

/// <summary>
/// Specific equivalency step for handling date and time types where the types are different and the failure message
/// should make that clear.
/// </summary>
public class DateAndTimeEquivalencyStep : IEquivalencyStep
{
    public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context,
        IValidateChildNodeEquivalency valueChildNodes)
    {
        object subject = comparands.Subject;
        object expectation = comparands.Expectation;

        Type expectedType = comparands.GetExpectedType(context.Options);
        Type subjectType = subject?.GetType();

        if (IsOfDateOrTimeType(expectedType) && IsOfDateOrTimeType(subjectType) && subjectType != expectedType)
        {
            AssertionChain.GetOrCreate()
                .For(context)
                .ForCondition(subject!.Equals(expectation))
                .FailWith("Expected {context} to be {0} (of type {1}){reason}, but found {2} (of type {3}).", expectation,
                    expectedType, subject, subjectType);

            return EquivalencyResult.EquivalencyProven;
        }

        return EquivalencyResult.ContinueWithNext;
    }

#if NET6_0_OR_GREATER
    private static bool IsOfDateOrTimeType(Type type) =>
        type == typeof(DateTime) ||
        type == typeof(DateTimeOffset) ||
        type == typeof(TimeSpan) ||
        type == typeof(TimeOnly) ||
        type == typeof(DateOnly);
#else
    private static bool IsOfDateOrTimeType(Type type) =>
        type == typeof(DateTime) ||
        type == typeof(DateTimeOffset) ||
        type == typeof(TimeSpan);
#endif
}

#pragma warning restore AV1000
