using System.Linq;

namespace FluentAssertions.Structural
{
    internal class ApplyAssertionRulesEqualityStep : IStructuralEqualityStep
    {
        /// <summary>
        /// Gets a value indicating whether this step can handle the current subject and/or expectation.
        /// </summary>
        public bool CanHandle(StructuralEqualityContext context)
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
        public bool Handle(StructuralEqualityContext context, IStructuralEqualityValidator parent)
        {
            if (context.CurrentProperty != null)
            {
                var assertionContext =
                    new AssertionContext(context.CurrentProperty, context.Subject, context.Expectation, context.Reason,
                                         context.ReasonArgs);

                return context.Config.AssertionRules.Any(rule => rule.AssertEquality(assertionContext));
            }

            return false;
        }
    }
}