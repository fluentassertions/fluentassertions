using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertionsAsync.Execution;
using static System.FormattableString;

namespace FluentAssertionsAsync.Equivalency.Steps;

public class DictionaryEquivalencyStep : EquivalencyStep<IDictionary>
{
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    protected override async Task<EquivalencyResult> OnHandleAsync(Comparands comparands, IEquivalencyValidationContext context,
        IEquivalencyValidator nestedValidator)
    {
        var subject = comparands.Subject as IDictionary;
        var expectation = comparands.Expectation as IDictionary;

        if (PreconditionsAreMet(expectation, subject) && expectation is not null)
        {
            foreach (object key in expectation.Keys)
            {
                if (context.Options.IsRecursive)
                {
                    context.Tracer.WriteLine(member =>
                        Invariant($"Recursing into dictionary item {key} at {member.Description}"));

                    await nestedValidator.RecursivelyAssertEqualityAsync(new Comparands(subject[key], expectation[key], typeof(object)),
                        context.AsDictionaryItem<object, IDictionary>(key));
                }
                else
                {
                    context.Tracer.WriteLine(member =>
                        Invariant(
                            $"Comparing dictionary item {key} at {member.Description} between subject and expectation"));

                    subject[key].Should().Be(expectation[key], context.Reason.FormattedMessage, context.Reason.Arguments);
                }
            }
        }

        return EquivalencyResult.AssertionCompleted;
    }

    private static bool PreconditionsAreMet(IDictionary expectation, IDictionary subject)
    {
        return AssertIsDictionary(subject)
            && AssertEitherIsNotNull(expectation, subject)
            && AssertSameLength(expectation, subject);
    }

    private static bool AssertEitherIsNotNull(IDictionary expectation, IDictionary subject)
    {
        return AssertionScope.Current
            .ForCondition((expectation is null && subject is null) || expectation is not null)
            .FailWith("Expected {context:subject} to be {0}{reason}, but found {1}.", null, subject);
    }

    private static bool AssertIsDictionary(IDictionary subject)
    {
        return AssertionScope.Current
            .ForCondition(subject is not null)
            .FailWith("Expected {context:subject} to be a dictionary, but it is not.");
    }

    private static bool AssertSameLength(IDictionary expectation, IDictionary subject)
    {
        return AssertionScope.Current
            .ForCondition(expectation is null || subject.Keys.Count == expectation.Keys.Count)
            .FailWith("Expected {context:subject} to be a dictionary with {0} item(s), but it only contains {1} item(s).",
                expectation?.Keys.Count, subject?.Keys.Count);
    }
}
