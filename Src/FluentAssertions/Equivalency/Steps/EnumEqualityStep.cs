#region

using System;
using System.Globalization;
using FluentAssertions.Execution;

#endregion

namespace FluentAssertions.Equivalency.Steps
{
    public class EnumEqualityStep : IEquivalencyStep
    {
        public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context, IEquivalencyValidator nestedValidator)
        {
            if (!comparands.GetExpectedType(context.Options).IsEnum)
            {
                return EquivalencyResult.ContinueWithNext;
            }

            bool succeeded = Execute.Assertion
                .ForCondition(comparands.Subject?.GetType().IsEnum == true)
                .FailWith(() =>
                {
                    decimal? expectationsUnderlyingValue = ExtractDecimal(comparands.Expectation);
                    string expectationName = GetDisplayNameForEnumComparison(comparands.Expectation, expectationsUnderlyingValue);

                    return new FailReason($"Expected {{context:enum}} to be equivalent to {expectationName}{{reason}}, but found {{0}}.", comparands.Subject);
                });

            if (succeeded)
            {
                switch (context.Options.EnumEquivalencyHandling)
                {
                    case EnumEquivalencyHandling.ByValue:
                        HandleByValue(comparands);
                        break;

                    case EnumEquivalencyHandling.ByName:
                        HandleByName(comparands);
                        break;

                    default:
                        throw new InvalidOperationException($"Do not know how to handle {context.Options.EnumEquivalencyHandling}");
                }
            }

            return EquivalencyResult.AssertionCompleted;
        }

        private static void HandleByValue(Comparands comparands)
        {
            decimal? subjectsUnderlyingValue = ExtractDecimal(comparands.Subject);
            decimal? expectationsUnderlyingValue = ExtractDecimal(comparands.Expectation);

            Execute.Assertion
                .ForCondition(subjectsUnderlyingValue == expectationsUnderlyingValue)
                .FailWith(() =>
                {
                    string subjectsName = GetDisplayNameForEnumComparison(comparands.Subject, subjectsUnderlyingValue);
                    string expectationName = GetDisplayNameForEnumComparison(comparands.Expectation, expectationsUnderlyingValue);

                    return new FailReason($"Expected {{context:enum}} to equal {expectationName} by value{{reason}}, but found {subjectsName}.");
                });
        }

        private static void HandleByName(Comparands comparands)
        {
            string subject = comparands.Subject.ToString();
            string expected = comparands.Expectation.ToString();

            Execute.Assertion
                .ForCondition(subject == expected)
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
}
