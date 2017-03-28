using System;
using System.Globalization;

using FluentAssertions.Common;

namespace FluentAssertions.Equivalency
{
    public class TryConversionEquivalencyStep : IEquivalencyStep
    {
        /// <summary>
        /// Gets a value indicating whether this step can handle the current subject and/or expectation.
        /// </summary>
        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            return true;
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
        public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator structuralEqualityValidator, IEquivalencyAssertionOptions config)
        {
            if (!ReferenceEquals(context.Expectation, null) && !ReferenceEquals(context.Subject, null)
                && !context.Subject.GetType().IsSameOrInherits(context.Expectation.GetType()))
            {
                Type expectationType = context.Expectation.GetType();

                object convertedSubject;
                if (TryChangeType(context.Subject, expectationType, out convertedSubject))
                {
                    context.TraceSingle(path => $"Converted subject {context.Subject} at {path} to {expectationType}");

                    var newContext = context.CreateWithDifferentSubject(convertedSubject, expectationType);

                    structuralEqualityValidator.AssertEqualityUsing(newContext);
                    return true;
                }

                context.TraceSingle(path => $"Subject {context.Subject} at {path} could not be converted to {expectationType}");
            }

            return false;
        }

        private static bool TryChangeType(object subject, Type expectationType, out object conversionResult)
        {
            conversionResult = null;
            try
            {
                conversionResult = Convert.ChangeType(subject, expectationType, CultureInfo.CurrentCulture);
                return true;
            }
            catch (FormatException)
            {
            }
            catch (InvalidCastException)
            {
            }

            return false;
        }
    }
}