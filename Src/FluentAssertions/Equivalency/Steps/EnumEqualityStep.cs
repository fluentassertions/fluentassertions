#region

using System;
using System.Globalization;
using System.Threading.Tasks;
using FluentAssertionsAsync.Execution;

#endregion

namespace FluentAssertionsAsync.Equivalency.Steps;

public class EnumEqualityStep : IEquivalencyStep
{
    public Task<EquivalencyResult> HandleAsync(Comparands comparands, IEquivalencyValidationContext context,
        IEquivalencyValidator nestedValidator)
    {
        if (!comparands.GetExpectedType(context.Options).IsEnum)
        {
            return Task.FromResult(EquivalencyResult.ContinueWithNext);
        }

        bool succeeded = Execute.Assertion
            .ForCondition(comparands.Subject?.GetType().IsEnum == true)
            .BecauseOf(context.Reason)
            .FailWith(() =>
            {
                decimal? expectationsUnderlyingValue = ExtractDecimal(comparands.Expectation);
                string expectationName = GetDisplayNameForEnumComparison(comparands.Expectation, expectationsUnderlyingValue);

                return new FailReason(
                    $"Expected {{context:enum}} to be equivalent to {expectationName}{{reason}}, but found {{0}}.",
                    comparands.Subject);
            });

        if (succeeded)
        {
            switch (context.Options.EnumEquivalencyHandling)
            {
                case EnumEquivalencyHandling.ByValue:
                    HandleByValue(comparands, context.Reason);
                    break;

                case EnumEquivalencyHandling.ByName:
                    HandleByName(comparands, context.Reason);
                    break;

                default:
                    throw new InvalidOperationException($"Do not know how to handle {context.Options.EnumEquivalencyHandling}");
            }
        }

        return Task.FromResult(EquivalencyResult.AssertionCompleted);
    }

    private static void HandleByValue(Comparands comparands, Reason reason)
    {
        decimal? subjectsUnderlyingValue = ExtractDecimal(comparands.Subject);
        decimal? expectationsUnderlyingValue = ExtractDecimal(comparands.Expectation);

        Execute.Assertion
            .ForCondition(subjectsUnderlyingValue == expectationsUnderlyingValue)
            .BecauseOf(reason)
            .FailWith(() =>
            {
                string subjectsName = GetDisplayNameForEnumComparison(comparands.Subject, subjectsUnderlyingValue);
                string expectationName = GetDisplayNameForEnumComparison(comparands.Expectation, expectationsUnderlyingValue);

                return new FailReason(
                    $"Expected {{context:enum}} to equal {expectationName} by value{{reason}}, but found {subjectsName}.");
            });
    }

    private static void HandleByName(Comparands comparands, Reason reason)
    {
        string subject = comparands.Subject.ToString();
        string expected = comparands.Expectation.ToString();

        Execute.Assertion
            .ForCondition(subject == expected)
            .BecauseOf(reason)
            .FailWith(() =>
            {
                decimal? subjectsUnderlyingValue = ExtractDecimal(comparands.Subject);
                decimal? expectationsUnderlyingValue = ExtractDecimal(comparands.Expectation);

                string subjectsName = GetDisplayNameForEnumComparison(comparands.Subject, subjectsUnderlyingValue);
                string expectationName = GetDisplayNameForEnumComparison(comparands.Expectation, expectationsUnderlyingValue);

                return new FailReason(
                    $"Expected {{context:enum}} to equal {expectationName} by name{{reason}}, but found {subjectsName}.");
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
        return $"{typePart}.{namePart} {{{{value: {valuePart}}}}}";
    }

    private static decimal? ExtractDecimal(object o)
    {
        return o is not null ? Convert.ToDecimal(o, CultureInfo.InvariantCulture) : null;
    }
}
