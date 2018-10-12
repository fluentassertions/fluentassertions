#region

using System;
using System.Globalization;
using System.Reflection;
using FluentAssertions.Execution;

#endregion

namespace FluentAssertions.Equivalency
{
    public class EnumEqualityStep : IEquivalencyStep
    {
        /// <summary>
        /// Gets a value indicating whether this step can handle the current subject and/or expectation.
        /// </summary>
        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            Type subjectType = config.GetExpectationType(context);

            return (subjectType?.GetTypeInfo().IsEnum == true) ||
                   (context.Expectation?.GetType().GetTypeInfo().IsEnum == true);
        }

        /// <summary>
        /// Applies a step as part of the task to compare two objects for structural equality.
        /// </summary>
        /// <value>
        /// Should return <c>true</c> if the subject matches the expectation or if no additional assertions
        /// have to be executed. Should return <c>false</c> otherwise.
        /// </value>
        /// <remarks>
        /// May throw when preconditions are not met or if it detects mismatching data.
        /// </remarks>
        public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent,
            IEquivalencyAssertionOptions config)
        {
            switch (config.EnumEquivalencyHandling)
            {
                case EnumEquivalencyHandling.ByValue:
                    HandleByValue(context);
                    break;

                case EnumEquivalencyHandling.ByName:
                    HandleByName(context);
                    break;

                default:
                    throw new InvalidOperationException(string.Format("Do not know how to handle {0}",
                        config.EnumEquivalencyHandling));
            }

            return true;
        }

        private static void HandleByValue(IEquivalencyValidationContext context)
        {
            decimal? subjectsUnderlyingValue = ExtractDecimal(context.Subject);
            decimal? expectationsUnderlyingValue = ExtractDecimal(context.Expectation);

            Execute.Assertion
                .ForCondition(subjectsUnderlyingValue == expectationsUnderlyingValue)
                .FailWith(() =>
                {
                    string subjectsName = GetDisplayNameForEnumComparison(context.Subject, subjectsUnderlyingValue);
                    string expectationName = GetDisplayNameForEnumComparison(context.Expectation, expectationsUnderlyingValue);

                    return new FailReason($"Expected {{context:enum}} to equal {expectationName} by value{{reason}}, but found {subjectsName}.");
                });
        }

        private static void HandleByName(IEquivalencyValidationContext context)
        {
            string subject = context.Subject?.ToString();
            string expected = context.Expectation.ToString();

            Execute.Assertion
                .ForCondition(subject == expected)
                .FailWith(() =>
                {
                    decimal? subjectsUnderlyingValue = ExtractDecimal(context.Subject);
                    decimal? expectationsUnderlyingValue = ExtractDecimal(context.Expectation);

                    string subjectsName = GetDisplayNameForEnumComparison(context.Subject, subjectsUnderlyingValue);
                    string expectationName = GetDisplayNameForEnumComparison(context.Expectation, expectationsUnderlyingValue);
                    return new FailReason(
                            $"Expected {{context:enum}} to equal {expectationName} by name{{reason}}, but found {subjectsName}.");
                });
        }

        private static string GetDisplayNameForEnumComparison(object o, decimal? v)
        {
            if (o is null || v is null)
            {
                return "null";
            }

            if (o.GetType().GetTypeInfo().IsEnum)
            {
                string typePart = o.GetType().Name;
                string namePart = Enum.GetName(o.GetType(), o);
                string valuePart = v.Value.ToString(CultureInfo.InvariantCulture);
                return $"{typePart}.{namePart}({valuePart})";
            }

            return v.Value.ToString(CultureInfo.InvariantCulture);
        }

        private static decimal? ExtractDecimal(object o)
        {
            return o != null ? Convert.ToDecimal(o) : (decimal?)null;
        }
    }
}
