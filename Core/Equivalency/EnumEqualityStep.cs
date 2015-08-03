#region

using System;
using System.Globalization;

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
            Type subjectType = config.GetSubjectType(context);

            return ((subjectType != null) && subjectType.IsEnum()) ||
                   ((context.Expectation != null) && context.Expectation.GetType().IsEnum());
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
                    long subjectsUnderlyingValue = Convert.ToInt64(context.Subject);
                    long expectationsUnderlyingValue = Convert.ToInt64(context.Expectation);

                    subjectsUnderlyingValue.Should().Be(expectationsUnderlyingValue, context.Reason, context.ReasonArgs);
                    break;

                case EnumEquivalencyHandling.ByName:
                    context.Subject.ToString().Should().Be(context.Expectation.ToString(), context.Reason, context.ReasonArgs);
                    break;

                default:
                    throw new InvalidOperationException(string.Format("Don't know how to handle {0}",
                        config.EnumEquivalencyHandling));
            }

            return true;
        }
    }
}