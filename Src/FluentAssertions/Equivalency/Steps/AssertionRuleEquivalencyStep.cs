using System;
using System.Linq.Expressions;
using FluentAssertions.Common;
using FluentAssertions.Equivalency.Execution;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Steps;

public class AssertionRuleEquivalencyStep<TSubject> : IEquivalencyStep
{
    private readonly Func<IObjectInfo, bool> predicate;
    private readonly string description;
    private readonly Action<IAssertionContext<TSubject>> assertionAction;
    private readonly AutoConversionStep converter = new();

    public AssertionRuleEquivalencyStep(
        Expression<Func<IObjectInfo, bool>> predicate,
        Action<IAssertionContext<TSubject>> assertionAction)
    {
        this.predicate = predicate.Compile();
        this.assertionAction = assertionAction;
        description = predicate.ToString();
    }

    public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context,
        IValidateChildNodeEquivalency valueChildNodes)
    {
        bool success = false;

        using (var scope = new AssertionScope())
        {
            // Try without conversion
            if (AppliesTo(comparands, context.CurrentNode))
            {
                success = ExecuteAssertion(comparands, context);
            }

            bool converted = false;

            if (!success && context.Options.ConversionSelector.RequiresConversion(comparands, context.CurrentNode))
            {
                // Convert into a child context
                context = context.Clone();
                converter.Handle(comparands, context, valueChildNodes);
                converted = true;
            }

            if (converted && AppliesTo(comparands, context.CurrentNode))
            {
                // Try again after conversion
                success = ExecuteAssertion(comparands, context);
                if (success)
                {
                    // If the assertion succeeded after conversion, discard the failures from
                    // the previous attempt. If it didn't, let the scope throw with those failures.
                    scope.Discard();
                }
            }
        }

        return success ? EquivalencyResult.EquivalencyProven : EquivalencyResult.ContinueWithNext;
    }

    private bool AppliesTo(Comparands comparands, INode currentNode) => predicate(new ObjectInfo(comparands, currentNode));

    private bool ExecuteAssertion(Comparands comparands, IEquivalencyValidationContext context)
    {
        bool subjectIsNull = comparands.Subject is null;
        bool expectationIsNull = comparands.Expectation is null;

        var assertionChain = AssertionChain.GetOrCreate().For(context);

        assertionChain
                .ForCondition(subjectIsNull || comparands.Subject.GetType().IsSameOrInherits(typeof(TSubject)))
                .FailWith("Expected {0} from subject to be a {1}{reason}, but found a {2}.",
                    context.CurrentNode.Subject.AsNonFormattable(),
                    typeof(TSubject), comparands.Subject?.GetType())
                .Then
                .ForCondition(expectationIsNull || comparands.Expectation.GetType().IsSameOrInherits(typeof(TSubject)))
                .FailWith(
                    "Expected {0} from expectation to be a {1}{reason}, but found a {2}.",
                    context.CurrentNode.Subject.AsNonFormattable(),
                    typeof(TSubject), comparands.Expectation?.GetType());

        if (assertionChain.Succeeded)
        {
            if ((subjectIsNull || expectationIsNull) && !CanBeNull<TSubject>())
            {
                return false;
            }

            // Caller identitification should not get confused about invoking a Should within the assertion action
            string callerIdentifier = context.CurrentNode.Subject.ToString();
            assertionChain.OverrideCallerIdentifier(() => callerIdentifier);
            assertionChain.ReuseOnce();

            assertionAction(AssertionContext<TSubject>.CreateFrom(comparands, context));
            return true;
        }

        return false;
    }

    private static bool CanBeNull<T>() => !typeof(T).IsValueType || Nullable.GetUnderlyingType(typeof(T)) is not null;

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>
    /// A string that represents the current object.
    /// </returns>
    /// <filterpriority>2</filterpriority>
    public override string ToString()
    {
        return "Invoke Action<" + typeof(TSubject).Name + "> when " + description;
    }
}
