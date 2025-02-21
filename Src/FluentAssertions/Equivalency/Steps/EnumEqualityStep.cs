#region

using System;
using System.Globalization;
using FluentAssertions.Execution;

#endregion

namespace FluentAssertions.Equivalency.Steps;

public class EnumEqualityStep : IEquivalencyStep
{
    public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context,
        IValidateChildNodeEquivalency valueChildNodes)
    {
        if (!comparands.GetExpectedType(context.Options).IsEnum)
        {
            return EquivalencyResult.ContinueWithNext;
        }

        var assertionChain = AssertionChain.GetOrCreate().For(context);

        assertionChain
            .ForCondition(comparands.Subject?.GetType().IsEnum == true)
            .BecauseOf(context.Reason)
            .FailWith(() =>
            {
                decimal? expectationsUnderlyingValue = ExtractDecimal(comparands.Expectation);
                string expectationName = GetDisplayNameForEnumComparison(comparands.Expectation, expectationsUnderlyingValue);

                return new FailReason(
                    "Expected {context:enum} to be equivalent to {0}{reason}, but found {1}.",
                    expectationName.AsNonFormattable(),
                    comparands.Subject);
            });

        if (assertionChain.Succeeded)
        {
            switch (context.Options.EnumEquivalencyHandling)
            {
                case EnumEquivalencyHandling.ByValue:
                    HandleByValue(assertionChain, comparands, context.Reason);
                    break;

                case EnumEquivalencyHandling.ByName:
                    HandleByName(assertionChain, comparands, context.Reason);
                    break;

                default:
                    throw new InvalidOperationException($"Do not know how to handle {context.Options.EnumEquivalencyHandling}");
            }
        }

        return EquivalencyResult.EquivalencyProven;
    }

    private static void HandleByValue(AssertionChain assertionChain, Comparands comparands, Reason reason)
    {
        decimal? subjectsUnderlyingValue = ExtractDecimal(comparands.Subject);
        decimal? expectationsUnderlyingValue = ExtractDecimal(comparands.Expectation);

        assertionChain
            .ForCondition(subjectsUnderlyingValue == expectationsUnderlyingValue)
            .BecauseOf(reason)
            .FailWith(() =>
            {
                string subjectsName = GetDisplayNameForEnumComparison(comparands.Subject, subjectsUnderlyingValue);
                string expectationName = GetDisplayNameForEnumComparison(comparands.Expectation, expectationsUnderlyingValue);

                return new FailReason(
                    "Expected {context:enum} to equal {0} by value{reason}, but found {1}.",
                    expectationName.AsNonFormattable(), subjectsName.AsNonFormattable());
            });
    }

    private static void HandleByName(AssertionChain assertionChain, Comparands comparands, Reason reason)
    {
        string subject = comparands.Subject.ToString();
        string expected = comparands.Expectation.ToString();

        assertionChain
            .ForCondition(subject == expected)
            .BecauseOf(reason)
            .FailWith(() =>
            {
                decimal? subjectsUnderlyingValue = ExtractDecimal(comparands.Subject);
                decimal? expectationsUnderlyingValue = ExtractDecimal(comparands.Expectation);

                string subjectsName = GetDisplayNameForEnumComparison(comparands.Subject, subjectsUnderlyingValue);
                string expectationName = GetDisplayNameForEnumComparison(comparands.Expectation, expectationsUnderlyingValue);

                return new FailReason(
                    "Expected {context:enum} to equal {0} by name{reason}, but found {1}.",
                    expectationName.AsNonFormattable(), subjectsName.AsNonFormattable());
            });
    }

    private static string GetDisplayNameForEnumComparison(object o, decimal? v)
    {
        if (o is null || v is null)
        {
            return "<null>";
        }

        string typePart = o.GetType().Name;
        string namePart = o.ToString().Replace(", ", "|", StringComparison.Ordinal);
        string valuePart = v.Value.ToString(CultureInfo.InvariantCulture);
        return $"{typePart}.{namePart} {{value: {valuePart}}}";
    }

    private static decimal? ExtractDecimal(object o)
    {
        return o is not null ? Convert.ToDecimal(o, CultureInfo.InvariantCulture) : null;
    }
}
