using System;
using System.Globalization;
using FluentAssertions.Common;

namespace FluentAssertions.Assertions.Structure
{
    internal class TryConversionEqualityStep : IStructuralEqualityStep
    {
        /// <summary>
        /// Applies a step as part of the task to compare two objects for structural equality.
        /// </summary>
        /// <value>
        /// Returns <c>true</c> if this step finalizes the comparison task, returns <c>false</c> otherwise.
        /// </value>
        /// <remarks>
        /// May throw when preconditions are not met, or if it detects mismatching data.
        /// </remarks>
        public bool Execute(StructuralEqualityContext context, IStructuralEqualityValidator structuralEqualityValidator)
        {
            if (!ReferenceEquals(context.Expectation, null) && !ReferenceEquals(context.Subject, null)
                && !context.Subject.GetType().IsSameOrInherits(context.Expectation.GetType()))
            {
                try
                {
                    context.Subject = Convert.ChangeType(context.Subject, context.Expectation.GetType(), CultureInfo.CurrentCulture);
                }
                catch (FormatException)
                {
                }
                catch (InvalidCastException)
                {
                }
            }

            return false;
        }
    }
}