using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using static System.FormattableString;

namespace FluentAssertions.Equivalency.Steps;

public class DictionaryEquivalencyStep : EquivalencyStep<IDictionary>
{
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    protected override EquivalencyResult OnHandle(Comparands comparands,
        IEquivalencyValidationContext context,
        IValidateChildNodeEquivalency nestedValidator)
    {
        var subject = comparands.Subject as IDictionary;
        var expectation = comparands.Expectation as IDictionary;

        var assertionChain = AssertionChain.GetOrCreate().For(context);

        if (PreconditionsAreMet(expectation, subject, assertionChain) && expectation is not null)
        {
            foreach (object key in expectation.Keys)
            {
                if (context.Options.IsRecursive)
                {
                    context.Tracer.WriteLine(member =>
                        Invariant($"Recursing into dictionary item {key} at {member.Description}"));

                    nestedValidator.AssertEquivalencyOf(new Comparands(subject[key], expectation[key], typeof(object)), context.AsDictionaryItem<object, IDictionary>(key));
                }
                else
                {
                    context.Tracer.WriteLine(member =>
                        Invariant(
                            $"Comparing dictionary item {key} at {member.Description} between subject and expectation"));

                    assertionChain.WithCallerPostfix($"[{key.ToFormattedString()}]").ReuseOnce();
                    subject[key].Should().Be(expectation[key], context.Reason.FormattedMessage, context.Reason.Arguments);
                }
            }
        }

        return EquivalencyResult.EquivalencyProven;
    }

    private static bool PreconditionsAreMet(IDictionary expectation, IDictionary subject, AssertionChain assertionChain)
    {
        return AssertIsDictionary(subject, assertionChain)
            && AssertEitherIsNotNull(expectation, subject, assertionChain)
            && AssertSameLength(expectation, subject, assertionChain);
    }

    private static bool AssertEitherIsNotNull(IDictionary expectation, IDictionary subject, AssertionChain assertionChain)
    {
        assertionChain
            .ForCondition((expectation is null && subject is null) || expectation is not null)
            .FailWith("Expected {context:subject} to be {0}{reason}, but found {1}.", null, subject);

        return assertionChain.Succeeded;
    }

    private static bool AssertIsDictionary(IDictionary subject, AssertionChain assertionChain)
    {
        assertionChain
            .ForCondition(subject is not null)
            .FailWith("Expected {context:subject} to be a dictionary, but it is not.");

        return assertionChain.Succeeded;
    }

    private static bool AssertSameLength(IDictionary expectation, IDictionary subject, AssertionChain assertionChain)
    {
        assertionChain
            .ForCondition(expectation is null || subject.Keys.Count == expectation.Keys.Count)
            .FailWith("Expected {context:subject} to be a dictionary with {0} item(s), but it only contains {1} item(s).",
                expectation?.Keys.Count, subject?.Keys.Count);

        return assertionChain.Succeeded;
    }
}
