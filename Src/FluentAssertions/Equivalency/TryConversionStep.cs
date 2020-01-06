using System;
using System.Globalization;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Attempts to convert the subject's property value to the expected type.
    /// </summary>
    /// <remarks>
    /// Whether or not the conversion is attempted depends on the <see cref="ConversionSelector"/>.
    /// </remarks>
    public class TryConversionStep : IEquivalencyStep
    {
        private readonly ConversionSelector selector;

        public TryConversionStep(ConversionSelector selector)
        {
            this.selector = selector;
        }

        /// <summary>
        /// Gets a value indicating whether this step can handle the current subject and/or expectation.
        /// </summary>
        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            return selector.RequiresConversion(context);
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
        public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator structuralEqualityValidator,
            IEquivalencyAssertionOptions config)
        {
            if ((context.Expectation is null) || (context.Subject is null))
            {
                return false;
            }

            Type subjectType = context.Subject.GetType();
            Type expectationType = context.Expectation.GetType();

            if (subjectType.IsSameOrInherits(expectationType))
            {
                return false;
            }

            if (TryChangeType(context.Subject, expectationType, out object convertedSubject))
            {
                context.TraceSingle(path => $"Converted subject {context.Subject} at {path} to {expectationType}");

                IEquivalencyValidationContext newContext = context.CreateWithDifferentSubject(convertedSubject, expectationType);

                structuralEqualityValidator.AssertEqualityUsing(newContext);
                return true;
            }

            context.TraceSingle(path => $"Subject {context.Subject} at {path} could not be converted to {expectationType}");

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

        public override string ToString()
        {
            return selector.ToString();
        }
    }
}
